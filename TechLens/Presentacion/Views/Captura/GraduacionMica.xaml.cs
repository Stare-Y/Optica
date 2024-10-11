using JetBrains.Annotations;

namespace TechLens.Presentacion.Views.Captura;

public partial class GraduacionMica : ContentPage
{
	public GraduacionMica()
	{
		InitializeComponent();
	}

    private void entryEsfera_TextChanged(object sender, TextChangedEventArgs e)
    {
       var entry = sender as Entry;
       if(entry != null)
        {
            var text = entry.Text;
            if (!string.IsNullOrEmpty(text))
            {
                text = new string(text.Where((c, i) => char.IsDigit(c) || c == '.' || (i == 0 && c == '-')).ToArray());
            }

            if (text.Contains("-") && text.IndexOf('-') > 0)
            {
                text = text.Replace("-", "");
            }

            if (text.Length > 7)
            {
                text = text.Substring(0, 7);
            }

            if (entry.Text != text)
            {
                entry.Text = text;
            }
        }
    }

    private void entryCilindro_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry != null)
        {
            var text = entry.Text;

            // Permite n�meros, punto y gui�n solo en la primera posici�n.
            if (!string.IsNullOrEmpty(text))
            {
                text = new string(text.Where((c, i) => char.IsDigit(c) || c == '.' || (i == 0 && c == '-')).ToArray());
            }

            // Asegurarse de que el gui�n solo est� en la primera posici�n.
            if (text.Contains("-") && text.IndexOf('-') > 0)
            {
                text = text.Replace("-", "");
            }

            if (text.Length > 7) // 6 caracteres m�s el gui�n opcional.
            {
                text = text.Substring(0, 7);
            }

            if (entry.Text != text)
            {
                entry.Text = text;
            }
        }
    }
}