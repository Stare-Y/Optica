using Application.ViewModels;
using Domain.Entities;

namespace TechLens.Presentacion.Views.Captura;

public partial class Capturas : ContentPage
{
	private readonly ViewModelCapturas _viewModelCapturas;
	public Capturas(ViewModelCapturas viewModelCapturas)
	{
		InitializeComponent();
		_viewModelCapturas = viewModelCapturas;
        this.BindingContext = _viewModelCapturas;
    }

    public Capturas() : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelCapturas>())
    {
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModelCapturas.Initialize();
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

        _viewModelCapturas.Lote.FechaEntrada = DateTime.Now;
        _viewModelCapturas.Lote.Proveedor = "sadklfjf".ToUpper();
        _viewModelCapturas.Lote.FechaCaducidad = DateTime.Now.AddDays(100);
        _viewModelCapturas.Lote.Referencia = "REF1234";

        try
        {
            ValidarLote();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
            return;
        }

        await DisplayAlert("Guardado", "Se ha guardado la captura de datos", "Aceptar");

        var seleccionMicas = new SeleccionMicas(_viewModelCapturas.Lote);

        await Shell.Current.Navigation.PushAsync(seleccionMicas);
    }

    private void ValidarLote()
    {
        if (_viewModelCapturas.Lote.Proveedor == string.Empty)
        {
            throw new Exception("El lote debe tener un proveedor");
        }
        if (_viewModelCapturas.Lote.FechaCaducidad < DateTime.Now)
        {
            throw new Exception("El lote debe tener fecha de caducidad mayor a la fecha actual");
        }
        //Pendiente
        //if (_viewModelCapturas.Lote.FechaEntrada > DateTime.Now)
        //{
        //    throw new Exception("El lote debe tener fecha de entrada menor a la fecha actual");
        //}
    }
}