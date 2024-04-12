using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Routini.MAUI.Shared.Shells;

namespace Routini.MAUI.Features.ListRoutines
{
    public partial class RoutinePreviewViewModel : ObservableObject
    {
        private readonly IShell _shell;

        public Guid Id { get; }
        public string Name { get; }
        public int StepsCount { get; }

        public RoutinePreviewViewModel(Guid id, string name, int stepsCount, IShell shell)
        {
            Id = id;
            Name = name;
            StepsCount = stepsCount;
            _shell = shell;
        }

        [RelayCommand]
        private async Task NavigateRoutinePlay()
        {
            await _shell.GoToAsync($"Detail?Id={Id}");
        }
    }
}
