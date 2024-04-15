using Microsoft.Extensions.DependencyInjection.Extensions;
using Routini.MAUI.Application.Database;
using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Features.ListRoutines;
using Routini.MAUI.Shared.Databases;

namespace Routini.MAUI.Test.Mocks
{
    public class MockEnvironment
    {
        public IServiceProvider ServiceProvider { get; }

        private MockEnvironment()
        {
            ServiceProvider = new ServiceCollection()
                .AddLogging()
                .AddRoutini(NSubstitute.Substitute.For<Serilog.ILogger>())
                .Replace(ServiceDescriptor.Singleton<ISqliteConnectionFactory, InMemorySqliteConnectionFactory>())
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
