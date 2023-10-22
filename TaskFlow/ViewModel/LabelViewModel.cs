using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaskFlow.Model;
using TaskFlow.View;

namespace TaskFlow.ViewModel;

/// <summary>
/// View model for logic of the <see cref="LabelPage"/> view.
/// </summary>
public partial class LabelViewModel : ObservableObject
{
    private IDatabase<LabelItem> _lm; // Label model

    [ObservableProperty]
    ObservableCollection<LabelItem> labelItems;

    [ObservableProperty]
    ObservableCollection<object> selectedLabels;

    [ObservableProperty]
    string newLabelTitle;

    public LabelViewModel()
    {
        _lm = App.LabelModel;
        LabelItems = new ObservableCollection<LabelItem>();
        SelectedLabels = new ObservableCollection<object>(); 
        NewLabelTitle = string.Empty;
    }

    /// <summary>
    /// Loads label items from the database and updates local collection.
    /// </summary>
    public void LoadLabels()
    {
        var tempLabels = _lm.GetData();
        if (tempLabels == null)
            return;

        LabelItems.Clear();
        foreach (var item in tempLabels)
            LabelItems.Add(item);
    }

    /// <summary>
    /// Adds a new label item with title to the database.
    /// </summary>
    [RelayCommand]
    public void AddNewLabel()
    {
        if (!string.IsNullOrWhiteSpace(NewLabelTitle))
        {
            _lm.Insert(new LabelItem(NewLabelTitle));
        }

        NewLabelTitle = string.Empty;
        LoadLabels();
    }

    /// <summary>
    /// Handles the event when a label item is tapped. Opens a <see cref="LabelPopup"/> popup with the tapped label item to be edited. 
    /// If the return value of the popup is type string, the label item will be edited with the string as new title in the database. 
    /// If the return value is boolean, the label item will be deleted from the database.
    /// </summary>
    /// <param name="obj">The object representing the tapped label item.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    [RelayCommand]
    public async Task ItemTapped(object obj)
    {
        // Extracting the label item from the event arguments.
        var eventArgs = obj as Syncfusion.Maui.ListView.ItemTappedEventArgs;
        LabelItem label = eventArgs.DataItem as LabelItem;

        // Showing the LabelPopup and awaiting the result.
        var result = await Shell.Current.ShowPopupAsync(new LabelPopup(new LabelPopupViewModel(label)));

        // Handling the result of the popup.
        if (result is string newTitle && !string.IsNullOrWhiteSpace(newTitle))
        {
            label.Title = newTitle;
            _lm.Insert(label);
        }
        else if (result is bool)
        {
            _lm.Delete(label);
        }
        LoadLabels();
    }

}
