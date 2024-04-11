using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.ListRoutines;

namespace Routini.MAUI.Pages
{
    public partial class PlayRoutineViewModel : ObservableObject, IQueryAttributable
    {
        private readonly GetRoutineByIdQuery _query;

        private Routine? _routine;
        public Routine? Routine
        {
            get => _routine;
            set
            {
                if (_routine != null)
                {
                    _routine.Updated -= OnRoutineUpdated;
                }

                _routine = value;

                if (_routine != null)
                {
                    _routine.Updated += OnRoutineUpdated;
                }

                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CurrentStepName));
                OnPropertyChanged(nameof(CurrentStepSecondsRemaining));
            }
        }

        public string Name => Routine?.Name ?? string.Empty;
        public string CurrentStepName => Routine?.CurrentStep?.Name ?? string.Empty;
        public double CurrentStepSecondsRemaining => Routine?.CurrentStepSecondsRemaining ?? 0;

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

                Routine = await _query.Execute(id);
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
        private void StartRoutine()
        {
            Routine?.Start();
        }

        [RelayCommand]
        private void PauseRoutine()
        {
            Routine?.Pause();
        }

        [RelayCommand]
        private void ResumeRoutine()
        {
            Routine?.Resume();
        }

        [RelayCommand]
        private void CancelRoutine()
        {
            Routine?.Cancel();
        }

        private void OnRoutineUpdated()
        {
            OnPropertyChanged(nameof(CurrentStepName));
            OnPropertyChanged(nameof(CurrentStepSecondsRemaining));
        }
    }
}
