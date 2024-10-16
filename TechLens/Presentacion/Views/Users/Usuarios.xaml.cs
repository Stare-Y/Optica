using Application.ViewModels;

namespace TechLens.Presentacion.Views.Users;

public partial class Usuarios : ContentPage
{
    private readonly ViewModelUsuario _viewModelUsuario;
	public Usuarios(ViewModelUsuario viewModelUsuario)
	{   
		InitializeComponent();
        _viewModelUsuario = viewModelUsuario;
        this.BindingContext = _viewModelUsuario;
	}
    public Usuarios() : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelUsuario>())
    {
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            await _viewModelUsuario.Inicializar();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error inicializando la vista: {ex.Message} (Inner: {ex.InnerException})", "Aceptar");
        }
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        return;
    }

    private async void BtnNuevoUsuario_Clicked(object sender, EventArgs e)
    {
        BtnNuevoUsuario.Opacity = 0;
        await BtnNuevoUsuario.FadeTo(1, 200);

        await Shell.Current.GoToAsync(nameof(Crear_EditarUsuario));
    }

    private async void BtnRegresar_Clicked(object sender, EventArgs e)
    {
        BtnRegresar.Opacity = 0;
        await BtnRegresar.FadeTo(1, 200);

        await Shell.Current.GoToAsync("..");

    }
}