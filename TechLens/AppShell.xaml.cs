namespace TechLens;

using TechLens.Presentacion.Views;
using TechLens.Presentacion.Views.Lotes;
using TechLens.Presentacion.Views.Pedidos;
using TechLens.Presentacion.Views.Users;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    
        //Routing 
        Routing.RegisterRoute(nameof(AddLoteView), typeof(AddLoteView));
        Routing.RegisterRoute(nameof(ReportesPanelView), typeof(ReportesPanelView));
        Routing.RegisterRoute(nameof(AddPedidoView), typeof(AddPedidoView));
        Routing.RegisterRoute(nameof(UsuariosManageView), typeof(UsuariosManageView));
        Routing.RegisterRoute(nameof(ListedMicasView), typeof(ListedMicasView));
        Routing.RegisterRoute(nameof(SeleccionMicasLoteView), typeof(SeleccionMicasLoteView));
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
