
using CommunityToolkit.Maui.Views;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Captura;

public partial class GraduacionMica : ContentPage
{

    public GraduacionMica()
    {
        InitializeComponent();

        double rangoMinimo = -3;
        double rangoMaximo = 3;

        // Configurar valores iniciales en los Entry
        MinGraduacion.Text = rangoMinimo.ToString();
        MaxGraduacion.Text = rangoMaximo.ToString();

        TablaDeGraduaciones(rangoMinimo, rangoMaximo);
    }

    private async void CargarTabla_Clicked(object sender, EventArgs e)
    {
        CargarTabla.Opacity = 0;
        await CargarTabla.FadeTo(1, 200);
        var popup = new SpinnerPopup();
        this.ShowPopup(popup);
        try
        {
            if (double.TryParse(MinGraduacion.Text, out double minGraduacion) &&
                double.TryParse(MaxGraduacion.Text, out double maxGraduacion))
            {
                // Llamar al método para crear la tabla con el rango especificado
                TablaDeGraduaciones(minGraduacion, maxGraduacion);
            }

        }
        catch (Exception)
        {
            await DisplayAlert("Error", "Por favor, ingresa valores numéricos válidos.", "OK");
        }
        finally
        {
            popup.Close();
        }

    }
    private void TablaDeGraduaciones(double minGraduacion, double maxGraduacion)
    {
        double incremento = 0.25;

        Graduaciones.RowDefinitions.Clear();
        Graduaciones.ColumnDefinitions.Clear();
        Graduaciones.Children.Clear();

        int rowCount = (int)((maxGraduacion - minGraduacion) / incremento) + 1;

        // Definiciones de filas y columnas
        for (int i = 0; i <= rowCount; i++)
        {
            Graduaciones.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
        }

        for (int i = 0; i <= rowCount; i++)
        {
            Graduaciones.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
        }

        // Encabezado principal
        var encabezadoTabla = new Label
        {
            Text = "ESF / CIL",
            BackgroundColor = Colors.Purple,
            TextColor = Colors.Black,
            FontSize = 10,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };
        var headerFrame = new Frame
        {
            BorderColor = Colors.Black,
            Content = encabezadoTabla,
            Padding = 0,
            Margin = new Thickness(5),
            HasShadow = false
        };
        Graduaciones.Children.Add(headerFrame);
        Grid.SetRow(headerFrame, 0);
        Grid.SetColumn(headerFrame, 0);

        // Esferas
        for (int row = 1; row <= rowCount; row++)
        {
            double sphereValue = minGraduacion + (row - 1) * incremento;
            var sphereLabel = new Label
            {
                Text = $"{sphereValue:N2}",
                BackgroundColor = Colors.Gray,
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Padding = 5
            };
            var frame = new Frame
            {
                BorderColor = Colors.Black,
                Content = sphereLabel,
                Padding = 0,
                Margin = new Thickness(5),
                HasShadow = false
            };
            Graduaciones.Children.Add(frame);
            Grid.SetRow(frame, row);
            Grid.SetColumn(frame, 0);
        }

        // Cilindros
        for (int col = 1; col <= rowCount; col++)
        {
            double cylinderValue = minGraduacion + (col - 1) * incremento;
            var cylinderLabel = new Label
            {
                Text = $"{cylinderValue:N2}",
                BackgroundColor = Colors.Gray,
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Padding = 5
            };
            var frame = new Frame
            {
                BorderColor = Colors.Black,
                Content = cylinderLabel,
                Padding = 0,
                Margin = new Thickness(5),
                HasShadow = false
            };
            Graduaciones.Children.Add(frame);
            Grid.SetRow(frame, 0);
            Grid.SetColumn(frame, col);
        }

        // Celdas vacías
        for (int row = 1; row <= rowCount; row++)
        {
            for (int col = 1; col <= rowCount; col++)
            {
                var cellLabel = new Button
                {
                    
                    BackgroundColor = Colors.White,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,                   
                    
                };

                if (App.Current.Resources.TryGetValue("Boton", out var style))
                {
                    cellLabel.Style = (Style)style;
                }

                cellLabel.Clicked += (s, e) => Button_Clicked(s, e, row, col, minGraduacion, incremento);
               
                var frame = new Frame
                {
                    BorderColor = Colors.Black,
                    BackgroundColor = Colors.White,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Content = cellLabel,
                    Padding = 0,
                    Margin = new Thickness(5),
                    HasShadow = false
                };
                Graduaciones.Children.Add(frame);
                Grid.SetRow(frame, row);
                Grid.SetColumn(frame, col);
            }
        }
    }


    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        BtnCancelar.Opacity = 0;
        await BtnCancelar.FadeTo(1, 200);

        await Shell.Current.Navigation.PopAsync();

    }

    private void BtnGuardar_Clicked(object sender, EventArgs e)
    {

    }

    private void GraduacionesChecked_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void MicasSeleccionadas_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private async void Button_Clicked (Object sender, EventArgs e, int row, int col, double minGraduacion, double incremento)
    {

        double sphereValue = minGraduacion + (row - 1) * incremento;
        double cylinderValue = minGraduacion + (col - 1) * incremento;

        try
        {
            var button = (Button)sender;
            if (button != null)
            {
                var popup = new GetDatosPopup(button);

                await this.ShowPopupAsync(popup);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }

    }

    private void DatosGuardados_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void MinGraduacion_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void MaxGraduacion_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    
}