using System.Globalization;

namespace TaskFlow.Converters;

/// <summary>
/// Converts a DateTime object to a string with input parameter format.
/// </summary>
public class DateTimeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is DateTime dateTime)
        {
            string format = parameter as string;
            return dateTime.ToString(format, CultureInfo.InvariantCulture);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
