using Application.ViewModels;
using Domain.Entities;
using TechLens.Presentacion.Events;

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
        micas.MicaSelected += OnMicaSelected;

        await Shell.Current.Navigation.PushAsync(micas);
    }

    private async void BtnConfirmar_Clicked(object sender, EventArgs e)
    {
        BtnConfirmar.Opacity = 0;
        await BtnConfirmar.FadeTo(1, 200);
        try
        {
            await _viewModel.SavePedido();
            await DisplayAlert("Guardado", "Se ha guardado la captura de datos", "Aceptar");
            await Shell.Current.GoToAsync("//MainPage");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }

        var graduacionMica = new GraduacionMica();

        await Shell.Current.GoToAsync(nameof(GraduacionMica));
    }
}