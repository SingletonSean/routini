using CommunityToolkit.Mvvm.ComponentModel;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Shells;

namespace Routini.MAUI.Features.EditRoutine
{
    public class EditRoutineFormViewModel : ObservableObject
    {
        private readonly Routine _routine;
        private readonly UpdateRoutineMutation _updateRoutineMutation;
        private readonly IShell _shell;

        public RoutineFormViewModel RoutineFormViewModel { get; }

        public EditRoutineFormViewModel(
            Routine routine,
            UpdateRoutineMutation updateRoutineMutation,
            IShell shell)
        {
            _routine = routine;
            _updateRoutineMutation = updateRoutineMutation;

            RoutineFormViewModel = new RoutineFormViewModel(EditRoutine);
            
            RoutineFormViewModel.Name = _routine.Name;
            RoutineFormViewModel.ResetRoutineSteps(_routine.Steps);

            _shell = shell;
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

                await _updateRoutineMutation.Execute(updatedRoutine);

                await _shell.GoToAsync("..");
            }
            catch (Exception)
            {
                RoutineFormViewModel.ErrorMessage = "Failed to update routine. Please try again later.";
            }
            finally
            {
                RoutineFormViewModel.Submitting = false;
            }
        }
    }
}
