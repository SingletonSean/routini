using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Routini.MAUI.Entities.Routines
{
    public partial class RoutineFormViewModel : ObservableValidator
    {
        private readonly Func<Task> _onSubmit;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Required")]
        [MinLength(1, ErrorMessage = "Required")]
        private string _name = string.Empty;
        public string? NameErrorMessage => GetErrors(nameof(Name)).FirstOrDefault()?.ErrorMessage;

        public ObservableCollection<RoutineStepFormViewModel> RoutineSteps { get; set; }

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool? _submitting;

        public RoutineFormViewModel(Func<Task> onSubmit)
        {
            _onSubmit = onSubmit;

            RoutineSteps = new ObservableCollection<RoutineStepFormViewModel>();

            ErrorsChanged += OnErrorsChanged;
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
            ValidateAllProperties();

            foreach (RoutineStepFormViewModel step in RoutineSteps)
            {
                step.Validate();
            }

            if (HasErrors || RoutineSteps.Any(s => s.HasErrors))
            {
                return;
            }

            await _onSubmit();
        }

        private void OnErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(NameErrorMessage));
        }
    }
}
