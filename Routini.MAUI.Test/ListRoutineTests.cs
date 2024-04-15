using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Pages;
using Routini.MAUI.Test.Mocks;

namespace Routini.MAUI.Test
{
    [TestFixture]
    public class ListRoutineTests
    {
        [Test]
        public async Task ListRoutines_WhenSuccessful()
        {
            MockEnvironment mockEnvironment = await MockEnvironment.Initialize();

            await mockEnvironment.AddRoutine(new NewRoutine("Stretch 1", new List<NewRoutineStep>
            {
                new NewRoutineStep("Hamstrings", TimeSpan.FromSeconds(15)),
            }));
            await mockEnvironment.AddRoutine(new NewRoutine("Stretch 2", new List<NewRoutineStep>
            {
                new NewRoutineStep("Quadriceps", TimeSpan.FromSeconds(15)),
            }));
            await mockEnvironment.AddRoutine(new NewRoutine("Stretch 3", new List<NewRoutineStep>
            {
                new NewRoutineStep("Hip Flexors", TimeSpan.FromSeconds(15)),
            }));

            ListRoutinesViewModel listRoutinesViewModel = mockEnvironment
                .ServiceProvider
                .GetRequiredService<ListRoutinesViewModel>();

            await listRoutinesViewModel.LoadRoutinesCommand.ExecuteAsync(null);

            Assert.That(listRoutinesViewModel.RoutinePreviews, Has.Count.EqualTo(3));
        }
    }
}
