using Microsoft.VisualBasic;
using Syncfusion.Maui.DataSource.Extensions;
using System.Globalization;
using TaskFlow.Model;

namespace TaskFlow.Comparers;

/// <summary>
/// Comparer class for sorting todo list by due date.
/// Compares two group results based on their key using a list of <see cref="DueDateGroup"/> string values.
/// </summary>
public class DueDateGroupComparer : IComparer<GroupResult>
{
    public int Compare(GroupResult x, GroupResult y)
    {
        // Convert keys to string representations
        var xKey = x.Key.ToString();
        var yKey = y.Key.ToString();
        
        List<string> definedCategories = new List<string>
        {
            DueDateGroup.Overdue.ToFriendlyString(),
            DueDateGroup.Today.ToFriendlyString(),
            DueDateGroup.Tomorrow.ToFriendlyString(),
            DueDateGroup.NextWeek.ToFriendlyString(),
            DueDateGroup.ThisMonth.ToFriendlyString(),
            DueDateGroup.NextMonth.ToFriendlyString(),
            DueDateGroup.ThisYearAfterNextMonth.ToFriendlyString(),
            DueDateGroup.NextYear.ToFriendlyString(),
            DueDateGroup.AfterNextYear.ToFriendlyString()
        };

        return definedCategories.IndexOf(xKey) - definedCategories.IndexOf(yKey);
    }
}
