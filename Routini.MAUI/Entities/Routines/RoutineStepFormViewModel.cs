using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Routini.MAUI.Shared.ViewModels;

namespace Routini.MAUI.Entities.Routines
{
    public partial class RoutineStepFormViewModel : ValidationViewModel
    {
        private readonly Action<RoutineStepFormViewModel> _onDelete;

        private string _name = string.Empty;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;

                ValidateName();

                OnPropertyChanged(nameof(Name));
            }
        }

        public string? NameErrorMessage => GetFirstError(nameof(Name));
        public bool HasNameErrorMessage => !string.IsNullOrEmpty(NameErrorMessage);

        private void ValidateName()
        {
            ClearErrors(nameof(Name));

            if (string.IsNullOrEmpty(Name))
            {
                AddError(nameof(Name), "Required");
            }

            OnPropertyChanged(nameof(NameErrorMessage));
            OnPropertyChanged(nameof(HasNameErrorMessage));
        }

        private double _durationSeconds = 30;
        public double DurationSeconds
        {
            get
            {
                return _durationSeconds;
            }
            set
            {
                if (_durationSeconds == value)
                {
                    return;
                }

                _durationSeconds = value;

                ValidateDurationSeconds();

                OnPropertyChanged(nameof(DurationSeconds));
            }
        }

        public string? DurationSecondsErrorMessage => GetFirstError(nameof(DurationSeconds));
        public bool HasDurationSecondsErrorMessage => !string.IsNullOrEmpty(DurationSecondsErrorMessage);

        private void ValidateDurationSeconds()
        {
            ClearErrors(nameof(DurationSeconds));

            if (DurationSeconds <= 0)
            {
                AddError(nameof(DurationSeconds), "Must be greater than 0");
            }

            OnPropertyChanged(nameof(DurationSecondsErrorMessage));
            OnPropertyChanged(nameof(HasDurationSecondsErrorMessage));
        }

        public RoutineStepFormViewModel(Action<RoutineStepFormViewModel> onDelete)
        {
            _onDelete = onDelete;
        }

        public void Validate()
        {
            ValidateName();
            ValidateDurationSeconds();
        }

        public void Reset()
        {
            Name = string.Empty;
            DurationSeconds = 30;
        }

        [RelayCommand]
        private void Delete()
        {
            _onDelete(this);
        }
    }
}
