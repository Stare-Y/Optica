using Application.ViewModels;
using Domain.Entities;
using TechLens.Presentacion.Events;
using TechLens.Presentacion.Views.Captura;


namespace TechLens.Presentacion.Views;

public partial class Ventas : ContentPage
{
	public Ventas()
	{
		InitializeComponent();
	}

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        BtnCancelar.Opacity = 0;
        await BtnCancelar.FadeTo(1, 200);

        await Shell.Current.Navigation.PopAsync();
    }

    private async void BtnSeleccionar_Clicked(object sender, EventArgs e)
    {
        BtnSeleccionar.Opacity = 0;
        await BtnSeleccionar.FadeTo(1, 200);

        var micas = new Micas();

        await Shell.Current.Navigation.PushAsync(micas);
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
        try
        {
            await DisplayAlert("Guardado", "Se ha guardado la captura de datos", "Aceptar");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }

        await Shell.Current.Navigation.PopToRootAsync();
    }
}