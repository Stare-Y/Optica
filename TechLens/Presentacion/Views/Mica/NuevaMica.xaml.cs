using Application.ViewModels;
using CommunityToolkit.Maui.Views;
using Domain.Entities;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Lotes;

public partial class NuevaMica : ContentPage
{
    private readonly VMNuevaMica _viewModel;
    public NuevaMica(VMNuevaMica viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        this.BindingContext = _viewModel;
    }

    public NuevaMica() : this(MauiProgram.ServiceProvider.GetRequiredService<VMNuevaMica>())
    {
    }

    public NuevaMica(Mica mica) : this()
    {
        _viewModel.Mica = mica;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.Mica.Id == 0)
        {
            await _viewModel.Initialize();
        }
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        BtnCancelar.Opacity = 0;
        await BtnCancelar.FadeTo(1, 200);

        await Shell.Current.Navigation.PopAsync();
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
        BtnGuardar.Opacity = 0;
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        try
        {
            await BtnGuardar.FadeTo(1, 200);

            await _viewModel.GuardarMica();

            await Shell.Current.Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error guardando la mica: {ex.Message} (Inner: {ex.InnerException})", "Aceptar");
        }
        finally
        {
            popup.Close();
        }
    }
}