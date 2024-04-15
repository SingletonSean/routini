using SQLite;

namespace Routini.MAUI.Shared.Databases
{
    public class InMemorySqliteConnectionFactory : ISqliteConnectionFactory
    {
        public ISQLiteAsyncConnection Create()
        {
            return new SQLiteAsyncConnection("Data Source=InMemorySample;Mode=Memory;Cache=Shared");
        }
    }
}
