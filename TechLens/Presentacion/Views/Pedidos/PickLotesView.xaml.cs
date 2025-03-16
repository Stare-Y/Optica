using Application.ViewModels;
using Domain.Entities;
using System.Threading.Tasks;
using TechLens.Presentacion.Events;
using TechLens.Presentacion.Views.Lotes;

namespace TechLens.Presentacion.Views.Pedidos;

public partial class PickLotesView : ContentPage
{
	public VMLotesView _viewModel = null!;
	public PickLotesView(VMLotesView viewModel)
	{
		InitializeComponent();

		_viewModel = viewModel;
		this.BindingContext = viewModel;
	}

    protected override void OnAppearing()
	{
		base.OnAppearing();

		_viewModel.NotifyPedidoRegistered();
	}


    public PickLotesView() : this(MauiProgram.ServiceProvider.GetRequiredService<VMLotesView>())
	{

	}

	public PickLotesView(Pedido pedido) : this()
	{
		_viewModel.PedidoLevantado = pedido;
	}

	private async void OnLotePicked(object? sender, LoteSelectedEventArgs e)
	{
		try
		{
            if (e.SelectedLote == null)
                throw new InvalidDataException("Hubo un problema tomando el lote para el pedido, el lote fue nulo");

            _viewModel.LotesElegidos.Add(e.SelectedLote);
        }
		catch(Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "Ok");
		}
		finally
		{
            await Shell.Current.Navigation.PopAsync();
        }
    }
    private async void BtnGetLote_Clicked(object sender, EventArgs e)
    {
		var displayLotesView = new DisplayLotesView();

		displayLotesView.LoteSelected += OnLotePicked;

		await Shell.Current.Navigation.PushAsync(displayLotesView);
    }
}