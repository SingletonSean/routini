using Routini.MAUI.Application.Database;

namespace Routini.MAUI
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private readonly RoutiniDatabaseInitializer _databaseInitializer;

        public App(RoutiniDatabaseInitializer databaseInitializer)
        {
            _databaseInitializer = databaseInitializer;

            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            await _databaseInitializer.Initialize();

            base.OnStart();
        }
    }
}
