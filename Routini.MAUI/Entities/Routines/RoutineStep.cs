namespace Routini.MAUI.Entities.Routines
{
    public class RoutineStep
    {
        public string Name { get; }
        public TimeSpan Duration { get; }

        public RoutineStep(string name, TimeSpan duration)
        {
            Name = name;
            Duration = duration;
        }
    }
}
