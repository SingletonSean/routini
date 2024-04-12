using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Must be greater than 0")]
        private double _durationSeconds = 30;

        public RoutineStepFormViewModel(Action<RoutineStepFormViewModel> onDelete)
        {
            _onDelete = onDelete;
        }

        [RelayCommand]
        private void Delete()
        {
            _onDelete(this);
        }
    }
}
