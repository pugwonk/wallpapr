﻿using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Media;
using Avalon.Internal.Utility;
using Microsoft.Win32;

namespace Avalon.Internal.Win32
{
    internal static class NativeMethods
    {
        #region Kernel

        [SecurityCritical, SuppressUnmanagedCodeSecurity, DllImport("kernel32", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern void SetLastError(int dwErrorCode);

        #endregion

        #region User

        #region Enums

        public enum WindowLongValue
        {
            WndProc = -4,
            HInstace = -6,
            HwndParent = -8,
            Style = -16,
            ExtendedStyle = -20,
            UserData = -21,
            ID = -12,
        }

        [Flags]
        public enum WindowStyles
        {
            SysMemu = 0x80000,
            MinimizeBox = 0x20000,
            MaximizeBox = 0x10000,
            ThickFrame = 0x40000,
        }

        [Flags]
        public enum WindowExStyles
        {
            DlgModalFrame = 0x1,
        }

        public enum WindowMessage
        {
            Destroy = 0x2,
            Close = 0x10,
            SetIcon = 0x80,
            MeasureItem = 0x2c,
            MouseMove = 0x200,
            MouseDown = 0x201,
            LButtonUp = 0x0202,
            LButtonDblClk = 0x0203,
            RButtonDown = 0x0204,
            RButtonUp = 0x0205,
            RButtonDblClk = 0x0206,
            MButtonDown = 0x0207,
            MButtonUp = 0x0208,
            MButtonDblClk = 0x0209,
            TrayMouseMessage = 0x800,
        }

        public enum NotifyIconMessage
        {
            BalloonShow = 0x402,
            BalloonHide = 0x403,
            BalloonTimeout = 0x404,
            BalloonUserClick = 0x405,
            PopupOpen = 0x406,
            PopupClose = 0x407,
        }

        public enum SystemMenu
        {
            Size = 0xF000,
            Close = 0xF060,
            Restore = 0xF120,
            Minimize = 0xF020,
            Maximize = 0xF030,
        }

        #endregion

        [SecurityCritical, SuppressUnmanagedCodeSecurity, DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(HandleRef hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

        [SecurityCritical, SuppressUnmanagedCodeSecurity, DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(HandleRef hwnd, WindowMessage msg, IntPtr wparam, IntPtr lparam);

        [SecurityCritical, SuppressUnmanagedCodeSecurity, DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(HandleRef hWnd);

        [SecurityCritical, SuppressUnmanagedCodeSecurity, DllImport("user32", CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string msg);

        [SecurityCritical, SuppressUnmanagedCodeSecurity, DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [SecurityCritical, SuppressUnmanagedCodeSecurity, DllImport("user32", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int IntGetWindowLong(HandleRef hWnd, int nIndex);

        [SecurityCritical, SuppressUnmanagedCodeSecurity, DllImport("user32", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr IntGetWindowLongPtr(HandleRef hWnd, int nIndex);

        [DllImport("user32", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int IntSetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        [SecurityCritical]
        public static int GetWindowLong(HandleRef hWnd, WindowLongValue nIndex)
        {
            int result = 0;
            int error = 0;
            SetLastError(0);
            if (IntPtr.Size == 4)
            {
                result = IntGetWindowLong(hWnd, (int)nIndex);
                error = Marshal.GetLastWin32Error();
            }
            else
            {
                IntPtr resultPtr = IntGetWindowLongPtr(hWnd, (int)nIndex);
                error = Marshal.GetLastWin32Error();
                result = IntPtrToInt32(resultPtr);
            }
            return result;
        }

        [SecurityCritical, SecurityTreatAsSafe]
        public static IntPtr SetWindowLong(HandleRef hWnd, WindowLongValue nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            SetLastError(0);
            if (IntPtr.Size == 4)
            {
                int intResult = IntSetWindowLong(hWnd, (int)nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(intResult);
            }
            else
            {
                result = IntSetWindowLongPtr(hWnd, (int)nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }
            return result;
        }

        [SecurityCritical, DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool EnableMenuItem(HandleRef hMenu, SystemMenu UIDEnabledItem, int uEnable);

        [SecurityCritical, DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetSystemMenu(HandleRef hWnd, bool bRevert);

        [SecurityCritical]
        public static void SetSystemMenuItems(HandleRef hwnd, bool isEnabled, params SystemMenu[] menus)
        {
            if (menus != null && menus.Length > 0)
            {
                HandleRef hMenu = new HandleRef(null, GetSystemMenu(hwnd, false));

                foreach (SystemMenu menu in menus)
                {
                    SetMenuItem(hMenu, menu, isEnabled);
                }
            }
        }

        [SecurityCritical]
        public static void SetMenuItem(HandleRef hMenu, SystemMenu menu, bool isEnabled)
        {
            EnableMenuItem(hMenu, menu, (isEnabled) ? ~1 : 1);
        }

        #endregion

        #region Shell

        #region Structures and Enums

        [StructLayout(LayoutKind.Sequential)]
        public struct BROWSEINFO
        {
            /// <summary>
            /// Handle to the owner window for the dialog box.
            /// </summary>
            public IntPtr HwndOwner;

            /// <summary>
            /// Pointer to an item identifier list (PIDL) specifying the 
            /// location of the root folder from which to start browsing.
            /// </summary>
            public IntPtr Root;

            /// <summary>
            /// Address of a buffer to receive the display name of the
            /// folder selected by the user.
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            public string DisplayName;

            /// <summary>
            /// Address of a null-terminated string that is displayed 
            /// above the tree view control in the dialog box.
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            public string Title;

            /// <summary>
            /// Flags specifying the options for the dialog box.
            /// </summary>
            public uint Flags;

            /// <summary>
            /// Address of an application-defined function that the
            /// dialog box calls when an event occurs.
            /// </summary>
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public WndProc Callback;

            /// <summary>
            /// Application-defined value that the dialog box passes to 
            /// the callback function
            /// </summary>
            public int LParam;

            /// <summary>
            /// Variable to receive the image associated with the selected folder.
            /// </summary>
            public int Image;
        }

        [Flags]
        public enum FolderBrowserOptions
        {
            /// <summary>
            /// None.
            /// </summary>
            None = 0,
            /// <summary>
            /// For finding a folder to start document searching
            /// </summary>
            FolderOnly = 0x0001,
            /// <summary>
            /// For starting the Find Computer
            /// </summary>
            FindComputer = 0x0002,
            /// <summary>
            /// Top of the dialog has 2 lines of text for BROWSEINFO.lpszTitle and 
            /// one line if this flag is set.  Passing the message 
            /// BFFM_SETSTATUSTEXTA to the hwnd can set the rest of the text.  
            /// This is not used with BIF_USENEWUI and BROWSEINFO.lpszTitle gets
            /// all three lines of text.
            /// </summary>
            ShowStatusText = 0x0004,
            ReturnAncestors = 0x0008,
            /// <summary>
            /// Add an editbox to the dialog
            /// </summary>
            ShowEditBox = 0x0010,
            /// <summary>
            /// insist on valid result (or CANCEL)
            /// </summary>
            ValidateResult = 0x0020,
            /// <summary>
            /// Use the new dialog layout with the ability to resize
            /// Caller needs to call OleInitialize() before using this API
            /// </summary>
            UseNewStyle = 0x0040,
            UseNewStyleWithEditBox = (UseNewStyle | ShowEditBox),
            /// <summary>
            /// Allow URLs to be displayed or entered. (Requires BIF_USENEWUI)
            /// </summary>
            AllowUrls = 0x0080,
            /// <summary>
            /// Add a UA hint to the dialog, in place of the edit box. May not be
            /// combined with BIF_EDITBOX.
            /// </summary>
            ShowUsageHint = 0x0100,
            /// <summary>
            /// Do not add the "New Folder" button to the dialog.  Only applicable 
            /// with BIF_NEWDIALOGSTYLE.
            /// </summary>
            HideNewFolderButton = 0x0200,
            /// <summary>
            /// don't traverse target as shortcut
            /// </summary>
            GetShortcuts = 0x0400,
            /// <summary>
            /// Browsing for Computers.
            /// </summary>
            BrowseComputers = 0x1000,
            /// <summary>
            /// Browsing for Printers.
            /// </summary>
            BrowsePrinters = 0x2000,
            /// <summary>
            /// Browsing for Everything
            /// </summary>
            BrowseFiles = 0x4000,
            /// <summary>
            /// sharable resources displayed (remote shares, requires BIF_USENEWUI)
            /// </summary>
            BrowseShares = 0x8000
        }

        #endregion

        #region Notify Icon

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class NOTIFYICONDATA
        {
            public int cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));
            public IntPtr hWnd;
            public int uID;
            public NotifyIconFlags uFlags;
            public int uCallbackMessage;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)]
            public string szTip;
            public int dwState;
            public int dwStateMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
            public string szInfo;
            public int uTimeoutOrVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x40)]
            public string szInfoTitle;
            public int dwInfoFlags;
        }

        [Flags]
        public enum NotifyIconFlags
        {
            /// <summary>
            /// The hIcon member is valid.
            /// </summary>
            Icon = 2,
            /// <summary>
            /// The uCallbackMessage member is valid.
            /// </summary>
            Message = 1,
            /// <summary>
            /// The szTip member is valid.
            /// </summary>
            ToolTip = 4,
            /// <summary>
            /// The dwState and dwStateMask members are valid.
            /// </summary>
            State = 8,
            /// <summary>
            /// Use a balloon ToolTip instead of a standard ToolTip. The szInfo, uTimeout, szInfoTitle, and dwInfoFlags members are valid.
            /// </summary>
            Balloon = 0x10,
        }

        [SecurityCritical, DllImport("shell32", CharSet = CharSet.Auto)]
        public static extern int Shell_NotifyIcon(int message, NOTIFYICONDATA pnid);

        #endregion

        #region Malloc

        [ComImport, SuppressUnmanagedCodeSecurity, Guid("00000002-0000-0000-c000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IMalloc
        {
            [PreserveSig]
            IntPtr Alloc(int cb);
            [PreserveSig]
            IntPtr Realloc(IntPtr pv, int cb);
            [PreserveSig]
            void Free(IntPtr pv);
            [PreserveSig]
            int GetSize(IntPtr pv);
            [PreserveSig]
            int DidAlloc(IntPtr pv);
            [PreserveSig]
            void HeapMinimize();
        }

        [SecurityCritical]
        public static IMalloc GetSHMalloc()
        {
            IMalloc[] ppMalloc = new IMalloc[1];
            SHGetMalloc(ppMalloc);
            return ppMalloc[0];
        }

        [SecurityCritical, DllImport("shell32")]
        private static extern int SHGetMalloc([Out, MarshalAs(UnmanagedType.LPArray)] IMalloc[] ppMalloc);

        #endregion

        #region Folders

        [SecurityCritical, DllImport("shell32")]
        public static extern int SHGetFolderLocation(IntPtr hwndOwner, Int32 nFolder, IntPtr hToken, uint dwReserved, out IntPtr ppidl);

        [SecurityCritical, DllImport("shell32")]
        public static extern int SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)]string pszName, IntPtr pbc, out IntPtr ppidl, uint sfgaoIn, out uint psfgaoOut);

        [SecurityCritical, DllImport("shell32")]
        public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lbpi);

        [SecurityCritical, DllImport("shell32", CharSet = CharSet.Auto)]
        public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

        #endregion

        #endregion

        #region Winmm

        #region Enums

        [Flags]
        private enum PlaySoundFlags
        {
            SND_ALIAS = 0x10000,
            SND_APPLICATION = 0x80,
            SND_ASYNC = 1,
            SND_FILENAME = 0x20000,
            SND_LOOP = 8,
            SND_MEMORY = 4,
            SND_NODEFAULT = 2,
            SND_NOSTOP = 0x10,
            SND_NOWAIT = 0x2000,
            SND_PURGE = 0x40,
            SND_RESOURCE = 0x40000,
            SND_SYNC = 0,
        }

        #endregion

        [SecurityCritical]
        private static string GetSystemSound(string soundName)
        {
            string path = null;
            string name = InvariantString.Format(@"AppEvents\Schemes\Apps\.Default\{0}\.current\", soundName);
            PermissionSet set = new PermissionSet(null);
            set.AddPermission(new RegistryPermission(RegistryPermissionAccess.Read, @"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\"));
            set.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
            set.Assert();
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(name))
                {
                    if (key != null)
                    {
                        path = (string)key.GetValue("");
                    }
                    return path;
                }
            }
            finally
            {
                CodeAccessPermission.RevertAssert();
            }
        }

        [SecurityCritical, SuppressUnmanagedCodeSecurity, DllImport("winmm", CharSet = CharSet.Unicode)]
        private static extern bool PlaySound(string soundName, IntPtr hmod, PlaySoundFlags soundFlags);

        [SecurityCritical, SecurityTreatAsSafe]
        public static void PlaySound(string soundName)
        {
            string systemSound = GetSystemSound(soundName);
            if (!string.IsNullOrEmpty(systemSound))
            {
                PlaySound(systemSound, IntPtr.Zero, PlaySoundFlags.SND_FILENAME | PlaySoundFlags.SND_NOSTOP | PlaySoundFlags.SND_NODEFAULT | PlaySoundFlags.SND_ASYNC);
            }
        }

        #endregion

        #region Helpers

        public static int IntPtrToInt32(IntPtr intPtr)
        {
            return (int)intPtr.ToInt64();
        }

        public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [SecurityCritical]
        public static IntPtr GetHIcon(ImageSource source)
        {
            System.Windows.Media.Imaging.BitmapFrame frame = source as System.Windows.Media.Imaging.BitmapFrame;

            if (frame != null && frame.Decoder.Frames.Count > 0)
            {
                frame = frame.Decoder.Frames[0];

                int width = frame.PixelWidth;
                int height = frame.PixelHeight;
                int stride = width * ((frame.Format.BitsPerPixel + 7) / 8);

                byte[] bits = new byte[height * stride];

                frame.CopyPixels(bits, stride, 0);

                // pin the bytes in memory (avoids using unsafe context)
                GCHandle gcHandle = GCHandle.Alloc(bits, GCHandleType.Pinned);

                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(
                    width,
                    height,
                    stride,
                    System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                    gcHandle.AddrOfPinnedObject());

                IntPtr hIcon = bitmap.GetHicon();

                gcHandle.Free();

                return hIcon;
            }

            return IntPtr.Zero;
        }

        #endregion
    }
}
