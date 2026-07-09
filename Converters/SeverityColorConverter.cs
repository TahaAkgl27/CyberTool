using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

namespace CyberTool.Converters;

public class SeverityColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        try
        {
            if (parameter is string param)
            {
                if (param == "Visibility")
                {
                    // If string (Severity): Show if valid severity
                    if (value is string s) return !string.IsNullOrEmpty(s) ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
                    // If int (Count): Show if > 0
                    if (value is int i) return i > 0 ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
                }
                else if (param == "InverseVisibility")
                {
                    // If int (Count): Show if == 0
                    if (value is int i) return i == 0 ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
                }
                else if (param == "Foreground")
                {
                     var severity = value as string ?? "Info";
                     return new SolidColorBrush(severity switch
                     {
                        "Critical" or "🔴 Kritik" => Color.FromArgb(255, 139, 0, 0),
                        "High" or "High" or "Yüksek" => Color.FromArgb(255, 128, 64, 0),
                        "Medium" or "Orta" or "🟠 Orta" => Color.FromArgb(255, 102, 82, 0),
                        "Low" or "Low" or "Düşük" or "🟢 Düşük" => Color.FromArgb(255, 0, 77, 0),
                        _ => Color.FromArgb(255, 64, 64, 64)
                     });
                }
            }

            // Default: Background Brush
            var severityCode = value as string ?? "Info";
            return new SolidColorBrush(severityCode switch
            {
                "Critical" or "🔴 Kritik" => Color.FromArgb(255, 255, 185, 185),
                "High" or "High" or "Yüksek" => Color.FromArgb(255, 255, 214, 170),
                "Medium" or "Orta" or "🟠 Orta" => Color.FromArgb(255, 255, 240, 153),
                "Low" or "Low" or "Düşük" or "🟢 Düşük" => Color.FromArgb(255, 200, 230, 201),
                _ => Color.FromArgb(255, 224, 224, 224)
            });
        }
        catch
        {
            // Fallback
            return new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
