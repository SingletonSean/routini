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
using Serilog;

namespace Routini.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Inter-Regular.ttf", "Inter");
                    fonts.AddFont("Inter-Bold.ttf", "InterBold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            IServiceCollection services = builder.Services;

            services.AddSerilog(
                new LoggerConfiguration()
                    .WriteTo.Debug()
                    .WriteTo.File(Path.Combine(FileSystem.Current.AppDataDirectory, "logs", "log.txt"), rollingInterval: RollingInterval.Day)
                    .CreateLogger());

            services.AddSingleton<SqliteConnectionFactory>();
            services.AddSingleton<IShell, MauiShell>();
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

            return builder.Build();
        }
    }
}
