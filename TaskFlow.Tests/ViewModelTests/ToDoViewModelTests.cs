namespace TaskFlow.Tests.ViewModelTests;

public class ToDoViewModelTests
{
    //[Fact]
    //public void Add_ReturnsCorrectSum()
    //{
    //    // Arrange
    //    ToDoViewModel vm = new ToDoViewModel();
    //    int num1 = 5;
    //    int num2 = 7;

    //    // Act
    //    int result = vm.Add(num1, num2);

    //    // Assert
    //    Assert.Equal(12, result);
    //}

    [Theory]
    [InlineData("2", 1)]  // SearchBarText = "2", expected result: 1 item in the list.
    [InlineData("Task", 3)]  // SearchBarText = "Task", expected result: 3 items in the list.
    [InlineData("", 3)]  // SearchBarText is empty, expected result: no filtering, 3 items should be in the list.
    [InlineData("Invalid", 0)]  // SearchBarText that doesn't match any items, expected result: no items in the list.
    public void SearchList_SearchBarTextGiven_ShouldFilterTodoItems(string searchBarText, int expectedItemCount)
    {
        // Arrange
        LabelItem labelItem = new() { Title = "Label", Id = 1};

        var mockTodoModel = new Mock<IDatabase<TodoItem>>();
        mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem>
        {
            new TodoItem { Title = "Task 1", InTrash = false, Archived = false, Completed = false, Labels = new List<LabelItem> { labelItem } },
            new TodoItem { Title = "Task 2", InTrash = false, Archived = false, Completed = false, Labels = new List<LabelItem> { labelItem } },
            new TodoItem { Title = "Task 3", InTrash = true, Archived = false, Completed = false, Labels = new List<LabelItem> { labelItem } },  // in trash is true; should not be in the list.
            new TodoItem { Title = "Task 4", InTrash = false, Archived = true, Completed = false, Labels = new List<LabelItem> { labelItem } },  // archived is true; should not be in the list.
            new TodoItem { Title = "Task 5", InTrash = false, Archived = false, Completed = true, Labels = new List<LabelItem> { labelItem } },  // completed is true; should not be in the list.
            new TodoItem { Title = "Task 6", InTrash = false, Archived = false, Completed = false, Labels = new List<LabelItem> { labelItem } }
        });
        App.TodoModel = mockTodoModel.Object;
        var viewModel = new ToDoViewModel();

        // Act
        viewModel.SearchBarText = searchBarText;
        viewModel.SelectedLabel = labelItem;
        viewModel.SearchAndFilterByLabel();

        // Assert
        Assert.Equal(expectedItemCount, viewModel.TodoItems.Count);
    }

    [Theory]
    [ClassData(typeof(FilterByLabelTestData))]
    public void FilterByLabel_LabelGiven_ShouldFilterTodoListByGivenLabel(List<LabelItem> labelItems, LabelItem label, int expectedValue)
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
        viewModel.SearchAndFilterByLabel();

        // Assert
        Assert.Equal(expectedValue, viewModel.TodoItems.Count);
    }

}

/// <summary>
/// Class which provides test data for the filter by label feature test method.
/// </summary>
public class FilterByLabelTestData : IEnumerable<object[]>
{
    private LabelItem label1 = new LabelItem { Title = "Label 1", Id = 1 };
    private LabelItem label2 = new LabelItem { Title = "Label 2", Id = 2 };
    private LabelItem label3 = new LabelItem { Title = "Label 3", Id = 3 };
    private LabelItem label4 = new LabelItem { Title = "Label 4", Id = 4 };
    private LabelItem label5 = new LabelItem { Title = "Label 5", Id = 5 };

    private List<LabelItem> labelItems;

    public FilterByLabelTestData()
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