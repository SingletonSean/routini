using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Databases;
using SQLite;

namespace Routini.MAUI.Application.Database
{
    public class RoutiniDatabaseInitializer
    {
        private readonly SqliteConnectionFactory _sqliteConnectionFactory;

        public RoutiniDatabaseInitializer(SqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        public async Task Initialize()
        {
            ISQLiteAsyncConnection database = _sqliteConnectionFactory.Create();

            await database.CreateTableAsync<RoutineDto>();
        }
    }
}
