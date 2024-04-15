using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Databases;
using SQLite;

namespace Routini.MAUI.Features.ListRoutines
{
    public class GetRoutineByIdQuery
    {
        private readonly ISqliteConnectionFactory _sqliteConnectionFactory;

        public GetRoutineByIdQuery(ISqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        public async Task<Routine> Execute(Guid id)
        {
            ISQLiteAsyncConnection database = _sqliteConnectionFactory.Create();

            RoutineDto routineDto = await database
                .Table<RoutineDto>()
                .FirstOrDefaultAsync(r => r.Id == id);

            IEnumerable<RoutineStepDto> routineStepDtos = await database
                .Table<RoutineStepDto>()
                .Where(s => s.RoutineId == id)
                .ToListAsync();

            return new Routine(
                routineDto.Id,
                routineDto.Name ?? string.Empty,
                routineStepDtos
                    .OrderBy(s => s.Order)
                    .Select(s => new RoutineStep(
                        s.Name ?? string.Empty, 
                        TimeSpan.FromSeconds(s.DurationSeconds ?? 0))));
        }
    }
}
