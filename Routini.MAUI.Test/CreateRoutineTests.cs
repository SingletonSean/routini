using Microsoft.Extensions.DependencyInjection.Extensions;
using Routini.MAUI.Application.Database;
using Routini.MAUI.Pages;
using Routini.MAUI.Shared.Databases;
using Routini.MAUI.Test.Mocks;

namespace Routini.MAUI.Test
{
    [TestFixture]
    public class CreateRoutineTests
    {
        [Test]
        public async Task CreatesRoutine_WhenSuccessful()
        {
            MockEnvironment mockEnvironment = await MockEnvironment.Initialize();

            ListRoutinesViewModel listRoutinesViewModel = mockEnvironment
                .ServiceProvider
                .GetRequiredService<ListRoutinesViewModel>();
            CreateRoutineViewModel createRoutineViewModel = mockEnvironment
                .ServiceProvider
                .GetRequiredService<CreateRoutineViewModel>();

            await listRoutinesViewModel.LoadRoutinesCommand.ExecuteAsync(null);

            Assert.That(listRoutinesViewModel.RoutinePreviews, Is.Empty);

            createRoutineViewModel.RoutineFormViewModel.Name = "Stretching";
            createRoutineViewModel.RoutineFormViewModel.RoutineSteps[0].Name = "Hamstrings";
            createRoutineViewModel.RoutineFormViewModel.RoutineSteps[0].DurationSeconds = 15;
            createRoutineViewModel.RoutineFormViewModel.AddRoutineStepCommand.Execute(null);
            createRoutineViewModel.RoutineFormViewModel.RoutineSteps[1].Name = "Quadriceps";
            createRoutineViewModel.RoutineFormViewModel.RoutineSteps[1].DurationSeconds = 15;
            await createRoutineViewModel.RoutineFormViewModel.SubmitCommand.ExecuteAsync(null);

            await listRoutinesViewModel.LoadRoutinesCommand.ExecuteAsync(null);

            Assert.That(listRoutinesViewModel.RoutinePreviews, Has.Count.EqualTo(1));
        }
    }
}
