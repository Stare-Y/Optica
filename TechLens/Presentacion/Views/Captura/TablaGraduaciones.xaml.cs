using Application.ViewModels;
using Domain.Entities;
using TechLens.Presentacion.Events;

namespace TechLens.Presentacion.Views.Captura;

public partial class TablaGraduaciones : ContentPage
{
    public event EventHandler<GraduacionesSelectedEventArgs>? GraduacionesSelected;
    public TablaGraduaciones()
	{
		InitializeComponent();
    }

    //Para confirmar y llamar al evento de seleccion de graduaciones, primero validar precios, y luego llamar al evento asignando sus respectivos elementos
}