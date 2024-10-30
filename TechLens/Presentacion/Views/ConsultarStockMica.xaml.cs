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

    public ConsultarStockMica(Pedido pedido, Mica mica, bool readOnly) : this()
    {
        _viewModel.mica = mica;
        _viewModel.pedido = pedido;
        this.readOnly = readOnly;
    }
    public ConsultarStockMica(Mica mica, bool readOnly) : this()
    {
        _viewModel.mica = mica;
        this.readOnly = readOnly;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        try
        {
            if (!this.readOnly)
            {
                ConfirmarEleccion.IsEnabled = true;
            }
            await _viewModel.Initialize();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Ok");
        }
        finally
        {
            popup.Close();
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
        popup.StockTaken += async (s, e) =>
        {
            var stock = e.showConsultaStock;
            var stockIndex = _viewModel.ShowConsultaStock.IndexOf(row);
            _viewModel.ShowConsultaStock[stockIndex] = stock;
        };
        await this.ShowPopupAsync(popup);
    }

    private void ConfirmarEleccion_Clicked(object sender, EventArgs e)
    {
        try
        {
            var pedidosMicas = _viewModel.GetPedidosMicas();
            GraduacionesSelected?.Invoke(this, new GraduacionesSelectedEventArgs { GraduacionesPedidoMicaSelected = pedidosMicas });
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}