using Microsoft.UI.Xaml;
using Windows.Storage.Pickers;

namespace CyberTool.Services;

public static class PickerHelper
{
    public static void InitializeWithWindow(FileOpenPicker picker, Window window)
    {
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
    }
}
