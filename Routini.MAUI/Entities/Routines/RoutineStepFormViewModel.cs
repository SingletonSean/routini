using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Routini.MAUI.Entities.Routines
{
    public partial class RoutineStepFormViewModel : ObservableObject
    {
        private readonly Action<RoutineStepFormViewModel> _onDelete;

        private string _name = string.Empty;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private double _durationSeconds = 30;
        public double DurationSeconds
        {
            get
            {
                return _durationSeconds;
            }
            set
            {
                _durationSeconds = value;
                OnPropertyChanged(nameof(DurationSeconds));
            }
        }

        public RoutineStepFormViewModel(Action<RoutineStepFormViewModel> onDelete)
        {
            _onDelete = onDelete;
        }

        public void Reset()
        {
            Name = string.Empty;
            DurationSeconds = 30;
        }

        [RelayCommand]
        private void Delete()
        {
            _onDelete(this);
        }
    }
}
