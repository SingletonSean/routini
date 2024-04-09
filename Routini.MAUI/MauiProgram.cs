using Microsoft.Extensions.Logging;
using Routini.MAUI.Application.Database;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Features.ListRoutines;
using Routini.MAUI.Shared.Databases;

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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            IServiceCollection services = builder.Services;

            services.AddSingleton<SqliteConnectionFactory>();
            services.AddSingleton<RoutiniDatabaseInitializer>();

            services.AddSingleton<GetAllRoutinesQuery>();

            services.AddSingleton<CreateRoutineCommand>();

            return builder.Build();
        }
    }
}
