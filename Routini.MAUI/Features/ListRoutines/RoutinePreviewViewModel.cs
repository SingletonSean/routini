using CommunityToolkit.Mvvm.ComponentModel;

namespace Routini.MAUI.Features.ListRoutines
{
    public class RoutinePreviewViewModel : ObservableObject
    {
        public Guid Id { get; }
        public string Name { get; }

        public RoutinePreviewViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
