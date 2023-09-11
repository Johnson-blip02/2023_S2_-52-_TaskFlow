using Microsoft.Maui.Graphics.Converters;
using Practice.Model;
using System.Globalization;

namespace TaskFlow.Converters;

/// <summary>
/// Converts a hexadecimal string representation of a color to a Color object.
/// </summary>
public class HexStringToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string hexColor && hexColor.StartsWith("#") && hexColor.Length == 7)
        {
            return Color.FromArgb(hexColor);
        }

        // Return a default color if input is not a valid hex color
        return Colors.White;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

