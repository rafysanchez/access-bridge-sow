﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JABConsole
{
    ublic static class WinAPI
    {
        // Messagae pump with C# and p/invke  
        // please check out   
        //   http://msdn.microsoft.com/en-us/library/windows/desktop/ms644928(v=vs.85).aspx  
        //  
        // General Reference Page:  
        //    pinvoke.net : http://www.pinvoke.net  
        //    Howto: Marshal Structures Using PInvoke : http://msdn.microsoft.com/en-us/library/ef4c3t39(v=vs.80).aspx  
        //    using P/Invoke to call Unmanaged APIs from your Managed Classes: http://msdn.microsoft.com/en-us/library/aa719104(v=vs.10).aspx  
        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref MSG lpMsg);
        [DllImport("user32.dll")]
        public static extern sbyte GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
           uint wMsgFilterMax);

        // To use CreateWindow, just pass 0 as the first argument.  
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "CreateWindowEx")]
        public static extern IntPtr CreateWindowEx(
           WindowStylesEx dwExStyle,
           string lpClassName,
           string lpWindowName,
           WindowStyles dwStyle,
           int x,
           int y,
           int nWidth,
           int nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam);

        // Create a window, but accept a atom value.  
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "CreateWindowEx")]
        public static extern IntPtr CreateWindowEx2(
           WindowStylesEx dwExStyle,
           UInt16 lpClassName,
           string lpWindowName,
           WindowStyles dwStyle,
           int x,
           int y,
           int nWidth,
           int nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam);

        [DllImport("user32.dll")]
        public static extern ushort RegisterClass([In] ref WNDCLASS lpWndClass);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, WM uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern int DrawText(IntPtr hDC, string lpString, int nCount, ref RECT lpRect, uint uFormat);

        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern void PostQuitMessage(int nExitCode);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(IntPtr hInstance, string lpIconName);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIConName);

        [DllImport("gdi32.dll")]
        public static extern IntPtr GetStockObject(StockObjects fnObject);

        [DllImport("user32.dll")]
        public static extern MessageBoxResult MessageBox(IntPtr hWnd, string text, string caption, int options);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport("user32.dll")]
        //public static extern short RegisterClassEx([In] ref WNDCLASS lpwcx);  
        public static extern short RegisterClassEx([In] ref WNDCLASSEX lpwcx);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "RegisterClassEx")]
        public static extern UInt16 RegisterClassEx2([In] ref WNDCLASSEX lpwcx);
    }


    While you can find most of the function's definition from the internet, the messages/constants/structures are not that straigforward, and especially there are some marshalling strategy related to the StructureLayout, MarshalAs, Enum and so on... So we will give some of the DataStructure each by each...


First is constant values for the DrawText function.



C#代码  收藏代码
// Check on this page: http://www.pinvoke.net/default.aspx/user32/DrawText.html  
// for the DrawText constants.  
public static class Win32_DT_Constant
    {
        public const int DT_TOP = 0x00000000;

        public const int DT_LEFT = 0x00000000;

        public const int DT_CENTER = 0x00000001;

        public const int DT_RIGHT = 0x00000002;

        public const int DT_VCENTER = 0x00000004;

        public const int DT_BOTTOM = 0x00000008;

        public const int DT_WORDBREAK = 0x00000010;

        public const int DT_SINGLELINE = 0x00000020;

        public const int DT_EXPANDTABS = 0x00000040;

        public const int DT_TABSTOP = 0x00000080;

        public const int DT_NOCLIP = 0x00000100;

        public const int DT_EXTERNALLEADING = 0x00000200;

        public const int DT_CALCRECT = 0x00000400;

        public const int DT_NOPREFIX = 0x00000800;

        public const int DT_INTERNAL = 0x00001000;
    }

    Then, following is the Constant definition for the Initial X/Y, Initial X/Y size from the CreateWindowEx call.



   C#代码  收藏代码
// Did not find the the definition, thought this is for value -1  
public static class Win32_CW_Constant
    {
        //public const int CW_USEDEFAULT = (int)0x80000000;  
        public const int CW_USEDEFAULT = -1;
    }


    To show you how this Win32_CW_Constants is used, chck on the following code snippet. 


    C#代码  收藏代码
IntPtr hwnd = WinAPI.CreateWindowEx2(
        0,
        regRest,
        "The hello proram",
        WindowStyles.WS_OVERLAPPEDWINDOW,
        Win32_CW_Constant.CW_USEDEFAULT,
        Win32_CW_Constant.CW_USEDEFAULT,
        Win32_CW_Constant.CW_USEDEFAULT,
        Win32_CW_Constant.CW_USEDEFAULT,
        IntPtr.Zero,
        IntPtr.Zero,
        hInstance,
        IntPtr.Zero);  





    Then, Get StockObject can return you some pre-defined GDI object, you may pass in a enum value to get the right stock object back from Windows.


    C#代码  收藏代码
// Check on this page : http://www.pinvoke.net/default.aspx/gdi32/GetStockObject.html  
// for the StockObjects enum  
public enum StockObjects
    {
        WHITE_BRUSH = 0,
        LTGRAY_BRUSH = 1,
        GRAY_BRUSH = 2,
        DKGRAY_BRUSH = 3,
        BLACK_BRUSH = 4,
        NULL_BRUSH = 5,
        HOLLOW_BRUSH = NULL_BRUSH,
        WHITE_PEN = 6,
        BLACK_PEN = 7,
        NULL_PEN = 8,
        OEM_FIXED_FONT = 10,
        ANSI_FIXED_FONT = 11,
        ANSI_VAR_FONT = 12,
        SYSTEM_FONT = 13,
        DEVICE_DEFAULT_FONT = 14,
        DEFAULT_PALETTE = 15,
        SYSTEM_FIXED_FONT = 16,
        DEFAULT_GUI_FONT = 17,
        DC_BRUSH = 18,
        DC_PEN = 19,
    }
    When you need to load some system cursors, you will need the following IDC_ constants.

   C#代码  收藏代码
// check on this page on the IDC_ constants  
// http://www.pinvoke.net/default.aspx/Constants/IDC_.html  
public static class Win32_IDC_Constants
    {
        public const int
         IDC_ARROW = 32512,
         IDC_IBEAM = 32513,
         IDC_WAIT = 32514,
         IDC_CROSS = 32515,
         IDC_UPARROW = 32516,
         IDC_SIZE = 32640,
         IDC_ICON = 32641,
         IDC_SIZENWSE = 32642,
         IDC_SIZENESW = 32643,
         IDC_SIZEWE = 32644,
         IDC_SIZENS = 32645,
         IDC_SIZEALL = 32646,
         IDC_NO = 32648,
         IDC_HAND = 32649,
         IDC_APPSTARTING = 32650,
         IDC_HELP = 32651;
    }
    To display some Win32 message boxes, you may need the following MessageGoxOptions.


   C#代码  收藏代码
// check on this page:    
    //  http://www.pinvoke.net/default.aspx/Enums/MessageBoxOptions.html  
    ///<summary>  
    /// Flags that define appearance and behaviour of a standard message box displayed by a call to the MessageBox function.  
    /// </summary>      
    [Flags]
    public enum MessageBoxOptions : uint
    {
        OkOnly = 0x000000,
        OkCancel = 0x000001,
        AbortRetryIgnore = 0x000002,
        YesNoCancel = 0x000003,
        YesNo = 0x000004,
        RetryCancel = 0x000005,
        CancelTryContinue = 0x000006,
        IconHand = 0x000010,
        IconQuestion = 0x000020,
        IconExclamation = 0x000030,
        IconAsterisk = 0x000040,
        UserIcon = 0x000080,
        IconWarning = IconExclamation,
        IconError = IconHand,
        IconInformation = IconAsterisk,
        IconStop = IconHand,
        DefButton1 = 0x000000,
        DefButton2 = 0x000100,
        DefButton3 = 0x000200,
        DefButton4 = 0x000300,
        ApplicationModal = 0x000000,
        SystemModal = 0x001000,
        TaskModal = 0x002000,
        Help = 0x004000,
        NoFocus = 0x008000,
        SetForeground = 0x010000,
        DefaultDesktopOnly = 0x020000,
        Topmost = 0x040000,
        Right = 0x080000,
        RTLReading = 0x100000
    }
    Please take note on the enum's attribute and representation data type, it is a Flag enum (?),  and it is represented  with a uint (32 bit unsigned integer). Some of the enums explicitly use a 8 bit character structure.

 

And MessageBox may return a Result in enum, which we will name it as MessageBoxResult, and the value of it is like this:

C#代码  收藏代码
// check on this page:  
// http://www.pinvoke.net/default.aspx/Enums/MessageBoxResult.html  
/// <summary>  
/// Represents possible values returned by the MessageBox function.  
/// </summary>  
    public enum MessageBoxResult : uint
    {
        Ok = 1,
        Cancel,
        Abort,
        Retry,
        Ignore,
        Yes,
        No,
        Close,
        Help,
        TryAgain,
        Continue,
        Timeout = 32000
    }


    The windows message is denoted by the following strucutre, I have listed the original definition and the translated pinvoke c# structure, 

C#代码  收藏代码
// Check on this article on   
//  http://msdn.microsoft.com/en-us/library/ef4c3t39(v=vs.80).aspx  
// Howto : marshal structuring Using PInvoke  
//   
//typedef struct tagMSG {  
//  HWND   hwnd;  
//  UINT   message;  
//  WPARAM wParam;  
//  LPARAM lParam;  
//  DWORD  time;  
//  POINT  pt;  
//} MSG, *PMSG, *LPMSG;  
  
// MSG structure   
// http://msdn.microsoft.com/en-us/library/windows/desktop/ms644958(v=vs.85).aspx  
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct MSG
    {
        public IntPtr hwnd;
        public UInt32 message;
        public UIntPtr wParam;
        public UIntPtr lParam;
        public UInt32 time;
        public POINT pt;
    }


    Point Structure, the one that you will deal with very commonly.

    C#代码  收藏代码
// PONIT Structure  
//  http://msdn.microsoft.com/en-us/library/windows/desktop/dd162805(v=vs.85).aspx  
//typedef struct tagPOINT {  
//  LONG x;  
//  LONG y;  
//} POINT, *PPOINT;  
    public struct POINT
    {
        public Int32 x;
        public Int32 Y;
    }



    window's RegisterClass has a extended version, which is called RegisterClassEx, and each is using a different structure .

let's first check on the WNDCLASS structure. 

C#代码  收藏代码
// http://www.pinvoke.net/default.aspx/Structures.WNDCLASS  
[StructLayout(LayoutKind.Sequential)]
public struct WNDCLASS
    {
        public ClassStyles style;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WndProc lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpszMenuName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpszClassName;
    }


    and as we have seen above, we have given a delegate type to the field lpfnWndProc, and with the help of MarhslAs(UnmanagedType.FunctionPtr), we have the following delegate declaration.

    C#代码  收藏代码
// WndProcDelegate  
// http://www.pinvoke.net/default.aspx/user32/WndProcDelegate.html  
public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);



    and if you are using the RegiserClassEx, you should use the following structure.

    C#代码  收藏代码
// check on this: http://www.pinvoke.net/default.aspx/Structures/WNDCLASSEX.html  
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct WNDCLASSEX
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public int style;
        public IntPtr lpfnWndProc; // not WndProc -- careful   
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public string lpszMenuName;
        public string lpszClassName;
        public IntPtr hIconSm;
    }


    Comparing this to the hand-written one (the hand-written on is listed below because I wantted to show you that it is quite often to get it wrong rather than to get it right for the equivalent datastructure).

