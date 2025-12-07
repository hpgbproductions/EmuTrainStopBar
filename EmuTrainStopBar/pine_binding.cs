// Modified from https://github.com/GovanifY/pine/blob/master/bindings/csharp/Program.cs

using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace EmuTrainStopBar
{
    class Pine
    {
        const string libipc = "pine_c.dll";

        // illegal
        /*
        public static string TryAccessUnmanagedString(IntPtr ptr, out bool success)
        {
            try
            {
                string str = ReadUnmanagedString(ptr);
                success = true;
                return str;
            }
            catch (AccessViolationException)
            {
                success = false;
                return "";
            }
        }
        */

        public enum IPCCommand : byte
        {
            MsgRead8 = 0,           /**< Read 8 bit value to memory. */
            MsgRead16 = 1,          /**< Read 16 bit value to memory. */
            MsgRead32 = 2,          /**< Read 32 bit value to memory. */
            MsgRead64 = 3,          /**< Read 64 bit value to memory. */
            MsgWrite8 = 4,          /**< Write 8 bit value to memory. */
            MsgWrite16 = 5,         /**< Write 16 bit value to memory. */
            MsgWrite32 = 6,         /**< Write 32 bit value to memory. */
            MsgWrite64 = 7,         /**< Write 64 bit value to memory. */
            MsgVersion = 8,         /**< Returns the emulator version. */
            MsgSaveState = 9,       /**< Saves a savestate. */
            MsgLoadState = 0xA,     /**< Loads a savestate. */
            MsgTitle = 0xB,         /**< Returns the game title. */
            MsgID = 0xC,            /**< Returns the game ID. */
            MsgUUID = 0xD,          /**< Returns the game UUID. */
            MsgGameVersion = 0xE,   /**< Returns the game verion. */
            MsgStatus = 0xF,        /**< Returns the emulator status. */
            MsgUnimplemented = 0xFF /**< Unimplemented IPC message. */
        }

        public enum IPCStatus : UInt32
        {
            Success = 0,       /**< IPC command successfully completed. */
            Fail = 1,          /**< IPC command failed to execute. */
            OutOfMemory = 2,   /**< IPC command too big to send. */
            NoConnection = 3,  /**< Cannot connect to the IPC socket. */
            Unimplemented = 4, /**< Unimplemented IPC command. */
            Unknown = 5        /**< Unknown status. */
        }

        public enum EmuStatus : UInt32
        {
            Running = 0,  /**< Game is running */
            Paused = 1,   /**< Game is paused */
            Shutdown = 2, /**< Game is shutdown */
        }

        public static Int16 ReadInt16(IntPtr v, UInt32 address)
        {
            byte msg = (byte)IPCCommand.MsgRead16;
            byte[] bs = BitConverter.GetBytes((UInt16)(pine_read(v, address, msg, false) & 0xFFFF));
            return BitConverter.ToInt16(bs, 0);
        }

        public static Int32 ReadInt32(IntPtr v, UInt32 address)
        {
            byte msg = (byte)IPCCommand.MsgRead32;
            byte[] bs = BitConverter.GetBytes((UInt32)(pine_read(v, address, msg, false) & 0xFFFFFFFF));
            return BitConverter.ToInt32(bs, 0);
        }

        public static Int64 ReadInt64(IntPtr v, UInt32 address)
        {
            byte msg = (byte)IPCCommand.MsgRead64;
            byte[] bs = BitConverter.GetBytes(pine_read(v, address, msg, false));
            return BitConverter.ToInt64(bs, 0);
        }

        public static float ReadSingle(IntPtr v, UInt32 address)
        {
            byte msg = (byte)IPCCommand.MsgRead32;
            byte[] bs = BitConverter.GetBytes((UInt32)(pine_read(v, address, msg, false) & 0xFFFFFFFF));
            return BitConverter.ToSingle(bs, 0);
        }

        public static double ReadDouble(IntPtr v, UInt32 address)
        {
            byte msg = (byte)IPCCommand.MsgRead64;
            byte[] bs = BitConverter.GetBytes(pine_read(v, address, msg, false));
            return BitConverter.ToDouble(bs, 0);
        }

        public static string ReadUnmanagedString(IntPtr ptr)
        {
            return Marshal.PtrToStringAnsi(ptr);
        }

        // if the function you want to use isn't there you'll have to define the binding to the function
        // rule of thumb: pointers = IntPtr, enum = their underlying type(by default it's int in C but 
        // I should have properly defined their type explicitely).
        [DllImport(libipc)]
        public static extern IntPtr pine_pcsx2_new();

        [DllImport(libipc)]
        public static extern IntPtr pine_rpcs3_new();

        [DllImport(libipc)]
        public static extern IntPtr pine_duckstation_new();

        [DllImport(libipc)]
        public static extern UInt64 pine_read(IntPtr v, UInt32 address, Byte msg, bool batch);

        [DllImport(libipc)]
        public static extern int pine_status(IntPtr v, bool batch);

        [DllImport(libipc)]
        public static extern int pine_get_error(IntPtr v, bool batch);

        [DllImport(libipc)]
        public static extern IntPtr pine_getgametitle(IntPtr v, bool batch);

        [DllImport(libipc)]
        public static extern IntPtr pine_getgameid(IntPtr v, bool batch);

        [DllImport(libipc)]
        public static extern IntPtr pine_getgameuuid(IntPtr v, bool batch);

        [DllImport(libipc)]
        public static extern IntPtr pine_getgameversion(IntPtr v, bool batch);

        [DllImport(libipc)]
        public static extern void pine_pcsx2_delete(IntPtr v);

        [DllImport(libipc)]
        public static extern void pine_rpcs3_delete(IntPtr v);

        [DllImport(libipc)]
        public static extern void pine_duckstation_delete(IntPtr v);

        [DllImport(libipc)]
        public static extern UInt32 pine_get_error(IntPtr v);

        /*
        static void Main(string[] args)
        {
            // we get our ipc object
            IntPtr ipc = pine_pcsx2_new();

            // we read an uint8_t from memory location 0x00347D34
            Console.WriteLine(pine_read(ipc, 0x00347D34, 0, false));

            // we check for errors
            Console.WriteLine("Error (if any): {0}", pine_get_error(ipc));

            // we delete the object and free the resources
            pine_pcsx2_delete(ipc);

            // for more infos check out the C bindings documentation :D !
        }
        */
    }
}