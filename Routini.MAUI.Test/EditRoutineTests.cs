using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Pages;
using Routini.MAUI.Test.Mocks;

namespace Routini.MAUI.Test
{
    [TestFixture]
    public class EditRoutineTests
    {
        [Test]
        public async Task UpdatesRoutine_WhenSuccessful()
        {
            MockEnvironment mockEnvironment = await MockEnvironment.Initialize();

            await mockEnvironment.AddRoutine(new NewRoutine("Stretch 1", new List<NewRoutineStep>
            {
                new NewRoutineStep("Hamstrings", TimeSpan.FromSeconds(15)),
            }));
            Routine routine = await mockEnvironment.GetAllRoutines().ContinueWith(r => r.Result.First());

            RoutineDetailViewModel routineDetailsViewModel = mockEnvironment
                .ServiceProvider
                .GetRequiredService<RoutineDetailViewModel>();
            EditRoutineViewModel editRoutineViewModel = mockEnvironment
                .ServiceProvider
                .GetRequiredService<EditRoutineViewModel>();

            await routineDetailsViewModel.ApplyQueryAttributesAsync(new Dictionary<string, object>()
            {
                { "Id", routine.Id }
            });
            Assert.That(routineDetailsViewModel.Name, Is.EqualTo("Stretch 1"));
            Assert.That(routineDetailsViewModel.PlayRoutineViewModel?.Steps.First().Name, Is.EqualTo("Hamstrings"));
            Assert.That(routineDetailsViewModel.PlayRoutineViewModel?.Steps.First().DurationSeconds, Is.EqualTo(15));

            await editRoutineViewModel.ApplyQueryAttributesAsync(new Dictionary<string, object>()
            {
                { "Id", routine.Id }
            });
            RoutineFormViewModel? editRoutineFormViewModel = editRoutineViewModel.EditRoutineFormViewModel?.RoutineFormViewModel;
            if (editRoutineFormViewModel == null)
            {
                Assert.Fail("Failed to load routine");
                return;
            }

            editRoutineFormViewModel.Name = "Stretch 2";
            editRoutineFormViewModel.RoutineSteps[0].Name = "Quadriceps";
            editRoutineFormViewModel.RoutineSteps[0].DurationSeconds = 30;
            await editRoutineFormViewModel.SubmitCommand.ExecuteAsync(null);

            await routineDetailsViewModel.ApplyQueryAttributesAsync(new Dictionary<string, object>()
            {
                { "Id", routine.Id }
            });
            Assert.That(routineDetailsViewModel.Name, Is.EqualTo("Stretch 2"));
            Assert.That(routineDetailsViewModel.PlayRoutineViewModel?.Steps.First().Name, Is.EqualTo("Quadriceps"));
            Assert.That(routineDetailsViewModel.PlayRoutineViewModel?.Steps.First().DurationSeconds, Is.EqualTo(30));
        }
    }
}
