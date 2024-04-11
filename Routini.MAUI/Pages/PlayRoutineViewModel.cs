using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.ListRoutines;
using System.Timers;

namespace Routini.MAUI.Pages
{
    public partial class PlayRoutineViewModel : ObservableObject, IQueryAttributable
    {
        private readonly GetRoutineByIdQuery _query;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Name))]
        private Routine? _routine;

        public string Name => Routine?.Name ?? string.Empty;

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

        private System.Timers.Timer _timer = new System.Timers.Timer();
        private int _currentStepIndex = 0;
        private DateTimeOffset _currentStepStartTime;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentStepName))]
        private RoutineStep? _currentStep;

        public string CurrentStepName => CurrentStep?.Name ?? string.Empty;

        public double CurrentStepSecondsRemaining
        {
            get
            {
                if (CurrentStep == null)
                {
                    return 0;
                }

                return Math.Ceiling(
                    CurrentStep.Duration.TotalSeconds - 
                    DateTimeOffset.Now.Subtract(_currentStepStartTime).TotalSeconds);
            }
        }

        [RelayCommand]
        private void StartRoutine()
        {
            if (Routine == null)
            {
                return;
            }

            _timer.Interval = 250;
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();

            _currentStepIndex = 0;
            _currentStepStartTime = DateTimeOffset.Now;
            CurrentStep = Routine.Steps.ElementAt(_currentStepIndex);
        }

        [RelayCommand]
        private void PauseRoutine()
        {
            _timer.Stop();
        }

        [RelayCommand]
        private void ResumeRoutine()
        {
            _timer.Start();
        }

        [RelayCommand]
        private void CancelRoutine()
        {
            _timer.Stop();
            _timer.Elapsed -= OnTimerElapsed;
            _currentStepIndex = 0;
            CurrentStep = null;
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (Routine == null)
            {
                return;
            }

            OnPropertyChanged(nameof(CurrentStepSecondsRemaining));

            if (CurrentStepSecondsRemaining <= 0)
            {
                _currentStepIndex += 1;
                _currentStepStartTime = DateTimeOffset.Now;
                CurrentStep = Routine.Steps.ElementAt(_currentStepIndex);
            }
        }
    }
}
