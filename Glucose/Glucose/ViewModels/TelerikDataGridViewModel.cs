using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

using Glucose.Models;
using Glucose.Services;

namespace Glucose.ViewModels
{
    public class TelerikDataGridViewModel : ViewModelBase
    {
        public ObservableCollection<Record2> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                //return SampleDataService.GetGridSampleData();
                return DataService.GetGridData();
            }
        }
    }
}
