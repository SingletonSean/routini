namespace Routini.MAUI.Shared.Shells
{
    public interface IShell
    {
        Task DisplayAlert(string title, string message, string cancel);
        Task GoToAsync(string route);
    }
}