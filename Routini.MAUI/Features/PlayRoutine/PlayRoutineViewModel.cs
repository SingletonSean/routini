using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Time;
using System.Collections.ObjectModel;

namespace Routini.MAUI.Features.PlayRoutine
{
    public partial class PlayRoutineViewModel : ObservableObject, IDisposable
    {
        private readonly PlayableRoutine _routine;
        private readonly IAudioManager _audio;
        private readonly ILogger _logger;

        private IAudioPlayer? _stepChangedSound;

        public bool Started => _routine.Started;
        public bool Paused => _routine.Paused;
        public IEnumerable<PlayRoutineStepViewModel> Steps { get; }
        public int CurrentStepOrder => _routine.CurrentStepOrder + 1;
        public string? CurrentStepName => _routine.CurrentStep?.Name;
        public double CurrentStepSecondsRemaining => _routine.CurrentStepSecondsRemaining;

        public PlayRoutineViewModel(
            Routine routine, 
            IAudioManager audio, 
            ILogger logger,
            IDateTimeProvider dateTimeProvider,
            Shared.Timers.ITimer timer)
        {
            _routine = new PlayableRoutine(routine, dateTimeProvider, timer);
            _audio = audio;
            _logger = logger;

            Steps = new ObservableCollection<PlayRoutineStepViewModel>(
                routine.Steps.Select((s, i) => new PlayRoutineStepViewModel(s.Name, s.Duration.TotalSeconds, i))
            );

            _routine.Updated += OnRoutineUpdated;
            _routine.StepChanged += OnRoutineStepChanged;
        }

        public void Dispose()
        {
            _routine.Dispose();
        }

        [RelayCommand]
        private void StartRoutine()
        {
            _routine.Start();
        }

        [RelayCommand]
        private void PauseRoutine()
        {
            _routine.Pause();
        }

        [RelayCommand]
        private void ResumeRoutine()
        {
            _routine.Resume();
        }

        [RelayCommand]
        private void CancelRoutine()
        {
            _routine.Cancel();
        }

        private void OnRoutineUpdated()
        {
            OnPropertyChanged(nameof(Started));
            OnPropertyChanged(nameof(Paused));
            OnPropertyChanged(nameof(CurrentStepOrder));
            OnPropertyChanged(nameof(CurrentStepName));
            OnPropertyChanged(nameof(CurrentStepSecondsRemaining));
        }

        private async void OnRoutineStepChanged()
        {
            try
            {
                if (_stepChangedSound == null)
                {
                    _stepChangedSound = _audio.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("ping.mp3"));
                }

                _stepChangedSound.Play();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to play routine step change audio.");
            }
        }
    }
}
