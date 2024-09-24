namespace TechLens.Presentacion.Views;

public partial class Capturas : ContentPage
{
	public Capturas()
	{
		InitializeComponent();
	}

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
		BtnCancelar.Opacity = 0;
		await BtnCancelar.FadeTo(1, 200);

		await Shell.Current.GoToAsync("..");
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
		BtnGuardar.Opacity = 0;
        await BtnGuardar.FadeTo(1, 200);

		await DisplayAlert("Guardado", "Se ha guardado la captura de datos", "Aceptar");
		await Shell.Current.GoToAsync(nameof(Micas));
    }
}