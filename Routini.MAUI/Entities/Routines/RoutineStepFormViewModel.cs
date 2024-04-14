using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Routini.MAUI.Entities.Routines
{
    public partial class RoutineStepFormViewModel : ObservableValidator
    {
        private readonly Action<RoutineStepFormViewModel> _onDelete;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Required")]
        [MinLength(1, ErrorMessage = "Required")]
        private string _name = string.Empty;
        public string? NameErrorMessage => GetErrors(nameof(Name)).FirstOrDefault()?.ErrorMessage;
        public bool HasNameErrorMessage => !string.IsNullOrEmpty(NameErrorMessage);

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Must be greater than 0")]
        private double _durationSeconds = 30;
        public string? DurationSecondsErrorMessage => GetErrors(nameof(DurationSeconds)).FirstOrDefault()?.ErrorMessage;
        public bool HasDurationSecondsErrorMessage => !string.IsNullOrEmpty(DurationSecondsErrorMessage);

        public RoutineStepFormViewModel(Action<RoutineStepFormViewModel> onDelete)
        {
            _onDelete = onDelete;

            ErrorsChanged += OnErrorsChanged;
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

        public void Validate()
        {
            ValidateAllProperties();
        }

        private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(NameErrorMessage));
            OnPropertyChanged(nameof(HasNameErrorMessage));
            OnPropertyChanged(nameof(DurationSecondsErrorMessage));
            OnPropertyChanged(nameof(HasDurationSecondsErrorMessage));
        }
    }
}
