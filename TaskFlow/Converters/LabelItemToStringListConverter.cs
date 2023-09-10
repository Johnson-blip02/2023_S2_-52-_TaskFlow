using System.Globalization;
using TaskFlow.Model;

namespace TaskFlow.Converters;

/// <summary>
/// Converts a List of LabelItem objects to a List of their Titles as strings.
/// </summary>
public class LabelItemsToStringListConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is List<LabelItem> labels)
        {
            return labels.Select(l => l.Title).ToList();
        }
        return new List<string>();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
