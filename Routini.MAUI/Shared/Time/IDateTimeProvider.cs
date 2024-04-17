namespace Routini.MAUI.Shared.Time
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow { get; }
    }
}
