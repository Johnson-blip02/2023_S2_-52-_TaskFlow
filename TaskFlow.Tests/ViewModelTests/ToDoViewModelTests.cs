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
}
