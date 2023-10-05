using System.Collections.ObjectModel;
using TaskFlow.Model;
using System.Windows.Input;
using Practice.Model;

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
    /// Value to store the pre-edited importance value
    /// </summary>
    private int OriginalImportance;

    /// <summary>
    /// The bound todo object
    /// </summary>
	public TodoItem Todo
	{
        get
        {
            if ((TodoItem)GetValue(TodoProperty) != null)
            {
                TempTodo = (TodoItem)GetValue(TodoProperty);

                _colorLabel = DefinedColorsExtension.Parse(Color.Parse(((TodoItem)GetValue(TodoProperty)).Color));
                _tempColorLabel = _colorLabel;

                OriginalImportance = ((TodoItem)GetValue(TodoProperty)).Importance;
                ResetSelected(((TodoItem)GetValue(TodoProperty)).Importance);

                OnPropertyChanged(nameof(ColourLabel));
                OnPropertyChanged(nameof(TempColourLabel));
                OnPropertyChanged(nameof(TempTodo));
            }

            return (TodoItem)GetValue(TodoProperty);
        }
        set
        {
            SetValue(TodoProperty, value);
            OnPropertyChanged(nameof(Todo));
        }
    }

    private TodoItem _tempTodo;
    /// <summary>
    /// Editable Todo object
    /// </summary>
    public TodoItem TempTodo 
    { 
        get
        {
            return _tempTodo;
        }
        set
        {
            _tempTodo = value;
            OnPropertyChanged(nameof(TempTodo));
        }
    }

    private string _tempColorLabel;
    /// <summary>
    /// Editable label for the current colour
    /// </summary>
    public string TempColourLabel
    {
        get => _tempColorLabel;
        set
        {
            if (TempTodo != null)
            {
                _tempColorLabel = value;
                TempTodo.Color = DefinedColorsExtension.Parse(value).ToArgbHex();
                OnPropertyChanged(nameof(TempTodo));
            }
            OnPropertyChanged(nameof(TempColourLabel));
        }
    }

    private string _colorLabel;
    /// <summary>
    /// Non editable label for the current colour
    /// </summary>
    public string ColourLabel
    {
        get => _colorLabel;
        set
        {
            if((TodoItem)GetValue(TodoProperty) != null)
            {
                _colorLabel = value;
                Todo.Color = DefinedColorsExtension.Parse(value).ToArgbHex();
                OnPropertyChanged(nameof(Todo));
            }

            OnPropertyChanged(nameof(ColourLabel));
        }
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
    /// <summary>
    /// List of all selectable colours
    /// </summary>
    public List<string> ColoursList { get; set; }

    private TimeSpan _tempSelectedTime;
    /// <summary>
    /// Editable time selection data
    /// </summary>
    public TimeSpan TempSelectedTime
    {
        get => _tempSelectedTime;
        set
        {
            _tempSelectedTime = value;
            TempTodo.DueDate = TempTodo.DueDate.Date;
            TempTodo.DueDate = TempTodo.DueDate.AddMinutes(value.TotalMinutes);
        }
    }

    //The following boolean are for enabling the state of which
    //importance button is highlighted/selected
    public bool LowEnable { get; set; }
    public bool MedEnable { get; set; }
    public bool HighEnable { get; set; }
    public bool VHighEnable { get; set; }

    public TodoPopup()
	{
        var colours = new List<string>();
        foreach (string dc in Enum.GetNames<DefinedColors>())
        {
            colours.Add(dc);
        }
        ColoursList = colours;
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

        ColourLabel = TempColourLabel;

        RunWhenSave.Execute(Todo);
        OnPropertyChanged(nameof(Todo));

        todoPopupHidden.IsOpen = true;
    }

    /// <summary>
    /// Closes the non-editable popup, opens the editable
    /// </summary>
    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        todoPopup.IsOpen = false;
        todoPopupHidden.IsOpen = true;
        ResetSelected(OriginalImportance);
    }

    /// <summary>
    /// Low importance button event handler
    /// </summary>
    private void Low_Pressed(object sender, EventArgs e)
    {
        TempTodo.Importance = 1;
        ResetSelected(1);
    }
    /// <summary>
    /// Medium importance button event handler
    /// </summary>
    private void Med_Pressed(object sender, EventArgs e)
    {
        TempTodo.Importance = 2;
        ResetSelected(2);
    }
    /// <summary>
    /// High importance button event handler
    /// </summary>
    private void High_Pressed(object sender, EventArgs e)
    {
        TempTodo.Importance = 3;
        ResetSelected(3);
    }
    /// <summary>
    /// Urgent importance button event handler
    /// </summary>
    private void Urgent_Pressed(object sender, EventArgs e)
    {
        TempTodo.Importance = 4;
        ResetSelected(4);
    }

    /// <summary>
    /// Resets all button visual to not be selected, then sets the button with
    /// the corresponding importance value to be selected.
    /// </summary>
    /// <param name="importance">Importance value to select</param>
    private void ResetSelected(int importance)
    {
        LowEnable = false;
        MedEnable = false; 
        HighEnable = false;
        VHighEnable = false;

        switch(importance)
        {
            case 1:
                LowEnable = true;
                break;
            case 2:
                MedEnable = true;
                break;
            case 3:
                HighEnable = true;
                break;
            case 4:
                VHighEnable = true;
                break;
        }

        OnPropertyChanged(nameof(LowEnable));
        OnPropertyChanged(nameof(MedEnable));
        OnPropertyChanged(nameof(HighEnable));
        OnPropertyChanged(nameof(VHighEnable));
    }
}