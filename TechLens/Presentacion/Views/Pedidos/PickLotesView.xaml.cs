using Application.ViewModels;
using CommunityToolkit.Maui.Views;
using Domain.Entities;
using System.Threading.Tasks;
using TechLens.Presentacion.Events;
using TechLens.Presentacion.Views.Lotes;
using TechLens.Presentacion.Views.Popups;

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

		if(_viewModel.LotesElegidos.Count > 0)
		{
            BtnSavePedido.IsVisible = true;
        }
		else
		{
            BtnSavePedido.IsVisible = false;
        }
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
		BtnGetLote.Opacity = 0;
		await BtnGetLote.FadeTo(1, 200); 

        var displayLotesView = new DisplayLotesView();

		displayLotesView.LoteSelected += OnLotePicked;

		var popup = new SpinnerPopup();
		this.ShowPopup(popup);

		try
		{
			await displayLotesView.InitializeLotes();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "Ok");
		}
		finally
		{
			popup.Close();
		}

        await Shell.Current.Navigation.PushAsync(displayLotesView);
    }



    private async void LotesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		if (LotesCollection.SelectedItem is null)
			return;
		var popup = new SpinnerPopup();

		this.ShowPopup(popup);

		try
		{
			var selectedLote = (Lote)LotesCollection.SelectedItem;
            var loteRelationsDetails = new LoteRelationsDetails(selectedLote, _viewModel.PedidoLevantado);

			loteRelationsDetails.GraduacionesSelected += async (s, e) =>
			{
				if (e.GraduacionesPedidoMicaSelected == null)
					throw new InvalidDataException("No se pudo castear la entidad Lote de la coleccion.");

                foreach (var item in e.GraduacionesPedidoMicaSelected)
                {
                    item.IdLoteOrigen = selectedLote.Id;
					item.Precio = selectedLote.Costo;
					
					_viewModel.PedidoMicas.Add(item);
                }

				await Shell.Current.Navigation.PopAsync();
				await Shell.Current.Navigation.PopAsync();
            };

            await Shell.Current.Navigation.PushAsync(loteRelationsDetails);
        }
		catch(Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "Ok");
		}
		finally
		{
			popup.Close();
			LotesCollection.SelectedItem = null;
		}
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        BtnCancelar.Opacity = 0;
        await BtnCancelar.FadeTo(1, 200);

        await Shell.Current.Navigation.PopAsync();

    }


	private async void BtnSavePedido_Clicked(object sender, EventArgs e)
	{
		var popup = new SpinnerPopup();
		this.ShowPopup(popup);
		try
		{
            await _viewModel.GenerarPedido();
            await Shell.Current.GoToAsync("//MainPage");

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

    private async void BtnEliminarLote_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.BindingContext is Lote lote)
            {
                bool confirm = await DisplayAlert("Confirmar",
                    $"¿Deseas eliminar el lote {lote.Id} de la lista?", "Sí", "No");

                if (confirm)
                {
                    _viewModel.LotesElegidos.Remove(lote);

                    var micasToRemove = _viewModel.PedidoMicas
                        .Where(pm => pm.IdLoteOrigen == lote.Id)
                        .ToList();

                    foreach (var mica in micasToRemove)
                    {
                        _viewModel.PedidoMicas.Remove(mica);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Ok");
        }
    }




}