using Application.ViewModels;
using CommunityToolkit.Maui.Views;
using Domain.Entities;
using Domain.Interfaces.Services.DisplayEntities;
using TechLens.Presentacion.Events;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views;

public partial class ConsultarStockMica : ContentPage
{
	private readonly VMConsultarStockMica _viewModel;
    private readonly bool readOnly;
    private List<PedidoMica>? _pedidoMicas;

    public event EventHandler<GraduacionesSelectedEventArgs>? GraduacionesSelected;
    public ConsultarStockMica(VMConsultarStockMica viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public ConsultarStockMica() : this(MauiProgram.ServiceProvider.GetRequiredService<VMConsultarStockMica>())
    {
    }

    public ConsultarStockMica(Pedido pedido, Mica mica, List<PedidoMica> pedidoMicas, bool readOnly) : this()
    {
        _viewModel.mica = mica;
        _viewModel.pedido = pedido;
        this.readOnly = readOnly;
        _pedidoMicas = pedidoMicas;
    }
    public ConsultarStockMica(Mica mica, bool readOnly) : this()
    {
        _viewModel.mica = mica;
        this.readOnly = readOnly;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            
            ConfirmarEleccion.IsVisible = !readOnly;
            await _viewModel.Initialize();
            if (_pedidoMicas is not null)
            {
                _viewModel.FillFromPedidoMicas(_pedidoMicas);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Ok");
        }
    }

    private async void CollectionViewMicasGraduaciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CollectionViewMicasGraduaciones.SelectedItem == null)
            return;
        if(readOnly)
        {
            return;
        }
        var row = (ShowConsultaStock)CollectionViewMicasGraduaciones.SelectedItem;
        var popup = new TakeStockPopup(row);
        popup.StockTaken += (s, e) =>
        {
            var stock = e.ShowConsultaStock;
            if (stock is not null)
            {
                _viewModel.UpdateConsultaStock(stock);
                ConfirmarEleccion.IsEnabled = true;
            }
        };
        await this.ShowPopupAsync(popup);
        CollectionViewMicasGraduaciones.SelectedItem = null;
    }

    private async void ConfirmarEleccion_Clicked(object sender, EventArgs e)
    {
        ConfirmarEleccion.Opacity = 0;
        await ConfirmarEleccion.FadeTo(1, 200);

        try
        {
            var pedidosMicas = _viewModel.GetPedidosMicas();
            GraduacionesSelected?.Invoke(this, new GraduacionesSelectedEventArgs { GraduacionesPedidoMicaSelected = pedidosMicas });
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Ok");
        }
    }


    private async void BtnRegresar_Clicked(object sender, EventArgs e)
    {
        BtnRegresar.Opacity = 0;
        await BtnRegresar.FadeTo(1, 200);

        await Shell.Current.GoToAsync("..");
    }
}