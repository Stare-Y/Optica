using Application.ViewModels;
using CommunityToolkit.Maui.Views;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Captura;

public partial class Capturas : ContentPage
{
	private readonly ViewModelCapturas _viewModelCapturas;
	public Capturas(ViewModelCapturas viewModelCapturas)
	{
		InitializeComponent();
		_viewModelCapturas = viewModelCapturas;
        this.BindingContext = _viewModelCapturas;

        DatePickerCaducidad.Date = DateTime.Now;
        DatePickerCaducidad.MinimumDate = DateTime.Now.AddDays(7);

    }

    public Capturas() : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelCapturas>())
    {
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModelCapturas.Initialize();
        EntryProveedor.Focus();
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
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        await BtnGuardar.FadeTo(1, 200);
        try
        {
            _viewModelCapturas.ValidarLote();

            await DisplayAlert("Guardado", "Se ha guardado la captura de datos", "Aceptar");

            var seleccionMicas = new SeleccionMicas(_viewModelCapturas.Lote);

            await Shell.Current.Navigation.PushAsync(seleccionMicas);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
        finally
        {
            popup.Close();
        }
    }

    private void DatePickerCaducidad_DateSelected(object sender, DateChangedEventArgs e)
    {
        _viewModelCapturas.Lote.FechaCaducidad = e.NewDate;
    }

    private void EntryReferencia_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModelCapturas.Lote.Referencia = e.NewTextValue;
    }

    private void DatePickerFechaEntrada_DateSelected(object sender, DateChangedEventArgs e)
    {
        _viewModelCapturas.Lote.FechaEntrada = e.NewDate;
    }

    private void EntryProveedor_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModelCapturas.Lote.Proveedor = e.NewTextValue;
    }
}