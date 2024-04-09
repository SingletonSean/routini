using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Databases;
using SQLite;

namespace Routini.MAUI.Features.CreateRoutine
{
    public class CreateRoutineCommand
    {
        private readonly SqliteConnectionFactory _sqliteConnectionFactory;

        public CreateRoutineCommand(SqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        public async Task Execute(NewRoutine routine)
        {
            ISQLiteAsyncConnection database = _sqliteConnectionFactory.Create();

            RoutineDto dto = new RoutineDto()
            {
                Id = Guid.NewGuid(),
                Name = routine.Name
            };

            await database.InsertAsync(dto);
        }
    }
}
