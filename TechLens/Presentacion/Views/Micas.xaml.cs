using Application.ViewModels;
using Domain.Entities;
using TechLens.Presentacion.Events;

namespace TechLens.Presentacion.Views;

public partial class Micas : ContentPage

{
    private readonly ViewModelMicas _viewModelMicas;
    public event EventHandler<MicasSelectedEventArgs> MicasSelected;
	public Micas(ViewModelMicas viewModelMicas)
	{
		InitializeComponent();
        _viewModelMicas = viewModelMicas;
        this.BindingContext = _viewModelMicas;
	}

    public Micas() : this(MauiProgram.ServiceProvider.GetService<ViewModelMicas>())
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

    private void CollectionViewMicas_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
       
    }

    private void SearchBarMica_SearchButtonPressed(object sender, EventArgs e)
    {

    }

    private async void BtnNuevaMica_Clicked(object sender, EventArgs e)
    {
        BtnNuevaMica.Opacity = 0;
        await BtnNuevaMica.FadeTo(1, 200);

    }

    private void ConfirmarSeleccion(IEnumerable<Mica> micas)
    {
        MicasSelected?.Invoke(this, new MicasSelectedEventArgs { MicasSelected = micas });
    }
}