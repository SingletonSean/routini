using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Routini.MAUI.Entities.Routines
{
    public partial class RoutineFormViewModel : ObservableObject
    {
        private readonly Func<Task> _onSubmit;

        [ObservableProperty]
        private string _name = string.Empty;

        public ObservableCollection<RoutineStepFormViewModel> RoutineSteps { get; set; }

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool? _submitting;

        public RoutineFormViewModel(Func<Task> onSubmit)
        {
            _onSubmit = onSubmit;

            RoutineSteps = new ObservableCollection<RoutineStepFormViewModel>();
        }

        public void ResetRoutineSteps(IEnumerable<RoutineStep> steps)
        {
            RoutineSteps.Clear();

            foreach (RoutineStep step in steps)
            {
                RoutineStepFormViewModel stepViewModel = new RoutineStepFormViewModel(DeleteRoutineStep);

                stepViewModel.Name = step.Name;
                stepViewModel.DurationSeconds = step.Duration.TotalSeconds;

                RoutineSteps.Add(stepViewModel);
            }
        }

        [RelayCommand]
        private void AddRoutineStep()
        {
            RoutineSteps.Add(new RoutineStepFormViewModel(DeleteRoutineStep));
        }

        private void DeleteRoutineStep(RoutineStepFormViewModel viewModel)
        {
            RoutineSteps.Remove(viewModel);
        }

        [RelayCommand]
        private async Task Submit()
        {
            await _onSubmit();
        }
    }
}
