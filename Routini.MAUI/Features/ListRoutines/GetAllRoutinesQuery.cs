using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Databases;
using SQLite;

namespace Routini.MAUI.Features.ListRoutines
{
    public class GetAllRoutinesQuery
    {
        private readonly SqliteConnectionFactory _sqliteConnectionFactory;

        public GetAllRoutinesQuery(SqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        public async Task<IEnumerable<Routine>> Execute()
        {
            ISQLiteAsyncConnection database = _sqliteConnectionFactory.Create();

            IEnumerable<RoutineDto> routineDtos = await database
                .Table<RoutineDto>()
                .ToListAsync();
            IEnumerable<Guid> routineIds = routineDtos.Select(r => r.Id);

            IEnumerable<RoutineStepDto> routineStepDtos = await database
                .Table<RoutineStepDto>()
                .Where(routineStep => routineIds.Contains(routineStep.RoutineId))
                .ToListAsync();
            ILookup<Guid, RoutineStepDto> routineStepsForRoutine = routineStepDtos
                .ToLookup(r => r.RoutineId);

            return routineDtos.Select(d => new Routine(
                d.Id, 
                d.Name, 
                routineStepsForRoutine[d.Id]
                    .Select(s => new RoutineStep(s.Name, TimeSpan.FromMilliseconds(s.DurationMilliseconds ?? 0)))));
        }
    }
}
