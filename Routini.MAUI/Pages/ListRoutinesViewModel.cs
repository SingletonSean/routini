using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ListRoutinesViewModel> _logger;

        public ObservableCollection<RoutinePreviewViewModel> RoutinePreviews { get; }

        public bool HasRoutinePreviews => RoutinePreviews.Any();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        private string? _errorMessage;

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        [ObservableProperty]
        private bool? _loading;

        public ListRoutinesViewModel(GetAllRoutinesQuery query, IShell shell, ILogger<ListRoutinesViewModel> logger)
        {
            _query = query;
            _shell = shell;
            _logger = logger;

            RoutinePreviews = new ObservableCollection<RoutinePreviewViewModel>();
            RoutinePreviews.CollectionChanged += OnRoutinePreviewsCollectionChanged;
        }

        [RelayCommand]
        private async Task LoadRoutines()
        {
            Loading = true;
            ErrorMessage = null;

            try
            {
                _logger.LogInformation("Loading routines.");

                IEnumerable<Routine> routines = await _query.Execute();

                ResetRoutines(routines);

                _logger.LogInformation("Successfully loaded routines: {RoutineCount}", routines.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load routines.");

                ErrorMessage = "Failed to load routines. Please try again later.";
            }
            finally
            {
                Loading = false;
            }
        }

        private void ResetRoutines(IEnumerable<Routine> routines)
        {
            RoutinePreviews.Clear();

            foreach (Routine routine in routines)
            {
                RoutinePreviews.Add(CreateRoutinePreviewViewModel(routine));
            }
        }

        [RelayCommand]
        private async Task NavigateCreateRoutine()
        {
            await _shell.GoToAsync("Create");
        }

        private RoutinePreviewViewModel CreateRoutinePreviewViewModel(Routine routine)
        {
            return new RoutinePreviewViewModel(routine.Id, routine.Name, routine.Steps.Count(), _shell);
        }

        private void OnRoutinePreviewsCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasRoutinePreviews));
        }
    }
}