C#代码  收藏代码
// compare this to the hand-written one  
//   
//public struct WNDCLASS  
//{  
//    public UInt32 style;  
//    public IntPtr lpfnWndProc;  
//    public UInt32 cbClsExtra;  
//    public UInt32 cbWndExtra;  
//    public IntPtr hInstance;  
//    public IntPtr hIcon;  
//    public IntPtr hCursor;  
//    public IntPtr hbrBackground;  
//    public string lpszMenuName;  
//    public string lpszClassName;  
//}  
 

and as there are a window Style enum definition for the RegisterClass call, there is also an enum for the RegisterClassEx call.

First check on the WindowStyle enum.

C#代码  收藏代码
// Window Style  
// http://msdn.microsoft.com/en-us/library/windows/desktop/ms632600(v=vs.85).aspx  
  
// http://www.pinvoke.net/default.aspx/Enums/WindowStyles.html  
/// <summary>  
/// Window Styles.  
/// The following styles can be specified wherever a window style is required. After the control has been created, these styles cannot be modified, except as noted.  
/// </summary>  
[Flags()]
public enum WindowStyles : uint
    {
        /// <summary>The window has a thin-line border.</summary>  
        WS_BORDER = 0x800000,

        /// <summary>The window has a title bar (includes the WS_BORDER style).</summary>  
        WS_CAPTION = 0xc00000,

        /// <summary>The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.</summary>  
        WS_CHILD = 0x40000000,

        /// <summary>Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.</summary>  
        WS_CLIPCHILDREN = 0x2000000,

        /// <summary>  
        /// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated.  
        /// If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window.  
        /// </summary>  
        WS_CLIPSIBLINGS = 0x4000000,

        /// <summary>The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.</summary>  
        WS_DISABLED = 0x8000000,

        /// <summary>The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.</summary>  
        WS_DLGFRAME = 0x400000,

        /// <summary>  
        /// The window is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style.  
        /// The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys.  
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.  
        /// </summary>  
        WS_GROUP = 0x20000,

        /// <summary>The window has a horizontal scroll bar.</summary>  
        WS_HSCROLL = 0x100000,

        /// <summary>The window is initially maximized.</summary>   
        WS_MAXIMIZE = 0x1000000,

        /// <summary>The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>   
        WS_MAXIMIZEBOX = 0x10000,

        /// <summary>The window is initially minimized.</summary>  
        WS_MINIMIZE = 0x20000000,

        /// <summary>The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>  
        WS_MINIMIZEBOX = 0x20000,

        /// <summary>The window is an overlapped window. An overlapped window has a title bar and a border.</summary>  
        WS_OVERLAPPED = 0x0,

        /// <summary>The window is an overlapped window.</summary>  
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,

        /// <summary>The window is a pop-up window. This style cannot be used with the WS_CHILD style.</summary>  
        WS_POPUP = 0x80000000u,

        /// <summary>The window is a pop-up window. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible.</summary>  
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,

        /// <summary>The window has a sizing border.</summary>  
        WS_SIZEFRAME = 0x40000,

        /// <summary>The window has a window menu on its title bar. The WS_CAPTION style must also be specified.</summary>  
        WS_SYSMENU = 0x80000,

        /// <summary>  
        /// The window is a control that can receive the keyboard focus when the user presses the TAB key.  
        /// Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.    
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.  
        /// For user-created windows and modeless dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.  
        /// </summary>  
        WS_TABSTOP = 0x10000,

        /// <summary>The window is initially visible. This style can be turned on and off by using the ShowWindow or SetWindowPos function.</summary>  
        WS_VISIBLE = 0x10000000,

        /// <summary>The window has a vertical scroll bar.</summary>  
        WS_VSCROLL = 0x200000
    }
    and the definition of the WindowStyleEx is as follow.

   C#代码  收藏代码
// http://www.pinvoke.net/default.aspx/Enums/WindowStylesEx.html  
[Flags]
public enum WindowStylesEx : uint
    {
        /// <summary>  
        /// Specifies that a window created with this style accepts drag-drop files.  
        /// </summary>  
        WS_EX_ACCEPTFILES = 0x00000010,
        /// <summary>  
        /// Forces a top-level window onto the taskbar when the window is visible.  
        /// </summary>  
        WS_EX_APPWINDOW = 0x00040000,
        /// <summary>  
        /// Specifies that a window has a border with a sunken edge.  
        /// </summary>  
        WS_EX_CLIENTEDGE = 0x00000200,
        /// <summary>  
        /// Windows XP: Paints all descendants of a window in bottom-to-top painting order using double-buffering. For more information, see Remarks. This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.   
        /// </summary>  
        WS_EX_COMPOSITED = 0x02000000,
        /// <summary>  
        /// Includes a question mark in the title bar of the window. When the user clicks the question mark, the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message. The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command. The Help application displays a pop-up window that typically contains help for the child window.  
        /// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.  
        /// </summary>  
        WS_EX_CONTEXTHELP = 0x00000400,
        /// <summary>  
        /// The window itself contains child windows that should take part in dialog box navigation. If this style is specified, the dialog manager recurses into children of this window when performing navigation operations such as handling the TAB key, an arrow key, or a keyboard mnemonic.  
        /// </summary>  
        WS_EX_CONTROLPARENT = 0x00010000,
        /// <summary>  
        /// Creates a window that has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter.  
        /// </summary>  
        WS_EX_DLGMODALFRAME = 0x00000001,
        /// <summary>  
        /// Windows 2000/XP: Creates a layered window. Note that this cannot be used for child windows. Also, this cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.   
        /// </summary>  
        WS_EX_LAYERED = 0x00080000,
        /// <summary>  
        /// Arabic and Hebrew versions of Windows 98/Me, Windows 2000/XP: Creates a window whose horizontal origin is on the right edge. Increasing horizontal values advance to the left.   
        /// </summary>  
        WS_EX_LAYOUTRTL = 0x00400000,
        /// <summary>  
        /// Creates a window that has generic left-aligned properties. This is the default.  
        /// </summary>  
        WS_EX_LEFT = 0x00000000,
        /// <summary>  
        /// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical scroll bar (if present) is to the left of the client area. For other languages, the style is ignored.  
        /// </summary>  
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        /// <summary>  
        /// The window text is displayed using left-to-right reading-order properties. This is the default.  
        /// </summary>  
        WS_EX_LTRREADING = 0x00000000,
        /// <summary>  
        /// Creates a multiple-document interface (MDI) child window.  
        /// </summary>  
        WS_EX_MDICHILD = 0x00000040,
        /// <summary>  
        /// Windows 2000/XP: A top-level window created with this style does not become the foreground window when the user clicks it. The system does not bring this window to the foreground when the user minimizes or closes the foreground window.   
        /// To activate the window, use the SetActiveWindow or SetForegroundWindow function.  
        /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.  
        /// </summary>  
        WS_EX_NOACTIVATE = 0x08000000,
        /// <summary>  
        /// Windows 2000/XP: A window created with this style does not pass its window layout to its child windows.  
        /// </summary>  
        WS_EX_NOINHERITLAYOUT = 0x00100000,
        /// <summary>  
        /// Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.  
        /// </summary>  
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        /// <summary>  
        /// Combines the WS_EX_CLIENTEDGE and WS_EX_WINDOWEDGE styles.  
        /// </summary>  
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
        /// <summary>  
        /// Combines the WS_EX_WINDOWEDGE, WS_EX_TOOLWINDOW, and WS_EX_TOPMOST styles.  
        /// </summary>  
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
        /// <summary>  
        /// The window has generic "right-aligned" properties. This depends on the window class. This style has an effect only if the shell language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the style is ignored.  
        /// Using the WS_EX_RIGHT style for static or edit controls has the same effect as using the SS_RIGHT or ES_RIGHT style, respectively. Using this style with button controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles.  
        /// </summary>  
        WS_EX_RIGHT = 0x00001000,
        /// <summary>  
        /// Vertical scroll bar (if present) is to the right of the client area. This is the default.  
        /// </summary>  
        WS_EX_RIGHTSCROLLBAR = 0x00000000,
        /// <summary>  
        /// If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text is displayed using right-to-left reading-order properties. For other languages, the style is ignored.  
        /// </summary>  
        WS_EX_RTLREADING = 0x00002000,
        /// <summary>  
        /// Creates a window with a three-dimensional border style intended to be used for items that do not accept user input.  
        /// </summary>  
        WS_EX_STATICEDGE = 0x00020000,
        /// <summary>  
        /// Creates a tool window; that is, a window intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.   
        /// </summary>  
        WS_EX_TOOLWINDOW = 0x00000080,
        /// <summary>  
        /// Specifies that a window created with this style should be placed above all non-topmost windows and should stay above them, even when the window is deactivated. To add or remove this style, use the SetWindowPos function.  
        /// </summary>  
        WS_EX_TOPMOST = 0x00000008,
        /// <summary>  
        /// Specifies that a window created with this style should not be painted until siblings beneath the window (that were created by the same thread) have been painted. The window appears transparent because the bits of underlying sibling windows have already been painted.  
        /// To achieve transparency without these restrictions, use the SetWindowRgn function.  
        /// </summary>  
        WS_EX_TRANSPARENT = 0x00000020,
        /// <summary>  
        /// Specifies that a window has a border with a raised edge.  
        /// </summary>  
        WS_EX_WINDOWEDGE = 0x00000100
    }



    the ShowWindowCommands enum, which feeds to the call ShowWindow, dictate how the window will be displayed.

    C#代码  收藏代码
// http://www.pinvoke.net/default.aspx/Enums/ShowWindowCommand.html  
public enum ShowWindowCommands : int
    {
        /// <summary>  
        /// Hides the window and activates another window.  
        /// </summary>  
        Hide = 0,
        /// <summary>  
        /// Activates and displays a window. If the window is minimized or   
        /// maximized, the system restores it to its original size and position.  
        /// An application should specify this flag when displaying the window   
        /// for the first time.  
        /// </summary>  
        Normal = 1,
        /// <summary>  
        /// Activates the window and displays it as a minimized window.  
        /// </summary>  
        ShowMinimized = 2,
        /// <summary>  
        /// Maximizes the specified window.  
        /// </summary>  
        Maximize = 3, // is this the right value?  
                      /// <summary>  
                      /// Activates the window and displays it as a maximized window.  
                      /// </summary>         
        ShowMaximized = 3,
        /// <summary>  
        /// Displays a window in its most recent size and position. This value   
        /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except   
        /// the window is not activated.  
        /// </summary>  
        ShowNoActivate = 4,
        /// <summary>  
        /// Activates the window and displays it in its current size and position.   
        /// </summary>  
        Show = 5,
        /// <summary>  
        /// Minimizes the specified window and activates the next top-level   
        /// window in the Z order.  
        /// </summary>  
        Minimize = 6,
        /// <summary>  
        /// Displays the window as a minimized window. This value is similar to  
        /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the   
        /// window is not activated.  
        /// </summary>  
        ShowMinNoActive = 7,
        /// <summary>  
        /// Displays the window in its current size and position. This value is   
        /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the   
        /// window is not activated.  
        /// </summary>  
        ShowNA = 8,
        /// <summary>  
        /// Activates and displays the window. If the window is minimized or   
        /// maximized, the system restores it to its original size and position.   
        /// An application should specify this flag when restoring a minimized window.  
        /// </summary>  
        Restore = 9,
        /// <summary>  
        /// Sets the show state based on the SW_* value specified in the   
        /// STARTUPINFO structure passed to the CreateProcess function by the   
        /// program that started the application.  
        /// </summary>  
        ShowDefault = 10,
        /// <summary>  
        ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread   
        /// that owns the window is not responding. This flag should only be   
        /// used when minimizing windows from a different thread.  
        /// </summary>  
        ForceMinimize = 11
    }


    ClassStyle, which are the enum values that dictate how the registered window classes render and etc...It has the following values.



    C#代码  收藏代码
