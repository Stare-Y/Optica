using Application.ViewModels;

namespace TechLens.Presentacion.Views;

public partial class Micas : ContentPage

{
    private readonly ViewModelMicas _viewModelMicas;
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

    private void BtnNuevaMica_Clicked(object sender, EventArgs e)
    {

    }

}