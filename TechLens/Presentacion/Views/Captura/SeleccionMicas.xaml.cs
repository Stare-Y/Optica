namespace TechLens.Presentacion.Views.Captura;

public partial class SeleccionMicas : ContentPage
{
	public SeleccionMicas()
	{
		InitializeComponent();
	}

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        BtnCancelar.Opacity = 0;
        await BtnCancelar.FadeTo(1, 200);

        await Navigation.PopAsync();
    }

    private async void BtnSeleccionar_Clicked(object sender, EventArgs e)
    {
        BtnSeleccionar.Opacity = 0;
        await BtnSeleccionar.FadeTo(1, 200);

        var micas = new Micas();
        await Navigation.PushAsync(micas);

    }

    private async void BtnBorrarTodo_Clicked(object sender, EventArgs e)
    {
        BtnBorrarTodo.Opacity = 0;
        await BtnBorrarTodo.FadeTo(1, 200);

    }

    private async void BtnConfirmar_Clicked(object sender, EventArgs e)
    {
        BtnConfirmar.Opacity = 0;
        await BtnConfirmar.FadeTo(1, 200);

        await DisplayAlert("Guardado", "Se ha guardado la captura de datos", "Aceptar");

        await Navigation.PopToRootAsync();
    }
}