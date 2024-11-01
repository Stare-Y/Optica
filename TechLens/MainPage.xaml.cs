namespace TechLens;

using Application.ViewModels;
using TechLens.Presentacion.Views;
using TechLens.Presentacion.Views.Captura;
using Microsoft.Maui.Controls;
using Domain.Entities;
using TechLens.Presentacion.Views.Popups;
using CommunityToolkit.Maui.Views;
using DocumentFormat.OpenXml.Spreadsheet;
using TechLens.Presentacion.Views.Users;

public partial class MainPage : ContentPage
{
    private readonly ViewModelMainPage _viewModelMainPage;

    public MainPage(ViewModelMainPage viewModelMainPage)
    {
        InitializeComponent();
        this.BindingContext = viewModelMainPage;
        _viewModelMainPage = viewModelMainPage;
        _viewModelMainPage = viewModelMainPage;
    }

    public MainPage() : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelMainPage>())
    {
    }

    public MainPage(Usuario usuario) : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelMainPage>())
    {
        _viewModelMainPage.Usuario = usuario;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BtnCapturar.Focus();

    }

    private async void Users_Clicked(object sender, EventArgs e)
    {
        Users.Opacity = 0;
        await Users.FadeTo(1, 200);

        await Shell.Current.GoToAsync(nameof(Usuarios));
    }

    private async void BtnConsultas_Clicked(object sender, EventArgs e)
    {
        BtnConsultas.Opacity = 0;
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        await BtnConsultas.FadeTo(1, 200);
        try
        {
            await Shell.Current.GoToAsync(nameof(Micas));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
        finally
        {
            popup.Close();
        }
    }

    private async void BtnCapturas_Clicked(object sender, EventArgs e)
    {
        BtnCapturar.Opacity = 0;
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        await BtnCapturar.FadeTo(1, 200);
        try
        {
            await Shell.Current.GoToAsync(nameof(Capturas));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
        finally
        {
            popup.Close();
        }
    }

    private async void BtnReportes_Clicked(object sender, EventArgs e)
    {
        BtnReporte.Opacity = 0;
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        await BtnReporte.FadeTo(1, 200);
        try
        {
            await Shell.Current.GoToAsync(nameof(Reportes));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
        finally
        {
            popup.Close();
        }
    }

    private async void BtnVentas_Clicked(object sender, EventArgs e)
    {
        BtnVenta.Opacity = 0;
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        await BtnVenta.FadeTo(1, 200);
        try
        {
            var viewVentas = new Ventas(_viewModelMainPage.Usuario);

            await Shell.Current.Navigation.PushAsync(viewVentas);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
        finally
        {
            popup.Close();
        }
    }
}
