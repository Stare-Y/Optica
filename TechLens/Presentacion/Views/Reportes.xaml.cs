using Application.ViewModels;

namespace TechLens.Presentacion.Views;

public partial class Reportes : ContentPage
{
    private readonly ViewModelReportes _viewModel = null!;
	public Reportes(ViewModelReportes viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        this.BindingContext = _viewModel;
	}

    public Reportes() : this(MauiProgram.ServiceProvider.GetRequiredService<ViewModelReportes>())
    {
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.FechaInicio = DateTime.Now;
        _viewModel.FechaFin = DateTime.Now;
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
        await BtnImprimir.FadeTo(1, 200);

        //Generar reporte
        try
        {
            await _viewModel.ExportarReporteObtenido();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }

    private void DatePickerFechaFin_DateSelected(object sender, DateChangedEventArgs e)
    {
        _viewModel.FechaFin = e.NewDate;
    }

    private void DatePickerFechaInicio_DateSelected(object sender, DateChangedEventArgs e)
    {
        _viewModel.FechaFin = e.NewDate;
    }

    private async void BtnGenerarReporte_Clicked(object sender, EventArgs e)
    {
        try
        {
            await _viewModel.GetReportePedidos();
            BtnImprimir.IsEnabled = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }
}