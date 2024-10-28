using Application.ViewModels;
using Domain.Entities;
using TechLens.Presentacion.Events;

namespace TechLens.Presentacion.Views.Captura;

public partial class TablaGraduaciones : ContentPage
{
	public readonly VMTablaGraduaciones viewModel;
    public event EventHandler<GraduacionesSelectedEventArgs>? GraduacionesSelected;
    public TablaGraduaciones(VMTablaGraduaciones viewModel)
	{
		InitializeComponent();
        this.viewModel = viewModel;
        //Implementar logica para bindingcontext por si es pedidos o lote
    }
    public TablaGraduaciones() : this(MauiProgram.ServiceProvider.GetRequiredService<VMTablaGraduaciones>())
    {
    }
    public TablaGraduaciones(Mica mica, Lote lote) : this()
    {
        viewModel.Lote = lote;
        viewModel.Mica = mica;
    }
    public TablaGraduaciones(Mica mica, Pedido pedido) : this()
    {
        viewModel.Pedido = pedido;
        viewModel.Mica = mica;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if(viewModel.Lote is null && viewModel.Pedido is null || viewModel.Mica is null)
        {
            await DisplayAlert("Error", "No se ha asignado un lote o pedido", "Ok");
            await Navigation.PopAsync();
        }
    }

    //Para confirmar y llamar al evento de seleccion de graduaciones, primero validar precios, y luego llamar al evento asignando sus respectivos elementos
}