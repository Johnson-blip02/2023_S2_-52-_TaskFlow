using Android.Graphics;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using AColor = Android.Graphics.Color;

namespace TaskFlow.CustomControls;

/// <summary>
/// Custom handler for the SearchBar control. 
/// </summary>
public partial class CustomSearchBarHandler : SearchBarHandler
{
    /// <summary>
    /// Custom property mapper for associating the IconColor property with its handler.
    /// </summary>
    public static readonly IPropertyMapper<ISearchBar, SearchBarHandler> CustomMapper =
        new PropertyMapper<ISearchBar, SearchBarHandler>(Mapper)
        {
            [nameof(CustomSearchBar.IconColor)] = MapIconColor,
        };

    public CustomSearchBarHandler() : base(CustomMapper, CommandMapper)
    {
        // Remove underline.
        Mapper.AppendToMapping("NoUnderline", (handler, view) =>
        {
#if ANDROID
            Android.Widget.LinearLayout linearLayout = handler.PlatformView.GetChildAt(0) as Android.Widget.LinearLayout;
            linearLayout = linearLayout.GetChildAt(2) as Android.Widget.LinearLayout;
            linearLayout = linearLayout.GetChildAt(1) as Android.Widget.LinearLayout;
            linearLayout.Background = null;
#endif
        });
    }

#if ANDROID

    /// <summary>
    /// Sets the color of the search icon.
    /// </summary>
    /// <param name="value"></param>
    public void SetIconColor(AColor value)
    {
        var searchIcon = (ImageView)PlatformView.FindViewById(Resource.Id.search_mag_icon);
        searchIcon.SetColorFilter(value, PorterDuff.Mode.SrcAtop);
    }

    /// <summary>
    /// Maps the IconColor property to the Android platform.
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="searchBar"></param>
    public static void MapIconColor(ISearchBarHandler handler, ISearchBar searchBar)
    {
        if (handler is CustomSearchBarHandler customHandler)
            customHandler.SetIconColor(((CustomSearchBar)searchBar).IconColor.ToAndroid());
    }
#endif
}