// http://www.pinvoke.net/default.aspx/Enums/ClassStyles.html  
[Flags]
public enum ClassStyles : uint
    {
        /// <summary>Aligns the window's client area on a byte boundary (in the x direction). This style affects the width of the window and its horizontal placement on the display.</summary>  
        ByteAlignClient = 0x1000,

        /// <summary>Aligns the window on a byte boundary (in the x direction). This style affects the width of the window and its horizontal placement on the display.</summary>  
        ByteAlignWindow = 0x2000,

        /// <summary>  
        /// Allocates one device context to be shared by all windows in the class.  
        /// Because window classes are process specific, it is possible for multiple threads of an application to create a window of the same class.  
        /// It is also possible for the threads to attempt to use the device context simultaneously. When this happens, the system allows only one thread to successfully finish its drawing operation.  
        /// </summary>  
        ClassDC = 0x40,

        /// <summary>Sends a double-click message to the window procedure when the user double-clicks the mouse while the cursor is within a window belonging to the class.</summary>  
        DoubleClicks = 0x8,

        /// <summary>  
        /// Enables the drop shadow effect on a window. The effect is turned on and off through SPI_SETDROPSHADOW.  
        /// Typically, this is enabled for small, short-lived windows such as menus to emphasize their Z order relationship to other windows.  
        /// </summary>  
        DropShadow = 0x20000,

        /// <summary>Indicates that the window class is an application global class. For more information, see the "Application Global Classes" section of About Window Classes.</summary>  
        GlobalClass = 0x4000,

        /// <summary>Redraws the entire window if a movement or size adjustment changes the width of the client area.</summary>  
        HorizontalRedraw = 0x2,

        /// <summary>Disables Close on the window menu.</summary>  
        NoClose = 0x200,

        /// <summary>Allocates a unique device context for each window in the class.</summary>  
        OwnDC = 0x20,

        /// <summary>  
        /// Sets the clipping rectangle of the child window to that of the parent window so that the child can draw on the parent.  
        /// A window with the CS_PARENTDC style bit receives a regular device context from the system's cache of device contexts.  
        /// It does not give the child the parent's device context or device context settings. Specifying CS_PARENTDC enhances an application's performance.  
        /// </summary>  
        ParentDC = 0x80,

        /// <summary>  
        /// Saves, as a bitmap, the portion of the screen image obscured by a window of this class.  
        /// When the window is removed, the system uses the saved bitmap to restore the screen image, including other windows that were obscured.  
        /// Therefore, the system does not send WM_PAINT messages to windows that were obscured if the memory used by the bitmap has not been discarded and if other screen actions have not invalidated the stored image.  
        /// This style is useful for small windows (for example, menus or dialog boxes) that are displayed briefly and then removed before other screen activity takes place.  
        /// This style increases the time required to display the window, because the system must first allocate memory to store the bitmap.  
        /// </summary>  
        SaveBits = 0x800,

        /// <summary>Redraws the entire window if a movement or size adjustment changes the height of the client area.</summary>  
        VerticalRedraw = 0x1
    }

    The PAINTSTRUCT is the strucute use by the GDI function BegingPaint and EndPaint function call.

    C#代码  收藏代码
// http://www.pinvoke.net/default.aspx/Structures/PAINTSTRUCT.html  
[StructLayout(LayoutKind.Sequential)]
public struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public bool fErase;
        public RECT rcPaint;
        public bool fRestore;
        public bool fIncUpdate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;
    }


    And then we comes to the RECT structure, which represent a rectangle area.Interesing is that you can have method in the RECT structure much like you can have for native C# classes/structures. - marshalling only deal with the fields/members.

C#代码  收藏代码
// http://www.pinvoke.net/default.aspx/Structures/RECT.html  
[StructLayout(LayoutKind.Sequential)]
public struct RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }

        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(Left, Top); }
            set { X = value.X; Y = value.Y; }
        }

        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; }
        }

        public static implicit operator System.Drawing.Rectangle(RECT r)
        {
            return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        public static implicit operator RECT(System.Drawing.Rectangle r)
        {
            return new RECT(r);
        }

        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            if (obj is RECT)
                return Equals((RECT)obj);
            else if (obj is System.Drawing.Rectangle)
                return Equals(new RECT((System.Drawing.Rectangle)obj));
            return false;
        }

        public override int GetHashCode()
        {
            return ((System.Drawing.Rectangle)this).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }

    Next is the windows message enums, while Windows has defined tens of windows messages, and there is a huge list of supported messags.

    C#代码  收藏代码
