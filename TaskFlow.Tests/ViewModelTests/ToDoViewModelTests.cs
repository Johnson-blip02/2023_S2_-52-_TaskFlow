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
    [InlineData("2", 1)]  // SearchBarText = "2", expected result: 1 item in the list.
    [InlineData("Task", 3)]  // SearchBarText = "Task", expected result: 3 items in the list.
    [InlineData("", 3)]  // SearchBarText is empty, expected result: no filtering, 3 items should be in the list.
    [InlineData("Invalid", 0)]  // SearchBarText that doesn't match any items, expected result: no items in the list.
    public void SearchList_SearchBarTextGiven_ShouldFilterTodoItems(string searchBarText, int expectedItemCount)
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
        viewModel.SearchList();

        // Assert
        Assert.Equal(expectedItemCount, viewModel.TodoItems.Count);
    }
}