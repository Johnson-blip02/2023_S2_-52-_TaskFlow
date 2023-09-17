using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Model;
using TaskFlow.View;

namespace TaskFlow.ViewModel
{
    public partial class TodoPopupViewModel : ObservableObject
    {
        private LabelModel _lm; //Todo model

        [ObservableProperty]
        TodoItem selectedTodo;

        [ObservableProperty]
        ObservableCollection<TimeSpan> timeBlockList;

        [ObservableProperty]
        ObservableCollection<LabelItem> labelItems;

        public DateTime MinDate => DateTime.Now;

        public bool CanShowPopup { get; set; }

        TodoPopupViewModel() 
        { 
            _lm = new LabelModel();
            labelItems = new ObservableCollection<LabelItem>();

            foreach (var item in _lm.GetData())
            {
                labelItems.Add(item);
            }

            this.TimeBlockList = new ObservableCollection<TimeSpan>();

            for (int i = 0; i <= 24; i++)
            {
                TimeSpan increment = new TimeSpan(0, i * 15, 0);
                this.TimeBlockList.Add(increment);
            }
        }
    }
}
