using System.Timers;

namespace Routini.MAUI.Entities.Routines
{
    public class Routine
    {
        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<RoutineStep> Steps { get; }

        public Routine(Guid id, string name, IEnumerable<RoutineStep> steps)
        {
            Id = id;
            Name = name;
            Steps = steps;
        }
    }
}
