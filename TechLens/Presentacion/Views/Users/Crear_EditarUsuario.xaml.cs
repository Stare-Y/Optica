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
            var result = await DisplayAlert("Guardar", $"�Est�s seguro de que deseas guardar a {usuario.NombreDeUsuario}?", "S�", "No");
            if (result)
            {
                // L�gica para guardar el usuario
                await Navigation.PopAsync();
            }
        }

        

        private async void BtnCancelar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        


    }






}
//public int Id { get; set; }
//public string NombreDeUsuario { get; set; } = string.Empty;
//public string Password { get; set; } = string.Empty;
//public string Rol { get; set; } = string.Empty;

