using CommunityToolkit.Maui.Views;
using TaskFlow.Model;

namespace TaskFlow.View;

public partial class PomodoroPopupPage : Popup
{
	public TimeValues timevalues;


	//public string start = "30";
	//public string breaktime = "20";
	//public string whiletime = "2";

	public PomodoroPopupPage()
	{
		InitializeComponent();
		

	}

	void SubmitTime(System.Object sender, System.EventArgs e)
	{
		timevalues.StartValue = ValueStart.Text;
    }

    private void Button_Clicked(object sender, EventArgs e)
	{
		Close();
	}

  //  void OnStartChange(object sender, TextChangedEventArgs e)
  //  {
		//string oldTime = e.OldTextValue;
		//string newTime = e.NewTextValue;
		////string myTime = entry.Text;

  //  }
}