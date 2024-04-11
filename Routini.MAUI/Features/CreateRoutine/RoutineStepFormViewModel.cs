using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Routini.MAUI.Features.CreateRoutine
{
    public partial class RoutineStepFormViewModel : ObservableObject
    {
        private readonly Action<RoutineStepFormViewModel> _onDelete;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private int _durationSeconds = 30;

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
