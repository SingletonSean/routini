using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Routini.MAUI.Entities.Routines
{
    public partial class RoutineFormViewModel : ObservableObject
    {
        private readonly Dictionary<string, List<string>> _propertyNameToErrors = new Dictionary<string, List<string>>();

        public bool HasErrors => _propertyNameToErrors.Count > 0;

        private void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyNameToErrors.ContainsKey(propertyName))
            {
                _propertyNameToErrors.Add(propertyName, new List<string>());
            }

            _propertyNameToErrors[propertyName].Add(errorMessage);
        }

        public void ClearErrors(string propertyName)
        {
            _propertyNameToErrors.Remove(propertyName);
        }

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
                OnPropertyChanged(nameof(NameErrorMessage));
                OnPropertyChanged(nameof(HasNameErrorMessage));
            }
        }

        public string? NameErrorMessage => _propertyNameToErrors.GetValueOrDefault(nameof(Name))?.FirstOrDefault();
        public bool HasNameErrorMessage => !string.IsNullOrEmpty(NameErrorMessage);

        public void Validate()
        {
            ValidateName();
        }

        private void ValidateName()
        {
            ClearErrors(nameof(Name));

            if (string.IsNullOrEmpty(_name))
            {
                AddError(nameof(Name), "Required");
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
