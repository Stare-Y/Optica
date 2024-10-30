using Application.ViewModels;
using CommunityToolkit.Maui.Views;
using Domain.Entities;
using TechLens.Presentacion.Events;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Captura;

public partial class Micas : ContentPage

{
    private readonly ViewModelMicas _viewModelMicas;

    public event EventHandler<MicasSelectedEventArgs>? MicaSelected;

    private bool _needGoBack = false;

    public Micas(ViewModelMicas viewModelMicas)
	{
		InitializeComponent();
        _viewModelMicas = viewModelMicas;
        this.BindingContext = _viewModelMicas;
	}

    public Micas() : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelMicas>())
    {
    }

    public Micas(bool needGoBack) : this()
    {
        _needGoBack = needGoBack;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            await _viewModelMicas.Initialize();
            //para cada picker, seleccionar el primer item
            PickerTipo.SelectedIndex = 0;
            PickerMaterial.SelectedIndex = 0;
            PickerFabricante.SelectedIndex = 0;
            PickerTratamiento.SelectedIndex = 0;
            PickerProposito.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error inicializando la vista: {ex.Message} (Inner: {ex.InnerException})", "Aceptar");
        }
    }

    private async void CollectionViewMicas_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Mica mica = (Mica)CollectionViewMicas.SelectedItem;

        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        bool confirmacion = await DisplayAlert("Confirmacion", $"Seleccionar a: {mica.Material}?", "Volver", "Confirmar");

        if (!confirmacion) 
        {
            try
            {
                MicaSelected?.Invoke(this, new MicasSelectedEventArgs { SelectedMica = mica });
                if (!_needGoBack)
                {
                    var choice = await DisplayAlert("A donde ir?", "Quieres ver el Stock, o editar la mica?", "Stock", "Editar");
                    if (choice)
                    {
                        //TODO: ir a stock
                    }
                    else
                    {
                        var nuevaMica = new NuevaMica(mica);
                        await Shell.Current.Navigation.PushAsync(nuevaMica);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error seleccionando la mica: {ex.Message} (Inner: {ex.InnerException})", "Aceptar");
                await Shell.Current.Navigation.PopAsync();
            }
        }
        popup.Close();
    }

    private async void BtnNuevaMica_Clicked(object sender, EventArgs e)
    {
        BtnNuevaMica.Opacity = 0;
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        await BtnNuevaMica.FadeTo(1, 200);
        try
        {
            var nuevaMica = new NuevaMica();
            await Shell.Current.Navigation.PushAsync(nuevaMica);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error creando nueva mica: {ex.Message} (Inner: {ex.InnerException})", "Aceptar");
        }
        finally
        {
            popup.Close();
        }
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        BtnCancelar.Opacity = 0;
        await BtnCancelar.FadeTo(1, 200);

        await Shell.Current.Navigation.PopAsync();
    }

    private async void BtnAplicarFiltro_Clicked(object sender, EventArgs e)
    {
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        try
        {
            var tipo = PickerTipo.SelectedItem.ToString() ?? string.Empty;
            var material = PickerMaterial.SelectedItem.ToString() ?? string.Empty;
            var fabricante = PickerFabricante.SelectedItem.ToString() ?? string.Empty;
            var tratamiento = PickerTratamiento.SelectedItem.ToString() ?? string.Empty;
            var proposito = PickerProposito.SelectedItem.ToString() ?? string.Empty;
            await _viewModelMicas.AplicarFiltros(tipo, material, fabricante, tratamiento, proposito);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error aplicando filtros: {ex.Message} (Inner: {ex.InnerException})", "Aceptar");
        }
        finally
        {
            popup.Close();
        }
    }

    private void SearchBarMica_SearchButtonPressed(object sender, EventArgs e)
    {

    }
}