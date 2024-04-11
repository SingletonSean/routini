namespace Routini.MAUI.Pages;

public partial class EditRoutineView : ContentPage
{
	public EditRoutineView(EditRoutineViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}