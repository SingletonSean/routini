using NSubstitute;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Features.PlayRoutine;
using Routini.MAUI.Pages;
using Routini.MAUI.Test.Mocks;

namespace Routini.MAUI.Test
{
    [TestFixture]
    public class PlayRoutineTests
    {
        [Test]
        public async Task PlaysRoutine()
        {
            MockEnvironment mockEnvironment = await MockEnvironment.Initialize();

            await mockEnvironment.AddRoutine(new NewRoutine("Stretching", new List<NewRoutineStep>
            {
                new NewRoutineStep("Hamstrings", TimeSpan.FromSeconds(15)),
                new NewRoutineStep("Quadriceps", TimeSpan.FromSeconds(15)),
                new NewRoutineStep("Hip Flexors", TimeSpan.FromSeconds(15)),
            }));
            Routine routine = await mockEnvironment.GetAllRoutines().ContinueWith(r => r.Result.First());

            RoutineDetailViewModel routineDetailsViewModel = mockEnvironment
                .ServiceProvider
                .GetRequiredService<RoutineDetailViewModel>();

            await routineDetailsViewModel.ApplyQueryAttributesAsync(new Dictionary<string, object>()
            {
                { "Id", routine.Id }
            });
            PlayRoutineViewModel? playRoutineViewModel = routineDetailsViewModel.PlayRoutineViewModel;
            if (playRoutineViewModel == null)
            {
                Assert.Fail("Failed to load routine");
                return;
            }

            Assert.That(playRoutineViewModel.Started, Is.False);

            DateTimeOffset startTime = DateTimeOffset.UtcNow;
            mockEnvironment.MockDateTimeProvider.UtcNow.Returns(startTime);
            playRoutineViewModel.StartRoutineCommand.Execute(null);

            Assert.That(playRoutineViewModel.Started, Is.True);
            Assert.That(playRoutineViewModel.CurrentStepName, Is.EqualTo("Hamstrings"));

            mockEnvironment.MockDateTimeProvider.UtcNow.Returns(startTime.AddSeconds(16));
            mockEnvironment.MockTimer.RaiseElapsed();

            Assert.That(playRoutineViewModel.CurrentStepName, Is.EqualTo("Quadriceps"));

            mockEnvironment.MockDateTimeProvider.UtcNow.Returns(startTime.AddSeconds(31));
            mockEnvironment.MockTimer.RaiseElapsed();

            Assert.That(playRoutineViewModel.CurrentStepName, Is.EqualTo("Hip Flexors"));

            mockEnvironment.MockDateTimeProvider.UtcNow.Returns(startTime.AddSeconds(46));
            mockEnvironment.MockTimer.RaiseElapsed();

            Assert.That(playRoutineViewModel.CurrentStepName, Is.Null);
            Assert.That(playRoutineViewModel.Started, Is.False);
        }
    }
}
