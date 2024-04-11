using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Shared.Shells;

namespace Routini.MAUI.Pages
{
    public partial class CreateRoutineViewModel : ObservableObject
    {
        private readonly CreateRoutineMutation _mutation;
        private readonly IShell _shell;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool? _loading;

        public CreateRoutineViewModel(CreateRoutineMutation mutation, IShell shell)
        {
            _mutation = mutation;
            _shell = shell;
        }

        [RelayCommand]
        private void ResetForm()
        {
            Name = string.Empty;
            Loading = false;
            ErrorMessage = null;
        }

        [RelayCommand]
        private async Task CreateRoutine()
        {
            Loading = true;
            ErrorMessage = null;

            try
            {
                NewRoutine newRoutine = new NewRoutine(Name);

                await _mutation.Execute(newRoutine);

                await _shell.GoToAsync("..");
            }
            catch (Exception)
            {
                ErrorMessage = "Failed to create routine. Please try again later.";
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
