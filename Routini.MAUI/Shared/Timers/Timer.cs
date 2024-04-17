using System.Timers;

namespace Routini.MAUI.Shared.Timers
{
    public class Timer : ITimer
    {
        private readonly System.Timers.Timer _timer;

        public double Interval
        {
            get => _timer.Interval;
            set => _timer.Interval = value;
        }

        public event ElapsedEventHandler? Elapsed
        {
            add => _timer.Elapsed += value;
            remove => _timer.Elapsed -= value;
        }

        public Timer()
        {
            _timer = new System.Timers.Timer();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
