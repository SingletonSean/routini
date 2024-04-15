using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EditRoutineViewModel> _logger;

        [ObservableProperty]
        private EditRoutineFormViewModel? _editRoutineFormViewModel;

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool? _loading;

        public EditRoutineViewModel(
            GetRoutineByIdQuery getRoutineByIdQuery,
            UpdateRoutineMutation updateRoutineMutation,
            IShell shell,
            ILogger<EditRoutineViewModel> logger)
        {
            _getRoutineByIdQuery = getRoutineByIdQuery;
            _updateRoutineMutation = updateRoutineMutation;
            _shell = shell;
            _logger = logger;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> queryParameters)
        {
            Loading = true;
            ErrorMessage = null;

            try
            {
                Guid id = Guid.Parse(queryParameters["Id"]?.ToString() ?? "");
                _logger.LogInformation("Loading routine to edit: {RoutineId}", id);

                Routine routine = await _getRoutineByIdQuery.Execute(id);

                EditRoutineFormViewModel = new EditRoutineFormViewModel(routine, _updateRoutineMutation, _shell, _logger);
                
                _logger.LogInformation("Successfully loaded routine to edit: {RoutineId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load routine to edit: {EditQueryParameters}", queryParameters);

                ErrorMessage = "Failed to load routine. Please try again later.";
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
