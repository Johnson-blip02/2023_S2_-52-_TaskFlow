using CommunityToolkit.Maui.Views;

namespace TaskFlow.View;

public partial class PomodoroPopupPage : Popup
{
	public PomodoroPopupPage()
	{
		InitializeComponent();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		Close();
	}
}