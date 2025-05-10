using Application.ViewModels.Lotes;
using Application.ViewModels.Lotes.DisplayHelpers;
using CommunityToolkit.Maui.Views;
using Domain.Entities;
using TechLens.Presentacion.Events;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Lotes;

public partial class LoteRelationsDetails : ContentPage
{
	private readonly VMLoteRelationsDetails _viewModel;
    public event EventHandler<MicasSelectedEventArgs> MicaSelectedEvent = null!;
    public event EventHandler<GraduacionesSelectedEventArgs>? GraduacionesSelected;

    public LoteRelationsDetails(VMLoteRelationsDetails viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		this.BindingContext = _viewModel;
	}

	public LoteRelationsDetails() : this(MauiProgram.ServiceProvider.GetRequiredService<VMLoteRelationsDetails>())
	{

	}

	public LoteRelationsDetails(Lote lote, Pedido pedido) : this()
	{
		_viewModel.ParentLote = lote;
		_viewModel.ParentPedido = pedido;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
		var popup = new SpinnerPopup();

		this.ShowPopup(popup);

		try
		{
            await _viewModel.FetchMicas();
        }
		catch(Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "Ok");
		}
		finally
		{
			popup.Close();
		}
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.Navigation.PopAsync();
    }

    private void BtnSeleccionar_Clicked(object sender, EventArgs e)
    {

    }

    private async void ContenedorMicas_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		if (ContenedorMicas.SelectedItem is null)
			return;

		try
		{
			var selectedMica = (DisplayLoteAndTaken)ContenedorMicas.SelectedItem;

            if (selectedMica == null)
                throw new InvalidDataException("No se pudo castear la entidad Lote de la coleccion.");

			var popup = new SpinnerPopup();
            this.ShowPopup(popup);

			var pickGraduacionesView = new GraduacionMica(selectedMica.MicaElement, _viewModel.ParentPedido, _viewModel.ParentLote.Id);

			pickGraduacionesView.GraduacionesSelected += (s, e) => GraduacionesSelected.Invoke(this, e);


            try
			{
                await pickGraduacionesView.PreparePedido();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
			finally 
			{
				popup.Close(); 
			}

            await Shell.Current.Navigation.PushAsync(pickGraduacionesView);
        }
		catch(Exception ex)
		{
			await DisplayAlert("Error", ex.Message, "Ok");
		}
		finally
		{
			ContenedorMicas.SelectedItem = null;
		}
    }

    private void BtnEliminarMica_Clicked(object sender, EventArgs e)
    {

    }
}