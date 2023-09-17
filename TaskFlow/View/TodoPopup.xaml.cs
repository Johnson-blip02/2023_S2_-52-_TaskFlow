using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Platform;
using System.Collections.ObjectModel;
using TaskFlow.Model;
using TaskFlow.ViewModel;

namespace TaskFlow.View;

public partial class TodoPopup : ContentView
{
	public static readonly BindableProperty TodoProperty = BindableProperty.Create(nameof(Todo), typeof(TodoItem), typeof(TodoPopup), null);
	public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(TodoPopup), false);
	
	public bool IsOpen
	{
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

	public TodoItem Todo
	{
		get => (TodoItem)GetValue(TodoProperty);
		set => SetValue(TodoProperty, value);
	}

	public ObservableCollection<TimeSpan> TimeBlockList 
	{
		get => new ObservableCollection<TimeSpan>(TodoItem.TimeBlockGenerator());
    }

	public bool Editable { get; set; } = false;

	public DateTime MinDate
	{
		get => DateTime.Today;
	}

    public TodoPopup()
	{
        InitializeComponent();
    }
}