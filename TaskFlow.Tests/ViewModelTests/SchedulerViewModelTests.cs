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
            var mockScheduledTimeModel = new Mock<IDatabase<ScheduledTime>>();
            var mockDayModel = new Mock<IDatabase<Day>>();

            mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem> {
                new TodoItem { Title = "Task 1", TimeBlock = new TimeSpan(1, 0, 0), Color = "#FFFFFF" }
            });

            App.TodoModel = mockTodoModel.Object;

            mockScheduledTimeModel.Setup(m => m.GetData()).Returns(new List<ScheduledTime> { });
            mockDayModel.Setup(m => m.GetData()).Returns(new List<Day> { });

            App.ScheduledTimeModel = mockScheduledTimeModel.Object;
            App.DayModel = mockDayModel.Object;

            var viewModel = new SchedulerViewModel();

            viewModel.AddTodo(App.TodoModel.GetData()[0], new DateTime(2023, 10, 10, 9, 0, 0));

            // Act
            var isBookedResult = viewModel.IsBooked(new DateTime(2023, 10, 10, 1, 0, 0), new TimeSpan(1, 0, 0)); // Check if new potential time overlaps with original booking


            // Assert
            Assert.Equal(1, viewModel.ScheduleEvents.Count);
        }


        /// <summary>
        /// Check the successful identification if two tasks are in the same timeslot
        /// </summary>
        [Fact]
        public void IsBooked_DoubleBooked()
        {
            // Arrange
            var mockTodoModel = new Mock<IDatabase<TodoItem>>();
            var mockScheduledTimeModel = new Mock<IDatabase<ScheduledTime>>();
            var mockDayModel = new Mock<IDatabase<Day>>();

            mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem> {
                new TodoItem { Title = "Task 1", TimeBlock = new TimeSpan(1, 0, 0), Color = "#FFFFFF" }
            });

            App.TodoModel = mockTodoModel.Object;

            mockScheduledTimeModel.Setup(m => m.GetData()).Returns(new List<ScheduledTime> { });
            mockDayModel.Setup(m => m.GetData()).Returns(new List<Day> { });

            App.ScheduledTimeModel = mockScheduledTimeModel.Object;
            App.DayModel = mockDayModel.Object;

            var viewModel = new SchedulerViewModel();

            viewModel.AddTodo(App.TodoModel.GetData()[0], new DateTime(2023, 10, 10, 9, 0, 0));

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
            var mockScheduledTimeModel = new Mock<IDatabase<ScheduledTime>>();
            var mockDayModel = new Mock<IDatabase<Day>>();

            mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem> {
                new TodoItem { Title = "Task 1", TimeBlock = new TimeSpan(1, 0, 0), Color = "#FFFFFF" }
            });

            App.TodoModel = mockTodoModel.Object;

            mockScheduledTimeModel.Setup(m => m.GetData()).Returns(new List<ScheduledTime> { });
            mockDayModel.Setup(m => m.GetData()).Returns(new List<Day> { });

            App.ScheduledTimeModel = mockScheduledTimeModel.Object;
            App.DayModel = mockDayModel.Object;

            var viewModel = new SchedulerViewModel();

            viewModel.AddTodo(App.TodoModel.GetData()[0], new DateTime(2023, 10, 10, 9, 0, 0));

            // Act
            var isBookedResult = viewModel.IsBooked(new DateTime(2023, 10, 10, 1, 0, 0), new TimeSpan(1, 0, 0)); // Check if new potential time overlaps with original booking

            // Assert
            Assert.False(isBookedResult);
        }

        /// <summary>
        /// Test if you attempt to remove a scheduled task, it is removed drom the ScheduledEvents collection and therefore, is no longer displayed
        /// on the scheduler component.
        /// </summary>
        [Fact]
        public void RemoveTaskFromSchedule_SuccessfullyRemoved()
        {
            // Arrange
            var mockTodoModel = new Mock<IDatabase<TodoItem>>();
            var mockScheduledTimeModel = new Mock<IDatabase<ScheduledTime>>();
            var mockDayModel = new Mock<IDatabase<Day>>();

            mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem> {
                new TodoItem { Title = "Task 1", TimeBlock = new TimeSpan(1, 0, 0), Color = "#FFFFFF" }
            });

            App.TodoModel = mockTodoModel.Object;

            mockScheduledTimeModel.Setup(m => m.GetData()).Returns(new List<ScheduledTime> { });
            mockDayModel.Setup(m => m.GetData()).Returns(new List<Day> { });

            App.ScheduledTimeModel = mockScheduledTimeModel.Object;
            App.DayModel = mockDayModel.Object;

            var viewModel = new SchedulerViewModel();

            viewModel.AddTodo(App.TodoModel.GetData()[0], new DateTime(2023, 10, 10, 9, 0, 0));

            // Act
            int beforeRemove = viewModel.ScheduleEvents.Count;
            viewModel.RemoveTaskFromSchedule("Task 1", new DateTime(2023, 10, 10, 9, 0, 0));
            int afterRemove = viewModel.ScheduleEvents.Count;

            //Assert
            Assert.NotEqual(beforeRemove, afterRemove);
        }

        /// <summary>
        /// Test to check if you have multiple tasks scheduled and you try to remove one, then only one task is removed.
        /// </summary>
        [Fact]
        public void RemoveTaskFromSchedule_RemoveOnlyOne()
        {
            // Arrange
            var mockTodoModel = new Mock<IDatabase<TodoItem>>();
            var mockScheduledTimeModel = new Mock<IDatabase<ScheduledTime>>();
            var mockDayModel = new Mock<IDatabase<Day>>();

            // Setup the three mock models
            mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem> {
                new TodoItem { Title = "Task 1", TimeBlock = new TimeSpan(1, 0, 0), Color = "#FFFFFF" }, // Create a task in the database
                new TodoItem { Title = "Task 2", TimeBlock = new TimeSpan(1, 0, 0), Color = "#FFFFFF" } // Create a second task in the database
            });
            mockScheduledTimeModel.Setup(m => m.GetData()).Returns(new List<ScheduledTime> { });
            mockDayModel.Setup(m => m.GetData()).Returns(new List<Day> { });

            App.TodoModel = mockTodoModel.Object;
            App.ScheduledTimeModel = mockScheduledTimeModel.Object;
            App.DayModel = mockDayModel.Object;

            var viewModel = new SchedulerViewModel();

            viewModel.AddTodo(App.TodoModel.GetData()[0], new DateTime(2023, 10, 10, 9, 0, 0)); // Schedule a task
            viewModel.AddTodo(App.TodoModel.GetData()[1], new DateTime(2023, 10, 10, 12, 0, 0)); // Schedule a second task

            // Act
            int beforeRemove = viewModel.ScheduleEvents.Count;
            viewModel.RemoveTaskFromSchedule("Task 1", new DateTime(2023, 10, 10, 9, 0, 0)); // Remove this task
            int afterRemove = viewModel.ScheduleEvents.Count;

            int removedCount = afterRemove - beforeRemove;

            //Assert 
            Assert.Equal(-1, removedCount); // Check the difference in the number of events in the scheduler is equal to the number removed
        }


        /// <summary>
        /// Test to check if you have two tasks with the same title, and you try to remove one, ensure it does not remove both.
        /// </summary>
        [Fact]
        public void RemoveTaskFromSchedule_HandleTwoTasksSameName()
        {
            // Arrange
            var mockTodoModel = new Mock<IDatabase<TodoItem>>();
            var mockScheduledTimeModel = new Mock<IDatabase<ScheduledTime>>();
            var mockDayModel = new Mock<IDatabase<Day>>();

            // Setup the three mock models
            mockTodoModel.Setup(m => m.GetData()).Returns(new List<TodoItem> {
                new TodoItem { Title = "Title", TimeBlock = new TimeSpan(1, 0, 0), Color = "#FFFFFF" }, // Create a task in the database
                new TodoItem { Title = "Title", TimeBlock = new TimeSpan(2, 0, 0), Color = "#FF0000" } // Create a second task in the database
            });
            mockScheduledTimeModel.Setup(m => m.GetData()).Returns(new List<ScheduledTime> { });
            mockDayModel.Setup(m => m.GetData()).Returns(new List<Day> { });

            App.TodoModel = mockTodoModel.Object;
            App.ScheduledTimeModel = mockScheduledTimeModel.Object;
            App.DayModel = mockDayModel.Object;

            var viewModel = new SchedulerViewModel();

            viewModel.AddTodo(App.TodoModel.GetData()[0], new DateTime(2023, 10, 10, 9, 0, 0)); // Schedule a task
            viewModel.AddTodo(App.TodoModel.GetData()[1], new DateTime(2023, 10, 10, 12, 0, 0)); //Schedule a second task

            // Act
            viewModel.RemoveTaskFromSchedule("Title", new DateTime(2023, 10, 10, 9, 0, 0)); // Remove one of the scheduled tasks
            int remainingScheduledCount = viewModel.ScheduleEvents.Count;

            Assert.Equal(1, remainingScheduledCount); // Is only one of the two tasks remaining after removal?
        }
    }
}
