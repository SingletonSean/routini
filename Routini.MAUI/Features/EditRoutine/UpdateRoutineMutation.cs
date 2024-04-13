using Microsoft.Maui.Controls;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Databases;
using SQLite;

namespace Routini.MAUI.Features.EditRoutine
{
    public class UpdateRoutineMutation
    {
        private readonly SqliteConnectionFactory _sqliteConnectionFactory;

        public UpdateRoutineMutation(SqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        public async Task Execute(Routine updatedRoutine)
        {
            ISQLiteAsyncConnection database = _sqliteConnectionFactory.Create();

            RoutineDto routineDto = new RoutineDto()
            {
                Id = updatedRoutine.Id,
                Name = updatedRoutine.Name
            };
            await database.UpdateAsync(routineDto);

            await database.Table<RoutineStepDto>().DeleteAsync(s => s.RoutineId == updatedRoutine.Id);
            IEnumerable<RoutineStepDto> routineStepDtos = updatedRoutine.Steps.Select((s, i) => new RoutineStepDto()
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
