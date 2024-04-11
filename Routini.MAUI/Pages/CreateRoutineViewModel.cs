using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Shared.Shells;
using System.Collections.ObjectModel;

namespace Routini.MAUI.Pages
{
    public partial class CreateRoutineViewModel : ObservableObject
    {
        private readonly CreateRoutineMutation _mutation;
        private readonly IShell _shell;

        [ObservableProperty]
        private string _name = string.Empty;

        public ObservableCollection<RoutineStepFormViewModel> RoutineSteps { get; set; }

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool? _loading;

        public CreateRoutineViewModel(CreateRoutineMutation mutation, IShell shell)
        {
            _mutation = mutation;
            _shell = shell;

            RoutineSteps = new ObservableCollection<RoutineStepFormViewModel>();
        }

        [RelayCommand]
        private void ResetForm()
        {
            Name = string.Empty;
            RoutineSteps.Clear();
            Loading = false;
            ErrorMessage = null;
        }

        [RelayCommand]
        private void AddRoutineStep()
        {
            RoutineSteps.Add(new RoutineStepFormViewModel(DeleteRoutineStep));
        }

        private void DeleteRoutineStep(RoutineStepFormViewModel viewModel)
        {
            RoutineSteps.Remove(viewModel);
        }

        [RelayCommand]
        private async Task CreateRoutine()
        {
            Loading = true;
            ErrorMessage = null;

            try
            {
                IEnumerable<NewRoutineStep> newRoutineSteps = RoutineSteps
                    .Select(s => new NewRoutineStep(
                        s.Name, TimeSpan.FromSeconds(s.DurationSeconds)));
                NewRoutine newRoutine = new NewRoutine(Name, newRoutineSteps);

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
