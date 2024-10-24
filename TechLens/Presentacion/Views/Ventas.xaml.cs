using TechLens.Presentacion.Views.Captura;
using Application.ViewModels;
using Domain.Entities;

namespace TechLens.Presentacion.Views;

public partial class Ventas : ContentPage
{
    private readonly ViewModelCrearPedido _viewModel;
    public Ventas(ViewModelCrearPedido viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        this.BindingContext = _viewModel;
        DatePickerFechaSalida.Date = DateTime.Now;
    }

    public Ventas() : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelCrearPedido>())
    {
    }

    public Ventas(Usuario usuario) : this()
    {
        _viewModel.Pedido.IdUsuario = usuario.Id;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.Initialize();
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        BtnCancelar.Opacity = 0;
        await BtnCancelar.FadeTo(1, 200);

        await Shell.Current.GoToAsync("..");
    }

    private async void BtnConfirmar_Clicked(object sender, EventArgs e)
    {
        BtnConfirmar.Opacity = 0;
        await BtnConfirmar.FadeTo(1, 200);
        try
        {
            if (_viewModel.Pedido.Id == 0)
                throw new Exception("No se ha podido obtener el siguiente Id de pedido");
            else if(string.IsNullOrEmpty(_viewModel.Pedido.RazonSocial) || string.IsNullOrWhiteSpace(_viewModel.Pedido.RazonSocial))
                throw new Exception("La razón social no puede estar vacía");
            else if (_viewModel.Pedido.FechaSalida == DateTime.MinValue)
                throw new Exception("La fecha de salida no puede estar vacía");

            var peidoMicas = new SelecccionMicasPedidos(_viewModel.Pedido);

            await DisplayAlert("Guardado", "Se Continuara a la Captura de Micas y Graduaciones", "Aceptar");

            await Shell.Current.Navigation.PushAsync(peidoMicas);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }

    private void DatePickerFechaSalida_DateSelected(object sender, DateChangedEventArgs e)
    {
        _viewModel.Pedido.FechaSalida = e.NewDate;
    }

    private void EntryRazonSocial_TextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.Pedido.RazonSocial = e.NewTextValue;
    }
}