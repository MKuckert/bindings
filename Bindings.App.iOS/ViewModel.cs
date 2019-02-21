using System.ComponentModel;
using System.Windows.Input;

namespace Bindings.App.iOS
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _stringProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        public string StringProperty
        {
            get => _stringProperty;
            set
            {
                if (_stringProperty != value)
                {
                    _stringProperty = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StringProperty)));
                }
            }
        }
        public ICommand Clicked { get; }

        public ViewModel()
        {
            Clicked = new RelayCommand(OnClicked);
        }

        private void OnClicked(object parameter)
        {
            StringProperty = "Clicked!";
        }
    }
}
