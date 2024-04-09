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

            IEnumerable<RoutineDto> dtos = await database.Table<RoutineDto>().ToListAsync();

            return dtos.Select(d => new Routine(d.Name, new List<RoutineStep>()));
        }
    }
}
