using Application.ViewModels;
using CommunityToolkit.Maui.Views;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Domain.Entities;
using TechLens.Presentacion.Events;
using TechLens.Presentacion.Views.Popups;

namespace TechLens.Presentacion.Views.Captura;

public partial class GraduacionMica : ContentPage
{
    public VMTablaGraduaciones ViewModel;
    public event EventHandler<GraduacionesSelectedEventArgs>? GraduacionesSelected;
    public GraduacionMica(VMTablaGraduaciones viewModel)
    {
        InitializeComponent();

        ViewModel = viewModel;

        this.BindingContext = ViewModel;
    }

    public GraduacionMica() : this(MauiProgram.ServiceProvider.GetRequiredService<VMTablaGraduaciones>())
    {
    }

    public GraduacionMica(Mica mica, Lote lote) : this()
    {
        ViewModel.Lote = lote;
        ViewModel.Mica = mica;
    }

    public GraduacionMica(Mica mica, Pedido pedido) : this()
    {
        ViewModel.Pedido = pedido;
        ViewModel.Mica = mica;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (ViewModel.Lote is null && ViewModel.Pedido is null || ViewModel.Mica is null)
        {
            await DisplayAlert("Error", "No se ha asignado un lote o pedido", "Ok");
            await Navigation.PopAsync();
        }
        if(ViewModel.Lote is not null)
        {
        }
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
                await TablaDeGraduaciones(minGraduacion, maxGraduacion);
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

    private async Task TablaDeGraduaciones(double minGraduacion, double maxGraduacion)
    {
        await Task.Run(() =>
        {
            double incremento = 0.25;
            int rowCount = (int)((maxGraduacion - minGraduacion) / incremento) + 1;

            // Definición de datos y preparación de elementos en segundo plano
            var rows = new List<RowDefinition>();
            var columns = new List<ColumnDefinition>();
            var framesToAdd = new List<(Frame frame, int row, int col)>();


            for (int i = 0; i <= rowCount; i++)
            {
                rows.Add(new RowDefinition { Height = new GridLength(40) });
                columns.Add(new ColumnDefinition { Width = new GridLength(60) });
            }

            // Crear encabezado principal
            var encabezadoTabla = new Label
            {
                Text = "ESF / CIL",
                BackgroundColor = Colors.White,
                TextColor = Colors.Black,
                FontSize = 10,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            framesToAdd.Add((new Frame
            {
                BorderColor = Colors.Black,
                Content = encabezadoTabla,
                Padding = 0,
                Margin = new Thickness(5),
                HasShadow = false
            }, 0, 0));

            // Generar las filas de esferas
            for (int row = 1; row <= rowCount; row++)
            {
                double sphereValue = minGraduacion + (row - 1) * incremento;
                var sphereLabel = new Label
                {
                    Text = $"{sphereValue:N2}",
                    TextColor = Colors.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Padding = 5
                };

                if (App.Current.Resources.TryGetValue("SubTier", out var SubTierResource) && SubTierResource is Color SubTier)
                {
                    sphereLabel.BackgroundColor = SubTier;
                }

                framesToAdd.Add((new Frame
                {
                    BorderColor = Colors.White,
                    Content = sphereLabel,
                    Padding = 0,
                    Margin = new Thickness(5),
                    HasShadow = false
                }, row, 0));
            }

            // Generar las columnas de cilindros
            for (int col = 1; col <= rowCount; col++)
            {
                double cylinderValue = minGraduacion + (col - 1) * incremento;
                var cylinderLabel = new Label
                {
                    Text = $"{cylinderValue:N2}",
                    TextColor = Colors.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Padding = 5
                };

                if (App.Current.Resources.TryGetValue("SubTier", out var SubTierResource) && SubTierResource is Color SubTier)
                {
                    cylinderLabel.BackgroundColor = SubTier;
                }

                framesToAdd.Add((new Frame
                {
                    BorderColor = Colors.White,
                    Content = cylinderLabel,
                    Padding = 0,
                    Margin = new Thickness(5),
                    HasShadow = false
                }, 0, col));
            }

            // Generar las celdas vacías
            for (int row = 1; row <= rowCount; row++)
            {
                for (int col = 1; col <= rowCount; col++)
                {
                    var cellButton = new Button
                    {
                        BackgroundColor = Colors.White,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,

                    };

                    
                    if (App.Current.Resources.TryGetValue("BotonTabla", out var style))
                    {
                        cellButton.Style = (Style)style;
                        cellButton.CornerRadius = 0;
                    }

                    int capturedRow = row, capturedCol = col; // Capturar variables para usar en el evento
                    cellButton.Clicked += (s, e) => Button_Clicked(s, e, capturedRow, capturedCol, minGraduacion, incremento);

                    framesToAdd.Add((new Frame
                    {
                        BorderColor = Colors.Black,
                        BackgroundColor = Colors.White,
                        VerticalOptions = LayoutOptions.Fill,
                        HorizontalOptions = LayoutOptions.Fill,
                        Content = cellButton,
                        Padding = 0,
                        Margin = new Thickness (5),
                        HasShadow = false
                    }, row, col));
                }
            }

            // Vuelve al hilo principal para actualizar la UI
            this.Dispatcher.Dispatch(() =>
            {
                Graduaciones.RowDefinitions.Clear();
                Graduaciones.ColumnDefinitions.Clear();
                Graduaciones.Children.Clear();

                foreach (var row in rows)
                    Graduaciones.RowDefinitions.Add(row);

                foreach (var col in columns)
                    Graduaciones.ColumnDefinitions.Add(col);

                foreach (var (frame, row, col) in framesToAdd)
                {
                    Graduaciones.Children.Add(frame);
                    Grid.SetRow(frame, row);
                    Grid.SetColumn(frame, col);
                }
            });
        });
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
        try
        {
            if (ViewModel.Lote != null)
            {
                var lotesMicas = new List<LoteMica>();
                foreach (var item in ViewModel.MicasGraduacion)
                {
                    if (item.LoteMica is not null)
                    {
                        lotesMicas.Add(item.LoteMica);
                    }
                    else
                        throw new Exception("Hubo un error Obteniendo la relacion Lote/Mica, contacte a su administrador");
                }

                GraduacionesSelected?.Invoke(this, new GraduacionesSelectedEventArgs { GraduacionesLoteSelected = lotesMicas });
            }
            else if (ViewModel.Pedido != null)
            {
                var pedidosMicas = new List<PedidoMica>();
                foreach (var item in ViewModel.MicasGraduacion)
                {
                    if (item.PedidoMica is not null)
                    {
                        pedidosMicas.Add(item.PedidoMica);
                    }
                    else
                        throw new Exception("Hubo un error Obteniendo la relacion Pedido/Mica, contacte a su administrador");
                }

                GraduacionesSelected?.Invoke(this, new GraduacionesSelectedEventArgs { GraduacionesPedidoMicaSelected = pedidosMicas });
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void AddMicaCapturedToList(object? sender, MicaDataSelectedEventArgs e)
    {
        try
        {
            if (e.MicaGraduacionCaptured is not null)
            {
                //si ya existe, no mostrarla doble en la lista
                if (ViewModel.MicasGraduacion.Any(m => m.MicaGraduacion.Graduacioncil == e.MicaGraduacionCaptured.Graduacioncil
                                                         && m.MicaGraduacion.Graduacionesf == e.MicaGraduacionCaptured.Graduacionesf))
                    return;

                await ViewModel.AddSelectedMicaGraduacion(e.MicaGraduacionCaptured, e.Cantidad);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
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
                var popup = new GetDatosPopup(button, sphereValue, cylinderValue, ViewModel.Mica);

                popup.MicaDataSelected += AddMicaCapturedToList;

                await this.ShowPopupAsync(popup);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void BtnEliminarGraduacion_Clicked(object sender, EventArgs e)
    {
        //BtnEliminarGraduacion.Opacity = 0;
        //await BtnEliminarGraduacion.FadeTo(1, 200); 

    }
}