// http://www.pinvoke.net/default.aspx/Enums/WindowsMessages.html  
/// <summary>  
/// Windows Messages  
/// Defined in winuser.h from Windows SDK v6.1  
/// Documentation pulled from MSDN.  
/// </summary>  
public enum WM : uint
    {
        /// <summary>  
        /// The WM_NULL message performs no operation. An application sends the WM_NULL message if it wants to post a message that the recipient window will ignore.  
        /// </summary>  
        NULL = 0x0000,
        /// <summary>  
        /// The WM_CREATE message is sent when an application requests that a window be created by calling the CreateWindowEx or CreateWindow function. (The message is sent before the function returns.) The window procedure of the new window receives this message after the window is created, but before the window becomes visible.  
        /// </summary>  
        CREATE = 0x0001,
        /// <summary>  
        /// The WM_DESTROY message is sent when a window is being destroyed. It is sent to the window procedure of the window being destroyed after the window is removed from the screen.   
        /// This message is sent first to the window being destroyed and then to the child windows (if any) as they are destroyed. During the processing of the message, it can be assumed that all child windows still exist.  
        /// /// </summary>  
        DESTROY = 0x0002,
        /// <summary>  
        /// The WM_MOVE message is sent after a window has been moved.   
        /// </summary>  
        MOVE = 0x0003,
        /// <summary>  
        /// The WM_SIZE message is sent to a window after its size has changed.  
        /// </summary>  
        SIZE = 0x0005,
        /// <summary>  
        /// The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately.   
        /// </summary>  
        ACTIVATE = 0x0006,
        /// <summary>  
        /// The WM_SETFOCUS message is sent to a window after it has gained the keyboard focus.   
        /// </summary>  
        SETFOCUS = 0x0007,
        /// <summary>  
        /// The WM_KILLFOCUS message is sent to a window immediately before it loses the keyboard focus.   
        /// </summary>  
        KILLFOCUS = 0x0008,
        /// <summary>  
        /// The WM_ENABLE message is sent when an application changes the enabled state of a window. It is sent to the window whose enabled state is changing. This message is sent before the EnableWindow function returns, but after the enabled state (WS_DISABLED style bit) of the window has changed.   
        /// </summary>  
        ENABLE = 0x000A,
        /// <summary>  
        /// An application sends the WM_SETREDRAW message to a window to allow changes in that window to be redrawn or to prevent changes in that window from being redrawn.   
        /// </summary>  
        SETREDRAW = 0x000B,
        /// <summary>  
        /// An application sends a WM_SETTEXT message to set the text of a window.   
        /// </summary>  
        SETTEXT = 0x000C,
        /// <summary>  
        /// An application sends a WM_GETTEXT message to copy the text that corresponds to a window into a buffer provided by the caller.   
        /// </summary>  
        GETTEXT = 0x000D,
        /// <summary>  
        /// An application sends a WM_GETTEXTLENGTH message to determine the length, in characters, of the text associated with a window.   
        /// </summary>  
        GETTEXTLENGTH = 0x000E,
        /// <summary>  
        /// The WM_PAINT message is sent when the system or another application makes a request to paint a portion of an application's window. The message is sent when the UpdateWindow or RedrawWindow function is called, or by the DispatchMessage function when the application obtains a WM_PAINT message by using the GetMessage or PeekMessage function.   
        /// </summary>  
        PAINT = 0x000F,
        /// <summary>  
        /// The WM_CLOSE message is sent as a signal that a window or an application should terminate.  
        /// </summary>  
        CLOSE = 0x0010,
        /// <summary>  
        /// The WM_QUERYENDSESSION message is sent when the user chooses to end the session or when an application calls one of the system shutdown functions. If any application returns zero, the session is not ended. The system stops sending WM_QUERYENDSESSION messages as soon as one application returns zero.  
        /// After processing this message, the system sends the WM_ENDSESSION message with the wParam parameter set to the results of the WM_QUERYENDSESSION message.  
        /// </summary>  
        QUERYENDSESSION = 0x0011,
        /// <summary>  
        /// The WM_QUERYOPEN message is sent to an icon when the user requests that the window be restored to its previous size and position.  
        /// </summary>  
        QUERYOPEN = 0x0013,
        /// <summary>  
        /// The WM_ENDSESSION message is sent to an application after the system processes the results of the WM_QUERYENDSESSION message. The WM_ENDSESSION message informs the application whether the session is ending.  
        /// </summary>  
        ENDSESSION = 0x0016,
        /// <summary>  
        /// The WM_QUIT message indicates a request to terminate an application and is generated when the application calls the PostQuitMessage function. It causes the GetMessage function to return zero.  
        /// </summary>  
        QUIT = 0x0012,
        /// <summary>  
        /// The WM_ERASEBKGND message is sent when the window background must be erased (for example, when a window is resized). The message is sent to prepare an invalidated portion of a window for painting.   
        /// </summary>  
        ERASEBKGND = 0x0014,
        /// <summary>  
        /// This message is sent to all top-level windows when a change is made to a system color setting.   
        /// </summary>  
        SYSCOLORCHANGE = 0x0015,
        /// <summary>  
        /// The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.  
        /// </summary>  
        SHOWWINDOW = 0x0018,
        /// <summary>  
        /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.  
        /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.  
        /// </summary>  
        WININICHANGE = 0x001A,
        /// <summary>  
        /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.  
        /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.  
        /// </summary>  
        SETTINGCHANGE = WININICHANGE,
        /// <summary>  
        /// The WM_DEVMODECHANGE message is sent to all top-level windows whenever the user changes device-mode settings.   
        /// </summary>  
        DEVMODECHANGE = 0x001B,
        /// <summary>  
        /// The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.  
        /// </summary>  
        ACTIVATEAPP = 0x001C,
        /// <summary>  
        /// An application sends the WM_FONTCHANGE message to all top-level windows in the system after changing the pool of font resources.   
        /// </summary>  
        FONTCHANGE = 0x001D,
        /// <summary>  
        /// A message that is sent whenever there is a change in the system time.  
        /// </summary>  
        TIMECHANGE = 0x001E,
        /// <summary>  
        /// The WM_CANCELMODE message is sent to cancel certain modes, such as mouse capture. For example, the system sends this message to the active window when a dialog box or message box is displayed. Certain functions also send this message explicitly to the specified window regardless of whether it is the active window. For example, the EnableWindow function sends this message when disabling the specified window.  
        /// </summary>  
        CANCELMODE = 0x001F,
        /// <summary>  
        /// The WM_SETCURSOR message is sent to a window if the mouse causes the cursor to move within a window and mouse input is not captured.   
        /// </summary>  
        SETCURSOR = 0x0020,
        /// <summary>  
        /// The WM_MOUSEACTIVATE message is sent when the cursor is in an inactive window and the user presses a mouse button. The parent window receives this message only if the child window passes it to the DefWindowProc function.  
        /// </summary>  
        MOUSEACTIVATE = 0x0021,
        /// <summary>  
        /// The WM_CHILDACTIVATE message is sent to a child window when the user clicks the window's title bar or when the window is activated, moved, or sized.  
        /// </summary>  
        CHILDACTIVATE = 0x0022,
        /// <summary>  
        /// The WM_QUEUESYNC message is sent by a computer-based training (CBT) application to separate user-input messages from other messages sent through the WH_JOURNALPLAYBACK Hook procedure.   
        /// </summary>  
        QUEUESYNC = 0x0023,
        /// <summary>  
        /// The WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default maximized size and position, or its default minimum or maximum tracking size.   
        /// </summary>  
        GETMINMAXINFO = 0x0024,
        /// <summary>  
        /// Windows NT 3.51 and earlier: The WM_PAINTICON message is sent to a minimized window when the icon is to be painted. This message is not sent by newer versions of Microsoft Windows, except in unusual circumstances explained in the Remarks.  
        /// </summary>  
        PAINTICON = 0x0026,
        /// <summary>  
        /// Windows NT 3.51 and earlier: The WM_ICONERASEBKGND message is sent to a minimized window when the background of the icon must be filled before painting the icon. A window receives this message only if a class icon is defined for the window; otherwise, WM_ERASEBKGND is sent. This message is not sent by newer versions of Windows.  
        /// </summary>  
        ICONERASEBKGND = 0x0027,
        /// <summary>  
        /// The WM_NEXTDLGCTL message is sent to a dialog box procedure to set the keyboard focus to a different control in the dialog box.   
        /// </summary>  
        NEXTDLGCTL = 0x0028,
        /// <summary>  
        /// The WM_SPOOLERSTATUS message is sent from Print Manager whenever a job is added to or removed from the Print Manager queue.   
        /// </summary>  
        SPOOLERSTATUS = 0x002A,
        /// <summary>  
        /// The WM_DRAWITEM message is sent to the parent window of an owner-drawn button, combo box, list box, or menu when a visual aspect of the button, combo box, list box, or menu has changed.  
        /// </summary>  
        DRAWITEM = 0x002B,
        /// <summary>  
        /// The WM_MEASUREITEM message is sent to the owner window of a combo box, list box, list view control, or menu item when the control or menu is created.  
        /// </summary>  
        MEASUREITEM = 0x002C,
        /// <summary>  
        /// Sent to the owner of a list box or combo box when the list box or combo box is destroyed or when items are removed by the LB_DELETESTRING, LB_RESETCONTENT, CB_DELETESTRING, or CB_RESETCONTENT message. The system sends a WM_DELETEITEM message for each deleted item. The system sends the WM_DELETEITEM message for any deleted list box or combo box item with nonzero item data.  
        /// </summary>  
        DELETEITEM = 0x002D,
        /// <summary>  
        /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_KEYDOWN message.   
        /// </summary>  
        VKEYTOITEM = 0x002E,
        /// <summary>  
        /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_CHAR message.   
        /// </summary>  
        CHARTOITEM = 0x002F,
        /// <summary>  
        /// An application sends a WM_SETFONT message to specify the font that a control is to use when drawing text.   
        /// </summary>  
        SETFONT = 0x0030,
        /// <summary>  
        /// An application sends a WM_GETFONT message to a control to retrieve the font with which the control is currently drawing its text.   
        /// </summary>  
        GETFONT = 0x0031,
        /// <summary>  
        /// An application sends a WM_SETHOTKEY message to a window to associate a hot key with the window. When the user presses the hot key, the system activates the window.   
        /// </summary>  
        SETHOTKEY = 0x0032,
        /// <summary>  
        /// An application sends a WM_GETHOTKEY message to determine the hot key associated with a window.   
        /// </summary>  
        GETHOTKEY = 0x0033,
        /// <summary>  
        /// The WM_QUERYDRAGICON message is sent to a minimized (iconic) window. The window is about to be dragged by the user but does not have an icon defined for its class. An application can return a handle to an icon or cursor. The system displays this cursor or icon while the user drags the icon.  
        /// </summary>  
        QUERYDRAGICON = 0x0037,
        /// <summary>  
        /// The system sends the WM_COMPAREITEM message to determine the relative position of a new item in the sorted list of an owner-drawn combo box or list box. Whenever the application adds a new item, the system sends this message to the owner of a combo box or list box created with the CBS_SORT or LBS_SORT style.   
        /// </summary>  
        COMPAREITEM = 0x0039,
        /// <summary>  
        /// Active Accessibility sends the WM_GETOBJECT message to obtain information about an accessible object contained in a server application.   
        /// Applications never send this message directly. It is sent only by Active Accessibility in response to calls to AccessibleObjectFromPoint, AccessibleObjectFromEvent, or AccessibleObjectFromWindow. However, server applications handle this message.   
        /// </summary>  
        GETOBJECT = 0x003D,
        /// <summary>  
        /// The WM_COMPACTING message is sent to all top-level windows when the system detects more than 12.5 percent of system time over a 30- to 60-second interval is being spent compacting memory. This indicates that system memory is low.  
        /// </summary>  
        COMPACTING = 0x0041,
        /// <summary>  
        /// WM_COMMNOTIFY is Obsolete for Win32-Based Applications  
        /// </summary>  
        [Obsolete]
        COMMNOTIFY = 0x0044,
        /// <summary>  
        /// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.  
        /// </summary>  
        WINDOWPOSCHANGING = 0x0046,
        /// <summary>  
        /// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.  
        /// </summary>  
        WINDOWPOSCHANGED = 0x0047,
        /// <summary>  
        /// Notifies applications that the system, typically a battery-powered personal computer, is about to enter a suspended mode.  
        /// Use: POWERBROADCAST  
        /// </summary>  
        [Obsolete]
        POWER = 0x0048,
        /// <summary>  
        /// An application sends the WM_COPYDATA message to pass data to another application.   
        /// </summary>  
        COPYDATA = 0x004A,
        /// <summary>  
        /// The WM_CANCELJOURNAL message is posted to an application when a user cancels the application's journaling activities. The message is posted with a NULL window handle.   
        /// </summary>  
        CANCELJOURNAL = 0x004B,
        /// <summary>  
        /// Sent by a common control to its parent window when an event has occurred or the control requires some information.   
        /// </summary>  
        NOTIFY = 0x004E,
        /// <summary>  
        /// The WM_INPUTLANGCHANGEREQUEST message is posted to the window with the focus when the user chooses a new input language, either with the hotkey (specified in the Keyboard control panel application) or from the indicator on the system taskbar. An application can accept the change by passing the message to the DefWindowProc function or reject the change (and prevent it from taking place) by returning immediately.   
        /// </summary>  
        INPUTLANGCHANGEREQUEST = 0x0050,
        /// <summary>  
        /// The WM_INPUTLANGCHANGE message is sent to the topmost affected window after an application's input language has been changed. You should make any application-specific settings and pass the message to the DefWindowProc function, which passes the message to all first-level child windows. These child windows can pass the message to DefWindowProc to have it pass the message to their child windows, and so on.   
        /// </summary>  
        INPUTLANGCHANGE = 0x0051,
        /// <summary>  
        /// Sent to an application that has initiated a training card with Microsoft Windows Help. The message informs the application when the user clicks an authorable button. An application initiates a training card by specifying the HELP_TCARD command in a call to the WinHelp function.  
        /// </summary>  
        TCARD = 0x0052,
        /// <summary>  
        /// Indicates that the user pressed the F1 key. If a menu is active when F1 is pressed, WM_HELP is sent to the window associated with the menu; otherwise, WM_HELP is sent to the window that has the keyboard focus. If no window has the keyboard focus, WM_HELP is sent to the currently active window.   
        /// </summary>  
        HELP = 0x0053,
        /// <summary>  
        /// The WM_USERCHANGED message is sent to all windows after the user has logged on or off. When the user logs on or off, the system updates the user-specific settings. The system sends this message immediately after updating the settings.  
        /// </summary>  
        USERCHANGED = 0x0054,
        /// <summary>  
        /// Determines if a window accepts ANSI or Unicode structures in the WM_NOTIFY notification message. WM_NOTIFYFORMAT messages are sent from a common control to its parent window and from the parent window to the common control.  
        /// </summary>  
        NOTIFYFORMAT = 0x0055,
        /// <summary>  
        /// The WM_CONTEXTMENU message notifies a window that the user clicked the right mouse button (right-clicked) in the window.  
        /// </summary>  
        CONTEXTMENU = 0x007B,
        /// <summary>  
        /// The WM_STYLECHANGING message is sent to a window when the SetWindowLong function is about to change one or more of the window's styles.  
        /// </summary>  
        STYLECHANGING = 0x007C,
        /// <summary>  
        /// The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles  
        /// </summary>  
        STYLECHANGED = 0x007D,
        /// <summary>  
        /// The WM_DISPLAYCHANGE message is sent to all windows when the display resolution has changed.  
        /// </summary>  
        DISPLAYCHANGE = 0x007E,
        /// <summary>  
        /// The WM_GETICON message is sent to a window to retrieve a handle to the large or small icon associated with a window. The system displays the large icon in the ALT+TAB dialog, and the small icon in the window caption.   
        /// </summary>  
        GETICON = 0x007F,
        /// <summary>  
        /// An application sends the WM_SETICON message to associate a new large or small icon with a window. The system displays the large icon in the ALT+TAB dialog box, and the small icon in the window caption.   
        /// </summary>  
        SETICON = 0x0080,
        /// <summary>  
        /// The WM_NCCREATE message is sent prior to the WM_CREATE message when a window is first created.  
        /// </summary>  
        NCCREATE = 0x0081,
        /// <summary>  
        /// The WM_NCDESTROY message informs a window that its nonclient area is being destroyed. The DestroyWindow function sends the WM_NCDESTROY message to the window following the WM_DESTROY message. WM_DESTROY is used to free the allocated memory object associated with the window.   
        /// The WM_NCDESTROY message is sent after the child windows have been destroyed. In contrast, WM_DESTROY is sent before the child windows are destroyed.  
        /// </summary>  
        NCDESTROY = 0x0082,
        /// <summary>  
        /// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.  
        /// </summary>  
        NCCALCSIZE = 0x0083,
        /// <summary>  
        /// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.  
        /// </summary>  
        NCHITTEST = 0x0084,
        /// <summary>  
        /// The WM_NCPAINT message is sent to a window when its frame must be painted.   
        /// </summary>  
        NCPAINT = 0x0085,
        /// <summary>  
        /// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.  
        /// </summary>  
        NCACTIVATE = 0x0086,
        /// <summary>  
        /// The WM_GETDLGCODE message is sent to the window procedure associated with a control. By default, the system handles all keyboard input to the control; the system interprets certain types of keyboard input as dialog box navigation keys. To override this default behavior, the control can respond to the WM_GETDLGCODE message to indicate the types of input it wants to process itself.  
        /// </summary>  
        GETDLGCODE = 0x0087,
        /// <summary>  
        /// The WM_SYNCPAINT message is used to synchronize painting while avoiding linking independent GUI threads.  
        /// </summary>  
        SYNCPAINT = 0x0088,
        /// <summary>  
        /// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCMOUSEMOVE = 0x00A0,
        /// <summary>  
        /// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCLBUTTONDOWN = 0x00A1,
        /// <summary>  
        /// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCLBUTTONUP = 0x00A2,
        /// <summary>  
        /// The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCLBUTTONDBLCLK = 0x00A3,
        /// <summary>  
        /// The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCRBUTTONDOWN = 0x00A4,
        /// <summary>  
        /// The WM_NCRBUTTONUP message is posted when the user releases the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCRBUTTONUP = 0x00A5,
        /// <summary>  
        /// The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCRBUTTONDBLCLK = 0x00A6,
        /// <summary>  
        /// The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCMBUTTONDOWN = 0x00A7,
        /// <summary>  
        /// The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCMBUTTONUP = 0x00A8,
        /// <summary>  
        /// The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCMBUTTONDBLCLK = 0x00A9,
        /// <summary>  
        /// The WM_NCXBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCXBUTTONDOWN = 0x00AB,
        /// <summary>  
        /// The WM_NCXBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCXBUTTONUP = 0x00AC,
        /// <summary>  
        /// The WM_NCXBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.  
        /// </summary>  
        NCXBUTTONDBLCLK = 0x00AD,
        /// <summary>  
        /// The WM_INPUT_DEVICE_CHANGE message is sent to the window that registered to receive raw input. A window receives this message through its WindowProc function.  
        /// </summary>  
        INPUT_DEVICE_CHANGE = 0x00FE,
        /// <summary>  
        /// The WM_INPUT message is sent to the window that is getting raw input.   
        /// </summary>  
        INPUT = 0x00FF,
        /// <summary>  
        /// This message filters for keyboard messages.  
        /// </summary>  
        KEYFIRST = 0x0100,
        /// <summary>  
        /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.   
        /// </summary>  
        KEYDOWN = 0x0100,
        /// <summary>  
        /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, or a keyboard key that is pressed when a window has the keyboard focus.   
        /// </summary>  
        KEYUP = 0x0101,
        /// <summary>  
        /// The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed.   
        /// </summary>  
        CHAR = 0x0102,
        /// <summary>  
        /// The WM_DEADCHAR message is posted to the window with the keyboard focus when a WM_KEYUP message is translated by the TranslateMessage function. WM_DEADCHAR specifies a character code generated by a dead key. A dead key is a key that generates a character, such as the umlaut (double-dot), that is combined with another character to form a composite character. For example, the umlaut-O character (Ö) is generated by typing the dead key for the umlaut character, and then typing the O key.   
        /// </summary>  
        DEADCHAR = 0x0103,
        /// <summary>  
        /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10 key (which activates the menu bar) or holds down the ALT key and then presses another key. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.   
        /// </summary>  
        SYSKEYDOWN = 0x0104,
        /// <summary>  
        /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases a key that was pressed while the ALT key was held down. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.   
        /// </summary>  
        SYSKEYUP = 0x0105,
        /// <summary>  
        /// The WM_SYSCHAR message is posted to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. It specifies the character code of a system character key — that is, a character key that is pressed while the ALT key is down.   
        /// </summary>  
        SYSCHAR = 0x0106,
        /// <summary>  
        /// The WM_SYSDEADCHAR message is sent to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. WM_SYSDEADCHAR specifies the character code of a system dead key — that is, a dead key that is pressed while holding down the ALT key.   
        /// </summary>  
        SYSDEADCHAR = 0x0107,
        /// <summary>  
        /// The WM_UNICHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_UNICHAR message contains the character code of the key that was pressed.   
        /// The WM_UNICHAR message is equivalent to WM_CHAR, but it uses Unicode Transformation Format (UTF)-32, whereas WM_CHAR uses UTF-16. It is designed to send or post Unicode characters to ANSI windows and it can can handle Unicode Supplementary Plane characters.  
        /// </summary>  
        UNICHAR = 0x0109,
        /// <summary>  
        /// This message filters for keyboard messages.  
        /// </summary>  
        KEYLAST = 0x0109,
        /// <summary>  
        /// Sent immediately before the IME generates the composition string as a result of a keystroke. A window receives this message through its WindowProc function.   
        /// </summary>  
        IME_STARTCOMPOSITION = 0x010D,
        /// <summary>  
        /// Sent to an application when the IME ends composition. A window receives this message through its WindowProc function.   
        /// </summary>  
        IME_ENDCOMPOSITION = 0x010E,
        /// <summary>  
        /// Sent to an application when the IME changes composition status as a result of a keystroke. A window receives this message through its WindowProc function.   
        /// </summary>  
        IME_COMPOSITION = 0x010F,
        IME_KEYLAST = 0x010F,
        /// <summary>  
        /// The WM_INITDIALOG message is sent to the dialog box procedure immediately before a dialog box is displayed. Dialog box procedures typically use this message to initialize controls and carry out any other initialization tasks that affect the appearance of the dialog box.   
        /// </summary>  
        INITDIALOG = 0x0110,
        /// <summary>  
        /// The WM_COMMAND message is sent when the user selects a command item from a menu, when a control sends a notification message to its parent window, or when an accelerator keystroke is translated.   
        /// </summary>  
        COMMAND = 0x0111,
        /// <summary>  
        /// A window receives this message when the user chooses a command from the Window menu, clicks the maximize button, minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering this out.  
        /// </summary>  
        SYSCOMMAND = 0x0112,
        /// <summary>  
        /// The WM_TIMER message is posted to the installing thread's message queue when a timer expires. The message is posted by the GetMessage or PeekMessage function.   
        /// </summary>  
        TIMER = 0x0113,
        /// <summary>  
        /// The WM_HSCROLL message is sent to a window when a scroll event occurs in the window's standard horizontal scroll bar. This message is also sent to the owner of a horizontal scroll bar control when a scroll event occurs in the control.   
        /// </summary>  
        HSCROLL = 0x0114,
        /// <summary>  
        /// The WM_VSCROLL message is sent to a window when a scroll event occurs in the window's standard vertical scroll bar. This message is also sent to the owner of a vertical scroll bar control when a scroll event occurs in the control.   
        /// </summary>  
        VSCROLL = 0x0115,
        /// <summary>  
        /// The WM_INITMENU message is sent when a menu is about to become active. It occurs when the user clicks an item on the menu bar or presses a menu key. This allows the application to modify the menu before it is displayed.   
        /// </summary>  
        INITMENU = 0x0116,
        /// <summary>  
        /// The WM_INITMENUPOPUP message is sent when a drop-down menu or submenu is about to become active. This allows an application to modify the menu before it is displayed, without changing the entire menu.   
        /// </summary>  
        INITMENUPOPUP = 0x0117,
        /// <summary>  
        /// The WM_MENUSELECT message is sent to a menu's owner window when the user selects a menu item.   
        /// </summary>  
        MENUSELECT = 0x011F,
        /// <summary>  
        /// The WM_MENUCHAR message is sent when a menu is active and the user presses a key that does not correspond to any mnemonic or accelerator key. This message is sent to the window that owns the menu.   
        /// </summary>  
        MENUCHAR = 0x0120,
        /// <summary>  
        /// The WM_ENTERIDLE message is sent to the owner window of a modal dialog box or menu that is entering an idle state. A modal dialog box or menu enters an idle state when no messages are waiting in its queue after it has processed one or more previous messages.   
        /// </summary>  
        ENTERIDLE = 0x0121,
        /// <summary>  
        /// The WM_MENURBUTTONUP message is sent when the user releases the right mouse button while the cursor is on a menu item.   
        /// </summary>  
        MENURBUTTONUP = 0x0122,
        /// <summary>  
        /// The WM_MENUDRAG message is sent to the owner of a drag-and-drop menu when the user drags a menu item.   
        /// </summary>  
        MENUDRAG = 0x0123,
        /// <summary>  
        /// The WM_MENUGETOBJECT message is sent to the owner of a drag-and-drop menu when the mouse cursor enters a menu item or moves from the center of the item to the top or bottom of the item.   
        /// </summary>  
        MENUGETOBJECT = 0x0124,
        /// <summary>  
        /// The WM_UNINITMENUPOPUP message is sent when a drop-down menu or submenu has been destroyed.   
        /// </summary>  
        UNINITMENUPOPUP = 0x0125,
        /// <summary>  
        /// The WM_MENUCOMMAND message is sent when the user makes a selection from a menu.   
        /// </summary>  
        MENUCOMMAND = 0x0126,
        /// <summary>  
        /// An application sends the WM_CHANGEUISTATE message to indicate that the user interface (UI) state should be changed.  
        /// </summary>  
        CHANGEUISTATE = 0x0127,
        /// <summary>  
        /// An application sends the WM_UPDATEUISTATE message to change the user interface (UI) state for the specified window and all its child windows.  
        /// </summary>  
        UPDATEUISTATE = 0x0128,
        /// <summary>  
        /// An application sends the WM_QUERYUISTATE message to retrieve the user interface (UI) state for a window.  
        /// </summary>  
        QUERYUISTATE = 0x0129,
        /// <summary>  
        /// The WM_CTLCOLORMSGBOX message is sent to the owner window of a message box before Windows draws the message box. By responding to this message, the owner window can set the text and background colors of the message box by using the given display device context handle.   
        /// </summary>  
        CTLCOLORMSGBOX = 0x0132,
        /// <summary>  
        /// An edit control that is not read-only or disabled sends the WM_CTLCOLOREDIT message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the edit control.   
        /// </summary>  
        CTLCOLOREDIT = 0x0133,
        /// <summary>  
        /// Sent to the parent window of a list box before the system draws the list box. By responding to this message, the parent window can set the text and background colors of the list box by using the specified display device context handle.   
        /// </summary>  
        CTLCOLORLISTBOX = 0x0134,
        /// <summary>  
        /// The WM_CTLCOLORBTN message is sent to the parent window of a button before drawing the button. The parent window can change the button's text and background colors. However, only owner-drawn buttons respond to the parent window processing this message.   
        /// </summary>  
        CTLCOLORBTN = 0x0135,
        /// <summary>  
        /// The WM_CTLCOLORDLG message is sent to a dialog box before the system draws the dialog box. By responding to this message, the dialog box can set its text and background colors using the specified display device context handle.   
        /// </summary>  
        CTLCOLORDLG = 0x0136,
        /// <summary>  
        /// The WM_CTLCOLORSCROLLBAR message is sent to the parent window of a scroll bar control when the control is about to be drawn. By responding to this message, the parent window can use the display context handle to set the background color of the scroll bar control.   
        /// </summary>  
        CTLCOLORSCROLLBAR = 0x0137,
        /// <summary>  
        /// A static control, or an edit control that is read-only or disabled, sends the WM_CTLCOLORSTATIC message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the static control.   
        /// </summary>  
        CTLCOLORSTATIC = 0x0138,
        /// <summary>  
        /// Use WM_MOUSEFIRST to specify the first mouse message. Use the PeekMessage() Function.  
        /// </summary>  
        MOUSEFIRST = 0x0200,
        /// <summary>  
        /// The WM_MOUSEMOVE message is posted to a window when the cursor moves. If the mouse is not captured, the message is posted to the window that contains the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        MOUSEMOVE = 0x0200,
        /// <summary>  
        /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        LBUTTONDOWN = 0x0201,
        /// <summary>  
        /// The WM_LBUTTONUP message is posted when the user releases the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        LBUTTONUP = 0x0202,
        /// <summary>  
        /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        LBUTTONDBLCLK = 0x0203,
        /// <summary>  
        /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        RBUTTONDOWN = 0x0204,
        /// <summary>  
        /// The WM_RBUTTONUP message is posted when the user releases the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        RBUTTONUP = 0x0205,
        /// <summary>  
        /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        RBUTTONDBLCLK = 0x0206,
        /// <summary>  
        /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        MBUTTONDOWN = 0x0207,
        /// <summary>  
        /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        MBUTTONUP = 0x0208,
        /// <summary>  
        /// The WM_MBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        MBUTTONDBLCLK = 0x0209,
        /// <summary>  
        /// The WM_MOUSEWHEEL message is sent to the focus window when the mouse wheel is rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.  
        /// </summary>  
        MOUSEWHEEL = 0x020A,
        /// <summary>  
        /// The WM_XBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.   
        /// </summary>  
        XBUTTONDOWN = 0x020B,
        /// <summary>  
        /// The WM_XBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        XBUTTONUP = 0x020C,
        /// <summary>  
        /// The WM_XBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.  
        /// </summary>  
        XBUTTONDBLCLK = 0x020D,
        /// <summary>  
        /// The WM_MOUSEHWHEEL message is sent to the focus window when the mouse's horizontal scroll wheel is tilted or rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.  
        /// </summary>  
        MOUSEHWHEEL = 0x020E,
        /// <summary>  
        /// Use WM_MOUSELAST to specify the last mouse message. Used with PeekMessage() Function.  
        /// </summary>  
        MOUSELAST = 0x020E,
        /// <summary>  
        /// The WM_PARENTNOTIFY message is sent to the parent of a child window when the child window is created or destroyed, or when the user clicks a mouse button while the cursor is over the child window. When the child window is being created, the system sends WM_PARENTNOTIFY just before the CreateWindow or CreateWindowEx function that creates the window returns. When the child window is being destroyed, the system sends the message before any processing to destroy the window takes place.  
        /// </summary>  
        PARENTNOTIFY = 0x0210,
        /// <summary>  
        /// The WM_ENTERMENULOOP message informs an application's main window procedure that a menu modal loop has been entered.   
        /// </summary>  
        ENTERMENULOOP = 0x0211,
        /// <summary>  
        /// The WM_EXITMENULOOP message informs an application's main window procedure that a menu modal loop has been exited.   
        /// </summary>  
        EXITMENULOOP = 0x0212,
        /// <summary>  
        /// The WM_NEXTMENU message is sent to an application when the right or left arrow key is used to switch between the menu bar and the system menu.   
        /// </summary>  
        NEXTMENU = 0x0213,
        /// <summary>  
        /// The WM_SIZING message is sent to a window that the user is resizing. By processing this message, an application can monitor the size and position of the drag rectangle and, if needed, change its size or position.   
        /// </summary>  
        SIZING = 0x0214,
        /// <summary>  
        /// The WM_CAPTURECHANGED message is sent to the window that is losing the mouse capture.  
        /// </summary>  
        CAPTURECHANGED = 0x0215,
        /// <summary>  
        /// The WM_MOVING message is sent to a window that the user is moving. By processing this message, an application can monitor the position of the drag rectangle and, if needed, change its position.  
        /// </summary>  
        MOVING = 0x0216,
        /// <summary>  
        /// Notifies applications that a power-management event has occurred.  
        /// </summary>  
        POWERBROADCAST = 0x0218,
        /// <summary>  
        /// Notifies an application of a change to the hardware configuration of a device or the computer.  
        /// </summary>  
        DEVICECHANGE = 0x0219,
        /// <summary>  
        /// An application sends the WM_MDICREATE message to a multiple-document interface (MDI) client window to create an MDI child window.   
        /// </summary>  
        MDICREATE = 0x0220,
        /// <summary>  
        /// An application sends the WM_MDIDESTROY message to a multiple-document interface (MDI) client window to close an MDI child window.   
        /// </summary>  
        MDIDESTROY = 0x0221,
        /// <summary>  
        /// An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window.   
        /// </summary>  
        MDIACTIVATE = 0x0222,
        /// <summary>  
        /// An application sends the WM_MDIRESTORE message to a multiple-document interface (MDI) client window to restore an MDI child window from maximized or minimized size.   
        /// </summary>  
        MDIRESTORE = 0x0223,
        /// <summary>  
        /// An application sends the WM_MDINEXT message to a multiple-document interface (MDI) client window to activate the next or previous child window.   
        /// </summary>  
        MDINEXT = 0x0224,
        /// <summary>  
        /// An application sends the WM_MDIMAXIMIZE message to a multiple-document interface (MDI) client window to maximize an MDI child window. The system resizes the child window to make its client area fill the client window. The system places the child window's window menu icon in the rightmost position of the frame window's menu bar, and places the child window's restore icon in the leftmost position. The system also appends the title bar text of the child window to that of the frame window.   
        /// </summary>  
        MDIMAXIMIZE = 0x0225,
        /// <summary>  
        /// An application sends the WM_MDITILE message to a multiple-document interface (MDI) client window to arrange all of its MDI child windows in a tile format.   
        /// </summary>  
        MDITILE = 0x0226,
        /// <summary>  
        /// An application sends the WM_MDICASCADE message to a multiple-document interface (MDI) client window to arrange all its child windows in a cascade format.   
        /// </summary>  
        MDICASCADE = 0x0227,
        /// <summary>  
        /// An application sends the WM_MDIICONARRANGE message to a multiple-document interface (MDI) client window to arrange all minimized MDI child windows. It does not affect child windows that are not minimized.   
        /// </summary>  
        MDIICONARRANGE = 0x0228,
        /// <summary>  
        /// An application sends the WM_MDIGETACTIVE message to a multiple-document interface (MDI) client window to retrieve the handle to the active MDI child window.   
        /// </summary>  
        MDIGETACTIVE = 0x0229,
        /// <summary>  
        /// An application sends the WM_MDISETMENU message to a multiple-document interface (MDI) client window to replace the entire menu of an MDI frame window, to replace the window menu of the frame window, or both.   
        /// </summary>  
        MDISETMENU = 0x0230,
        /// <summary>  
        /// The WM_ENTERSIZEMOVE message is sent one time to a window after it enters the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.   
        /// The system sends the WM_ENTERSIZEMOVE message regardless of whether the dragging of full windows is enabled.  
        /// </summary>  
        ENTERSIZEMOVE = 0x0231,
        /// <summary>  
        /// The WM_EXITSIZEMOVE message is sent one time to a window, after it has exited the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.   
        /// </summary>  
        EXITSIZEMOVE = 0x0232,
        /// <summary>  
        /// Sent when the user drops a file on the window of an application that has registered itself as a recipient of dropped files.  
        /// </summary>  
        DROPFILES = 0x0233,
        /// <summary>  
        /// An application sends the WM_MDIREFRESHMENU message to a multiple-document interface (MDI) client window to refresh the window menu of the MDI frame window.   
        /// </summary>  
        MDIREFRESHMENU = 0x0234,
        /// <summary>  
        /// Sent to an application when a window is activated. A window receives this message through its WindowProc function.   
        /// </summary>  
        IME_SETCONTEXT = 0x0281,
        /// <summary>  
        /// Sent to an application to notify it of changes to the IME window. A window receives this message through its WindowProc function.   
        /// </summary>  
        IME_NOTIFY = 0x0282,
        /// <summary>  
        /// Sent by an application to direct the IME window to carry out the requested command. The application uses this message to control the IME window that it has created. To send this message, the application calls the SendMessage function with the following parameters.  
        /// </summary>  
        IME_CONTROL = 0x0283,
        /// <summary>  
        /// Sent to an application when the IME window finds no space to extend the area for the composition window. A window receives this message through its WindowProc function.   
        /// </summary>  
        IME_COMPOSITIONFULL = 0x0284,
        /// <summary>  
        /// Sent to an application when the operating system is about to change the current IME. A window receives this message through its WindowProc function.   
        /// </summary>  
        IME_SELECT = 0x0285,
        /// <summary>  
        /// Sent to an application when the IME gets a character of the conversion result. A window receives this message through its WindowProc function.   
        /// </summary>  
        IME_CHAR = 0x0286,
        /// <summary>  
        /// Sent to an application to provide commands and request information. A window receives this message through its WindowProc function.   
        /// </summary>  
        IME_REQUEST = 0x0288,
        /// <summary>  
        /// Sent to an application by the IME to notify the application of a key press and to keep message order. A window receives this message through its WindowProc function.   
        /// </summary>  
        IME_KEYDOWN = 0x0290,
        /// <summary>  
        /// Sent to an application by the IME to notify the application of a key release and to keep message order. A window receives this message through its WindowProc function.   
        /// </summary>  
        IME_KEYUP = 0x0291,
        /// <summary>  
        /// The WM_MOUSEHOVER message is posted to a window when the cursor hovers over the client area of the window for the period of time specified in a prior call to TrackMouseEvent.  
        /// </summary>  
        MOUSEHOVER = 0x02A1,
        /// <summary>  
        /// The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.  
        /// </summary>  
        MOUSELEAVE = 0x02A3,
        /// <summary>  
        /// The WM_NCMOUSEHOVER message is posted to a window when the cursor hovers over the nonclient area of the window for the period of time specified in a prior call to TrackMouseEvent.  
        /// </summary>  
        NCMOUSEHOVER = 0x02A0,
        /// <summary>  
        /// The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.  
        /// </summary>  
        NCMOUSELEAVE = 0x02A2,
        /// <summary>  
        /// The WM_WTSSESSION_CHANGE message notifies applications of changes in session state.  
        /// </summary>  
        WTSSESSION_CHANGE = 0x02B1,
        TABLET_FIRST = 0x02c0,
        TABLET_LAST = 0x02df,
        /// <summary>  
        /// An application sends a WM_CUT message to an edit control or combo box to delete (cut) the current selection, if any, in the edit control and copy the deleted text to the clipboard in CF_TEXT format.   
        /// </summary>  
        CUT = 0x0300,
        /// <summary>  
        /// An application sends the WM_COPY message to an edit control or combo box to copy the current selection to the clipboard in CF_TEXT format.   
        /// </summary>  
        COPY = 0x0301,
        /// <summary>  
        /// An application sends a WM_PASTE message to an edit control or combo box to copy the current content of the clipboard to the edit control at the current caret position. Data is inserted only if the clipboard contains data in CF_TEXT format.   
        /// </summary>  
        PASTE = 0x0302,
        /// <summary>  
        /// An application sends a WM_CLEAR message to an edit control or combo box to delete (clear) the current selection, if any, from the edit control.   
        /// </summary>  
        CLEAR = 0x0303,
        /// <summary>  
        /// An application sends a WM_UNDO message to an edit control to undo the last operation. When this message is sent to an edit control, the previously deleted text is restored or the previously added text is deleted.  
        /// </summary>  
        UNDO = 0x0304,
        /// <summary>  
        /// The WM_RENDERFORMAT message is sent to the clipboard owner if it has delayed rendering a specific clipboard format and if an application has requested data in that format. The clipboard owner must render data in the specified format and place it on the clipboard by calling the SetClipboardData function.   
        /// </summary>  
        RENDERFORMAT = 0x0305,
        /// <summary>  
        /// The WM_RENDERALLFORMATS message is sent to the clipboard owner before it is destroyed, if the clipboard owner has delayed rendering one or more clipboard formats. For the content of the clipboard to remain available to other applications, the clipboard owner must render data in all the formats it is capable of generating, and place the data on the clipboard by calling the SetClipboardData function.   
        /// </summary>  
        RENDERALLFORMATS = 0x0306,
        /// <summary>  
        /// The WM_DESTROYCLIPBOARD message is sent to the clipboard owner when a call to the EmptyClipboard function empties the clipboard.   
        /// </summary>  
        DESTROYCLIPBOARD = 0x0307,
        /// <summary>  
        /// The WM_DRAWCLIPBOARD message is sent to the first window in the clipboard viewer chain when the content of the clipboard changes. This enables a clipboard viewer window to display the new content of the clipboard.   
        /// </summary>  
        DRAWCLIPBOARD = 0x0308,
        /// <summary>  
        /// The WM_PAINTCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area needs repainting.   
        /// </summary>  
        PAINTCLIPBOARD = 0x0309,
        /// <summary>  
        /// The WM_VSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's vertical scroll bar. The owner should scroll the clipboard image and update the scroll bar values.   
        /// </summary>  
        VSCROLLCLIPBOARD = 0x030A,
        /// <summary>  
        /// The WM_SIZECLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area has changed size.   
        /// </summary>  
        SIZECLIPBOARD = 0x030B,
        /// <summary>  
        /// The WM_ASKCBFORMATNAME message is sent to the clipboard owner by a clipboard viewer window to request the name of a CF_OWNERDISPLAY clipboard format.  
        /// </summary>  
        ASKCBFORMATNAME = 0x030C,
        /// <summary>  
        /// The WM_CHANGECBCHAIN message is sent to the first window in the clipboard viewer chain when a window is being removed from the chain.   
        /// </summary>  
        CHANGECBCHAIN = 0x030D,
        /// <summary>  
        /// The WM_HSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window. This occurs when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's horizontal scroll bar. The owner should scroll the clipboard image and update the scroll bar values.   
        /// </summary>  
        HSCROLLCLIPBOARD = 0x030E,
        /// <summary>  
        /// This message informs a window that it is about to receive the keyboard focus, giving the window the opportunity to realize its logical palette when it receives the focus.   
        /// </summary>  
        QUERYNEWPALETTE = 0x030F,
        /// <summary>  
        /// The WM_PALETTEISCHANGING message informs applications that an application is going to realize its logical palette.   
        /// </summary>  
        PALETTEISCHANGING = 0x0310,
        /// <summary>  
        /// This message is sent by the OS to all top-level and overlapped windows after the window with the keyboard focus realizes its logical palette.   
        /// This message enables windows that do not have the keyboard focus to realize their logical palettes and update their client areas.  
        /// </summary>  
        PALETTECHANGED = 0x0311,
        /// <summary>  
        /// The WM_HOTKEY message is posted when the user presses a hot key registered by the RegisterHotKey function. The message is placed at the top of the message queue associated with the thread that registered the hot key.   
        /// </summary>  
        HOTKEY = 0x0312,
        /// <summary>  
        /// The WM_PRINT message is sent to a window to request that it draw itself in the specified device context, most commonly in a printer device context.  
        /// </summary>  
        PRINT = 0x0317,
        /// <summary>  
        /// The WM_PRINTCLIENT message is sent to a window to request that it draw its client area in the specified device context, most commonly in a printer device context.  
        /// </summary>  
        PRINTCLIENT = 0x0318,
        /// <summary>  
        /// The WM_APPCOMMAND message notifies a window that the user generated an application command event, for example, by clicking an application command button using the mouse or typing an application command key on the keyboard.  
        /// </summary>  
        APPCOMMAND = 0x0319,
        /// <summary>  
        /// The WM_THEMECHANGED message is broadcast to every window following a theme change event. Examples of theme change events are the activation of a theme, the deactivation of a theme, or a transition from one theme to another.  
        /// </summary>  
        THEMECHANGED = 0x031A,
        /// <summary>  
        /// Sent when the contents of the clipboard have changed.  
        /// </summary>  
        CLIPBOARDUPDATE = 0x031D,
        /// <summary>  
        /// The system will send a window the WM_DWMCOMPOSITIONCHANGED message to indicate that the availability of desktop composition has changed.  
        /// </summary>  
        DWMCOMPOSITIONCHANGED = 0x031E,
        /// <summary>  
        /// WM_DWMNCRENDERINGCHANGED is called when the non-client area rendering status of a window has changed. Only windows that have set the flag DWM_BLURBEHIND.fTransitionOnMaximized to true will get this message.   
        /// </summary>  
        DWMNCRENDERINGCHANGED = 0x031F,
        /// <summary>  
        /// Sent to all top-level windows when the colorization color has changed.   
        /// </summary>  
        DWMCOLORIZATIONCOLORCHANGED = 0x0320,
        /// <summary>  
        /// WM_DWMWINDOWMAXIMIZEDCHANGE will let you know when a DWM composed window is maximized. You also have to register for this message as well. You'd have other windowd go opaque when this message is sent.  
        /// </summary>  
        DWMWINDOWMAXIMIZEDCHANGE = 0x0321,
        /// <summary>  
        /// Sent to request extended title bar information. A window receives this message through its WindowProc function.  
        /// </summary>  
        GETTITLEBARINFOEX = 0x033F,
        HANDHELDFIRST = 0x0358,
        HANDHELDLAST = 0x035F,
        AFXFIRST = 0x0360,
        AFXLAST = 0x037F,
        PENWINFIRST = 0x0380,
        PENWINLAST = 0x038F,
        /// <summary>  
        /// The WM_APP constant is used by applications to help define private messages, usually of the form WM_APP+X, where X is an integer value.   
        /// </summary>  
        APP = 0x8000,
        /// <summary>  
        /// The WM_USER constant is used by applications to help define private messages for use by private window classes, usually of the form WM_USER+X, where X is an integer value.   
        /// </summary>  
        USER = 0x0400,

        /// <summary>  
        /// An application sends the WM_CPL_LAUNCH message to Windows Control Panel to request that a Control Panel application be started.   
        /// </summary>  
        CPL_LAUNCH = USER + 0x1000,
        /// <summary>  
        /// The WM_CPL_LAUNCHED message is sent when a Control Panel application, started by the WM_CPL_LAUNCH message, has closed. The WM_CPL_LAUNCHED message is sent to the window identified by the wParam parameter of the WM_CPL_LAUNCH message that started the application.   
        /// </summary>  
        CPL_LAUNCHED = USER + 0x1001,
        /// <summary>  
        /// WM_SYSTIMER is a well-known yet still undocumented message. Windows uses WM_SYSTIMER for internal actions like scrolling.  
        /// </summary>  
        SYSTIMER = 0x118
    }


    To use the functions and the message structures, we can use the following code.


   C#代码  收藏代码
