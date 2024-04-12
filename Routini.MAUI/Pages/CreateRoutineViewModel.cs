using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Shared.Shells;

namespace Routini.MAUI.Pages
{
    public partial class CreateRoutineViewModel : ObservableObject
    {
        private readonly CreateRoutineMutation _mutation;
        private readonly IShell _shell;

        public RoutineFormViewModel RoutineFormViewModel { get; }

        public CreateRoutineViewModel(CreateRoutineMutation mutation, IShell shell)
        {
            _mutation = mutation;
            _shell = shell;

            RoutineFormViewModel = new RoutineFormViewModel(CreateRoutine);
        }

        [RelayCommand]
        private void ResetForm()
        {
            RoutineFormViewModel.Submitting = false;
            RoutineFormViewModel.ErrorMessage = null;

            RoutineFormViewModel.Name = string.Empty;
            RoutineFormViewModel.RoutineSteps.Clear();
        }

        private async Task CreateRoutine()
        {
            RoutineFormViewModel.Submitting = true;
            RoutineFormViewModel.ErrorMessage = null;

            try
            {
                IEnumerable<NewRoutineStep> newRoutineSteps = RoutineFormViewModel.RoutineSteps
                    .Select(s => new NewRoutineStep(
                        s.Name, TimeSpan.FromSeconds(s.DurationSeconds)));
                NewRoutine newRoutine = new NewRoutine(RoutineFormViewModel.Name, newRoutineSteps);

                await _mutation.Execute(newRoutine);

                await _shell.GoToAsync("..");
            }
            catch (Exception)
            {
                RoutineFormViewModel.ErrorMessage = "Failed to create routine. Please try again later.";
            }
            finally
            {
                RoutineFormViewModel.Submitting = false;
            }
        }
    }
}
