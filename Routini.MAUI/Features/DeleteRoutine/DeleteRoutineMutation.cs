using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Databases;
using SQLite;

namespace Routini.MAUI.Features.DeleteRoutine
{
    public class DeleteRoutineMutation
    {
        private readonly ISqliteConnectionFactory _sqliteConnectionFactory;

        public DeleteRoutineMutation(ISqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        public async Task Execute(Guid id)
        {
            ISQLiteAsyncConnection database = _sqliteConnectionFactory.Create();

            await database.DeleteAsync<RoutineDto>(id);
        }
    }
}
