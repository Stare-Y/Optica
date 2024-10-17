using Application.ViewModels;
using Domain.Entities;

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

    private async void BtnEditarUsuario_Clicked(object sender, EventArgs e)
    {
        var usuario = (Usuario)BindingContext;
        var result = await DisplayAlert("Editar", $"¿Estás seguro de que deseas editar a {usuario.NombreDeUsuario}?", "Sí", "No");
        if (result)
        {
            // Lógica para editar el usuario
            await Navigation.PopAsync();
        }
    }

    private async void BtnEliminarUsuario_Clicked(object sender, EventArgs e)
    {
        var usuario = (Usuario)BindingContext;
        var result = await DisplayAlert("Eliminar", $"¿Estás seguro de que deseas eliminar a {usuario.NombreDeUsuario}?", "Sí", "No");
        if (result)
        {
            // Lógica para eliminar el usuario
            await Navigation.PopAsync();
        }
    }

    private async void BtnRegresar_Clicked(object sender, EventArgs e)
    {
        BtnRegresar.Opacity = 0;
        await BtnRegresar.FadeTo(1, 200);

        await Shell.Current.GoToAsync("..");

    }
}