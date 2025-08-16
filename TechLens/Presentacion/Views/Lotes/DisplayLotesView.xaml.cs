using Application.ViewModels.Lotes;
using CommunityToolkit.Maui.Views;
using Domain.Entities;
using System.Collections.ObjectModel;
using TechLens.Presentacion.Events;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Lotes;

public partial class DisplayLotesView : ContentPage
{
	private VMDisplayLotes _viewModel;
    bool _orderByDesc = true;

    public event EventHandler<LoteSelectedEventArgs> LoteSelected = null!;
    public DisplayLotesView(VMDisplayLotes viewModel)
	{
		InitializeComponent();

		_viewModel = viewModel;

		this.BindingContext = _viewModel;
	}

    public async Task InitializeLotes()
    {
        await _viewModel.FetchLotes();

        PickerProveedor.ItemsSource = _viewModel.Lotes
            .Select(l=> l.Proveedor)
            .Distinct()
            .ToList();

        PickerReferencia.ItemsSource = _viewModel.Lotes
            .Select(l => l.Referencia)
            .Distinct()
            .ToList();

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

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        BtnCancelar.Opacity = 0;
        await BtnCancelar.FadeTo(1, 200);

        await Shell.Current.Navigation.PopAsync();

    }

    private void PickerProveedor_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedProveedor = PickerProveedor.SelectedItem?.ToString();

        if (string.IsNullOrWhiteSpace(selectedProveedor))
            return;

        var sorted = _viewModel.Lotes
            .OrderByDescending(l => l.Proveedor == selectedProveedor) // matches first
            .ThenBy(l => l.Proveedor) // optional: sort alphabetically within groups
            .ToList();

        _viewModel.Lotes = new ObservableCollection<Lote>(sorted);
    }

    private void PickerReferencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedReferencia = PickerReferencia.SelectedItem?.ToString();

        if (string.IsNullOrWhiteSpace(selectedReferencia))
            return;

        var sorted = _viewModel.Lotes
            .OrderByDescending(l => l.Referencia == selectedReferencia) // matches first
            .ThenBy(l => l.Referencia) // optional: sort alphabetically within groups
            .ToList();

        _viewModel.Lotes = new ObservableCollection<Lote>(sorted);
    }
}