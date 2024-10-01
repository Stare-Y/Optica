namespace TechLens.Presentacion.Views.Captura;

public partial class NuevaMica : ContentPage
{
	public NuevaMica()
	{
		InitializeComponent();
	}

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        BtnCancelar.Opacity = 0;
        await BtnCancelar.FadeTo(1, 200);

        await Navigation.PopAsync();
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
        BtnGuardar.Opacity = 0;
        await BtnGuardar.FadeTo(1, 200);

        var graduacionMica = new GraduacionMica();
        await Navigation.PushAsync(graduacionMica);
    }
}