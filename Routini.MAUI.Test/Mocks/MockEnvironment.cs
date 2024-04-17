using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Routini.MAUI.Application.Database;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Features.ListRoutines;
using Routini.MAUI.Shared.Databases;
using Routini.MAUI.Shared.Time;

namespace Routini.MAUI.Test.Mocks
{
    public class MockEnvironment
    {
        public IServiceProvider ServiceProvider { get; }
        public IDateTimeProvider MockDateTimeProvider { get; }
        public MockTimer MockTimer { get; }

        private MockEnvironment()
        {
            MockDateTimeProvider = Substitute.For<IDateTimeProvider>();
            MockTimer = new MockTimer();

            ServiceProvider = new ServiceCollection()
                .AddLogging()
                .AddRoutini(Substitute.For<Serilog.ILogger>())
                .Replace(ServiceDescriptor.Singleton<ISqliteConnectionFactory, InMemorySqliteConnectionFactory>())
                .Replace(ServiceDescriptor.Singleton(MockDateTimeProvider))
                .Replace(ServiceDescriptor.Singleton<Shared.Timers.ITimer>(MockTimer))
                .BuildServiceProvider();
        }

        public static async Task<MockEnvironment> Initialize()
        {
            MockEnvironment mockEnvironment = new MockEnvironment();

            RoutiniDatabaseInitializer databaseInitializer = mockEnvironment
                .ServiceProvider
                .GetRequiredService<RoutiniDatabaseInitializer>();
            await databaseInitializer.Initialize();

            return mockEnvironment;
        }

        public async Task<IEnumerable<Routine>> GetAllRoutines()
        {
            GetAllRoutinesQuery getAllRoutinesQuery = ServiceProvider.GetRequiredService<GetAllRoutinesQuery>();

            return await getAllRoutinesQuery.Execute();
        }

        public async Task AddRoutine(NewRoutine routine)
        {
            CreateRoutineMutation createRoutineMutation = ServiceProvider.GetRequiredService<CreateRoutineMutation>();

            await createRoutineMutation.Execute(routine);
        }
    }
}
