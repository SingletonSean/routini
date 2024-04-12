using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;
using Routini.MAUI.Entities.Routines;

namespace Routini.MAUI.Features.PlayRoutine
{
    public partial class PlayRoutineViewModel : ObservableObject
    {
        private readonly Routine _routine;
        private readonly IAudioManager _audio;

        private IAudioPlayer? _stepChangedSound;

        public string CurrentStepName => _routine.CurrentStep?.Name ?? string.Empty;
        public double CurrentStepSecondsRemaining => _routine.CurrentStepSecondsRemaining;

        public PlayRoutineViewModel(Routine routine, IAudioManager audio)
        {
            _routine = routine;
            _audio = audio;

            _routine.Updated += OnRoutineUpdated;
            _routine.StepChanged += OnRoutineStepChanged;
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
            catch (Exception)
            {

            }
        }
    }
}
