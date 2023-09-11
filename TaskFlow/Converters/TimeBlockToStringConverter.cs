using System.Globalization;

namespace TaskFlow.Converters;

/// <summary>
/// Converts a TimeSpan object to a string in the format "<see cref="TimeSpan.Hours"/> HR <see cref="TimeSpan.Minutes"/> MINS"
/// </summary>
internal class TimeBlockToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeBlock)
        {
            int hours = timeBlock.Hours;
            int minutes = timeBlock.Minutes;

            if (hours > 0 && minutes > 0)
            {
                return $"{hours} HR\n{minutes} MINS";
            }
            else if (hours > 0)
            {
                return $"{hours} HRS";
            }
            else if (minutes > 0)
            {
                return $"{minutes} MINS";
            }

            return string.Empty;
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

