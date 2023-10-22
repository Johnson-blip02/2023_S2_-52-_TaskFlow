using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskFlow.Tests.ViewModelTests
{
    public class SchedulerViewModelTests
    {
        /// <summary>
        /// Check if appointments are successfully added to calendar.
        /// </summary>
        [Fact]
        public void GenerateAppointment_CheckForAppointmentCount() // Test adding appointments to calendar
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
            var viewModel = new SchedulerViewModel();

            // Act
            viewModel.GenerateAppointments();
            int expectedValue = 0;
            foreach (var item in App.TodoModel.GetData())
            {
                if (!item.InTrash && !item.Archived && !item.Completed)
                    expectedValue++;
            }

            // Assert
            Assert.Equal(expectedValue, viewModel.Events.Count);
        }

        /// <summary>
        /// Check if appointments are scheduled to the scheduler.
        /// </summary>
        [Fact]
        public void AddTodo_ScheduleATask()
        {
            // Arrange
            var mockTodoModel = new Mock<IDatabase<TodoItem>>();
            var mockDayModel = new Mock<IDatabase<Day>>();

            mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem>
            {
                new TodoItem { Title = "Task 1", TimeBlock = new TimeSpan(1, 0, 0) }, // Timespan of one hour
            });

            mockDayModel.Setup(m => m.GetData()).Returns(new List<Day>
            {
                new Day { Date = new DateTime(2023, 10, 10, 0, 0, 0) } } // 10 October, 2023
            );

            App.TodoModel = mockTodoModel.Object;
            App.DayModel = mockDayModel.Object;
            var viewModel = new SchedulerViewModel(); // Schedule first task

            DateTime bookingTime = new DateTime(2023, 10, 10, 10, 0, 0); //10:00am, 10 October, 2023
            int expectedValue = 1; // Should have one task successfully scheduled

            // Act
            viewModel.AddTodo(App.TodoModel.GetData()[0], bookingTime);

            // Assert
            Assert.Equal(expectedValue, viewModel.ScheduleEvents.Count);
        }


        /// <summary>
        /// Check the successful identification if two tasks are in the same timeslot
        /// </summary>
        [Fact]
        public void IsBooked_DoubleBooked()
        {
            // Arrange
            var mockTodoModel = new Mock<IDatabase<TodoItem>>();
            var mockDayModel = new Mock<IDatabase<Day>>();

            mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem>
            {
                new TodoItem { Title = "Task Title", TimeBlock = new TimeSpan(1, 0, 0) } } // Timespan of one hour
            );

            mockDayModel.Setup(m => m.GetData()).Returns(new List<Day>
            {
                new Day { Date = new DateTime(2023, 10, 10) } // 10 October, 2023
            });

            App.TodoModel = mockTodoModel.Object;
            App.DayModel = mockDayModel.Object;
            var viewModel = new SchedulerViewModel();

            viewModel.AddTodo(App.TodoModel.GetData()[0], new DateTime(2023, 10, 10, 9, 0, 0)); // Book original appointment

            // Act
            var isBookedResult = viewModel.IsBooked(new DateTime(2023, 10, 10, 9, 0, 0), new TimeSpan(1, 0, 0)); // Check if new potential time overlaps with original booking

            // Assert
            Assert.True(isBookedResult);
        }

        /// <summary>
        /// Check the successful identification if two tasks are not in the same timeslot
        /// </summary>
        [Fact]
        public void IsBooked_NotDoubleBooked()
        {
            // Arrange
            var mockTodoModel = new Mock<IDatabase<TodoItem>>();
            var mockDayModel = new Mock<IDatabase<Day>>();

            mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem>
            {
                new TodoItem { Title = "Task Title", TimeBlock = new TimeSpan(1, 0, 0) } } // Timespan of one hour
            );

            mockDayModel.Setup(m => m.GetData()).Returns(new List<Day>
            {
                new Day { Date = new DateTime(2023, 10, 10) } } // 10 October, 2023
            );

            App.TodoModel = mockTodoModel.Object;
            App.DayModel = mockDayModel.Object;
            var viewModel = new SchedulerViewModel();

            viewModel.AddTodo(App.TodoModel.GetData()[0], new DateTime(2023, 10, 11, 10, 0, 0)); // Book original appointment

            // Act
            var isBookedResult = viewModel.IsBooked(new DateTime(2023, 10, 10, 9, 0, 0), new TimeSpan(1, 0, 0)); // Check if new potential time overlaps with original booking

            // Assert
            Assert.False(isBookedResult);
        }
    }
}
