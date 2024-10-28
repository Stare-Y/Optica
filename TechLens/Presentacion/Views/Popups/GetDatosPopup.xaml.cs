using CommunityToolkit.Maui.Views;

namespace TechLens.Presentacion.Views.Popups;

public partial class GetDatosPopup : Popup
{
	public GetDatosPopup(Button button)
	{
		InitializeComponent();
	}

    private void Precio_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void Cantidad_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private async void Confirmar_Clicked(object sender, EventArgs e)
    {
        BtnConfirmar.Opacity = 0;
        await BtnConfirmar.FadeTo(1, 200);

    }

    private async void Regresar_Clicked(object sender, EventArgs e)
    {
        BtnRegresar.Opacity = 0;
        await BtnRegresar.FadeTo(1, 200);

        this.Close();

    }
}