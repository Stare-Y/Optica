using Application.ViewModels;
using Domain.Entities;

namespace TechLens.Presentacion.Views.Users
{
    public partial class Crear_EditarUsuario : ContentPage
    {
        private ViewModelEditarUsuario _viewModel;
        public Crear_EditarUsuario(ViewModelEditarUsuario viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        public Crear_EditarUsuario() : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelEditarUsuario>())
        {

        }

        public Crear_EditarUsuario(Usuario usuario) : this()
        {
            _viewModel.UsuarioSeleccionado = usuario;
            RolePicker.SelectedItem = usuario.Rol;
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result = await DisplayAlert("Guardar", $"�Est�s seguro de que deseas guardar a {_viewModel.UsuarioSeleccionado.NombreDeUsuario}?", "S�", "No");
                await _viewModel.GuardarUsuario();

                if (result)
                {
                    // L�gica para guardar el usuario
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error guardando el usuario: {ex.Message} (Inner: {ex.InnerException})", "Aceptar");
            }
        }

        

        private async void BtnCancelar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Nombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(_viewModel == null)
                return;
            if (e.NewTextValue != null)
                _viewModel.UsuarioSeleccionado.NombreDeUsuario = e.NewTextValue;
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewModel == null)
                return;
            if (e.NewTextValue != null)
                _viewModel.UsuarioSeleccionado.Password = e.NewTextValue;
        }

        private async void RolePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_viewModel == null)
                    return;
                if (RolePicker.SelectedItem == null)
                    throw new Exception("El rol seleccionado es nulo");
                var rolSeleccionado = RolePicker.SelectedItem.ToString();
                if (rolSeleccionado != null)
                    _viewModel.UsuarioSeleccionado.Rol = rolSeleccionado;
                else
                    throw new Exception("El rol seleccionado es nulo");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error seleccionando el rol: {ex.Message} (Inner: {ex.InnerException})", "Aceptar");
            }
        }
    }
}