class Program
    {
        private static IntPtr hinst;
        private static UInt16 atom;
        static void Main(string[] args)
        {
            Main2(System.Diagnostics.Process.GetCurrentProcess().Handle, IntPtr.Zero, string.Empty, (int)ShowWindowCommands.Normal);

            //MessagePump messagePump = new MessagePump();  
            //messagePump.CreateMessagePump(IntPtr.Zero, IntPtr.Zero, "some string", 0);  
        }

        // Reference this page : Using Window Class -http://msdn.microsoft.com/en-us/library/ms633575%28v=VS.85%29.aspx  

        static bool Main2(IntPtr hinstance, IntPtr hPrevInstance, string lpCmdLine, int nCmdShow)
        {
            MSG msg;

            if (!InitApplication(hinstance))
                return false;

            if (!InitInstance(hinstance, nCmdShow))
                return false;

            sbyte hasMessage;

            while ((hasMessage = WinAPI.GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0 && hasMessage != -1)
            {
                WinAPI.TranslateMessage(ref msg);
                WinAPI.DispatchMessage(ref msg);
            }
            return msg.wParam == UIntPtr.Zero;
            //UNREFERENCED_PARAMETER(lpCmdLine);   


        }


        private static bool InitApplication(IntPtr hinstance)
        {

            WNDCLASSEX wcx = new WNDCLASSEX();

            wcx.cbSize = Marshal.SizeOf(wcx);
            wcx.style = (int)(ClassStyles.VerticalRedraw | ClassStyles.HorizontalRedraw);

            unsafe
            {
                //IntPtr address = MainWndProc; -- this is not necessary to put inside a Unsafe context  
                IntPtr address2 = Marshal.GetFunctionPointerForDelegate((Delegate)(WndProc)MainWndProc);
                wcx.lpfnWndProc = address2;
            }

            wcx.cbClsExtra = 0;
            wcx.cbWndExtra = 0;
            wcx.hInstance = hinstance;
            wcx.hIcon = WinAPI.LoadIcon(
                    IntPtr.Zero, new IntPtr((int)SystemIcons.IDI_APPLICATION));
            //wndClass.hCursor = WinAPI.LoadCursor(IntPtr.Zero, (int)IdcStandardCursor.IDC_ARROW);  
            wcx.hCursor = WinAPI.LoadCursor(IntPtr.Zero, (int)Win32_IDC_Constants.IDC_ARROW);
            wcx.hbrBackground = WinAPI.GetStockObject(StockObjects.WHITE_BRUSH);
            wcx.lpszMenuName = "MainMenu";
            wcx.lpszClassName = "MainWClass";
            //     wcx.hIconSm = LoadImage(hinstance, // small class icon   
            //MAKEINTRESOURCE(5),  
            //IMAGE_ICON,  
            //GetSystemMetrics(SM_CXSMICON),  
            //GetSystemMetrics(SM_CYSMICON),  
            //LR_DEFAULTCOLOR);   

            // it might be as this:  
            //   problems with p/invoke CreateWindowEx() and RegisterClassEx()  
            //  http://social.msdn.microsoft.com/Forums/vstudio/en-US/8580a805-383b-4b17-8bd8-514da4a5f3a4/problems-with-pinvoke-createwindowex-and-registerclassex   
            // ATOM?  
            UInt16 ret = WinAPI.RegisterClassEx2(ref wcx);
            if (ret != 0)
            {
                string message = new Win32Exception(Marshal.GetLastWin32Error()).Message;
                Console.WriteLine("Failed to call RegisterClasEx, error = {0}", message);
            }
            //return WinAPI.RegisterClassEx(ref wcx) != 0;  
            atom = ret;
            return ret != 0;
        }

        private static bool InitInstance(IntPtr hInstance, int nCmdShow)
        {
            IntPtr hwnd;

            hinst = hInstance;
            short a = 0;

            hwnd = WinAPI.CreateWindowEx2(
                0,
                //"MainWClass",  
                atom,
                "Sample",
                WindowStyles.WS_OVERLAPPED,
                Win32_CW_Constant.CW_USEDEFAULT,
                Win32_CW_Constant.CW_USEDEFAULT,
                Win32_CW_Constant.CW_USEDEFAULT,
                Win32_CW_Constant.CW_USEDEFAULT,
                IntPtr.Zero,
                IntPtr.Zero,
                hInstance,
                IntPtr.Zero);
            if (hwnd == IntPtr.Zero)
            {
                string error = new Win32Exception(Marshal.GetLastWin32Error()).Message;
                Console.WriteLine("Failed to InitInstance , error = {0}", error);
                return false;
            }
            WinAPI.ShowWindow(hwnd, (ShowWindowCommands)nCmdShow);
            WinAPI.UpdateWindow(hwnd);
            return true;
        }

        // check this post - http://stackoverflow.com/questions/1969049/c-sharp-p-invoke-marshalling-structures-containing-function-pointers  
        //   
        static IntPtr MainWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr hdc;
            PAINTSTRUCT ps;
            RECT rect;
            //switch ((WM) message)  
            //{  
            //    WinAPI.BeginPaint(hWnd, out ps);  
            //    break;  
            //}  
            switch ((WM)msg)
            {
                case WM.PAINT:
                    hdc = WinAPI.BeginPaint(hWnd, out ps);
                    WinAPI.GetClientRect(hWnd, out rect);
                    WinAPI.DrawText(hdc, "Hello, Windows 98!", -1, ref rect, Win32_DT_Constant.DT_SINGLELINE | Win32_DT_Constant.DT_CENTER | Win32_DT_Constant.DT_VCENTER);
                    WinAPI.EndPaint(hWnd, ref ps);
                    return IntPtr.Zero;
                    break;
                case WM.DESTROY:
                    WinAPI.PostQuitMessage(0);
                    return IntPtr.Zero;
                    break;
            }

            return WinAPI.DefWindowProc(hWnd, (WM)msg, wParam, lParam);
        }
    }


    Or,  you can use the following code. - it is just the same code but with different code structure.

   C#代码  收藏代码
