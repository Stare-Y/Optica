using Application.ViewModels;
using CommunityToolkit.Maui.Views;
using TechLens.Presentacion.Views.Popups;
using DocumentFormat.OpenXml.Vml.Spreadsheet;

namespace TechLens.Presentacion.Views;

public partial class ReportesPanelView : ContentPage
{
    private readonly ViewModelReportes _viewModel = null!;
	public ReportesPanelView(ViewModelReportes viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        this.BindingContext = _viewModel;
	}

    public ReportesPanelView() : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelReportes>())
    {
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.FechaInicio = DateTime.Now.Date;
        _viewModel.FechaFin = DateTime.Now.Date;
    }

    private async void BtnRegresar_Clicked(object sender, EventArgs e)
    {
        BtnRegresar.Opacity = 0;
        await BtnRegresar.FadeTo(1, 200);
        await Shell.Current.GoToAsync("..");
    }

    private async void BtnImprimir_Clicked(object sender, EventArgs e)
    {
        BtnImprimir.Opacity = 0;

        var popup = new SpinnerPopup();
        this.ShowPopup(popup);

        await BtnImprimir.FadeTo(1, 200);

        //Generar reporte
        try
        {
            await _viewModel.GenerarReporteExcel();
            await DisplayAlert("Exito", "Reporte generado con exito :)", "Aceptar");
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

    private void DatePickerFechaFin_DateSelected(object sender, DateChangedEventArgs e)
    {
        _viewModel.FechaFin = e.NewDate.Date;
    }

    private void DatePickerFechaInicio_DateSelected(object sender, DateChangedEventArgs e)
    {
        _viewModel.FechaInicio = e.NewDate.Date;
    }

    private async void BtnGenerarReporte_Clicked(object sender, EventArgs e)
    {
        BtnGenerarReporte.Opacity = 0;
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        await BtnGenerarReporte.FadeTo(1, 200);
        BtnGenerarReporte.Opacity = 0;
        await BtnGenerarReporte.FadeTo(1, 200);
        try
        {
            await _viewModel.GetReportePedidos();
            BtnImprimir.IsEnabled = true;

            if (App.Current.Resources.TryGetValue("SubTier", out var SubTierResource) && SubTierResource is Color SubTier)
            {
                BtnImprimir.BackgroundColor = SubTier;
            }
            if (App.Current.Resources.TryGetValue("Main", out var MainResource) && MainResource is Color Main)
            {
                BtnImprimir.TextColor = Main;
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");

            BtnImprimir.IsEnabled = false;
            BtnImprimir.BackgroundColor = Color.FromRgba("#3C3D37");
        }
        finally
        {
            popup.Close();
        }
    }
}