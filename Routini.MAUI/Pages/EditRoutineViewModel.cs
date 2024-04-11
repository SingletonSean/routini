using CommunityToolkit.Mvvm.ComponentModel;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.EditRoutine;
using Routini.MAUI.Features.ListRoutines;
using Routini.MAUI.Shared.Shells;

namespace Routini.MAUI.Pages
{
    public partial class EditRoutineViewModel : ObservableObject, IQueryAttributable
    {
        private readonly GetRoutineByIdQuery _getRoutineByIdQuery;
        private readonly UpdateRoutineMutation _updateRoutineMutation;
        private readonly IShell _shell;

        private Routine? _routine;

        public RoutineFormViewModel RoutineFormViewModel { get; }

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool? _loading;

        public EditRoutineViewModel(
            GetRoutineByIdQuery getRoutineByIdQuery,
            UpdateRoutineMutation updateRoutineMutation, 
            IShell shell)
        {
            _getRoutineByIdQuery = getRoutineByIdQuery;
            _updateRoutineMutation = updateRoutineMutation;
            _shell = shell;

            RoutineFormViewModel = new RoutineFormViewModel(EditRoutine);
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> queryParameters)
        {
            Loading = true;
            ErrorMessage = null;

            try
            {
                Guid id = Guid.Parse(queryParameters["Id"]?.ToString() ?? "");

                _routine = await _getRoutineByIdQuery.Execute(id);

                RoutineFormViewModel.Name = _routine.Name;
                RoutineFormViewModel.ResetRoutineSteps(_routine.Steps);
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to load routine. Please try again later.";
            }
            finally
            {
                Loading = false;
            }
        }

        private async Task EditRoutine()
        {
            if (_routine == null)
            {
                return;
            }

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
            catch(Exception)
            {
                ErrorMessage = "Failed to update routine. Please try again later.";
            }
            finally
            {
                RoutineFormViewModel.Submitting = false;
            }
        }
    }
}
