using SQLite;

namespace Routini.MAUI.Shared.Databases
{
    public class InMemorySqliteConnectionFactory : ISqliteConnectionFactory
    {
        private readonly Guid _id;

        public InMemorySqliteConnectionFactory()
        {
            _id = Guid.NewGuid();
        }

        public ISQLiteAsyncConnection Create()
        {
            return new SQLiteAsyncConnection($"Data Source={_id};Mode=Memory;Cache=Shared");
        }
    }
}
