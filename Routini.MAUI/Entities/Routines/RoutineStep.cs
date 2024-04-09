namespace Routini.MAUI.Entities.Routines
{
    public class RoutineStep
    {
        public string Name { get; }
        public string Description { get; }
        public TimeSpan Duration { get; }

        public RoutineStep(string name, string description, TimeSpan duration)
        {
            Name = name;
            Description = description;
            Duration = duration;
        }
    }
}
