using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Databases;
using SQLite;

namespace Routini.MAUI.Application.Database
{
    public class RoutiniDatabaseInitializer
    {
        private readonly ISqliteConnectionFactory _sqliteConnectionFactory;

        public RoutiniDatabaseInitializer(ISqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        public async Task Initialize()
        {
            ISQLiteAsyncConnection database = _sqliteConnectionFactory.Create();

            await database.CreateTableAsync<RoutineDto>();
            await database.CreateTableAsync<RoutineStepDto>();
        }
    }
}
