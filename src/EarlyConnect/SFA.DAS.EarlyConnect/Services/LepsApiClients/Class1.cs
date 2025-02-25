using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

public static class Win32ErrorHelper
{
    // P/Invoke to format a Win32 error code into a readable string
    [DllImport("kernel32.dll")]
    private static extern int FormatMessage(
        int flags,
        IntPtr source,
        int messageId,
        int languageId,
        System.Text.StringBuilder buffer,
        int size,
        IntPtr arguments);

    private const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
    private const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;

    // Converts a Win32 error code to a readable string
    public static string GetErrorMessage(int errorCode)
    {
        var messageBuffer = new System.Text.StringBuilder(256);

        int messageLength = FormatMessage(
            FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
            IntPtr.Zero,
            errorCode,
            0, // Use default language
            messageBuffer,
            messageBuffer.Capacity,
            IntPtr.Zero);

        if (messageLength > 0)
        {
            return messageBuffer.ToString();
        }
        else
        {
            return $"Unknown error code: {errorCode}";
        }
    }
}