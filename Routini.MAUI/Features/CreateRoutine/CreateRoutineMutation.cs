using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Databases;
using SQLite;

namespace Routini.MAUI.Features.CreateRoutine
{
    public class CreateRoutineMutation
    {
        private readonly ISqliteConnectionFactory _sqliteConnectionFactory;

        public CreateRoutineMutation(ISqliteConnectionFactory sqliteConnectionFactory)
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

            IEnumerable<RoutineStepDto> routineStepDtos = routine.Steps.Select((s, i) => new RoutineStepDto()
            {
                Id = Guid.NewGuid(),
                RoutineId = routineDto.Id,
                Name = s.Name,
                DurationSeconds = s.Duration.TotalSeconds,
                Order = i
            });
            await database.InsertAllAsync(routineStepDtos);
        }
    }
}
