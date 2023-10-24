using CommunityToolkit.Maui.Views;
using TaskFlow.Model;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class PomodoroSetupPage : ContentPage
{
	public PomodoroSetupPage(PomodoroViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

}