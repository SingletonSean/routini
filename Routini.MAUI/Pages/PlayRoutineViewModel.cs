using CommunityToolkit.Mvvm.ComponentModel;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.ListRoutines;

namespace Routini.MAUI.Pages
{
    public partial class PlayRoutineViewModel : ObservableObject, IQueryAttributable
    {
        private readonly GetRoutineByIdQuery _query;

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool? _loading;

        public PlayRoutineViewModel(GetRoutineByIdQuery query)
        {
            _query = query;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> queryParameters)
        {
            Loading = true;
            ErrorMessage = null;

            try
            {
                Guid id = Guid.Parse(queryParameters["Id"]?.ToString() ?? "");

                Routine routine = await _query.Execute(id);

                Name = routine.Name;
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
