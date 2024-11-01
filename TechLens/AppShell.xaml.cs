namespace TechLens;

using TechLens.Presentacion.Views;
using TechLens.Presentacion.Views.Captura;
using TechLens.Presentacion.Views.Users;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    
        //Routing 
        Routing.RegisterRoute(nameof(Capturas), typeof(Capturas));
        Routing.RegisterRoute(nameof(Reportes), typeof(Reportes));
        Routing.RegisterRoute(nameof(Ventas), typeof(Ventas));
        Routing.RegisterRoute(nameof(Usuarios), typeof(Usuarios));
        Routing.RegisterRoute(nameof(Micas), typeof(Micas));
        Routing.RegisterRoute(nameof(SeleccionMicas), typeof(SeleccionMicas));
        Routing.RegisterRoute(nameof(GraduacionMica), typeof(GraduacionMica));
        Routing.RegisterRoute(nameof(Crear_EditarUsuario), typeof(Crear_EditarUsuario));
    }

    private async void BtnMainPage_Clicked(object sender, EventArgs e)
    {
        MainMenu.Opacity = 0;
        await MainMenu.FadeTo(1, 200);

        await Shell.Current.GoToAsync("//MainPage");

    }
    
    private async void LogOut_Clicked(object sender, EventArgs e)
    {
        LogOut.Opacity = 0; 
        await LogOut.FadeTo(1, 200);

        await Shell.Current.GoToAsync("//LogIn");
    }

}
