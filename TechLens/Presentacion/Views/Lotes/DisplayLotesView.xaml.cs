using Application.ViewModels.Lotes;
using CommunityToolkit.Maui.Views;
using Domain.Entities;
using System.Threading.Tasks;
using TechLens.Presentacion.Events;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Lotes;

public partial class DisplayLotesView : ContentPage
{
	private VMDisplayLotes _viewModel;

	public event EventHandler<LoteSelectedEventArgs> LoteSelected = null!;
    public DisplayLotesView(VMDisplayLotes viewModel)
	{
		InitializeComponent();

		_viewModel = viewModel;

		this.BindingContext = _viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var popup = new SpinnerPopup();
        this.ShowPopup(popup);

        try
        {
            await _viewModel.FetchLotes();

        }
        catch(Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            popup.Close();
        }
    }

    public DisplayLotesView() : this(MauiProgram.ServiceProvider.GetRequiredService<VMDisplayLotes>())
	{

	}

    private async void LotesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LotesCollection.SelectedItem is null)
            return;

        var popup = new SpinnerPopup();
        this.ShowPopup(popup);

        try
        {
            Lote selectedLote = (Lote)LotesCollection.SelectedItem;

            if (selectedLote == null)
                throw new InvalidDataException("No se pudo castear la entidad Lote de la coleccion.");

            LoteSelected?.Invoke(this, new LoteSelectedEventArgs { SelectedLote = selectedLote});

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
}