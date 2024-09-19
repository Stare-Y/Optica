namespace TechLens;

using TechLens.Presentacion;
using TechLens.Presentacion.Views;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        //App
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsVisible = false });

        //Routing 
        Routing.RegisterRoute(nameof(Consultas), typeof(Consultas));
        Routing.RegisterRoute(nameof(Capturas), typeof(Capturas));
        Routing.RegisterRoute(nameof(Reportes), typeof(Reportes));
        Routing.RegisterRoute(nameof(Ventas), typeof(Ventas));
    }
}
