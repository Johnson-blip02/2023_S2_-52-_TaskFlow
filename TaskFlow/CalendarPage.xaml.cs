using Syncfusion.Maui.Calendar;
using Microsoft.Maui.Controls;

namespace TaskFlow
{
    public partial class CalendarPage : ContentPage
    {

        public CalendarPage()
        {
            InitializeComponent();
        }

        private void NavTabTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}