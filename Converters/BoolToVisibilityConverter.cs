using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace CyberTool.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool result = value is bool b && b;
        
        if (parameter is string s && (s.Equals("Inverse", StringComparison.OrdinalIgnoreCase) || s.Equals("!", StringComparison.OrdinalIgnoreCase)))
        {
            result = !result;
        }

        return result ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is Visibility v && v == Visibility.Visible;
    }
}
