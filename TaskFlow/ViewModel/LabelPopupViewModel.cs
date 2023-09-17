using CommunityToolkit.Mvvm.ComponentModel;
using TaskFlow.Model;
using TaskFlow.View;

namespace TaskFlow.ViewModel;

/// <summary>
/// View model for logic of the <see cref="LabelPopup"/> popup page, used to edit/delete label items.
/// </summary>
public partial class LabelPopupViewModel : ObservableObject
{
    [ObservableProperty]
    string newTitle;  // new label item title

    [ObservableProperty]
    string labelTitle;  // old label item title

    /// <summary>
    /// Initializes new instance of class; the parameter is the label item to be edited.
    /// </summary>
    /// <param name="label">Label to be edited/deleted</param>
    public LabelPopupViewModel(LabelItem label)
    {
        NewTitle = string.Empty;
        LabelTitle = label.Title;
    }
}
