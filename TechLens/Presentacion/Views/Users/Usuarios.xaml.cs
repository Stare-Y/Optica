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
            LblUsuarioSeleccionado.Text = string.Empty;
            BtnEditarUsuario.IsEnabled = false;
            BtnEliminarUsuario.IsEnabled = false;
            BtnEditarUsuario.BackgroundColor = Color.FromRgba("#3C3D37");
            BtnEliminarUsuario.BackgroundColor = Color.FromRgba("#3C3D37");

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error inicializando la vista: {ex.Message} (Inner: {ex.InnerException})", "Aceptar");
        }
    }

    private async void BtnNuevoUsuario_Clicked(object sender, EventArgs e)
    {
        BtnNuevoUsuario.Opacity = 0;
        await BtnNuevoUsuario.FadeTo(1, 200);

        await Shell.Current.GoToAsync(nameof(Crear_EditarUsuario));
    }

    private async void BtnEditarUsuario_Clicked(object sender, EventArgs e)
    {
        try
        {
            var usuario = (Usuario)ListaUsuarios.SelectedItem;
            if (usuario != null)
            {
                _viewModelUsuario.UsuarioSeleccionado = usuario;
                var result = await DisplayAlert("Editar", $"¿Estás seguro de que deseas editar a {usuario.NombreDeUsuario}?", "Sí", "No");
                if (result)
                {
                    var editarUsuarioView = new Crear_EditarUsuario(usuario);

                    await Shell.Current.Navigation.PushAsync(editarUsuarioView);
                }
            }
        }
        catch
        {
            await DisplayAlert("Error", "Error editando el usuario", "Aceptar");
        }
        finally
        {
            ListaUsuarios.SelectedItem = null;
            BtnEditarUsuario.IsEnabled = false;
        }
    }

    private async void BtnEliminarUsuario_Clicked(object sender, EventArgs e)
    {
        var usuario = (Usuario)ListaUsuarios.SelectedItem;
        if (usuario != null)
        {
            var result = await DisplayAlert("Eliminar", $"¿Estás seguro de que deseas eliminar a {usuario.NombreDeUsuario}?", "Sí", "No");
            if (result)
            {
                await _viewModelUsuario.EliminarUsuario(usuario);
                LblUsuarioSeleccionado.Text = string.Empty;
            }
        }
    }

    private async void BtnRegresar_Clicked(object sender, EventArgs e)
    {
        BtnRegresar.Opacity = 0;
        await BtnRegresar.FadeTo(1, 200);

        await Shell.Current.GoToAsync("..");
    }

    private void ListaUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(ListaUsuarios.SelectedItem is null) { return; }
        if (e.CurrentSelection != null)
        {
            BtnEditarUsuario.IsEnabled = true;
            BtnEliminarUsuario.IsEnabled = true;

            if (App.Current.Resources.TryGetValue("SubTier", out var SubTierResource) && SubTierResource is Color SubTier)
            {
                BtnEditarUsuario.BackgroundColor = SubTier;
                BtnEliminarUsuario.BackgroundColor = SubTier;
            }
            if (App.Current.Resources.TryGetValue("Main", out var MainResource) && MainResource is Color Main)
            {
                BtnEditarUsuario.TextColor = Main;
                BtnEliminarUsuario.TextColor = Main;
            }

        }
        if (e.CurrentSelection.Any())
        {

            if (e.CurrentSelection.FirstOrDefault() is Usuario usuarioSeleccionado)
            {
                LblUsuarioSeleccionado.Text = usuarioSeleccionado.NombreDeUsuario;
            }
        }
        else
        {
            LblUsuarioSeleccionado.Text = string.Empty;
            BtnEditarUsuario.IsEnabled = false;
            BtnEliminarUsuario.IsEnabled = false;
            BtnEditarUsuario.BackgroundColor = Color.FromRgba("#3C3D37");
            BtnEliminarUsuario.BackgroundColor = Color.FromRgba("#3C3D37");
        }
    }
}