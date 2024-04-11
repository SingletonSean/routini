namespace Routini.MAUI.Features.CreateRoutine
{
    public record NewRoutine(string Name, IEnumerable<NewRoutineStep> Steps);
}
