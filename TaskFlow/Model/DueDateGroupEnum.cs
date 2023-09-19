namespace TaskFlow.Model;

/// <summary>
/// Enumeration of due date groups that todo items can be sorted by.
/// </summary>
public enum DueDateGroup
{
    Overdue,
    Today,
    Tomorrow,
    NextWeek,
    ThisMonth,
    NextMonth,
    ThisYearAfterNextMonth,
    NextYear,
    AfterNextYear
}

public static class DueDateGroupExtension
{
    /// <summary>
    ///  Converts a DueDateGroup enum value to a user-friendly string representation.
    ///  The returned string is used as keys for todo list datasource group descriptors.
    /// </summary>
    public static string ToFriendlyString(this DueDateGroup dueDateGroup)
    {
        return dueDateGroup switch
        {
            DueDateGroup.Overdue => "Overdue",
            DueDateGroup.Today => "Today",
            DueDateGroup.Tomorrow => "Tomorrow",
            DueDateGroup.NextWeek => "Within a week",
            DueDateGroup.ThisMonth => "Later this month",
            DueDateGroup.NextMonth => "Next month",
            DueDateGroup.ThisYearAfterNextMonth => "Later this year",
            DueDateGroup.NextYear => "Next year",
            DueDateGroup.AfterNextYear => "Distant future",
            _ => string.Empty,
        };
    }

    /// <summary>
    /// Determines the DueDateGroup for a given date-time based on the current date and time.
    /// </summary>
    /// <param name="dueDate">The due date of the todo item</param>
    /// <returns>The DueDateGroup for the todo item</returns>
    public static DueDateGroup GetDueDateGroup(DateTime dueDate)
    {
        DateTime now = DateTime.Now;
        DateOnly dueDateDateOnly = DateOnly.FromDateTime(dueDate);
        DateOnly todayDateOnly = DateOnly.FromDateTime(now);  

        if (dueDate < now)
            return DueDateGroup.Overdue;
        else if(dueDateDateOnly == todayDateOnly)
        {
            if(dueDate > now)
                return DueDateGroup.Today;
            else 
                return DueDateGroup.Overdue;
        }
        else if (dueDateDateOnly == todayDateOnly.AddDays(1))
            return DueDateGroup.Tomorrow;
        else if (dueDateDateOnly <= todayDateOnly.AddDays(7))
            return DueDateGroup.NextWeek;
        else if (dueDate.Month == now.Month && dueDate.Year == now.Year)
            return DueDateGroup.ThisMonth;
        else if (dueDate.Month == now.Month + 1 && dueDate.Year == now.Year)
            return DueDateGroup.NextMonth;
        else if (dueDate.Month > now.Month + 1 && dueDate.Year == now.Year)
            return DueDateGroup.ThisYearAfterNextMonth;
        else if (dueDate.Year == now.Year + 1)
            return DueDateGroup.NextYear;
        else
            return DueDateGroup.AfterNextYear;
    }

}


