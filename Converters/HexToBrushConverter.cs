using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Globalization;
using Windows.UI;

namespace CyberTool.Converters;

public class HexToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string hex && !string.IsNullOrEmpty(hex))
        {
            try 
            {
                hex = hex.Replace("#", "");
                if (hex.Length == 6) hex = "FF" + hex;
                if (hex.Length == 8)
                {
                   byte a = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                   byte r = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                   byte g = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                   byte b = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
                   return new SolidColorBrush(Color.FromArgb(a, r, g, b));
                }
            }
            catch {}
        }
        return new SolidColorBrush(Color.FromArgb(255, 128, 128, 128)); // Fallback gray
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
