using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public bool HasNameErrorMessage => !string.IsNullOrEmpty(NameErrorMessage);

        public ObservableCollection<RoutineStepFormViewModel> RoutineSteps { get; private set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasErrorMessage))]
        private string? _errorMessage;

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        [ObservableProperty]
        private bool? _submitting;

        public RoutineFormViewModel(Func<Task> onSubmit)
        {
            _onSubmit = onSubmit;
            RoutineSteps = new ObservableCollection<RoutineStepFormViewModel>()
            {
                new RoutineStepFormViewModel(DeleteRoutineStep)
            };

            ErrorsChanged += OnErrorsChanged;
        }

        public void Reset(Routine? routine = null)
        {
            Submitting = false;
            ErrorMessage = null;
            ClearErrors();

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

        private void OnErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(NameErrorMessage));
            OnPropertyChanged(nameof(HasNameErrorMessage));
        }
    }
}
