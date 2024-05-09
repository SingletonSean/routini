using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.DeleteRoutine;
using Routini.MAUI.Features.ListRoutines;
using Routini.MAUI.Features.PlayRoutine;
using Routini.MAUI.Shared.Shells;
using Routini.MAUI.Shared.Time;
using System.ComponentModel;

namespace Routini.MAUI.Pages
{
    public partial class RoutineDetailViewModel : ObservableObject, IQueryAttributable
    {
        private readonly GetRoutineByIdQuery _query;
        private readonly DeleteRoutineMutation _deleteRoutineMutation;
        private readonly IAudioManager _audio;
        private readonly IShell _shell;
        private readonly ILogger<RoutineDetailViewModel> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly Func<Shared.Timers.ITimer> _timerFactory;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Name))]
        private Routine? _routine;

        public string Name => Routine?.Name ?? string.Empty;

        private PlayRoutineViewModel? _playRoutineViewModel;
        public PlayRoutineViewModel? PlayRoutineViewModel
        {
            get
            {
                return _playRoutineViewModel;
            }
            private set
            {
                if (_playRoutineViewModel != null)
                {
                    _playRoutineViewModel.PropertyChanged -= OnPlayRoutineViewModelPropertyChanged;
                }

                _playRoutineViewModel = value;

                if (_playRoutineViewModel != null)
                {
                    _playRoutineViewModel.PropertyChanged += OnPlayRoutineViewModelPropertyChanged;
                }

                OnPropertyChanged(nameof(PlayRoutineViewModel));
                OnPropertyChanged(nameof(Started));
            }
        }

        public bool Started => PlayRoutineViewModel?.Started ?? false;

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool? _loading;

        public RoutineDetailViewModel(
            GetRoutineByIdQuery query,
            DeleteRoutineMutation deleteRoutineMutation,
            IAudioManager audio,
            IShell shell,
            ILogger<RoutineDetailViewModel> logger,
            IDateTimeProvider dateTimeProvider,
            Func<Shared.Timers.ITimer> timerFactory)
        {
            _query = query;
            _deleteRoutineMutation = deleteRoutineMutation;
            _audio = audio;
            _shell = shell;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _timerFactory = timerFactory;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> queryParameters)
        {
            await ApplyQueryAttributesAsync(queryParameters);
        }

        public async Task ApplyQueryAttributesAsync(IDictionary<string, object> queryParameters)
        {
            Loading = true;
            ErrorMessage = null;

            try
            {
                Guid id = Guid.Parse(queryParameters["Id"]?.ToString() ?? "");
                _logger.LogInformation("Loading routine: {RoutineId}", id);

                Routine = await _query.Execute(id);

                PlayRoutineViewModel = new PlayRoutineViewModel(Routine, _audio, _logger, _dateTimeProvider, _timerFactory());
                _logger.LogInformation("Successfully loaded routine: {RoutineId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load routine: {DetailsQueryParameters}", queryParameters);
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

            _logger.LogInformation("Deleting routine: {RoutineId}", Routine.Id);

            await _deleteRoutineMutation.Execute(Routine.Id);

            await _shell.GoToAsync("..");
            _logger.LogInformation("Successfully deleted routine: {RoutineId}", Routine.Id);
        }

        [RelayCommand]
        private void DisposeRoutine()
        {
            PlayRoutineViewModel?.Dispose();
        }

        private void OnPlayRoutineViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Started));
        }
    }
}
