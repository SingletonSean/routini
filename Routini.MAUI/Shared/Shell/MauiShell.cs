﻿namespace Routini.MAUI.Shared.Shells
{
    internal class MauiShell : IShell
    {
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Shell.Current.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Shell.Current.DisplayAlert(title, message, accept, cancel);
        }

        public async Task GoToAsync(string route)
        {
            await Shell.Current.GoToAsync(route);
        }
    }
}