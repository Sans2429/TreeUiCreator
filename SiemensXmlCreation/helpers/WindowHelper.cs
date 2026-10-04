using System;
using System.Drawing;
using System.Runtime.InteropServices;

public static class WindowHelper
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public static Rectangle GetActiveWindowRectangle()
    {
        IntPtr handle = GetForegroundWindow();
        GetWindowRect(handle, out RECT rect);

        return new Rectangle(
            rect.Left,
            rect.Top,
            rect.Right - rect.Left,
            rect.Bottom - rect.Top);
    }
}
