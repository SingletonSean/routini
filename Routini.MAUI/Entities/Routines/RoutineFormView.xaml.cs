using MauiIcons.Core;

namespace Routini.MAUI.Entities.Routines;

public partial class RoutineFormView : ContentView
{
	public static readonly BindableProperty SubmitButtonTextProperty =
		BindableProperty.Create(nameof(SubmitButtonText), typeof(string), typeof(RoutineFormView), "Submit");

	public string SubmitButtonText
	{
		get => (string)GetValue(SubmitButtonTextProperty);
		set => SetValue(SubmitButtonTextProperty, value);
	}

	public RoutineFormView()
	{
		InitializeComponent();

        _ = new MauiIcon();
    }
}