using Application.ViewModels;
using CommunityToolkit.Maui.Views;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Users;

public partial class LogIn : ContentPage
{
    private readonly VMLogin _viewModel;
    public LogIn(VMLogin viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
    }

    public LogIn() : this(MauiProgram.ServiceProvider.GetRequiredService<VMLogin>())
    {
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        User.Text = "";
        Password.Text = "";
    }

    private async void BtnLogIn_Clicked(object sender, EventArgs e)
    {
#if DEBUG
        User.Text = "admin";
        Password.Text = "admin";
#endif
        BtnLogIn.Opacity = 0;
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        await BtnLogIn.FadeTo(1, 200);
        try
        {
            await _viewModel.IniciarSesion(User.Text, Password.Text);
            if (_viewModel.UsuarioSeleccionado != null)
            {
                var mainPage = new MainPage(_viewModel.UsuarioSeleccionado);
                await Shell.Current.Navigation.PushAsync(mainPage);
            }
            else
            {
                throw new Exception("Usuario no encontrado");
            }

        }
        catch(Exception ex)
        {
            await DisplayAlert("Error", $"{ex.Message}", "Aceptar");
            User.Text = "";
            Password.Text = "";
            User.Focus();
        }
        finally
        {
            popup.Close();
        }
    }
}