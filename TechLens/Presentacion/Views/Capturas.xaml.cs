using Application.ViewModels;

namespace TechLens.Presentacion.Views;

public partial class Capturas : ContentPage
{
	private readonly ViewModelCapturas _viewModelCapturas;
	public Capturas(ViewModelCapturas viewModelCapturas)
	{
		InitializeComponent();

		_viewModelCapturas = viewModelCapturas;
        this.BindingContext = _viewModelCapturas;

    }

    public Capturas() : this(MauiProgram.ServiceProvider.GetService<ViewModelCapturas>())
    {
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModelCapturas.Initialize();
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
		BtnCancelar.Opacity = 0;
		await BtnCancelar.FadeTo(1, 200);

		await Shell.Current.GoToAsync("..");
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
		BtnGuardar.Opacity = 0;
        await BtnGuardar.FadeTo(1, 200);

		await DisplayAlert("Guardado", "Se ha guardado la captura de datos", "Aceptar");
		await Shell.Current.GoToAsync(nameof(Micas));
    }
}