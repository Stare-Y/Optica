using Application.ViewModels;
using CommunityToolkit.Maui.Views;
using Domain.Entities;
using TechLens.Presentacion.Events;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Captura;

public partial class SelecccionMicasPedidos : ContentPage
{
    private readonly VMSeleccionarMicasPedido _viewModel;
    public SelecccionMicasPedidos(VMSeleccionarMicasPedido vMSeleccionMicas)
    {
        InitializeComponent();
        _viewModel = vMSeleccionMicas;
        this.BindingContext = _viewModel;
    }

    public SelecccionMicasPedidos(Pedido pedido) : this(MauiProgram.ServiceProvider.GetRequiredService<VMSeleccionarMicasPedido>())
    {
        _viewModel.Pedido = pedido;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LblRazonSocial.Text = "Pedido para: " +_viewModel.Pedido.RazonSocial;
    }

    private async void OnMicaSelected(object? sender, MicasSelectedEventArgs e)
    {
        if (e.SelectedMica is not null)
            _viewModel.MicasSeleccionadas.Add(e.SelectedMica);
        await Shell.Current.Navigation.PopAsync();
    }

    private async void OnGraduacionesSelected(object? sender, GraduacionesSelectedEventArgs e)
    {
        try
        {
            if (e.GraduacionesPedidoMicaSelected is not null)
            {
                //agregar a la lista de graduaciones, si ya existia, actualizarla con los nuevos valores
                foreach (var item in e.GraduacionesPedidoMicaSelected)
                {
                    var index = _viewModel.PedidosMicas.FindIndex(x => x.IdMicaGraduacion == item.IdMicaGraduacion);
                    if (index != -1)
                    {
                        _viewModel.PedidosMicas[index] = item;
                    }
                    else
                    {
                        _viewModel.PedidosMicas.Add(item);
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "No se han seleccionado graduaciones", "Aceptar");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
        finally
        {
            await Shell.Current.Navigation.PopAsync();
        }
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
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        try
        {
            await BtnSeleccionar.FadeTo(1, 200);

            var micas = new Micas(needGoBack: true);
            micas.MicaSelected += OnMicaSelected;

            await Shell.Current.Navigation.PushAsync(micas);
        }
        finally
        {
            popup.Close();
        }
    }

    private async void BtnConfirmar_Clicked(object sender, EventArgs e)
    {
        BtnConfirmar.Opacity = 0;
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        await BtnConfirmar.FadeTo(1, 200);
        try
        {
            await _viewModel.SavePedido();
            await DisplayAlert("Guardado", "Se ha guardado la captura de datos", "Aceptar");
            await Shell.Current.GoToAsync("//MainPage");

            var graduacionMica = new GraduacionMica();

            await Shell.Current.GoToAsync(nameof(GraduacionMica));
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

    private void ContenedorMicas_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void BtnEliminarMica_Clicked(object sender, EventArgs e)
    {

    }
}