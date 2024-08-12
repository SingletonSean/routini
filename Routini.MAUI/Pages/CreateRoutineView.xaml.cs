using Microsoft.ApplicationInsights;
using Routini.MAUI.Shared.Telemetry;
using System.Windows.Input;

namespace Routini.MAUI.Pages;

public partial class CreateRoutineView : TelemetryContentPage
{
    public static readonly BindableProperty OnAppearingCommandProperty =
        BindableProperty.Create(nameof(OnAppearingCommand), typeof(ICommand), typeof(ListRoutinesView), null);

    public ICommand OnAppearingCommand
    {
        get => (ICommand)GetValue(OnAppearingCommandProperty);
        set => SetValue(OnAppearingCommandProperty, value);
    }

    public CreateRoutineView(CreateRoutineViewModel viewModel, TelemetryClient telemetryClient) : base(telemetryClient)
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