using Microsoft.ApplicationInsights;
using Routini.MAUI.Shared.Telemetry;
using System.Windows.Input;

namespace Routini.MAUI.Pages;

public partial class RoutineDetailView : TelemetryContentPage
{
    public static readonly BindableProperty OnDisappearingCommandProperty =
    BindableProperty.Create(nameof(OnDisappearingCommand), typeof(ICommand), typeof(ListRoutinesView), null);

    public ICommand OnDisappearingCommand
    {
        get => (ICommand)GetValue(OnDisappearingCommandProperty);
        set => SetValue(OnDisappearingCommandProperty, value);
    }

    public RoutineDetailView(RoutineDetailViewModel viewModel, TelemetryClient telemetryClient) : base(telemetryClient)
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