using Routini.MAUI.Application.Database;
using Routini.MAUI.Features.CreateRoutine;
using Routini.MAUI.Features.ListRoutines;

namespace Routini.MAUI
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private readonly RoutiniDatabaseInitializer _databaseInitializer;
        private readonly CreateRoutineCommand _command;
        private readonly GetAllRoutinesQuery _query;

        public App(RoutiniDatabaseInitializer databaseInitializer, CreateRoutineCommand command, GetAllRoutinesQuery query)
        {
            _databaseInitializer = databaseInitializer;

            InitializeComponent();

            MainPage = new AppShell();
            _command = command;
            _query = query;
        }

        protected override async void OnStart()
        {
            await _databaseInitializer.Initialize();

            await _command.Execute(new NewRoutine("Stretching"));
            var routines = await _query.Execute();

            base.OnStart();
        }
    }
}
