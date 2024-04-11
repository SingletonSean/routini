using Routini.MAUI.Pages;

namespace Routini.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Routines/Create", typeof(CreateRoutineView));
        }
    }
}
