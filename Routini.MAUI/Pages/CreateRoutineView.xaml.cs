using MauiIcons.Core;
using System.Windows.Input;

namespace Routini.MAUI.Pages;

public partial class CreateRoutineView : ContentPage
{
    public static readonly BindableProperty OnAppearingCommandProperty =
        BindableProperty.Create(nameof(OnAppearingCommand), typeof(ICommand), typeof(ListRoutinesView), null);

    public ICommand OnAppearingCommand
    {
        get => (ICommand)GetValue(OnAppearingCommandProperty);
        set => SetValue(OnAppearingCommandProperty, value);
    }

    public CreateRoutineView(CreateRoutineViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;

        _ = new MauiIcon();
    }

    protected override void OnAppearing()
    {
        OnAppearingCommand?.Execute(null);

        base.OnAppearing();
    }
}