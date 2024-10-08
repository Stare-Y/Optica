namespace TechLens.Presentacion.Views.Users;

public partial class Usuarios : ContentPage
{
	public Usuarios()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }

    private async void BtnNuevoUsuario_Clicked(object sender, EventArgs e)
    {
        BtnNuevoUsuario.Opacity = 0;
        await BtnNuevoUsuario.FadeTo(1, 200);

    }

    private async void BtnRegresar_Clicked(object sender, EventArgs e)
    {
        BtnRegresar.Opacity = 0;
        await BtnRegresar.FadeTo(1, 200);

        await Shell.Current.GoToAsync("..");

    }
}