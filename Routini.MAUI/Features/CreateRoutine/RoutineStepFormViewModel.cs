using CommunityToolkit.Mvvm.ComponentModel;

namespace Routini.MAUI.Features.CreateRoutine
{
    public partial class RoutineStepFormViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private int _durationSeconds = 30;
    }
}
