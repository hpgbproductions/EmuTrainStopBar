
namespace EmuTrainStopBar
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnQuit = new System.Windows.Forms.Button();
            this.timerInfo = new System.Windows.Forms.Timer(this.components);
            this.labelGameId = new System.Windows.Forms.Label();
            this.labelGameName = new System.Windows.Forms.Label();
            this.btnDuckstation = new System.Windows.Forms.Button();
            this.btnPCSX2 = new System.Windows.Forms.Button();
            this.btnRPCS3 = new System.Windows.Forms.Button();
            this.labelSettings = new System.Windows.Forms.Label();
            this.labelDistance = new System.Windows.Forms.Label();
            this.timerFrame = new System.Windows.Forms.Timer(this.components);
            this.btnFullscreen = new System.Windows.Forms.Button();
            this.pictureBoxBar = new System.Windows.Forms.PictureBox();
            this.timerMouseMove = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBar)).BeginInit();
            this.SuspendLayout();
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.Black;
            this.btnQuit.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnQuit.FlatAppearance.BorderSize = 2;
            this.btnQuit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuit.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuit.ForeColor = System.Drawing.Color.Gray;
            this.btnQuit.Location = new System.Drawing.Point(12, 12);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(0);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(100, 28);
            this.btnQuit.TabIndex = 0;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // timerInfo
            // 
            this.timerInfo.Interval = 5000;
            this.timerInfo.Tick += new System.EventHandler(this.timerInfo_Tick);
            // 
            // labelGameId
            // 
            this.labelGameId.AutoSize = true;
            this.labelGameId.ForeColor = System.Drawing.Color.Gray;
            this.labelGameId.Location = new System.Drawing.Point(12, 68);
            this.labelGameId.Name = "labelGameId";
            this.labelGameId.Size = new System.Drawing.Size(96, 16);
            this.labelGameId.TabIndex = 1;
            this.labelGameId.Text = "labelGameId";
            // 
            // labelGameName
            // 
            this.labelGameName.AutoSize = true;
            this.labelGameName.ForeColor = System.Drawing.Color.Gray;
            this.labelGameName.Location = new System.Drawing.Point(12, 52);
            this.labelGameName.Name = "labelGameName";
            this.labelGameName.Size = new System.Drawing.Size(112, 16);
            this.labelGameName.TabIndex = 2;
            this.labelGameName.Text = "labelGameName";
            // 
            // btnDuckstation
            // 
            this.btnDuckstation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDuckstation.BackColor = System.Drawing.Color.Black;
            this.btnDuckstation.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnDuckstation.FlatAppearance.BorderSize = 2;
            this.btnDuckstation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnDuckstation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDuckstation.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDuckstation.ForeColor = System.Drawing.Color.Gray;
            this.btnDuckstation.Location = new System.Drawing.Point(85, 125);
            this.btnDuckstation.Margin = new System.Windows.Forms.Padding(0);
            this.btnDuckstation.Name = "btnDuckstation";
            this.btnDuckstation.Size = new System.Drawing.Size(200, 28);
            this.btnDuckstation.TabIndex = 1;
            this.btnDuckstation.Text = "Duckstation";
            this.btnDuckstation.UseVisualStyleBackColor = false;
            this.btnDuckstation.Click += new System.EventHandler(this.btnDuckstation_Click);
            // 
            // btnPCSX2
            // 
            this.btnPCSX2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPCSX2.BackColor = System.Drawing.Color.Black;
            this.btnPCSX2.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnPCSX2.FlatAppearance.BorderSize = 2;
            this.btnPCSX2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnPCSX2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPCSX2.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPCSX2.ForeColor = System.Drawing.Color.Gray;
            this.btnPCSX2.Location = new System.Drawing.Point(85, 165);
            this.btnPCSX2.Margin = new System.Windows.Forms.Padding(0);
            this.btnPCSX2.Name = "btnPCSX2";
            this.btnPCSX2.Size = new System.Drawing.Size(200, 28);
            this.btnPCSX2.TabIndex = 2;
            this.btnPCSX2.Text = "PCSX2";
            this.btnPCSX2.UseVisualStyleBackColor = false;
            this.btnPCSX2.Click += new System.EventHandler(this.btnPCSX2_Click);
            // 
            // btnRPCS3
            // 
            this.btnRPCS3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRPCS3.BackColor = System.Drawing.Color.Black;
            this.btnRPCS3.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnRPCS3.FlatAppearance.BorderSize = 2;
            this.btnRPCS3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnRPCS3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRPCS3.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRPCS3.ForeColor = System.Drawing.Color.Gray;
            this.btnRPCS3.Location = new System.Drawing.Point(85, 205);
            this.btnRPCS3.Margin = new System.Windows.Forms.Padding(0);
            this.btnRPCS3.Name = "btnRPCS3";
            this.btnRPCS3.Size = new System.Drawing.Size(200, 28);
            this.btnRPCS3.TabIndex = 3;
            this.btnRPCS3.Text = "RPCS3";
            this.btnRPCS3.UseVisualStyleBackColor = false;
            this.btnRPCS3.Click += new System.EventHandler(this.btnRPCS3_Click);
            // 
            // labelSettings
            // 
            this.labelSettings.AutoSize = true;
            this.labelSettings.ForeColor = System.Drawing.Color.Gray;
            this.labelSettings.Location = new System.Drawing.Point(12, 84);
            this.labelSettings.Name = "labelSettings";
            this.labelSettings.Size = new System.Drawing.Size(112, 16);
            this.labelSettings.TabIndex = 4;
            this.labelSettings.Text = "labelSettings";
            // 
            // labelDistance
            // 
            this.labelDistance.AutoSize = true;
            this.labelDistance.ForeColor = System.Drawing.Color.Gray;
            this.labelDistance.Location = new System.Drawing.Point(12, 100);
            this.labelDistance.Name = "labelDistance";
            this.labelDistance.Size = new System.Drawing.Size(112, 16);
            this.labelDistance.TabIndex = 5;
            this.labelDistance.Text = "labelDistance";
            // 
            // timerFrame
            // 
            this.timerFrame.Interval = 16;
            this.timerFrame.Tick += new System.EventHandler(this.timerFrame_Tick);
            // 
            // btnFullscreen
            // 
            this.btnFullscreen.BackColor = System.Drawing.Color.Black;
            this.btnFullscreen.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnFullscreen.FlatAppearance.BorderSize = 2;
            this.btnFullscreen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnFullscreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFullscreen.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFullscreen.ForeColor = System.Drawing.Color.Gray;
            this.btnFullscreen.Location = new System.Drawing.Point(125, 12);
            this.btnFullscreen.Margin = new System.Windows.Forms.Padding(0);
            this.btnFullscreen.Name = "btnFullscreen";
            this.btnFullscreen.Size = new System.Drawing.Size(100, 28);
            this.btnFullscreen.TabIndex = 6;
            this.btnFullscreen.Text = "Full";
            this.btnFullscreen.UseVisualStyleBackColor = false;
            this.btnFullscreen.Visible = false;
            this.btnFullscreen.Click += new System.EventHandler(this.btnFullscreen_Click);
            // 
            // pictureBoxBar
            // 
            this.pictureBoxBar.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxBar.Location = new System.Drawing.Point(360, 0);
            this.pictureBoxBar.Name = "pictureBoxBar";
            this.pictureBoxBar.Size = new System.Drawing.Size(30, 600);
            this.pictureBoxBar.TabIndex = 7;
            this.pictureBoxBar.TabStop = false;
            this.pictureBoxBar.Visible = false;
            // 
            // timerMouseMove
            // 
            this.timerMouseMove.Interval = 3000;
            this.timerMouseMove.Tick += new System.EventHandler(this.timerMouseMove_Tick);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.pictureBoxBar);
            this.Controls.Add(this.btnFullscreen);
            this.Controls.Add(this.labelDistance);
            this.Controls.Add(this.labelSettings);
            this.Controls.Add(this.btnRPCS3);
            this.Controls.Add(this.btnPCSX2);
            this.Controls.Add(this.btnDuckstation);
            this.Controls.Add(this.labelGameName);
            this.Controls.Add(this.labelGameId);
            this.Controls.Add(this.btnQuit);
            this.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "NTNC Stop Bar Overlay";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Timer timerInfo;
        private System.Windows.Forms.Label labelGameId;
        private System.Windows.Forms.Label labelGameName;
        private System.Windows.Forms.Button btnPCSX2;
        private System.Windows.Forms.Button btnDuckstation;
        private System.Windows.Forms.Button btnRPCS3;
        private System.Windows.Forms.Label labelSettings;
        private System.Windows.Forms.Label labelDistance;
        private System.Windows.Forms.Timer timerFrame;
        private System.Windows.Forms.Button btnFullscreen;
        private System.Windows.Forms.PictureBox pictureBoxBar;
        private System.Windows.Forms.Timer timerMouseMove;
    }
}

