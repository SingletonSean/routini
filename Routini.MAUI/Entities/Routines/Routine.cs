namespace Routini.MAUI.Entities.Routines
{
    public class Routine
    {
        public string Name { get; }
        public IEnumerable<RoutineStep> Steps { get; }

        public Routine(string name, IEnumerable<RoutineStep> steps)
        {
            Name = name;
            Steps = steps;
        }
    }
}
