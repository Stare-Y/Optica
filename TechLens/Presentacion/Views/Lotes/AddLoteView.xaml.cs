using Application.ViewModels;
using CommunityToolkit.Maui.Views;
using Domain.Entities;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Lotes;

public partial class AddLoteView : ContentPage
{
	private readonly ViewModelCapturas _viewModelCapturas;
	public AddLoteView(ViewModelCapturas viewModelCapturas)
	{
		InitializeComponent();
		_viewModelCapturas = viewModelCapturas;
        this.BindingContext = _viewModelCapturas;

        DatePickerCaducidad.Date = DateTime.Now;
        DatePickerCaducidad.MinimumDate = DateTime.Now.AddDays(7);
        DatePickerFechaEntrada.Date = DateTime.Now;
    }

    public AddLoteView(Usuario usuario) : this()
    {
        _viewModelCapturas.Usuario = usuario;
        _viewModelCapturas.Lote.IdUsuario = usuario.Id;
    }

    public AddLoteView() : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelCapturas>())
    {
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModelCapturas.Initialize();
        _viewModelCapturas.Lote.FechaCaducidad = DatePickerCaducidad.Date;
        _viewModelCapturas.Lote.FechaEntrada = DatePickerFechaEntrada.Date;
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

#if DEBUG
            EntryProveedor.Text = "proveedor debug";
            EntryReferencia.Text = "referencia debug";
            EntryCosto.Text = "1234";
#endif

            _viewModelCapturas.ValidarLote();

            await DisplayAlert("Guardado", "Se ha guardado la captura de datos", "Aceptar");

            var seleccionMicas = new SeleccionMicasLoteView(_viewModelCapturas.Lote);

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

    private void EntryCosto_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        // Si el campo est� vac�o, permitir borrar completamente
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            _viewModelCapturas.Lote.Costo = 0;
            return;
        }

        if (e.NewTextValue == ".")
            return;

        // Validar n�mero real
        if (double.TryParse(e.NewTextValue, out double costo))
        {
            _viewModelCapturas.Lote.Costo = costo;
        }
        else
        {

            entry.TextChanged -= EntryCosto_TextChanged;
            entry.Text = e.OldTextValue;
            entry.TextChanged += EntryCosto_TextChanged;
        }
    }


}