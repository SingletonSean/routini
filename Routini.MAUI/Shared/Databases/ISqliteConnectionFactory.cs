using SQLite;

namespace Routini.MAUI.Shared.Databases
{
    public interface ISqliteConnectionFactory
    {
        ISQLiteAsyncConnection Create();
    }
}