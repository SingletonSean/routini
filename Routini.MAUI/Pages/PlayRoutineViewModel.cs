using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.DeleteRoutine;
using Routini.MAUI.Features.ListRoutines;
using Routini.MAUI.Shared.Shells;

namespace Routini.MAUI.Pages
{
    public partial class PlayRoutineViewModel : ObservableObject, IQueryAttributable
    {
        private readonly GetRoutineByIdQuery _query;
        private readonly DeleteRoutineMutation _deleteRoutineMutation;
        private readonly IAudioManager _audio;
        private readonly IShell _shell;

        private IAudioPlayer? _stepChangedSound;

        private Routine? _routine;
        public Routine? Routine
        {
            get => _routine;
            set
            {
                if (_routine != null)
                {
                    _routine.Updated -= OnRoutineUpdated;
                    _routine.StepChanged -= OnRoutineStepChanged;
                }

                _routine = value;

                if (_routine != null)
                {
                    _routine.Updated += OnRoutineUpdated;
                    _routine.StepChanged += OnRoutineStepChanged;
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

        public PlayRoutineViewModel(
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

            await _deleteRoutineMutation.Execute(Routine.Id);

            await _shell.GoToAsync("..");
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
