using Microsoft.Extensions.DependencyInjection.Extensions;
using Routini.MAUI.Application.Database;
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
    }
}
