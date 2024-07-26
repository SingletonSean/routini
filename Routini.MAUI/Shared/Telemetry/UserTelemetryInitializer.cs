using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routini.MAUI.Shared.Telemetry
{
    public class UserTelemetryInitializer : ITelemetryInitializer
    {
        private const string USER_ID_PREFERENCES_KEY = "user_id"; 

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.User.Id = GetUserId();
            telemetry.Context.Device.Model = DeviceInfo.Model;
            telemetry.Context.Device.Type = DeviceInfo.DeviceType.ToString();
            telemetry.Context.Component.Version = AppInfo.VersionString;
        }

        private string GetUserId()
        {
            string? userId = Preferences.Get(USER_ID_PREFERENCES_KEY, null);

            if (userId != null)
            {
                return userId;
            }

            string newUserId = Guid.NewGuid().ToString();

            Preferences.Set(USER_ID_PREFERENCES_KEY, newUserId);

            return newUserId;
        }
    }
}
