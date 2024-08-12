using Microsoft.ApplicationInsights;

namespace Routini.MAUI.Shared.Telemetry
{
    public class TelemetryContentPage : ContentPage
    {
        private readonly TelemetryClient _telemetryClient;

        public static readonly BindableProperty PageViewEventNameProperty =
            BindableProperty.Create(nameof(PageViewEventName), typeof(string), typeof(TelemetryContentPage), null);

        public string PageViewEventName
        {
            get => (string)GetValue(PageViewEventNameProperty);
            set => SetValue(PageViewEventNameProperty, value);
        }

        public TelemetryContentPage(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        protected override void OnAppearing()
        {
            if (!string.IsNullOrEmpty(PageViewEventName))
            {
                _telemetryClient.TrackPageView(PageViewEventName);
            }

            base.OnAppearing();
        }
    }
}
