using Prism.Services;
using System.ComponentModel;

namespace SearchPeople.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel()
        {
        }

    }
}
