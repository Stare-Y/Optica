using Application.ViewModels;
using Domain.Entities;
using TechLens.Presentacion.Events;

namespace TechLens.Presentacion.Views.Captura;

public partial class SeleccionMicas : ContentPage
{
    private readonly VMSeleccionMicas _viewModel;
	public SeleccionMicas(VMSeleccionMicas vMSeleccionMicas)
	{
		InitializeComponent();
        _viewModel = vMSeleccionMicas;
        this.BindingContext = _viewModel;
	}

    public SeleccionMicas(Lote lote) : this(MauiProgram.ServiceProvider.GetRequiredService<VMSeleccionMicas>())
    {
        _viewModel.Lote = lote;
    }

    private async void OnMicaSelected(object? sender, MicasSelectedEventArgs e)
    {
        if(e.SelectedMica is not null)
            _viewModel.MicasSeleccionadas.Add(e.SelectedMica);
        await Shell.Current.Navigation.PopAsync();
    }

    private async void OnGraduacionesSelected(object? sender, GraduacionesSelectedEventArgs e)
    {
        try
        {
            if (e.GraduacionesLoteSelected is not null)
            {
                //agregar a la lista de graduaciones, si ya existia, actualizarla con los nuevos valores
                foreach (var item in e.GraduacionesLoteSelected)
                {
                    var index = _viewModel.LotesMicas.FindIndex(x => x.IdMicaGraduacion == item.IdMicaGraduacion);
                    if (index != -1)
                    {
                        _viewModel.LotesMicas[index] = item;
                    }
                    else
                    {
                        _viewModel.LotesMicas.Add(item);
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "No se han seleccionado graduaciones", "Aceptar");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
        finally
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        BtnCancelar.Opacity = 0;
        await BtnCancelar.FadeTo(1, 200);

        await Shell.Current.Navigation.PopAsync();
    }

    private async void BtnSeleccionar_Clicked(object sender, EventArgs e)
    {
        BtnSeleccionar.Opacity = 0;
        await BtnSeleccionar.FadeTo(1, 200);

        var micas = new Micas();
        micas.MicaSelected += OnMicaSelected;

        await Shell.Current.Navigation.PushAsync(micas);
    }

    private async void BtnBorrarTodo_Clicked(object sender, EventArgs e)
    {
        BtnBorrarTodo.Opacity = 0;
        await BtnBorrarTodo.FadeTo(1, 200);

    }

    private async void BtnConfirmar_Clicked(object sender, EventArgs e)
    {
        BtnConfirmar.Opacity = 0;
        await BtnConfirmar.FadeTo(1, 200);
        try
        {
            await _viewModel.SaveLote();
            await DisplayAlert("Guardado", "Se ha guardado la captura de datos", "Aceptar");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }

        var graduacionMica = new GraduacionMica();

        await Shell.Current.GoToAsync(nameof(GraduacionMica));
    }
}