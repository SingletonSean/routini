using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Routini.MAUI.Entities.Routines
{
    public partial class RoutineFormViewModel : ObservableObject
    {
        private readonly Func<Task> _onSubmit;

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

        public ObservableCollection<RoutineStepFormViewModel> RoutineSteps { get; private set; }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        private bool? _submitting;
        public bool? Submitting
        {
            get
            {
                return _submitting;
            }
            set
            {
                _submitting = value;
                OnPropertyChanged(nameof(Submitting));
            }
        }

        public RoutineFormViewModel(Func<Task> onSubmit)
        {
            _onSubmit = onSubmit;

            RoutineSteps = new ObservableCollection<RoutineStepFormViewModel>()
            {
                new RoutineStepFormViewModel(DeleteRoutineStep)
            };
        }

        public void Reset(Routine? routine = null)
        {
            Submitting = false;
            ErrorMessage = null;

            Name = routine?.Name ?? string.Empty;

            RoutineSteps.Clear();

            foreach (RoutineStep step in routine?.Steps ?? new List<RoutineStep>())
            {
                RoutineStepFormViewModel stepViewModel = new RoutineStepFormViewModel(DeleteRoutineStep);

                stepViewModel.Name = step.Name;
                stepViewModel.DurationSeconds = step.Duration.TotalSeconds;

                RoutineSteps.Add(stepViewModel);
            }

            if (RoutineSteps.Count == 0)
            {
                RoutineSteps.Add(new RoutineStepFormViewModel(DeleteRoutineStep));
            }
        }

        [RelayCommand]
        private void AddRoutineStep()
        {
            RoutineSteps.Add(new RoutineStepFormViewModel(DeleteRoutineStep));
        }

        private void DeleteRoutineStep(RoutineStepFormViewModel viewModel)
        {
            if (RoutineSteps.Count == 1)
            {
                viewModel.Reset();

                return;
            }

            RoutineSteps.Remove(viewModel);
        }

        [RelayCommand]
        private async Task Submit()
        {
            await _onSubmit();
        }
    }
}
