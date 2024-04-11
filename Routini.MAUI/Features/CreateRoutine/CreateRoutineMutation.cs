using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Databases;
using SQLite;

namespace Routini.MAUI.Features.CreateRoutine
{
    public class CreateRoutineMutation
    {
        private readonly SqliteConnectionFactory _sqliteConnectionFactory;

        public CreateRoutineMutation(SqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        public async Task Execute(NewRoutine routine)
        {
            ISQLiteAsyncConnection database = _sqliteConnectionFactory.Create();

            RoutineDto routineDto = new RoutineDto()
            {
                Id = Guid.NewGuid(),
                Name = routine.Name
            };
            await database.InsertAsync(routineDto);

            IEnumerable<RoutineStepDto> routineStepDtos = routine.Steps.Select(s => new RoutineStepDto()
            {
                Id = Guid.NewGuid(),
                RoutineId = routineDto.Id,
                Name = s.Name,
                DurationMilliseconds = s.Duration.TotalMilliseconds
            });
            await database.InsertAllAsync(routineStepDtos);
        }
    }
}
