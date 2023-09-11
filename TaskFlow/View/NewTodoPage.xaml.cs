using Practice.Model;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class NewTodoPage : ContentPage
{
    public NewTodoPage(NewTodoViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        var colours = new List<string>();
        foreach (string dc in Enum.GetNames<DefinedColors>())
        {
            colours.Add(dc);
        }
        colourPicker.ItemsSource = colours;
        colourPicker.SelectedIndex = 0;
    }

    Border lastPressed;

    private void Low_Pressed(object sender, EventArgs e)
    {
        importanceValue.Text = "1";
        RevertColour();
        lastPressed = lowButton;
        lowButton.Stroke = Color.FromArgb("#7EC8BA");
    }
    private void Med_Pressed(object sender, EventArgs e)
    {
        importanceValue.Text = "2";
        RevertColour();
        lastPressed = medButton;
        medButton.Stroke = Color.FromArgb("#7EC8BA");
    }
    private void High_Pressed(object sender, EventArgs e)
    {
        importanceValue.Text = "3";
        RevertColour();
        lastPressed = highButton;
        highButton.Stroke = Color.FromArgb("#7EC8BA");
    }
    private void Urgent_Pressed(object sender, EventArgs e)
    {
        importanceValue.Text = "4";
        RevertColour();
        lastPressed = urgentButton;
        urgentButton.Stroke = Color.FromArgb("#7EC8BA");
    }

    private void RevertColour()
    {
        if (lastPressed == null)
            return;
        lastPressed.Stroke = Color.FromArgb("#341C4F");
    }
}