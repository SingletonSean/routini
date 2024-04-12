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

        [ObservableProperty]
        private EditRoutineFormViewModel? _editRoutineFormViewModel;

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
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> queryParameters)
        {
            Loading = true;
            ErrorMessage = null;

            try
            {
                Guid id = Guid.Parse(queryParameters["Id"]?.ToString() ?? "");

                Routine routine = await _getRoutineByIdQuery.Execute(id);

                EditRoutineFormViewModel = new EditRoutineFormViewModel(routine, _updateRoutineMutation, _shell);
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
    }
}
