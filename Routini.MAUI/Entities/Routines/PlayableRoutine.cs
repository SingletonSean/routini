﻿using Routini.MAUI.Shared.Time;
using System.Timers;

namespace Routini.MAUI.Entities.Routines
{
    public class PlayableRoutine : IDisposable
    {
        private readonly Routine _routine;
        private readonly Shared.Timers.ITimer _timer;
        private readonly IDateTimeProvider _dateTimeProvider;

        public string Name => _routine.Name;
        public IEnumerable<RoutineStep> Steps => _routine.Steps;

        private int _currentStepIndex = 0;
        public int CurrentStepOrder => _currentStepIndex;

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

                double remaining = Math.Ceiling(
                    CurrentStep.Duration.TotalSeconds -
                    _dateTimeProvider.UtcNow.Subtract(_currentStepStartTime).TotalSeconds);

                if (remaining <= 0)
                {
                    return 0;
                }

                return remaining;
            }
        }

        public bool Started { get; private set; }
        private DateTimeOffset _pauseTime;
        public bool Paused { get; private set; }
        public bool Completed { get; private set; }

        public event Action? Updated;
        public event Action? StepChanged;

        public PlayableRoutine(Routine routine, IDateTimeProvider dateTimeProvider, Shared.Timers.ITimer timer)
        {
            _routine = routine;
            _dateTimeProvider = dateTimeProvider;
            _timer = timer;
            _timer.Interval = 100;
            _timer.Elapsed += OnTimerElapsed;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public void Start()
        {
            _currentStepIndex = 0;
            _currentStepStartTime = _dateTimeProvider.UtcNow;

            _timer.Start();

            Started = true;
            Paused = false;
            Completed = false;

            RaiseUpdated();
        }

        public void Pause()
        {
            _timer.Stop();
            _pauseTime = _dateTimeProvider.UtcNow;

            Paused = true;

            RaiseUpdated();
        }

        public void Resume()
        {
            _timer.Start();
            _currentStepStartTime = _currentStepStartTime.Add(_dateTimeProvider.UtcNow.Subtract(_pauseTime));

            Paused = false;

            RaiseUpdated();
        }

        public void Cancel()
        {
            _timer.Stop();

            Started = false;
            Paused = false;
            Completed = false;

            RaiseUpdated();
        }

        private void Complete()
        {
            _timer.Stop();

            Started = false;
            Paused = false;
            Completed = true;

            RaiseUpdated();
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (CurrentStepSecondsRemaining == 0)
            {
                _currentStepIndex++;
                _currentStepStartTime = _dateTimeProvider.UtcNow;

                StepChanged?.Invoke();
            }

            if (CurrentStep == null)
            {
                Complete();
            }

            RaiseUpdated();
        }

        private void RaiseUpdated()
        {
            Updated?.Invoke();
        }
    }
}
