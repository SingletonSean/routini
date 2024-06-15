using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Shells;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Routini.MAUI.Features.EditRoutine
{
    public partial class EditRoutineFormViewModel : ObservableValidator
    {
        private readonly Routine _routine;
        private readonly UpdateRoutineMutation _updateRoutineMutation;
        private readonly IShell _shell;
        private readonly ILogger _logger;

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

        public EditRoutineFormViewModel(
            Routine routine,
            UpdateRoutineMutation updateRoutineMutation,
            IShell shell,
            ILogger logger)
        {
            _routine = routine;
            _updateRoutineMutation = updateRoutineMutation;
            _shell = shell;
            _logger = logger;

            RoutineSteps = new ObservableCollection<RoutineStepFormViewModel>()
            {
                new RoutineStepFormViewModel(DeleteRoutineStep)
            };

            Reset(routine);

            ErrorsChanged += OnErrorsChanged;
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

            Submitting = true;
            ErrorMessage = null;

            try
            {
                Routine updatedRoutine = new Routine(
                    _routine.Id,
                    Name,
                    RoutineSteps.Select(s => new RoutineStep(s.Name, TimeSpan.FromSeconds(s.DurationSeconds)))
                );

                _logger.LogInformation("Updating routine: {RoutineId}", _routine.Id);

                await _updateRoutineMutation.Execute(updatedRoutine);

                _logger.LogInformation("Successfully updated routine: {RoutineId}", _routine.Id);

                await _shell.GoToAsync("..");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update routine: {RoutineId}", _routine.Id);
                ErrorMessage = "Failed to update routine. Please try again later.";
            }
            finally
            {
                Submitting = false;
            }
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
