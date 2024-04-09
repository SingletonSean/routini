using SQLite;

namespace Routini.MAUI.Shared.Databases
{
    public class SqliteConnectionFactory
    {
        public ISQLiteAsyncConnection Create()
        {
            return new SQLiteAsyncConnection(
                Path.Combine(FileSystem.Current.AppDataDirectory, "routini.db3"),
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
        }
    }
}
