using SQLite;

namespace Routini.MAUI.Entities.Routines
{
    public class RoutineDto
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
