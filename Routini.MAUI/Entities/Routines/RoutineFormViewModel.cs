using CommunityToolkit.Mvvm.Input;
using Routini.MAUI.Shared.ViewModels;
using System.Collections.ObjectModel;

namespace Routini.MAUI.Entities.Routines
{
    public partial class RoutineFormViewModel : ValidationViewModel
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
                if (_name == value)
                {
                    return;
                }

                _name = value;

                ValidateName();

                OnPropertyChanged(nameof(Name));
            }
        }

        public string? NameErrorMessage => GetFirstError(nameof(Name));
        public bool HasNameErrorMessage => !string.IsNullOrEmpty(NameErrorMessage);

        private void Validate()
        {
            ValidateName();

            foreach (RoutineStepFormViewModel step in RoutineSteps)
            {
                step.Validate();
            }
        }

        private void ValidateName()
        {
            ClearErrors(nameof(Name));

            if(string.IsNullOrEmpty(Name))
            {
                AddError(nameof(Name), "Required");
            }

            OnPropertyChanged(nameof(NameErrorMessage));
            OnPropertyChanged(nameof(HasNameErrorMessage));
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
            Validate();

            if (HasErrors || RoutineSteps.Any(s => HasErrors))
            {
                return;
            }

            await _onSubmit();
        }
    }
}
