using Microsoft.ApplicationInsights;
using Routini.MAUI.Shared.Telemetry;

namespace Routini.MAUI.Pages;

public partial class EditRoutineView : TelemetryContentPage
{
	public EditRoutineView(EditRoutineViewModel viewModel, TelemetryClient telemetryClient) : base(telemetryClient)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}