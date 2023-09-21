using CommunityToolkit.Maui.Views;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class LabelPage : ContentPage
{
	public LabelPage(LabelViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	} 

	/// <summary>
	/// Loads label items from view model whenever page is about to appear on screen.
	/// </summary>
	protected override void OnAppearing()
	{
		base.OnAppearing();
		((LabelViewModel)BindingContext).LoadLabels();
	}

}