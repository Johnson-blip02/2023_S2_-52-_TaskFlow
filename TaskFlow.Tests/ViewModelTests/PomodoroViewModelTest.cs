using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.View;

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
            Assert.Equal(59, viewModel.PointerValue);
        }

        [Fact]
        public void TestStartTimer()
        {
            // Arrange
            var viewModel = new PomodoroViewModel();

            // Act
            bool result = viewModel.Start();

            // Assert
            Assert.True(result); 
        }

        [Fact]
        public void StartTimer_WorkAndBreak_NotEqual()
        {
            // Arrange
            var viewModel = new PomodoroViewModel
            {
                Starter = 5,
                WorkStart = 2,  
                BreakStart = 3,  
            };

            // Act
            viewModel.Start();

            // Assert
            Assert.False(viewModel.IsWorking);
            Assert.NotEqual(viewModel.WorkStart, viewModel.BreakStart);
        }

    }
}
