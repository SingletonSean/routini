using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Shared.Shells;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Routini.MAUI.Pages
{
    public partial class CreateRoutineViewModel : ObservableValidator
    {
        private readonly CreateRoutineMutation _mutation;
        private readonly IShell _shell;
        private readonly ILogger<CreateRoutineViewModel> _logger;

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

        public CreateRoutineViewModel(CreateRoutineMutation mutation, IShell shell, ILogger<CreateRoutineViewModel> logger)
        {
            _mutation = mutation;
            _shell = shell;
            _logger = logger;

            RoutineSteps = new ObservableCollection<RoutineStepFormViewModel>()
            {
                new RoutineStepFormViewModel(DeleteRoutineStep)
            };

            ErrorsChanged += OnErrorsChanged;
        }

        [RelayCommand]
        private void ResetForm()
        {
            Submitting = false;
            ErrorMessage = null;
            ClearErrors();

            RoutineSteps.Clear();

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

            Submitting = true;
            ErrorMessage = null;

            try
            {
                _logger.LogInformation("Creating routine: {RoutineName}", Name);

                IEnumerable<NewRoutineStep> newRoutineSteps = RoutineSteps
                    .Select(s => new NewRoutineStep(
                        s.Name, TimeSpan.FromSeconds(s.DurationSeconds)));
                NewRoutine newRoutine = new NewRoutine(Name, newRoutineSteps);

                await _mutation.Execute(newRoutine);

                _logger.LogInformation("Successfully created routine: {RoutineName}", Name);

                await _shell.GoToAsync("..");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create routine: {RoutineName}", Name);

                ErrorMessage = "Failed to create routine. Please try again later.";
            }
            finally
            {
                Submitting = false;
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
