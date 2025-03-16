using Application.ViewModels.Lotes;
using CommunityToolkit.Maui.Views;
using Domain.Entities;
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

    private void LotesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LoteSelected?.Invoke(this, new LoteSelectedEventArgs { SelectedLote = (Lote)LotesCollection.SelectedItem});
    }
}