public class MessagePump
    {
        // please refer the following code to   
        //  http://www.pinvoke.net/default.aspx/user32/CreateWindowEx.html  
        // and check on this for the marshal  
        // http://www.pinvoke.net/default.aspx/Structures.WNDCLASS  
        public void CreateMessagePump(IntPtr hInstance, IntPtr hPrevInstance, string lpszCmdLine, int nCmdShow)
        {
            //MSG msg;  
            bool bRet;
            WNDCLASS wc;

            lpszCmdLine = null;

            hInstance = System.Diagnostics.Process.GetCurrentProcess().Handle;
            //WNDCLASS wndClass;  
            //WNDCLASS wndClass = new WNDCLASS();  
            WNDCLASSEX wndClass = new WNDCLASSEX();
            //wndClass.cbSize = sizeof(WNDCLASSEX); // you cannot use the sizeof operator, instead use the Marshal.Sizeof ()   
            wndClass.cbSize = Marshal.SizeOf(typeof(WNDCLASSEX));

            string szName = "HelloWin";
            //wndClass.style = (uint) (ClassStyles.HorizontalRedraw | ClassStyles.VerticalRedraw);  
            //wndClass.style = ClassStyles.HorizontalRedraw | ClassStyles.VerticalRedraw;  
            wndClass.style = (int)(ClassStyles.HorizontalRedraw | ClassStyles.VerticalRedraw);


            // all those Method.MethodHandle.GetFunctionPointer() call is so STUPID  
            //wndClass.lpfnWndProc = ((WndProc)((hWnd, message, wParam, lParam) =>  
            //{  
            //    IntPtr hdc;  
            //    PAINTSTRUCT ps;  
            //    RECT rect;  
            //    //switch ((WM) message)  
            //    //{  
            //    //    WinAPI.BeginPaint(hWnd, out ps);  
            //    //    break;  
            //    //}  
            //    switch ((WM)message)  
            //    {  
            //        case WM.PAINT:  
            //            hdc = WinAPI.BeginPaint(hWnd, out ps);  
            //            WinAPI.GetClientRect(hWnd, out rect);  
            //            WinAPI.DrawText(hdc, "Hello, Windows 98!", -1, ref rect, Win32_DT_Constant.DT_SINGLELINE | Win32_DT_Constant.DT_CENTER | Win32_DT_Constant.DT_VCENTER);  
            //            WinAPI.EndPaint(hWnd, ref ps);  
            //            return IntPtr.Zero;  
            //            break;  
            //        case WM.DESTROY:  
            //            WinAPI.PostQuitMessage(0);  
            //            return IntPtr.Zero;  
            //            break;  
            //    }  

            //    return WinAPI.DefWindowProc(hWnd, (WM)message, wParam, lParam);  
            //}  
            //)).Method.MethodHandle.GetFunctionPointer();  

            wndClass.lpfnWndProc = Marshal.GetFunctionPointerForDelegate((WndProc)((hWnd, message, wParam, lParam) =>
            {
                IntPtr hdc;
                PAINTSTRUCT ps;
                RECT rect;
                //switch ((WM) message)  
                //{  
                //    WinAPI.BeginPaint(hWnd, out ps);  
                //    break;  
                //}  
                switch ((WM)message)
                {
                    case WM.PAINT:
                        hdc = WinAPI.BeginPaint(hWnd, out ps);
                        WinAPI.GetClientRect(hWnd, out rect);
                        WinAPI.DrawText(hdc, "Hello, Windows 98!", -1, ref rect, Win32_DT_Constant.DT_SINGLELINE | Win32_DT_Constant.DT_CENTER | Win32_DT_Constant.DT_VCENTER);
                        WinAPI.EndPaint(hWnd, ref ps);
                        return IntPtr.Zero;
                        break;
                    case WM.DESTROY:
                        WinAPI.PostQuitMessage(0);
                        return IntPtr.Zero;
                        break;
                }

                return WinAPI.DefWindowProc(hWnd, (WM)message, wParam, lParam);
            }
            ));


            //wndClass.lpfnWndProc = (WndProc) ((hWnd, message, wParam, lParam) => {   
            //    IntPtr hdc;  
            //    PAINTSTRUCT ps;  
            //    RECT rect;  
            //    //switch ((WM) message)  
            //    //{  
            //    //    WinAPI.BeginPaint(hWnd, out ps);  
            //    //    break;  
            //    //}  
            //    switch ((WM)message)  
            //    {  
            //        case WM.PAINT:  
            //            hdc = WinAPI.BeginPaint(hWnd, out ps);  
            //            WinAPI.GetClientRect(hWnd, out rect);  
            //            WinAPI.DrawText(hdc, "Hello, Windows 98!", -1, ref rect, Win32_DT_Constant.DT_SINGLELINE | Win32_DT_Constant.DT_CENTER | Win32_DT_Constant.DT_VCENTER);  
            //            WinAPI.EndPaint(hWnd, ref ps);  
            //            return IntPtr.Zero;  
            //            break;  
            //        case WM.DESTROY:  
            //            WinAPI.PostQuitMessage(0);  
            //            return IntPtr.Zero;  
            //            break;  
            //    }  

            //    return WinAPI.DefWindowProc(hWnd, (WM)message, wParam, lParam);  
            //}  
            //);  


            wndClass.cbClsExtra = 0;
            wndClass.cbWndExtra = 0;
            wndClass.hInstance = hInstance;
            wndClass.hIcon = WinAPI.LoadIcon(
                IntPtr.Zero, new IntPtr((int)SystemIcons.IDI_APPLICATION));
            //wndClass.hCursor = WinAPI.LoadCursor(IntPtr.Zero, (int)IdcStandardCursor.IDC_ARROW);  
            wndClass.hCursor = WinAPI.LoadCursor(IntPtr.Zero, (int)Win32_IDC_Constants.IDC_ARROW);
            wndClass.hbrBackground = WinAPI.GetStockObject(StockObjects.WHITE_BRUSH);
            wndClass.lpszMenuName = null;
            wndClass.lpszClassName = szName;


            //WindowStyleEx.WS_EX_OVERLAPPEDWINDOW  
            //ushort regResult = WinAPI.RegisterClass(ref wndClass); // change to RegisterClassEx  
            //ushort regResult = (ushort)WinAPI.RegisterClassEx(ref wndClass); // change the varie RegisterClassEx2  
            UInt16 regRest = WinAPI.RegisterClassEx2(ref wndClass);

            //if (regResult == 0)  
            //{  
            //    int lastError = Marshal.GetLastWin32Error();  
            //    string errorMessage = new Win32Exception(lastError).Message;  

            //    WinAPI.MessageBox(IntPtr.Zero, "This program requires windows NT!",  
            //        szName, (int) MessageBoxOptions.IconError);  
            //}  

            // this varie of CreateWindowEx do no work   
            //IntPtr hwnd = WinAPI.CreateWindowEx(  
            //    0,  
            //    szName,  
            //    "The Hello Program",  
            //    WindowStyles.WS_OVERLAPPEDWINDOW,  
            //    Win32_CW_Constant.CW_USEDEFAULT,  
            //    Win32_CW_Constant.CW_USEDEFAULT,  
            //    Win32_CW_Constant.CW_USEDEFAULT,  
            //    Win32_CW_Constant.CW_USEDEFAULT,  
            //    IntPtr.Zero,  
            //    IntPtr.Zero,  
            //    hInstance,  
            //    IntPtr.Zero);  

            IntPtr hwnd = WinAPI.CreateWindowEx2(
                0,
                regRest,
                "The hello proram",
                WindowStyles.WS_OVERLAPPEDWINDOW,
                Win32_CW_Constant.CW_USEDEFAULT,
                Win32_CW_Constant.CW_USEDEFAULT,
                Win32_CW_Constant.CW_USEDEFAULT,
                Win32_CW_Constant.CW_USEDEFAULT,
                IntPtr.Zero,
                IntPtr.Zero,
                hInstance,
                IntPtr.Zero);

            //IntPtr hwnd = WinAPI.CreateWindowEx(  
            //    WindowStylesEx.WS_EX_OVERLAPPEDWINDOW,  
            //    //new IntPtr((int)(uint)regResult),  
            //    //Marshal.PtrToStringAnsi(new IntPtr((int) (uint) regResult)),  
            //    szName,  
            //    //szName, //Window Class name  
            //    "The Hello Program", // window caption  
            //    WindowStyles.WS_OVERLAPPEDWINDOW, // Window style  
            //    Win32_CW_Constant.CW_USEDEFAULT,  // initial x position  
            //    Win32_CW_Constant.CW_USEDEFAULT, // initial y position  
            //    Win32_CW_Constant.CW_USEDEFAULT, // initial x size  
            //    Win32_CW_Constant.CW_USEDEFAULT, // initial y size  
            //    IntPtr.Zero,                     // parent window handle  
            //    IntPtr.Zero,                    // program menu handle  
            //    hInstance,                      // program instance handle  
            //    IntPtr.Zero);                   // Creation Parameter  

            if (hwnd == IntPtr.Zero)
            {
                int lastError = Marshal.GetLastWin32Error();
                string errorMessage = new Win32Exception(lastError).Message;
            }
            WinAPI.ShowWindow(hwnd, ShowWindowCommands.Normal);
            WinAPI.UpdateWindow(hwnd);
            WinAPI.UpdateWindow(hwnd);
            MSG msg;
            while (WinAPI.GetMessage(out msg, IntPtr.Zero, 0, 0) != 0)
            {
                WinAPI.TranslateMessage(ref msg);
                WinAPI.DispatchMessage(ref msg);
            }

            return;
        }
    }
}
