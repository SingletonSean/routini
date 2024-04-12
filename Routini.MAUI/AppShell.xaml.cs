using Routini.MAUI.Pages;

namespace Routini.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Routines/Create", typeof(CreateRoutineView));
            Routing.RegisterRoute("Routines/Detail", typeof(RoutineDetailView));
            Routing.RegisterRoute("Routines/Detail/Edit", typeof(EditRoutineView));
        }
    }
}
