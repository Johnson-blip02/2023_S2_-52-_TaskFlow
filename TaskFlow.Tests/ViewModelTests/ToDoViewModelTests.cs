namespace TaskFlow.Tests.ViewModelTests;

public class ToDoViewModelTests
{
    [Fact]
    public void Add_ReturnsCorrectSum()
    {
        // Arrange
        ToDoViewModel vm = new ToDoViewModel();
        int num1 = 5;
        int num2 = 7;

        // Act
        int result = vm.Add(num1, num2);

        // Assert
        Assert.Equal(12, result);
    }

    [Theory]
    [InlineData("2", 1)]       // SearchBarText = "2", expected result: 1 item in the list.
    [InlineData("Task", 3)]    // SearchBarText = "Task", expected result: 3 items in the list.
    [InlineData("", 3)]        // SearchBarText is empty, expected result: no filtering, 3 items should be in the list.
    [InlineData("Invalid", 0)] // SearchBarText that doesn't match any items, expected result: no items in the list.
    public void SearchAndLabelFilter_SearchBarTextGiven_ShouldFilterBySearchText(string searchBarText, int expectedItemCount)
    {
        // Arrange
        var mockTodoModel = new Mock<IDatabase<TodoItem>>();
        mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem>
        {
            new TodoItem { Title = "Task 1", InTrash = false, Archived = false, Completed = false },
            new TodoItem { Title = "Task 2", InTrash = false, Archived = false, Completed = false },
            new TodoItem { Title = "Task 3", InTrash = true, Archived = false, Completed = false },  // in trash is true; should not be in the list.
            new TodoItem { Title = "Task 4", InTrash = false, Archived = true, Completed = false },  // archived is true; should not be in the list.
            new TodoItem { Title = "Task 5", InTrash = false, Archived = false, Completed = true },  // completed is true; should not be in the list.
            new TodoItem { Title = "Task 6", InTrash = false, Archived = false, Completed = false }
        });
        App.TodoModel = mockTodoModel.Object;
        var viewModel = new ToDoViewModel();

        // Act
        viewModel.SearchBarText = searchBarText;
        viewModel.SearchAndLabelFilter();

        // Assert
        Assert.Equal(expectedItemCount, viewModel.TodoItems.Count);
    }

    [Theory]
    [ClassData(typeof(LabelFilterTestData))]
    public void SearchAndLabelFilter_SelectedLabelGiven_ShouldFilterByLabel(List<LabelItem> labelItems, LabelItem label, int expectedValue)
    {
        // Arrange
        var mockTodoModel = new Mock<IDatabase<TodoItem>>();
        mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem>
        {
            new TodoItem { Title = "Task 1", InTrash = false, Archived = false, Completed = false, 
                Labels = labelItems.Where(label => label.Id==1 || label.Id == 2).ToList() },

            new TodoItem { Title = "Task 2", InTrash = false, Archived = false, Completed = false, 
                Labels = labelItems.Where(label => label.Id==1 || label.Id == 3).ToList() },

            new TodoItem { Title = "Task 3", InTrash = true, Archived = false, Completed = false, 
                Labels = labelItems.Where(label => label.Id==2).ToList() },                       

            new TodoItem { Title = "Task 4", InTrash = false, Archived = true, Completed = false, 
                Labels = labelItems.Where(label => label.Id==2).ToList() },      

            new TodoItem { Title = "Task 5", InTrash = false, Archived = false, Completed = true, 
                Labels = labelItems.Where(label => label.Id == 3).ToList() },                     

            new TodoItem { Title = "Task 6", InTrash = false, Archived = false, Completed = false, 
                Labels = labelItems.Where(label => label.Id==3 || label.Id == 4).ToList() },
        });

        App.TodoModel = mockTodoModel.Object;
        var viewModel = new ToDoViewModel();

        // Act
        viewModel.SelectedLabel = label;
        viewModel.SearchAndLabelFilter();

        // Assert
        Assert.Equal(expectedValue, viewModel.TodoItems.Count);
    }

    [Theory]
    [ClassData(typeof(SearchAndLabelFilterTestData))]
    public void SearchAndLabelFilter_SearchBarTextAndLabelGiven_ShouldFilterByBoth(string searchBarText, List<LabelItem> labelItems, LabelItem label, int expectedValue)
    {
        // Arrange
        var mockTodoModel = new Mock<IDatabase<TodoItem>>();
        mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem>
        {
            new TodoItem { Title = "Task 1, a", Labels = labelItems.Where(label => label.Id==1 || label.Id == 2).ToList() },

            new TodoItem { Title = "Task 2, app", Labels = labelItems.Where(label => label.Id==1 || label.Id == 3).ToList() },

            new TodoItem { Title = "Task 3, apple", Labels = labelItems.Where(label => label.Id==2).ToList() },

            new TodoItem { Title = "Task 4, bat", Labels = labelItems.Where(label => label.Id==2).ToList() },

            new TodoItem { Title = "Task 5, cat", Labels = labelItems.Where(label => label.Id == 3).ToList() },

            new TodoItem { Title = "Task 6, cattle", Labels = labelItems.Where(label => label.Id==3 || label.Id == 4).ToList() },
        });

        App.TodoModel = mockTodoModel.Object;
        var viewModel = new ToDoViewModel();

        // Act
        viewModel.SearchBarText = searchBarText;
        viewModel.SelectedLabel = label;
        viewModel.SearchAndLabelFilter();

        // Assert
        Assert.Equal(expectedValue, viewModel.TodoItems.Count);
    }

    [Theory]
    [ClassData(typeof(ScoreTestData))]
    public void LoadTodoITems_GivenCompletedTodoItems_ShouldUpdateScore(List<TodoItem> items, int expectedValue)
    {
        // Arrange
        var mockTodoModel = new Mock<IDatabase<TodoItem>> ();
        mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem>(items));

        App.TodoModel = mockTodoModel.Object;
        var viewModel = new ToDoViewModel();
        viewModel.Score = 0;

        // Act
        viewModel.LoadTodoItems();

        // Assert
        Assert.Equal(expectedValue, viewModel.Score);

    }
}

