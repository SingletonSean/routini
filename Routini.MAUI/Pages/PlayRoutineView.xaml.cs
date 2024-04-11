using System.Windows.Input;

namespace Routini.MAUI.Pages;

public partial class PlayRoutineView : ContentPage
{
    public static readonly BindableProperty OnDisappearingCommandProperty =
    BindableProperty.Create(nameof(OnDisappearingCommand), typeof(ICommand), typeof(ListRoutinesView), null);

    public ICommand OnDisappearingCommand
    {
        get => (ICommand)GetValue(OnDisappearingCommandProperty);
        set => SetValue(OnDisappearingCommandProperty, value);
    }

    public PlayRoutineView(PlayRoutineViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    protected override void OnDisappearing()
    {
        OnDisappearingCommand?.Execute(null);

        base.OnDisappearing();
    }
}