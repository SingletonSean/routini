using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using Routini.MAUI.Application.Database;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Features.DeleteRoutine;
using Routini.MAUI.Features.EditRoutine;
using Routini.MAUI.Features.ListRoutines;
using Routini.MAUI.Pages;
using Routini.MAUI.Shared.Databases;
using Routini.MAUI.Shared.Shells;
using Routini.MAUI.Shared.Time;
using Serilog;
using MauiIcons.Material.Rounded;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Routini.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMaterialRoundedMauiIcons()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Inter-Regular.ttf", "Inter");
                    fonts.AddFont("Inter-Bold.ttf", "InterBold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            IServiceCollection services = builder.Services;

            services.AddRoutini();

            services.AddApplicationInsightsTelemetryWorkerService(
                config => config.ConnectionString = "InstrumentationKey=ddf17597-b9d0-44c9-a999-c48bddb869f7;IngestionEndpoint=https://centralus-2.in.applicationinsights.azure.com/;LiveEndpoint=https://centralus.livediagnostics.monitor.azure.com/;ApplicationId=7000dd6a-5c6e-40b3-87a4-4e29e5202595");
            builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>(null, LogLevel.Information);

            return builder.Build();
        }

        public static IServiceCollection AddRoutini(this IServiceCollection services)
        {
            services.AddSingleton<ISqliteConnectionFactory, SqliteConnectionFactory>();
            services.AddSingleton<IShell, MauiShell>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<Func<Shared.Timers.ITimer>>(() => new Shared.Timers.Timer());
            services.AddSingleton(AudioManager.Current);

            services.AddSingleton<GetAllRoutinesQuery>();
            services.AddSingleton<ListRoutinesViewModel>();
            services.AddSingleton<ListRoutinesView>();

            services.AddSingleton<CreateRoutineMutation>();
            services.AddSingleton<CreateRoutineViewModel>();
            services.AddSingleton<CreateRoutineView>();

            services.AddSingleton<UpdateRoutineMutation>();
            services.AddSingleton<EditRoutineViewModel>();
            services.AddSingleton<EditRoutineView>();

            services.AddSingleton<DeleteRoutineMutation>();

            services.AddSingleton<GetRoutineByIdQuery>();
            services.AddSingleton<RoutineDetailViewModel>();
            services.AddSingleton<RoutineDetailView>();

            services.AddSingleton<RoutiniDatabaseInitializer>();

            return services;
        }
    }
}
