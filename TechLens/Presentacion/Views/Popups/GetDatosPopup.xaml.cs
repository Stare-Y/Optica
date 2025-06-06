using CommunityToolkit.Maui.Views;
using Domain.Entities;
using TechLens.Presentacion.Events;

namespace TechLens.Presentacion.Views.Popups;

public partial class GetDatosPopup : Popup
{
    public double Esfera { get; set; }
    public double Cilindro { get; set; }
    private int _cantidad { get; set; }
    private Mica _mica { get; set; }

    public event EventHandler<MicaDataSelectedEventArgs>? MicaDataSelected;
    public GetDatosPopup(Button button, double esfera, double cilindro, Mica mica)
	{
		InitializeComponent();
        Esfera = esfera;
        Cilindro = cilindro;
        _mica = mica;
        LblGraduacionCilindro.Text = Cilindro.ToString();
        LblGraduacionEsfera.Text = Esfera.ToString();

    }

    private void Cantidad_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(string.IsNullOrEmpty(Cantidad.Text))
        {
            return;
        }

        //intentar convertir a int
        if (int.TryParse(Cantidad.Text, out int cantidad))
        {
            _cantidad = cantidad;
        }
        else
        {
            Cantidad.Text = e.OldTextValue;
        }
    }

    private async void Confirmar_Clicked(object sender, EventArgs e)
    {
        BtnConfirmar.Opacity = 0;
        await BtnConfirmar.FadeTo(1, 200);
        try
        {
            var micaGraduacion = new MicaGraduacion
            {
                IdMica = _mica.Id,
                Graduacionesf = (float)Esfera,
                Graduacioncil = (float)Cilindro,
            };

            MicaDataSelected?.Invoke(this, new MicaDataSelectedEventArgs
            {
                MicaGraduacionCaptured = micaGraduacion,
                Cantidad = _cantidad
            });
        }
        catch
        {
            throw;
        }
        finally
        {
            this.Close();
        }
    }

    private async void Regresar_Clicked(object sender, EventArgs e)
    {
        BtnRegresar.Opacity = 0;
        await BtnRegresar.FadeTo(1, 200);

        this.Close();
    }
}