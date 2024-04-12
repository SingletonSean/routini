using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.DeleteRoutine;
using Routini.MAUI.Features.ListRoutines;
using Routini.MAUI.Features.PlayRoutine;
using Routini.MAUI.Shared.Shells;

namespace Routini.MAUI.Pages
{
    public partial class RoutineDetailViewModel : ObservableObject, IQueryAttributable
    {
        private readonly GetRoutineByIdQuery _query;
        private readonly DeleteRoutineMutation _deleteRoutineMutation;
        private readonly IAudioManager _audio;
        private readonly IShell _shell;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Name))]
        private Routine? _routine;

        public string Name => Routine?.Name ?? string.Empty;

        [ObservableProperty]
        private PlayRoutineViewModel? _playRoutineViewModel;

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool? _loading;

        public RoutineDetailViewModel(
            GetRoutineByIdQuery query,
            DeleteRoutineMutation deleteRoutineMutation,
            IAudioManager audio,
            IShell shell)
        {
            _query = query;
            _deleteRoutineMutation = deleteRoutineMutation;
            _audio = audio;
            _shell = shell;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> queryParameters)
        {
            Loading = true;
            ErrorMessage = null;

            try
            {
                Guid id = Guid.Parse(queryParameters["Id"]?.ToString() ?? "");

                Routine = await _query.Execute(id);

                PlayRoutineViewModel = new PlayRoutineViewModel(Routine, _audio);
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

        [RelayCommand]
        private async Task EditRoutine()
        {
            if (Routine == null)
            {
                return;
            }

            await _shell.GoToAsync($"Edit?Id={Routine.Id}");
        }

        [RelayCommand]
        private async Task DeleteRoutine()
        {
            if (Routine == null)
            {
                return;
            }

            if (!await _shell.DisplayAlert(
                "Delete Routine", 
                "Are you sure you want to delete this routine?",
                "Yes",
                "Cancel"))
            {
                return;
            }

            await _deleteRoutineMutation.Execute(Routine.Id);

            await _shell.GoToAsync("..");
        }

        [RelayCommand]
        private void DisposeRoutine()
        {
            PlayRoutineViewModel?.CancelRoutineCommand.Execute(null);
        }
    }
}
