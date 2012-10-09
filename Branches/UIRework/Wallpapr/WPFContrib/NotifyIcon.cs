using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using Avalon.Internal.Utility;
using Avalon.Internal.Win32;

namespace Avalon.Windows.Controls
{
    /// <summary>
    /// Specifies a component that creates an icon in the notification area.
    /// <remarks>
    /// When used in a visual tree, the <see cref="P:IsVisible"/> property will trigger the icon.
    /// If you want to override this behavior (e.g. show the icon even when it's not part of a visual tree),
    /// set the <see cref="P:Visibility"/> to <see cref="Visibility.Hidden"/>.
    /// To hide the icon in any state, set the <see cref="P:Visibility"/> to <see cref="Visibility.Collapsed"/>.
    /// </remarks>
    /// </summary>
    [ContentProperty("Text"), DefaultEvent("MouseDoubleClick"),
     System.Security.Permissions.UIPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Window = System.Security.Permissions.UIPermissionWindow.AllWindows)]
    public sealed class NotifyIcon : FrameworkElement, IDisposable, IAddChild
    {
        #region Fields

        private static readonly int TaskbarCreatedWindowMessage;

        private static readonly System.Security.Permissions.UIPermission _allWindowsPermission = new System.Security.Permissions.UIPermission(System.Security.Permissions.UIPermissionWindow.AllWindows);
        private static int _nextId;

        private readonly object _syncObj = new object();

        private NotifyIconHwndSource _hwndSource;
        private int _id = _nextId++;
        private bool _iconCreated;
        private bool _doubleClick;

        #endregion

        #region Events

        /// <summary>
        /// Identifies the <see cref="BalloonTipClick"/> routed event.
        /// </summary>
        public static readonly RoutedEvent BalloonTipClickEvent = EventManager.RegisterRoutedEvent("BalloonTipClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NotifyIcon));

        /// <summary>
        /// Occurs when the balloon tip is clicked.
        /// </summary>
        public event RoutedEventHandler BalloonTipClick
        {
            add { AddHandler(BalloonTipClickEvent, value); }
            remove { RemoveHandler(BalloonTipClickEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BalloonTipClosed"/> routed event.
        /// </summary>
        public static readonly RoutedEvent BalloonTipClosedEvent = EventManager.RegisterRoutedEvent("BalloonTipClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NotifyIcon));

        /// <summary>
        /// Occurs when the balloon tip is closed by the user.
        /// </summary>
        public event RoutedEventHandler BalloonTipClosed
        {
            add { AddHandler(BalloonTipClosedEvent, value); }
            remove { RemoveHandler(BalloonTipClosedEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BalloonTipShown"/> routed event.
        /// </summary>
        public static readonly RoutedEvent BalloonTipShownEvent = EventManager.RegisterRoutedEvent("BalloonTipShown", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NotifyIcon));

        /// <summary>
        /// Occurs when the balloon tip is displayed on the screen.
        /// </summary>
        public event RoutedEventHandler BalloonTipShown
        {
            add { AddHandler(BalloonTipShownEvent, value); }
            remove { RemoveHandler(BalloonTipShownEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Click"/> routed event.
        /// </summary>
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NotifyIcon));

        /// <summary>
        /// Occurs when the user clicks the icon in the notification area.
        /// </summary>
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DoubleClick"/> routed event.
        /// </summary>
        public static readonly RoutedEvent DoubleClickEvent = EventManager.RegisterRoutedEvent("DoubleClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NotifyIcon));

        /// <summary>
        /// Occurs when the user double-clicks the icon in the notification area of the taskbar.
        /// </summary>
        public event RoutedEventHandler DoubleClick
        {
            add { AddHandler(DoubleClickEvent, value); }
            remove { RemoveHandler(DoubleClickEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="MouseClick"/> routed event.
        /// </summary>
        public static readonly RoutedEvent MouseClickEvent = EventManager.RegisterRoutedEvent("MouseClick", RoutingStrategy.Bubble, typeof(MouseButtonEventHandler), typeof(NotifyIcon));

        /// <summary>
        /// Occurs when the user clicks a <see cref="NotifyIcon"/> with the mouse.
        /// </summary>
        public event MouseButtonEventHandler MouseClick
        {
            add { AddHandler(MouseClickEvent, value); }
            remove { RemoveHandler(MouseClickEvent, value); }
        }

        /// <summary>
        /// Identifies the <see cref="MouseDoubleClick"/> routed event.
        /// </summary>
        public static readonly RoutedEvent MouseDoubleClickEvent = EventManager.RegisterRoutedEvent("MouseDoubleClick", RoutingStrategy.Bubble, typeof(MouseButtonEventHandler), typeof(NotifyIcon));

        /// <summary>
        /// Occurs when the user double-clicks the <see cref="NotifyIcon"/> with the mouse.
        /// </summary>
        public event MouseButtonEventHandler MouseDoubleClick
        {
            add { AddHandler(MouseDoubleClickEvent, value); }
            remove { RemoveHandler(MouseDoubleClickEvent, value); }
        }

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes the <see cref="NotifyIcon"/> class.
        /// </summary>
        [SecurityCritical]
        static NotifyIcon()
        {
            TaskbarCreatedWindowMessage = NativeMethods.RegisterWindowMessage("TaskbarCreated");

            VisibilityProperty.OverrideMetadata(typeof(NotifyIcon), new FrameworkPropertyMetadata(OnVisibilityChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyIcon"/> class.
        /// </summary>
        [SecurityCritical]
        public NotifyIcon()
        {
            UpdateIconForVisibility();

            IsVisibleChanged += OnIsVisibleChanged;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="NotifyIcon"/> is reclaimed by garbage collection.
        /// </summary>
        [SecurityCritical]
        ~NotifyIcon()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [SecurityCritical]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SecurityCritical]
        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_hwndSource != null)
                {
                    UpdateIcon(false);
                    _hwndSource.Dispose();
                }
            }
            else if (_hwndSource != null)
            {
                NativeMethods.PostMessage(new HandleRef(_hwndSource, _hwndSource.Handle), NativeMethods.WindowMessage.Close, IntPtr.Zero, IntPtr.Zero);
                _hwndSource.Dispose();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Displays a balloon tip in the taskbar for the specified time period.
        /// </summary>
        /// <param name="timeout">The time period, in milliseconds, the balloon tip should display.</param>
        [SecurityCritical]
        public void ShowBalloonTip(int timeout)
        {
            ShowBalloonTip(timeout, BalloonTipTitle, BalloonTipText, BalloonTipIcon);
        }

        /// <summary>
        /// Displays a balloon tip with the specified title, text, and icon in the taskbar for the specified time period.
        /// </summary>
        /// <param name="timeout">The time period, in milliseconds, the balloon tip should display.</param>
        /// <param name="tipTitle">The title to display on the balloon tip.</param>
        /// <param name="tipText">The text to display on the balloon tip.</param>
        /// <param name="tipIcon">One of the <see cref="NotifyBalloonIcon"/> values.</param>
        [SecurityCritical]
        public void ShowBalloonTip(int timeout, string tipTitle, string tipText, NotifyBalloonIcon tipIcon)
        {
            if (timeout < 0)
            {
                throw new ArgumentOutOfRangeException("timeout", timeout, "Timeout cannot be negative.");
            }
            ArgumentValidator.NotNullOrEmptyString(tipText, "tipText");
            ArgumentValidator.EnumValueIsDefined(typeof(NotifyBalloonIcon), tipIcon, "tipIcon");

            if (_iconCreated)
            {
                _allWindowsPermission.Demand();

                NativeMethods.NOTIFYICONDATA pnid = new NativeMethods.NOTIFYICONDATA
                {
                    hWnd = _hwndSource.Handle,
                    uID = _id,
                    uFlags = NativeMethods.NotifyIconFlags.Balloon,
                    uTimeoutOrVersion = timeout,
                    szInfoTitle = tipTitle,
                    szInfo = tipText,
                    dwInfoFlags = (int)tipIcon
                };
                NativeMethods.Shell_NotifyIcon(1, pnid);
            }
        }

        #endregion

        #region Private Methods

        [SecurityCritical]
        private void ShowContextMenu()
        {
            if (ContextMenu != null)
            {
                NativeMethods.SetForegroundWindow(new HandleRef(_hwndSource, _hwndSource.Handle));

                ContextMenuService.SetPlacement(ContextMenu, PlacementMode.MousePoint);
                ContextMenu.IsOpen = true;
            }
        }

        /// <summary>
        /// Shows or hides the icon according to the <see cref="P:IsVisible"/> and <see cref="P:Visibility"/> properties.
        /// </summary>
        [SecurityCritical]
        private void UpdateIconForVisibility()
        {
            UpdateIcon((IsVisible && Visibility == Visibility.Visible) || Visibility == Visibility.Hidden);
        }

        [SecurityCritical]
        private void UpdateIcon(bool showIconInTray)
        {
            lock (_syncObj)
            {
                if (!DesignerProperties.GetIsInDesignMode(this))
                {
                    IntPtr iconHandle = IntPtr.Zero;

                    try
                    {
                        _allWindowsPermission.Demand();

                        if (showIconInTray && _hwndSource == null)
                        {
                            _hwndSource = new NotifyIconHwndSource(this);
                        }

                        if (_hwndSource != null)
                        {
                            _hwndSource.LockReference(showIconInTray);

                            NativeMethods.NOTIFYICONDATA pnid = new NativeMethods.NOTIFYICONDATA
                            {
                                uCallbackMessage = (int)NativeMethods.WindowMessage.TrayMouseMessage,
                                uFlags = NativeMethods.NotifyIconFlags.Message | NativeMethods.NotifyIconFlags.ToolTip,
                                hWnd = _hwndSource.Handle,
                                uID = _id,
                                szTip = Text
                            };
                            if (Icon != null)
                            {
                                iconHandle = NativeMethods.GetHIcon(Icon);

                                pnid.uFlags |= NativeMethods.NotifyIconFlags.Icon;
                                pnid.hIcon = iconHandle;
                            }

                            if (showIconInTray && iconHandle != null)
                            {
                                if (!_iconCreated)
                                {
                                    NativeMethods.Shell_NotifyIcon(0, pnid);
                                    _iconCreated = true;
                                }
                                else
                                {
                                    NativeMethods.Shell_NotifyIcon(1, pnid);
                                }
                            }
                            else if (_iconCreated)
                            {
                                NativeMethods.Shell_NotifyIcon(2, pnid);
                                _iconCreated = false;
                            }
                        }
                    }
                    finally
                    {
                        if (iconHandle != IntPtr.Zero)
                        {
                            NativeMethods.DestroyIcon(iconHandle);
                        }
                    }
                }
            }
        }

        [SecurityCritical]
        private static void OnVisibilityChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((NotifyIcon)o).UpdateIconForVisibility();
        }

        [SecurityCritical]
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateIconForVisibility();
        }

        #endregion

        #region WndProc Methods

        private void WmMouseDown(MouseButton button, int clicks)
        {
            MouseButtonEventArgs args = null;

            if (clicks == 2)
            {
                RaiseEvent(new RoutedEventArgs(DoubleClickEvent));

                args = new MouseButtonEventArgs(InputManager.Current.PrimaryMouseDevice, 0, button);
                args.RoutedEvent = MouseDoubleClickEvent;
                RaiseEvent(args);

                _doubleClick = true;
            }

            args = new MouseButtonEventArgs(InputManager.Current.PrimaryMouseDevice, 0, button);
            args.RoutedEvent = MouseDownEvent;
            RaiseEvent(args);
        }

        private void WmMouseMove()
        {
            MouseEventArgs args = new MouseEventArgs(InputManager.Current.PrimaryMouseDevice, 0);
            args.RoutedEvent = MouseMoveEvent;
            RaiseEvent(args);
        }

        private void WmMouseUp(MouseButton button)
        {
            MouseButtonEventArgs args = new MouseButtonEventArgs(InputManager.Current.PrimaryMouseDevice, 0, button);
            args.RoutedEvent = MouseUpEvent;
            RaiseEvent(args);

            if (!_doubleClick)
            {
                RaiseEvent(new RoutedEventArgs(ClickEvent));

                args = new MouseButtonEventArgs(InputManager.Current.PrimaryMouseDevice, 0, button);
                args.RoutedEvent = MouseClickEvent;
                RaiseEvent(args);
            }

            _doubleClick = false;
        }

        [SecurityCritical]
        private void WmTaskbarCreated()
        {
            _iconCreated = false;
            UpdateIcon(IsVisible);
        }

        [SecurityCritical]
        private void WndProc(int message, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            handled = true;

            if (message <= (int)NativeMethods.WindowMessage.MeasureItem)
            {
                if (message == (int)NativeMethods.WindowMessage.Destroy)
                {
                    UpdateIcon(false);
                    return;
                }
            }
            else
            {
                if (message != (int)NativeMethods.WindowMessage.TrayMouseMessage)
                {
                    if (message == TaskbarCreatedWindowMessage)
                    {
                        WmTaskbarCreated();
                    }
                    handled = false;
                    return;
                }
                switch ((NativeMethods.WindowMessage)lParam)
                {
                    case NativeMethods.WindowMessage.MouseMove:
                        WmMouseMove();
                        return;
                    case NativeMethods.WindowMessage.MouseDown:
                        WmMouseDown(MouseButton.Left, 1);
                        return;
                    case NativeMethods.WindowMessage.LButtonUp:
                        WmMouseUp(MouseButton.Left);
                        return;
                    case NativeMethods.WindowMessage.LButtonDblClk:
                        WmMouseDown(MouseButton.Left, 2);
                        return;
                    case NativeMethods.WindowMessage.RButtonDown:
                        WmMouseDown(MouseButton.Right, 1);
                        return;
                    case NativeMethods.WindowMessage.RButtonUp:
                        ShowContextMenu();
                        WmMouseUp(MouseButton.Right);
                        return;
                    case NativeMethods.WindowMessage.RButtonDblClk:
                        WmMouseDown(MouseButton.Right, 2);
                        return;
                    case NativeMethods.WindowMessage.MButtonDown:
                        WmMouseDown(MouseButton.Middle, 1);
                        return;
                    case NativeMethods.WindowMessage.MButtonUp:
                        WmMouseUp(MouseButton.Middle);
                        return;
                    case NativeMethods.WindowMessage.MButtonDblClk:
                        WmMouseDown(MouseButton.Middle, 2);
                        return;
                }
                switch ((NativeMethods.NotifyIconMessage)lParam)
                {
                    case NativeMethods.NotifyIconMessage.BalloonShow:
                        RaiseEvent(new RoutedEventArgs(BalloonTipShownEvent));
                        return;
                    case NativeMethods.NotifyIconMessage.BalloonHide:
                    case NativeMethods.NotifyIconMessage.BalloonTimeout:
                        RaiseEvent(new RoutedEventArgs(BalloonTipClosedEvent));
                        return;
                    case NativeMethods.NotifyIconMessage.BalloonUserClick:
                        RaiseEvent(new RoutedEventArgs(BalloonTipClickEvent));
                        return;
                }
                return;
            }
            if (message == TaskbarCreatedWindowMessage)
            {
                WmTaskbarCreated();
            }
            handled = false;
        }

        [SecurityCritical]
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            WndProc(msg, wParam, lParam, out handled);

            return IntPtr.Zero;
        }

        #endregion

        #region Properties

        #region BalloonTipIcon

        /// <summary>
        /// Gets or sets the icon to display on the balloon tip.
        /// </summary>
        /// <value>The balloon tip icon.</value>
        public NotifyBalloonIcon BalloonTipIcon
        {
            get { return (NotifyBalloonIcon)GetValue(BalloonTipIconProperty); }
            set { SetValue(BalloonTipIconProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BalloonTipIcon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BalloonTipIconProperty =
            DependencyProperty.Register("BalloonTipIcon", typeof(NotifyBalloonIcon), typeof(NotifyIcon), new FrameworkPropertyMetadata(NotifyBalloonIcon.None));

        #endregion

        #region BalloonTipText

        /// <summary>
        /// Gets or sets the text to display on the balloon tip.
        /// </summary>
        /// <value>The balloon tip text.</value>
        public string BalloonTipText
        {
            get { return (string)GetValue(BalloonTipTextProperty); }
            set { SetValue(BalloonTipTextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BalloonTipText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BalloonTipTextProperty =
            DependencyProperty.Register("BalloonTipText", typeof(string), typeof(NotifyIcon), new FrameworkPropertyMetadata());

        #endregion

        #region BalloonTipTitle

        /// <summary>
        /// Gets or sets the title of the balloon tip.
        /// </summary>
        /// <value>The balloon tip title.</value>
        public string BalloonTipTitle
        {
            get { return (string)GetValue(BalloonTipTitleProperty); }
            set { SetValue(BalloonTipTitleProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BalloonTipTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BalloonTipTitleProperty =
            DependencyProperty.Register("BalloonTipTitle", typeof(string), typeof(NotifyIcon), new FrameworkPropertyMetadata());

        #endregion

        #region Text

        /// <summary>
        /// Gets or sets the tooltip text displayed when the mouse pointer rests on a notification area icon.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NotifyIcon), new FrameworkPropertyMetadata(OnTextPropertyChanged, OnCoerceTextProperty), ValidateTextPropety);

        private static bool ValidateTextPropety(object baseValue)
        {
            string value = (string)baseValue;

            return value == null || value.Length <= 0x3f;
        }

        [SecurityCritical]
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NotifyIcon notifyIcon = (NotifyIcon)d;

            if (notifyIcon._iconCreated)
            {
                notifyIcon.UpdateIcon(true);
            }
        }

        private static object OnCoerceTextProperty(DependencyObject d, object baseValue)
        {
            string value = (string)baseValue;

            if (value == null)
            {
                value = string.Empty;
            }

            return value;
        }

        #endregion

        #region Icon

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Icon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            Window.IconProperty.AddOwner(typeof(NotifyIcon), new FrameworkPropertyMetadata(OnNotifyIconChanged) { Inherits = true });

        [SecurityCritical]
        private static void OnNotifyIconChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            NotifyIcon notifyIcon = (NotifyIcon)o;

            notifyIcon.UpdateIcon(notifyIcon.IsVisible);
        }

        #endregion

        #endregion

        #region IAddChild Members

        void IAddChild.AddChild(object value)
        {
            throw new InvalidOperationException(SR.IAddChild_TextOnly);
        }

        void IAddChild.AddText(string text)
        {
            Text = text;
        }

        #endregion

        #region NotifyIconNativeWindow Class

        private class NotifyIconHwndSource : HwndSource
        {
            private NotifyIcon _reference;
            private GCHandle _rootRef;

            [SecurityCritical]
            internal NotifyIconHwndSource(NotifyIcon component)
                : base(0, 0, 0, 0, 0, null, IntPtr.Zero)
            {
                _reference = component;

                AddHook(_reference.WndProc);
            }

            [SecurityCritical]
            ~NotifyIconHwndSource()
            {
                if (Handle != IntPtr.Zero)
                {
                    NativeMethods.PostMessage(new HandleRef(this, Handle), NativeMethods.WindowMessage.Close, IntPtr.Zero, IntPtr.Zero);
                }
            }

            public void LockReference(bool locked)
            {
                if (locked)
                {
                    if (!_rootRef.IsAllocated)
                    {
                        _rootRef = GCHandle.Alloc(_reference, GCHandleType.Normal);
                    }
                }
                else if (_rootRef.IsAllocated)
                {
                    _rootRef.Free();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Defines a set of standardized icons that can be associated with a balloon tip.
    /// </summary>
    public enum NotifyBalloonIcon
    {
        /// <summary>
        /// No icon.
        /// </summary>
        None,
        /// <summary>
        /// An information icon.
        /// </summary>
        Info,
        /// <summary>
        /// A warning icon.
        /// </summary>
        Warning,
        /// <summary>
        /// An error icon.
        /// </summary>
        Error,
    }

}
