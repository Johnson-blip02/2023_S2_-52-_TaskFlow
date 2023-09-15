using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Model;

namespace TaskFlow.Converters;

/// <summary>
/// Converts a List of LabelItem objects to a single string with a count suffix.
/// </summary>
public class LabelListToSingleStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is List<LabelItem> labels && labels.Count > 0)
        {
            string firstLabelTitle = labels[0].Title;
            int additionalLabelCount = labels.Count - 1;

            if (additionalLabelCount > 0)
            {
                return $"{firstLabelTitle}+{additionalLabelCount}";
            }
            else
            {
                return firstLabelTitle;
            }
        }

        return string.Empty; // Handle empty list or null value
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