/// <summary>
/// Class which provides test data for the filter by label feature tests.
/// </summary>
public class LabelFilterTestData : IEnumerable<object[]>
{
    private LabelItem label1 = new LabelItem { Title = "Label 1", Id = 1 };
    private LabelItem label2 = new LabelItem { Title = "Label 2", Id = 2 };
    private LabelItem label3 = new LabelItem { Title = "Label 3", Id = 3 };
    private LabelItem label4 = new LabelItem { Title = "Label 4", Id = 4 };
    private LabelItem label5 = new LabelItem { Title = "Label 5", Id = 5 };

    private List<LabelItem> labelItems;

    public LabelFilterTestData()
    {
        labelItems = new List<LabelItem>() { label1, label2, label3, label4 };
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { labelItems, label1, 2 };  // SelectedLabel = label1. Expected result: 2 items in list.
        yield return new object[] { labelItems, label2, 1 };  // SelectedLabel that 3 todo items have, 2 of which are in trash or archived. Expected result: 1 item in list.
        yield return new object[] { labelItems, label3, 2 };  // SelectedLabel that 3 todo items have, 1 of which is completed. Expected result: 2 items in list.
        yield return new object[] { labelItems, label4, 1 };  // SelectedLabel = label4. Expected result: 1 item in list.
        yield return new object[] { labelItems, label5, 0 };  // SelectedLabel that no todo item has, expected result: no items in list.
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Class which provides test data for searching and label filtering in conjunction tests.
/// </summary>
public class SearchAndLabelFilterTestData : IEnumerable<object[]>
{
    private LabelItem label1 = new LabelItem { Title = "Label 1", Id = 1 };
    private LabelItem label2 = new LabelItem { Title = "Label 2", Id = 2 };
    private LabelItem label3 = new LabelItem { Title = "Label 3", Id = 3 };
    private LabelItem label4 = new LabelItem { Title = "Label 4", Id = 4 };
    private LabelItem label5 = new LabelItem { Title = "Label 5", Id = 5 };

    private List<LabelItem> labelItems;

    public SearchAndLabelFilterTestData()
    {
        labelItems = new List<LabelItem>() { label1, label2, label3, label4 };
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "Task", labelItems, null , 6 };    // Search text in all items, no selected label. Expected result: all items in list.
        yield return new object[] { "at", labelItems, label3, 2 };     // Search text in 3 items, selected label in 3, overlap = 2. Expected result: 2 items in list.
        yield return new object[] { "app", labelItems, label3, 1 };    // Search text in 2 items, selected label in 3, overlap = 1. Expected result, 1 item in list.
        yield return new object[] { "Invalid", labelItems, null, 0 };  // Search text in no items, no selected label. Expected result = no items in list.
        yield return new object[] { "Task", labelItems, label5, 0 };   // Search text in all items, selected labe in no items. Expected result = no items in list.
        yield return new object[] { "", labelItems, null, 6 };         // No search text, no selected label. Expected result: all items in list.
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Class which provides test data for the score feature tests.
/// </summary>
public class ScoreTestData : IEnumerable<object[]>
{
    private TodoItem item1 = new TodoItem { Completed = true, Importance = 1 };
    private TodoItem item2 = new TodoItem { Completed = true, Importance = 2 };
    private TodoItem item3 = new TodoItem { Completed = true, Importance = 3 };
    private TodoItem item4 = new TodoItem { Completed = false, Importance = 3 };
    private TodoItem item5 = new TodoItem { Completed = false, Importance = 4 };

    private List<TodoItem> items1;
    private List<TodoItem> items2;
    private List<TodoItem> items3;

    public ScoreTestData()
    {
        items1 = new List<TodoItem>() { item1, item2, item3 };
        items2 = new List<TodoItem>() { item1, item2, item4 };
        items3 = new List<TodoItem>() { item4, item5 };
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { items1, 6 };  // All items are completed; Score should be sum of their importance = 6.
        yield return new object[] { items2, 3 };  // 2 of 3 items are completed; Score should be sum of only completed items importance = 3
        yield return new object[] { items3, 0 };  // All items are incomplete; Should return zero.
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}