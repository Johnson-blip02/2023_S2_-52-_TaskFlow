namespace TaskFlow.CustomControls;

/// <summary>
/// Custom search bar control that extends the standard search bar.
/// </summary>
public class CustomSearchBar : SearchBar
{
    /// <summary>
    /// Identifies the IconColor bindable property.
    /// </summary>
    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
            nameof(IconColor),
            typeof(Color),            
            typeof(CustomSearchBar),
            Colors.White);

    /// <summary>
    /// Gets or sets the color of the search icon.
    /// </summary>
    public Color IconColor
    {
        get { return (Color)GetValue(IconColorProperty); }
        set { SetValue(IconColorProperty, value); }
    }
}
