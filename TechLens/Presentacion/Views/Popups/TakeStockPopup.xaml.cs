using CommunityToolkit.Maui.Views;
using Domain.Interfaces.Services.DisplayEntities;
using TechLens.Presentacion.Events;

namespace TechLens.Presentacion.Views.Popups;

public partial class TakeStockPopup: Popup
{
    public ShowConsultaStock ConsultaStock;

    public event EventHandler<StockTakenEventArgs>? StockTaken;
    public TakeStockPopup(ShowConsultaStock micaStock)
	{
		InitializeComponent();
        ConsultaStock = micaStock;
        LblGraduacionCilindro.Text = micaStock.MicaGraduacion.Graduacioncil.ToString();
        LblGraduacionEsfera.Text = micaStock.MicaGraduacion.Graduacionesf.ToString();
        LblStock.Text = "0/" + micaStock.Stock.ToString();
        Tomar.Text = micaStock.Taken == 0? "" : micaStock.Taken.ToString();
        Dispatcher.Dispatch(() =>
        {
            Tomar.Focus();
        });
    }

    private async void Confirmar_Clicked(object sender, EventArgs e)
    {
        BtnConfirmar.Opacity = 0;
        await BtnConfirmar.FadeTo(1, 200);
        try
        {
            if (ConsultaStock.Taken < 1)
            {
                throw new Exception("No haz elegido cuanto stock haz tomado");
            }

            if(ConsultaStock.Stock < ConsultaStock.Taken)
            {
                throw new Exception("No puedes tomar mas stock del que hay");
            }

            StockTaken?.Invoke(this, new StockTakenEventArgs
            {
                ShowConsultaStock = ConsultaStock
            });
        }
        catch(Exception ex)
        {
            LblStock.Text = $"{ex.Message}";
            Tomar.Focus();
            return;
        }
        this.Close();
    }

    private async void Regresar_Clicked(object sender, EventArgs e)
    {
        BtnRegresar.Opacity = 0;
        await BtnRegresar.FadeTo(1, 200);

        this.Close();
    }

    private void Tomar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            LblStock.Text = "0/" + ConsultaStock.Stock;
            return;
        }
        //parse the text to an int
        if (int.TryParse(e.NewTextValue, out int taken))
        {
            ConsultaStock.Taken = taken;
            LblStock.Text = taken + "/" + ConsultaStock.Stock;
        }
        else
        {
            Tomar.Text = e.OldTextValue;
        }
    }
}