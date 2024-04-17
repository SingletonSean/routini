using System.Timers;

namespace Routini.MAUI.Shared.Timers
{
    public interface ITimer : IDisposable
    {
        double Interval { get; set; }

        event ElapsedEventHandler? Elapsed;

        void Start();
        void Stop();
    }
}
