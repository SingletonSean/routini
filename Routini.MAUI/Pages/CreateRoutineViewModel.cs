using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Shared.Shells;

namespace Routini.MAUI.Pages
{
    public partial class CreateRoutineViewModel : ObservableValidator
    {
        private readonly CreateRoutineMutation _mutation;
        private readonly IShell _shell;
        private readonly ILogger<CreateRoutineViewModel> _logger;

        public RoutineFormViewModel RoutineFormViewModel { get; }

        public CreateRoutineViewModel(CreateRoutineMutation mutation, IShell shell, ILogger<CreateRoutineViewModel> logger)
        {
            _mutation = mutation;
            _shell = shell;
            _logger = logger;

            RoutineFormViewModel = new RoutineFormViewModel(OnSubmit);
        }

        [RelayCommand]
        private void ResetForm()
        {
            RoutineFormViewModel.Reset();
        }

        private async Task OnSubmit()
        {
            RoutineFormViewModel.Submitting = true;
            RoutineFormViewModel.ErrorMessage = null;

            try
            {
                _logger.LogInformation("Creating routine: {RoutineName}", RoutineFormViewModel.Name);

                IEnumerable<NewRoutineStep> newRoutineSteps = RoutineFormViewModel.RoutineSteps
                    .Select(s => new NewRoutineStep(
                        s.Name, TimeSpan.FromSeconds(s.DurationSeconds)));
                NewRoutine newRoutine = new NewRoutine(RoutineFormViewModel.Name, newRoutineSteps);

                await _mutation.Execute(newRoutine);

                _logger.LogInformation("Successfully created routine: {RoutineName}", RoutineFormViewModel.Name);

                await _shell.GoToAsync("..");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create routine: {RoutineName}", RoutineFormViewModel.Name);

                RoutineFormViewModel.ErrorMessage = "Failed to create routine. Please try again later.";
            }
            finally
            {
                RoutineFormViewModel.Submitting = false;
            }
        }
    }
}
