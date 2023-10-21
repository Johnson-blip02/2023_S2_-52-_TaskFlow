using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Tests.ViewModelTests
{
    public class PomodoroViewModelTest
    {
        [Fact]
        public void StartTimer_WhenCalled_DecreasesPointerValue()
        {
            // Arrange
            var viewModel = new PomodoroViewModel();

            // Act
            viewModel.Start();

            // Assert
            Assert.Equal(59, viewModel.PointerValue); // Assuming PointerValue decreases by 1
        }

        [Fact]
        public void StartTimer_WhenPointerValueIsZero_TogglesBetweenWorkAndBreak()
        {
            // Arrange
            var viewModel = new PomodoroViewModel
            {
                WorkStart = 5,  // Set a short work time for testing
                BreakStart = 3,  // Set a short break time for testing
            };

            // Act
            viewModel.Start();

            // Assert
            Assert.False(viewModel.IsWorking); // Expected to be on a break
            Assert.Equal(viewModel.WorkStart, viewModel.BreakStart); // Starter should be set to BreakStart
        }

    }
}
