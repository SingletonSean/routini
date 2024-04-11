using System.Windows.Input;

namespace Routini.MAUI.Pages;

public partial class ListRoutinesView : ContentPage
{
	public static readonly BindableProperty OnAppearingCommandProperty =
		BindableProperty.Create(nameof(OnAppearingCommand), typeof(ICommand), typeof(ListRoutinesView), null);

	public ICommand OnAppearingCommand
	{
		get => (ICommand)GetValue(OnAppearingCommandProperty);
		set => SetValue(OnAppearingCommandProperty, value);
	}

	public ListRoutinesView(ListRoutinesViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
		OnAppearingCommand?.Execute(null);

        base.OnAppearing();
    }
}