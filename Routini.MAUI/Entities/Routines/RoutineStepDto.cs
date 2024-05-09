using SQLite;

namespace Routini.MAUI.Entities.Routines
{
    public class RoutineStepDto
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid RoutineId { get; set; }
        public string? Name { get; set; }
        public double? DurationSeconds { get; set; }
        public int? Order { get; set; }
    }
}
