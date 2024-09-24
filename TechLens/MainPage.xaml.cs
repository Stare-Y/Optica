namespace TechLens;

using Application.ViewModels;
using TechLens.Presentacion.Views;
using Microsoft.Maui.Controls;

public partial class MainPage : ContentPage
{
    //private readonly ViewModelMainPage _viewModelMainPage;

    public MainPage()
    {
        InitializeComponent();
        //this.BindingContext = viewModelMainPage;
        //_viewModelMainPage = viewModelMainPage;
    }

    private async void BtnConsultas_Clicked(object sender, EventArgs e)
    {
        BtnConsultas.Opacity = 0;
        await BtnConsultas.FadeTo(1, 200);

        await Shell.Current.GoToAsync(nameof(Consultas));
    }
    private async void BtnCapturas_Clicked(object sender, EventArgs e)
    {
        BtnCapturar.Opacity = 0;
        await BtnCapturar.FadeTo(1, 200);

        await Shell.Current.GoToAsync(nameof(Capturas));
    }

    private async void BtnReportes_Clicked(object sender, EventArgs e)
    {
        BtnReporte.Opacity = 0;
        await BtnReporte.FadeTo(1, 200);

        await Shell.Current.GoToAsync(nameof(Reportes));
    }

    private async void BtnVentas_Clicked(object sender, EventArgs e)
    {
        BtnVenta.Opacity = 0;
        await BtnVenta.FadeTo(1, 200);

        await Shell.Current.GoToAsync(nameof(Ventas));
    }



}


