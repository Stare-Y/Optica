using Application.ViewModels;
using Domain.Entities;
using TechLens.Presentacion.Events;

namespace TechLens.Presentacion.Views;

public partial class Reportes : ContentPage
{
	public Reportes()
	{
		InitializeComponent();
	}

    private void CollectionViewMicas_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

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

    }
}