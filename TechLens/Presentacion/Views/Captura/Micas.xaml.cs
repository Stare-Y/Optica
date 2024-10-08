using Application.ViewModels;
using Domain.Entities;
using TechLens.Presentacion.Events;

namespace TechLens.Presentacion.Views.Captura;

public partial class Micas : ContentPage

{
    private readonly ViewModelMicas _viewModelMicas;

    public event EventHandler<MicasSelectedEventArgs>? MicaSelected;

    public Micas(ViewModelMicas viewModelMicas)
	{
		InitializeComponent();
        _viewModelMicas = viewModelMicas;
        this.BindingContext = _viewModelMicas;
	}

    public Micas() : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelMicas>())
    {
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            await _viewModelMicas.Initialize();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error inicializando la vista: {ex.Message} (Inner: {ex.InnerException})", "Aceptar");
        }
    }

    private async void CollectionViewMicas_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Mica mica = (Mica)CollectionViewMicas.SelectedItem;

        bool confirmacion = await DisplayAlert("Confirmacion", $"Seleccionar a: {mica.Material}?", "Volver", "Confirmar");

        if (!confirmacion) 
        {
            try
            {
                MicaSelected?.Invoke(this, new MicasSelectedEventArgs { SelectedMica = mica });

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error seleccionando la mica: {ex.Message} (Inner: {ex.InnerException})", "Aceptar");
                await Shell.Current.Navigation.PopAsync();
            }

        }

    }

    private void SearchBarMica_SearchButtonPressed(object sender, EventArgs e)
    {

    }

    private async void BtnNuevaMica_Clicked(object sender, EventArgs e)
    {
        BtnNuevaMica.Opacity = 0;
        await BtnNuevaMica.FadeTo(1, 200);

        var nuevaMica = new NuevaMica();
        await Shell.Current.Navigation.PushAsync(nuevaMica);
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        BtnCancelar.Opacity = 0;
        await BtnCancelar.FadeTo(1, 200);

        await Shell.Current.Navigation.PopAsync();

    }
}