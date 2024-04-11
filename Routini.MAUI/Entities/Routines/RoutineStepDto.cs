namespace Routini.MAUI.Entities.Routines
{
    public class RoutineStepDto
    {
        public Guid Id { get; set; }
        public Guid RoutineId { get; set; }
        public string? Name { get; set; }
        public double? DurationSeconds { get; set; }
    }
}
