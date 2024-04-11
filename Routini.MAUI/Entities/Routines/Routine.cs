using System.Timers;

namespace Routini.MAUI.Entities.Routines
{
    public class Routine
    {
        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<RoutineStep> Steps { get; }

        public Routine(Guid id, string name, IEnumerable<RoutineStep> steps)
        {
            Id = id;
            Name = name;
            Steps = steps;

            _timer = new System.Timers.Timer();
            _timer.Interval = 100;
            _timer.Elapsed += OnTimerElapsed;
        }

        private readonly System.Timers.Timer _timer;

        public event Action? Updated;
        
        private int _currentStepIndex = 0;
        private DateTimeOffset _currentStepStartTime;
        public RoutineStep? CurrentStep
        {
            get
            {
                if (_currentStepIndex >= Steps.Count())
                {
                    return null;
                }

                return Steps.ElementAt(_currentStepIndex);
            }
        }

        public double CurrentStepSecondsRemaining
        {
            get
            {
                if (CurrentStep == null)
                {
                    return 0;
                }

                return Math.Ceiling(
                    CurrentStep.Duration.TotalSeconds -
                    DateTimeOffset.Now.Subtract(_currentStepStartTime).TotalSeconds);
            }
        }

        public bool Started { get; set; }

        private DateTimeOffset _pauseTime;
        public bool Paused { get; set; }
        
        public bool Completed { get; set; }

        public void Start()
        {
            _currentStepIndex = 0;
            _currentStepStartTime = DateTimeOffset.Now;

            _timer.Start();

            Started = true;
            Paused = false;
            Completed = false;
        }

        public void Pause()
        {
            _timer.Stop();
            _pauseTime = DateTimeOffset.Now;

            Paused = true;
        }

        public void Resume()
        {
            _timer.Start();
            _currentStepStartTime = _currentStepStartTime.Add(DateTimeOffset.Now.Subtract(_pauseTime));

            Paused = false;
        }

        public void Cancel()
        {
            _timer.Stop();

            Started = false;
            Paused = false;
            Completed = false;
        }

        private void Complete()
        {
            _timer.Stop();

            Started = false;
            Paused = false;
            Completed = true;
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (CurrentStepSecondsRemaining <= 0)
            {
                _currentStepIndex++;
                _currentStepStartTime = DateTimeOffset.Now;
            }

            if (CurrentStep == null)
            {
                Complete();
            }

            Updated?.Invoke();
        }
    }
}
