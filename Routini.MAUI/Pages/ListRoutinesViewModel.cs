using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.ListRoutines;
using Routini.MAUI.Shared.Shells;
using System.Collections.ObjectModel;

namespace Routini.MAUI.Pages
{
    public partial class ListRoutinesViewModel : ObservableObject
    {
        private readonly GetAllRoutinesQuery _query;
        private readonly IShell _shell;

        public ObservableCollection<RoutinePreviewViewModel> RoutinePreviews { get; }

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool? _loading;

        public ListRoutinesViewModel(GetAllRoutinesQuery query, IShell shell)
        {
            _query = query;
            _shell = shell;

            RoutinePreviews = new ObservableCollection<RoutinePreviewViewModel>();
        }

        [RelayCommand]
        private async Task LoadRoutines()
        {
            Loading = true;
            ErrorMessage = null;
            RoutinePreviews.Clear();

            try
            {
                IEnumerable<Routine> routines = await _query.Execute();

                foreach (Routine routine in routines)
                {
                    RoutinePreviews.Add(CreateRoutinePreviewViewModel(routine));
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load routines. Please try again later.";
            }
            finally
            {
                Loading = false;
            }
        }

        [RelayCommand]
        private async Task NavigateCreateRoutine()
        {
            await _shell.GoToAsync("Create");
        }

        private RoutinePreviewViewModel CreateRoutinePreviewViewModel(Routine routine)
        {
            return new RoutinePreviewViewModel(routine.Id, routine.Name, routine.Steps.Count());
        }
    }
}
