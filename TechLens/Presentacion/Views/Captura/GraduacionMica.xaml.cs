using JetBrains.Annotations;

namespace TechLens.Presentacion.Views.Captura;

public partial class GraduacionMica : ContentPage
{
	public GraduacionMica()
	{
		InitializeComponent();
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

    }

    private void entryEsfera_TextChanged(object sender, TextChangedEventArgs e)
    {
       var entry = sender as Entry;
       var text = entry.Text;

        // Permite números, punto y guión solo en la primera posición.
        if (!string.IsNullOrEmpty(text))
        {
            text = new string(text.Where((c, i) => char.IsDigit(c) || c == '.' || (i == 0 && c == '-')).ToArray());
        }

        // Asegurarse de que el guión solo esté en la primera posición.
        if (text.Contains("-") && text.IndexOf('-') > 0)
        {
            text = text.Replace("-", "");
        }

        if (text.Length > 7) // 6 caracteres más el guión opcional.
        {
            text = text.Substring(0, 7);
        }

        if (entry.Text != text)
        {
            entry.Text = text;
        }

    }

    private void entryCilindro_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        var text = entry.Text;

        // Permite números, punto y guión solo en la primera posición.
        if (!string.IsNullOrEmpty(text))
        {
            text = new string(text.Where((c, i) => char.IsDigit(c) || c == '.' || (i == 0 && c == '-')).ToArray());
        }

        // Asegurarse de que el guión solo esté en la primera posición.
        if (text.Contains("-") && text.IndexOf('-') > 0)
        {
            text = text.Replace("-", "");
        }

        if (text.Length > 7) // 6 caracteres más el guión opcional.
        {
            text = text.Substring(0, 7);
        }

        if (entry.Text != text)
        {
            entry.Text = text;
        }

    }
}