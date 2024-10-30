using Application.ViewModels;
using Domain.Entities;

namespace TechLens.Presentacion.Views.Captura;

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
        await BtnGuardar.FadeTo(1, 200);

        await Shell.Current.Navigation.PopAsync();
    }
}