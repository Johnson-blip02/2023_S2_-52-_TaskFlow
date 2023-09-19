using System.Collections.ObjectModel;
using TaskFlow.Model;
using Syncfusion.Maui.Popup;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;

namespace TaskFlow.View;

public partial class TodoPopup : ContentView
{
	public static readonly BindableProperty TodoProperty = BindableProperty.Create(nameof(Todo), typeof(TodoItem), typeof(TodoPopup), null);
	public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(TodoPopup), false);
	public static readonly BindableProperty RunWhenSaveProperty = BindableProperty.Create(nameof(RunWhenSave), typeof(ICommand), typeof(ICommand), null);

    /// <summary>
    /// The bound command which is to be run when the popup's save
    /// button is pressed.
    /// </summary>
    public ICommand RunWhenSave
	{
        get => (ICommand)GetValue(RunWhenSaveProperty);
        set => SetValue(RunWhenSaveProperty, value);
    }

    /// <summary>
    /// Bound property to set whether the popup is open or not
    /// </summary>
    public bool IsOpen
	{
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// The bound todo object
    /// </summary>
	public TodoItem Todo
	{
		get => (TodoItem)GetValue(TodoProperty);
		set => SetValue(TodoProperty, value);
	}

    /// <summary>
    /// The list of selectable time blocks
    /// </summary>
	public ObservableCollection<TimeSpan> TimeBlockList 
	{
		get => new ObservableCollection<TimeSpan>(TodoItem.TimeBlockGenerator());
    }

    /// <summary>
    /// Gets the minimum date as today using:
    /// <code>DateTime.Today</code>
    /// </summary>
	public DateTime MinDate
	{
		get => DateTime.Today;
	}

    public TodoPopup()
	{
        InitializeComponent();
    }

    /// <summary>
    /// Opens the editable popup, closes the non-editable one
    /// </summary>
    private void EditButton_Clicked(object sender, EventArgs e)
    {
		todoPopup.IsOpen = true;
		todoPopupHidden.IsOpen = false;
    }

    /// <summary>
    /// <list type="number">
    /// <item>Closes the editable popup.</item>
    /// <item>Executes the RunWhenSave command.</item>
    /// <item>Opens the non-editable popup</item>
    /// </list>
    /// </summary>
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        todoPopup.IsOpen = false;
		RunWhenSave.Execute(Todo);
        todoPopupHidden.IsOpen = true;
    }

    /// <summary>
    /// Closes the non-editable popup, opens the editable
    /// </summary>
    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        todoPopup.IsOpen = false;
        todoPopupHidden.IsOpen = true;
    }
}