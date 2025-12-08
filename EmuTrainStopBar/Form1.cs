using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmuTrainStopBar
{
    public partial class Form1 : Form
    {
        // Supported emulators
        // Note that the enum value names are important, they are also used for string operations
        enum PineModes { Inactive, Duckstation, PCSX2, RPCS3 }

        PineModes PineMode = PineModes.Inactive;
        IntPtr ipc;

        // Store some design settings so they can be reloaded later
        Color startBackColor;
        FormBorderStyle startFormBorderStyle;
        int startWidth;
        int startHeight;

        double startOpacity;
        double InGameOpacity = 0.7;

        enum DistanceDataTypes { s16, s32, s64, f32, f64 }

        // Emulator process
        Process EmuProcess;
        IntPtr EmuWindowHandle = IntPtr.Zero;
        Window.RECT EmuRect;

        // Overlay sizing
        bool OverlayFullscreen = false;
        int CaptionHeight = SystemInformation.CaptionHeight;
        int MenuHeight = SystemInformation.MenuHeight;

        // Settings for the settings
        string SettingsFileLocation = "game.txt";
        string SettingsLinePattern = @"^([^#\n].+?),(.+?),([0-9A-Fa-f]+?),(.+?),([0-9.-]+),([0-9.]+),([0-9]+?),([0-9]+)";

        string[] SettingsLines;
        int SettingsLineNum;

        // Applied game settings
        bool SettingsLoadSuccess = false;
        string GameId = "";                 // 1
        string GameVersion = "";            // 2
        uint DistanceAddress;               // 3
        DistanceDataTypes DistanceDataType; // 4
        float DistanceScale;                // 5
        float AspectRatio;                  // 6
        int PixelsPerMeter = 70;            // 7
        int BarWidth = 20;                  // 8

        const int CenterLineThickness = 4;

        // Stopbar
        Bitmap BmBar;
        SolidBrush VeryCloseBrush = new SolidBrush(Color.White);
        SolidBrush M1Brush = new SolidBrush(Color.FromArgb(0xC0, 0xE0, 0xF8));
        SolidBrush M2Brush = new SolidBrush(Color.FromArgb(0x70, 0xA0, 0xD0));
        SolidBrush M3Brush = new SolidBrush(Color.FromArgb(0x38, 0x50, 0x80));
        SolidBrush MarkerBrush = new SolidBrush(Color.FromArgb(0xFF, 0xA8, 0x00));
        const double MaxDistance = 50;

        // For auto-hide function
        Point PrevCursorPosition = new Point(0, 0);

        // This function is run when starting the form
        public Form1()
        {
            InitializeComponent();

            // Default strings before the first tick
            labelGameName.Text = "System standby";
            labelGameId.Text = "1. Enable PINE in your emulator";
            labelSettings.Text = "2. Choose your emulator below";
            labelDistance.Text = "";

            // Store the style options for the restore function
            startBackColor = this.BackColor;
            startFormBorderStyle = this.FormBorderStyle;
            startWidth = this.Width;
            startHeight = this.Height;
            startOpacity = this.Opacity;

            BmBar = new Bitmap(BarWidth, this.Height);
            pictureBoxBar.Image = BmBar;
            pictureBoxBar.Width = BarWidth;
        }

        private double ReadAsDouble(IntPtr v, UInt32 address, DistanceDataTypes storedDataType)
        {
            switch (storedDataType)
            {
                case DistanceDataTypes.s16:
                    return Pine.ReadInt16(v, address);
                case DistanceDataTypes.s32:
                    return Pine.ReadInt32(v, address);
                case DistanceDataTypes.s64:
                    return Pine.ReadInt64(v, address);
                case DistanceDataTypes.f32:
                    return Pine.ReadSingle(v, address);
                case DistanceDataTypes.f64:
                    return Pine.ReadDouble(v, address);
                default:
                    return 0;
            }
        }

        // Reset to idle appearance
        // Also disables fullscreen
        private void RestoreStyle()
        {
            SetMenuContentVisibility(true);
            btnFullscreen.Visible = false;
            pictureBoxBar.Visible = false;

            this.BackColor = startBackColor;
            this.FormBorderStyle = startFormBorderStyle;
            this.Width = startWidth;
            this.Height = startHeight;
            this.Opacity = startOpacity;

            timerFrame.Stop();
        }

        private void SetMenuContentVisibility(bool vis)
        {
            btnQuit.Visible = vis;
            btnFullscreen.Visible = vis;
            labelGameName.Visible = vis;
            labelGameId.Visible = vis;
            labelSettings.Visible = vis;
            labelDistance.Visible = vis;
        }

        // Fit the window within a determined rectangular area while setting aspect ratio
        private void SetWindowPosition(int centerX, int centerY, int maxWidth, int maxHeight, float aspectRatio)
        {
            if (maxWidth / aspectRatio < maxHeight)
            {
                // Use height limit
                this.Width = (int)Math.Round(maxHeight * aspectRatio);
                this.Height = maxHeight;
            }
            else
            {
                // Use width limit
                this.Width = maxWidth;
                this.Height = (int)Math.Round(maxWidth / aspectRatio);
            }

            this.Left = centerX - this.Width / 2;
            this.Top = centerY - this.Height / 2;
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            // Clear resources by closing IPC
            switch (PineMode)
            {
                case PineModes.Duckstation:
                    Pine.pine_duckstation_delete(ipc);
                    break;
                case PineModes.PCSX2:
                    Pine.pine_pcsx2_delete(ipc);
                    break;
                case PineModes.RPCS3:
                    Pine.pine_rpcs3_delete(ipc);
                    break;
            }

            // Actually close the app and its window
            Application.Exit();
        }

        private void btnFullscreen_Click(object sender, EventArgs e)
        {
            OverlayFullscreen = !OverlayFullscreen;

            if (OverlayFullscreen)
            {
                Point formCenter = new Point(
                    this.Left + this.Width / 2,
                    this.Top + this.Height / 2);

                Screen targetScreen = null;

                // Choose the screen that the center of the form is currently in
                // ==> where the emulator is
                foreach (Screen s in Screen.AllScreens)
                {
                    if (s.Bounds.Contains(formCenter))
                    {
                        targetScreen = s;
                        break;
                    }
                }

                // else get the primary screen
                if (targetScreen == null)
                {
                    targetScreen = Screen.PrimaryScreen;
                }

                Rectangle screenRect = targetScreen.Bounds;
                int centerX = (screenRect.Left + screenRect.Right) / 2;
                int centerY = (screenRect.Top + screenRect.Bottom) / 2;
                int maxWidth = screenRect.Right - screenRect.Left;
                int maxHeight = screenRect.Bottom - screenRect.Top;
                SetWindowPosition(centerX, centerY, maxWidth, maxHeight, AspectRatio);

                // Deprecated
                // Set size and position
                /*
                this.Left = targetScreen.Bounds.Left;
                this.Top = targetScreen.Bounds.Top;
                this.Width = targetScreen.Bounds.Width;
                this.Height = targetScreen.Bounds.Height;
                */
            }
        }

        private void btnDuckstation_Click(object sender, EventArgs e)
        {
            // Initialize in Duckstation mode
            ipc = Pine.pine_duckstation_new();
            PineMode = PineModes.Duckstation;
            CommonPineInit();
        }

        private void btnPCSX2_Click(object sender, EventArgs e)
        {
            // Initialize in PCSX2 mode
            ipc = Pine.pine_pcsx2_new();
            PineMode = PineModes.PCSX2;
            CommonPineInit();
        }

        private void btnRPCS3_Click(object sender, EventArgs e)
        {
            // Initialize in RPCS3 mode
            ipc = Pine.pine_rpcs3_new();
            PineMode = PineModes.RPCS3;
            CommonPineInit();
        }

        // Common operations run after clicking any of the emulator choices
        private void CommonPineInit()
        {
            // Hide emulator choices
            btnDuckstation.Visible = false;
            btnPCSX2.Visible = false;
            btnRPCS3.Visible = false;
            btnDuckstation.Refresh();
            btnPCSX2.Refresh();
            btnRPCS3.Refresh();

            // Start timers
            timerInfo.Start();

            // Run info update once immediately
            timerInfo_Tick(this, null);
        }

        private Process GetEmulatorProcess(PineModes mode)
        {
            // Convert both to the uppercase to provide case-insensitive search (just in case)
            string checkStr = mode.ToString().ToUpperInvariant();

            Process[] allProcesses = Process.GetProcesses();
            foreach (Process p in allProcesses)
            {
                if (p.ProcessName.ToUpperInvariant().Contains(checkStr))
                {
                    return p;
                }
            }
            return null;
        }

        // Read settings file and apply them if applicable
        private bool ApplySettings(string id, string version)
        {
            SettingsLineNum = -1;

            // Open and read settings file
            SettingsLines = File.ReadAllLines(SettingsFileLocation);

            // Use the first valid and matching line for settings
            for (int i = 0; i < SettingsLines.Length; i++)
            {
                // Use regex to check that the line is valid
                string line = SettingsLines[i];
                Match match = Regex.Match(line, SettingsLinePattern);

                if (!match.Success)
                {
                    // Invalid line
                    continue;
                }

                if (match.Groups[1].Value == id && match.Groups[2].Value == version)
                {
                    // Try to parse
                    uint _address;
                    DistanceDataTypes _dataTypeAsEnum;
                    float _scale;
                    float _aspectRatio;
                    int _pixelsPerMeter;
                    int _barWidth;
                    try
                    {
                        _address = uint.Parse(match.Groups[3].Value, NumberStyles.HexNumber);
                        _dataTypeAsEnum = (DistanceDataTypes)Enum.Parse(typeof(DistanceDataTypes), match.Groups[4].Value, true);
                        _scale = float.Parse(match.Groups[5].Value, NumberStyles.Float);
                        _aspectRatio = float.Parse(match.Groups[6].Value, NumberStyles.Float);
                        _pixelsPerMeter = int.Parse(match.Groups[7].Value);
                        _barWidth = int.Parse(match.Groups[8].Value);
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }
                    catch (OverflowException)
                    {
                        continue;
                    }

                    // Apply settings
                    SettingsLineNum = i;
                    DistanceAddress = _address;
                    DistanceDataType = _dataTypeAsEnum;
                    DistanceScale = _scale;
                    AspectRatio = _aspectRatio;
                    PixelsPerMeter = _pixelsPerMeter;
                    BarWidth = _barWidth;

                    // Do not search any more
                    break;
                }
                else
                {
                    continue;
                }
            }

            // Apply settings complete (whether passed or failed)
            // Set new game id and version to prevent reading the file again
            GameId = id;
            GameVersion = version;
            return SettingsLineNum >= 0;
        }

        // Info tick is used to update:
            // Window style: 4 lines of debug text, background
            // Communications: PINE state, game settings, etc.
        private void timerInfo_Tick(object sender, EventArgs e)
        {
            if (PineMode == PineModes.Inactive)
            {
                return;
            }
            else
            {
                // Get emulator process
                // (PineMode is already set in each button's respective function)
                EmuProcess = GetEmulatorProcess(PineMode);
                if (EmuProcess == null)
                {
                    // Emulator is not running
                    labelGameName.Text = $"System standby ({PineMode})";
                    labelGameId.Text = "Emulator not available.";
                    labelSettings.Text = "";
                    labelDistance.Text = "";

                    RestoreStyle();
                    return;
                }
                EmuWindowHandle = EmuProcess.MainWindowHandle;

                int emuState = Pine.pine_status(ipc, false);
                if (emuState == 2)
                {
                    // PINE server is active, but emulator is not
                    labelGameName.Text = $"System standby ({PineMode})";
                    labelGameId.Text = "PINE connection established.";
                    labelSettings.Text = "Waiting for game...";
                    labelDistance.Text = "";

                    RestoreStyle();
                    return;
                }

                // Check for error codes after getting emulator state
                // Required as the emulator state is 0
                    // both in normal operation and communication errors
                int ipcError = Pine.pine_get_error(ipc, false);
                if (ipcError > 0)
                {
                    // Usually happens when the PINE server is not active yet
                    labelGameName.Text = $"System standby ({PineMode})";
                    labelGameId.Text = "PINE communication error.";
                    labelSettings.Text = "Check that it is enabled,";
                    labelDistance.Text = "and a game is running.";

                    RestoreStyle();
                    return;
                }

                // Everything below only happens in gameplay and when PINE is working

                IntPtr gameTitlePtr = Pine.pine_getgametitle(ipc, false);
                string gameTitle = Pine.ReadUnmanagedString(gameTitlePtr);

                IntPtr gameIdPtr = Pine.pine_getgameid(ipc, false);
                string gameId = Pine.ReadUnmanagedString(gameIdPtr);

                IntPtr gameUuidPtr = Pine.pine_getgameuuid(ipc, false);
                string gameUuid = Pine.ReadUnmanagedString(gameUuidPtr);

                IntPtr gameVersionPtr = Pine.pine_getgameversion(ipc, false);
                string gameVersion = Pine.ReadUnmanagedString(gameVersionPtr);

                labelGameName.Text = gameTitle;
                labelGameId.Text = $"{gameId} ({gameVersion})";

                // Reload settings if the game has changed
                if (GameId != gameId || GameVersion != gameVersion)
                {
                    SettingsLoadSuccess = ApplySettings(gameId, gameVersion);
                    if (SettingsLoadSuccess)
                    {
                        labelSettings.Text = $"[Line {SettingsLineNum}] 0x{DistanceAddress.ToString("X8")}, {DistanceDataType}, x{DistanceScale}, AR{AspectRatio}, {PixelsPerMeter}px/m, {BarWidth}px";
                    }
                    else
                    {
                        labelSettings.Text = $"No configuration for this game.";
                        labelDistance.Text = $"Add it to {SettingsFileLocation} and restart the app.";
                        RestoreStyle();
                        return;
                    }
                }

                // Everything below only happens if settings are loaded
                if (SettingsLoadSuccess)
                {
                    this.BackColor = this.TransparencyKey;
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.Opacity = InGameOpacity;

                    // Features for window size and position change
                    if (!timerFrame.Enabled)
                    {
                        timerFrame.Start();
                    }

                    pictureBoxBar.Visible = true;
                }
            }
        }

        // Frame tick is run very frequently
        private void timerFrame_Tick(object sender, EventArgs e)
        {
            // Emulator has not been detected
            if (EmuWindowHandle == IntPtr.Zero)
            {
                return;
            }

            bool getRectSuccess = Window.GetWindowRect(EmuWindowHandle, out EmuRect);

            if (!OverlayFullscreen)
            {
                // Update window size and position
                if (getRectSuccess)
                {
                    int centerX = (EmuRect.Left + EmuRect.Right) / 2;
                    int centerY = (EmuRect.Top + EmuRect.Bottom + CaptionHeight) / 2;
                    int maxWidth = EmuRect.Right - EmuRect.Left;
                    int maxHeight = EmuRect.Bottom - EmuRect.Top - CaptionHeight - 2 * MenuHeight;
                    SetWindowPosition(centerX, centerY, maxWidth, maxHeight, AspectRatio);

                    // Deprecated
                    /*
                    this.Top = EmuRect.Top + CaptionHeight + MenuHeight;
                    this.Left = EmuRect.Left;
                    this.Width = EmuRect.Right - EmuRect.Left;
                    this.Height = EmuRect.Bottom - EmuRect.Top - CaptionHeight - 2*MenuHeight;
                    */
                }
            }

            // Stopbar
            pictureBoxBar.Left = this.Width - pictureBoxBar.Width;
            pictureBoxBar.Top = 0;
            pictureBoxBar.Width = pictureBoxBar.Width;
            pictureBoxBar.Height = this.Height;

            // Get stop distance
            double distance = ReadAsDouble(ipc, DistanceAddress, DistanceDataType);
            labelDistance.Text = $"{distance :F2} m";

            // Create stopbar image
            Graphics gb = Graphics.FromImage(BmBar);
            gb.Clear(this.TransparencyKey);

            // Actual stopbar background
            int stopbarTop = this.Height / 2 - 3 * PixelsPerMeter;
            gb.FillRectangle(M3Brush, new Rectangle(0, stopbarTop, BarWidth, PixelsPerMeter));
            gb.FillRectangle(M3Brush, new Rectangle(0, stopbarTop + 5 * PixelsPerMeter, BarWidth, PixelsPerMeter));
            gb.FillRectangle(M2Brush, new Rectangle(0, stopbarTop + PixelsPerMeter, BarWidth, 4 * PixelsPerMeter));
            gb.FillRectangle(M1Brush, new Rectangle(0, stopbarTop + 2 * PixelsPerMeter, BarWidth, 2 * PixelsPerMeter));
            gb.FillRectangle(VeryCloseBrush, new Rectangle(0, stopbarTop + 3 * PixelsPerMeter - CenterLineThickness / 2, BarWidth, CenterLineThickness));

            // Marker
            if (Math.Abs(distance) < MaxDistance)
            {
                int markerCenterY = (int)(this.Height / 2 - distance * PixelsPerMeter);
                Point[] poly = new Point[]
                {
                new Point(BarWidth / 2, markerCenterY - BarWidth / 2),
                new Point(BarWidth, markerCenterY),
                new Point(BarWidth / 2, markerCenterY + BarWidth / 2),
                new Point(0, markerCenterY)
                };
                gb.FillPolygon(MarkerBrush, poly);
            }

            pictureBoxBar.Image = BmBar;
            gb.Dispose();

            // For auto-hide function
            bool mouseMoved = Cursor.Position != PrevCursorPosition;
            if (mouseMoved)
            {
                timerMouseMove.Stop();
                timerMouseMove.Start();
                SetMenuContentVisibility(true);
            }
            PrevCursorPosition = Cursor.Position;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // Assume that the height has changed
            BmBar = new Bitmap(BarWidth, this.Height);
            pictureBoxBar.Image = BmBar;
        }

        private void timerMouseMove_Tick(object sender, EventArgs e)
        {
            timerMouseMove.Stop();

            // Since the stopbar is only visible in game, it can be used as a check for being in game (hacky)
            if (pictureBoxBar.Visible)
            {
                SetMenuContentVisibility(false);
            }
        }
    }
}
