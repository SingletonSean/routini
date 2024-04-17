using System.Timers;

namespace Routini.MAUI.Test.Mocks
{
    public class MockTimer : Shared.Timers.ITimer
    {
        private bool _running;

        public double Interval { get; set; }

        public event ElapsedEventHandler? Elapsed;

        public void Dispose() { }

        public void Start()
        {
            _running = true;
        }

        public void Stop()
        {
            _running = false;
        }

        public void RaiseElapsed()
        {
            if (!_running)
            {
                return;
            }

            Elapsed?.Invoke(this, null!);
        }
    }
}
