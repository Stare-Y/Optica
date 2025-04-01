using Application.ViewModels.Lotes;
using Application.ViewModels.Lotes.DisplayHelpers;
using CommunityToolkit.Maui.Views;
using Domain.Entities;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Lotes;

public partial class LoteRelationsDetails : ContentPage
{
	private readonly VMLoteRelationsDetails _viewModel;
	public LoteRelationsDetails(VMLoteRelationsDetails viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		this.BindingContext = _viewModel;
	}

	public LoteRelationsDetails() : this(MauiProgram.ServiceProvider.GetRequiredService<VMLoteRelationsDetails>())
	{

	}

	public LoteRelationsDetails(Lote lote) : this()
	{
		_viewModel.ParentLote = lote;
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
			if(e.CurrentSelection.FirstOrDefault() is DisplayLoteAndTaken micaSeleccionada)
			{
				if (micaSeleccionada.showTakenString)
					micaSeleccionada.showTakenString = false;
				else
				{
					micaSeleccionada.showTakenString = true;
					micaSeleccionada.TakenMicasString = "Hola me seleccionaste";
				}

			}
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