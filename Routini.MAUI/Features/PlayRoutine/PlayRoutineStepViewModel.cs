using CommunityToolkit.Mvvm.ComponentModel;

namespace Routini.MAUI.Features.PlayRoutine
{
    public class PlayRoutineStepViewModel : ObservableObject
    {
        public string Name { get; }
        public double DurationSeconds { get; }
        public int Order { get; }

        public PlayRoutineStepViewModel(string name, double durationSeconds, int order)
        {
            Name = name;
            DurationSeconds = durationSeconds;
            Order = order + 1;
        }
    }
}
