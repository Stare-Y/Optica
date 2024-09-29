namespace TechLens.Presentacion.Views.Users;

public partial class LogIn : ContentPage
{
	public LogIn()
	{
		InitializeComponent();
	}

    private async void BtnLogIn_Clicked(object sender, EventArgs e)
    {
        BtnLogIn.Opacity = 0;
        await BtnLogIn.FadeTo(1, 200);
    }

    private async void BtnReg_Clicked(object sender, EventArgs e)
    {
        BtnReg.Opacity = 0;
        await BtnReg.FadeTo(1, 200);
    }
}