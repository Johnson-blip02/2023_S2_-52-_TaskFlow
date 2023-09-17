using CommunityToolkit.Maui.Views;
using System.Collections;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class LabelPopup : Popup
{
    object result;  // return value when popup is closed

	public LabelPopup(LabelPopupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        result = new ArrayList();
	}

    /// <summary>
    /// Closes the popup with no return value.
    /// </summary>
    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Closes the popup and returns the new title that the label item is to have.
    /// </summary>
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        result = ((LabelPopupViewModel)BindingContext).NewTitle;
		Close(result);
    }    

    /// <summary>
    /// Closes the popup and returns boolean value to indicate that the label item is to be deleted.
    /// </summary>
    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        result = false;
        Close(result);
    }
}