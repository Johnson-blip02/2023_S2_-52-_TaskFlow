using System.Globalization;

namespace TaskFlow.Converters;

/// <summary>
/// Converts a DateTime object to a string in the "dd/MM" format.
/// </summary>
internal class DateTimeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is DateTime dateTime)
        {
            return dateTime.ToString("dd/MM", CultureInfo.InvariantCulture);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
