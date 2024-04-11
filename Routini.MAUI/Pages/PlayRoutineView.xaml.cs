namespace Routini.MAUI.Pages;

public partial class PlayRoutineView : ContentPage
{
	public PlayRoutineView(PlayRoutineViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}