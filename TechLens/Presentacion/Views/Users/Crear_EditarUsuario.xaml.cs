using Domain.Entities;

namespace TechLens.Presentacion.Views.Users
{
    public partial class Crear_EditarUsuario : ContentPage
    {
        public Crear_EditarUsuario()
        {
            InitializeComponent();
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            var usuario = (Usuario)BindingContext;
            var result = await App.Current.MainPage.DisplayAlert("Guardar", $"¿Estás seguro de que deseas guardar a {usuario.NombreDeUsuario}?", "Sí", "No");
            if (result)
            {
                // Lógica para guardar el usuario
                await Navigation.PopAsync();
            }
        }

        private async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            var usuario = (Usuario)BindingContext;
            var result = await App.Current.MainPage.DisplayAlert("Eliminar", $"¿Estás seguro de que deseas eliminar a {usuario.NombreDeUsuario}?", "Sí", "No");
            if (result)
            {
                // Lógica para eliminar el usuario
                await Navigation.PopAsync();
            }
        }

        private async void BtnCancelar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void BtnRegresar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }






}
//public int Id { get; set; }
//public string NombreDeUsuario { get; set; } = string.Empty;
//public string Password { get; set; } = string.Empty;
//public string Rol { get; set; } = string.Empty;

