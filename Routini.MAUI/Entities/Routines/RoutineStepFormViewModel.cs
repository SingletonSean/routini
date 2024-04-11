using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Routini.MAUI.Entities.Routines
{
    public partial class RoutineStepFormViewModel : ObservableObject
    {
        private readonly Action<RoutineStepFormViewModel> _onDelete;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private double _durationSeconds = 30;

        public RoutineStepFormViewModel(Action<RoutineStepFormViewModel> onDelete)
        {
            _onDelete = onDelete;
        }

        [RelayCommand]
        private void Delete()
        {
            _onDelete(this);
        }
    }
}
