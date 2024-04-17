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

            Assert.That(() => playRoutineViewModel.CurrentStepName, Is.EqualTo("Quadriceps").After(1).Seconds);

            mockEnvironment.MockDateTimeProvider.UtcNow.Returns(startTime.AddSeconds(31));

            Assert.That(() => playRoutineViewModel.CurrentStepName, Is.EqualTo("Hip Flexors").After(1).Seconds);

            mockEnvironment.MockDateTimeProvider.UtcNow.Returns(startTime.AddSeconds(46));

            Assert.That(() => playRoutineViewModel.CurrentStepName, Is.Null.After(1).Seconds);
            Assert.That(() => playRoutineViewModel.Started, Is.False.After(1).Seconds);
        }
    }
}
