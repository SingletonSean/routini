using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Shells;

namespace Routini.MAUI.Features.EditRoutine
{
    public class EditRoutineFormViewModel : ObservableObject
    {
        private readonly Routine _routine;
        private readonly UpdateRoutineMutation _updateRoutineMutation;
        private readonly IShell _shell;
        private readonly ILogger _logger;

        public RoutineFormViewModel RoutineFormViewModel { get; }

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

            RoutineFormViewModel = new RoutineFormViewModel(EditRoutine);

            RoutineFormViewModel.Reset(_routine);
        }

        private async Task EditRoutine()
        {
            RoutineFormViewModel.Submitting = true;
            RoutineFormViewModel.ErrorMessage = null;

            try
            {
                Routine updatedRoutine = new Routine(
                    _routine.Id,
                    RoutineFormViewModel.Name,
                    RoutineFormViewModel.RoutineSteps.Select(s => new RoutineStep(s.Name, TimeSpan.FromSeconds(s.DurationSeconds)))
                );
                _logger.LogInformation("Updating routine: {RoutineId}", _routine.Id);

                await _updateRoutineMutation.Execute(updatedRoutine);

                _logger.LogInformation("Successfully updated routine: {RoutineId}", _routine.Id);

                await _shell.GoToAsync("..");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update routine: {RoutineId}", _routine.Id);
                RoutineFormViewModel.ErrorMessage = "Failed to update routine. Please try again later.";
            }
            finally
            {
                RoutineFormViewModel.Submitting = false;
            }
        }
    }
}
