using Application.ViewModels;
using CommunityToolkit.Maui.Views;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Domain.Entities;
using Domain.Interfaces.Services.DisplayEntities;
using Microsoft.UI.Xaml.Input;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TechLens.Presentacion.Events;
using TechLens.Presentacion.Views.Popups;
using Windows.System;

namespace TechLens.Presentacion.Views.Lotes;

public partial class GraduacionMica : ContentPage
{
    public VMTablaGraduaciones ViewModel;
    public event EventHandler<GraduacionesSelectedEventArgs>? GraduacionesSelected;

    private int _minGraduacion;
    private int _maxGraduacion;

    bool _presettedTable = false;

    private int _loteOrigen;

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

    public GraduacionMica(Mica mica, Pedido pedido, int loteOrigen) : this()
    {
        ViewModel.Pedido = pedido;
        ViewModel.Mica = mica;
        _loteOrigen = loteOrigen;
        TableGeneratorBorder.IsVisible = false;
        TableGeneratorContainer.IsVisible = false;
    }

    public async Task PreparePedido()
    {
        await ViewModel.PrepareLotesMicas(ViewModel.Mica.Id, _loteOrigen);

        //get min graduacion from viewmodel.micasgraduacionlist of graduacion esf and graduacion cil, and the max from graduacion esf and cil
        var minGraduacionEsf = ViewModel.MicasGraduacionList.Min(x => x.Graduacionesf);
        var minGraduacionCil = ViewModel.MicasGraduacionList.Min(x => x.Graduacioncil);

        float minGraduacion = minGraduacionEsf < minGraduacionCil ? minGraduacionEsf : minGraduacionCil;

        var maxGraduacionEsf = ViewModel.MicasGraduacionList.Max(x => x.Graduacionesf);
        var maxGraduacionCil = ViewModel.MicasGraduacionList.Max(x => x.Graduacioncil);

        float maxGraduacion = maxGraduacionEsf > maxGraduacionCil ? maxGraduacionEsf : maxGraduacionCil;

        _minGraduacion = (int)minGraduacion;
        _maxGraduacion = (int)maxGraduacion;

        _presettedTable = true;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (ViewModel.Lote is null && ViewModel.Pedido is null || ViewModel.Mica is null)
        {
            await DisplayAlert("Error", "No se ha asignado un lote o pedido", "Ok");
            await Navigation.PopAsync();
        }

        if (_presettedTable)
        {
            await TablaDeGraduaciones(_minGraduacion, _maxGraduacion);
            _presettedTable = false;
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

            var rows = new List<RowDefinition>();
            var columns = new List<ColumnDefinition>();
            var framesToAdd = new List<(Frame frame, int row, int col)>();

            for (int i = 0; i <= rowCount; i++)
            {
                rows.Add(new RowDefinition { Height = new GridLength(60) });
                columns.Add(new ColumnDefinition { Width = new GridLength(80) });
            }

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

            for (int row = 1; row <= rowCount; row++)
            {
                for (int col = 1; col <= rowCount; col++)
                {
                    double sphereValue = minGraduacion + (row - 1) * incremento;
                    double cylinderValue = minGraduacion + (col - 1) * incremento;

                    View cellContent;

                    MicaGraduacion? existingGraduation = null;

                    if(ViewModel.MicasGraduacionList is not null)
                    {
                        existingGraduation = ViewModel.MicasGraduacionList
                        .FirstOrDefault(x => x.Graduacionesf == sphereValue && x.Graduacioncil == cylinderValue);
                    }

                    if (existingGraduation is not null && ViewModel.Pedido is not null)
                    {
                        var existingGraduationLote = ViewModel.LotesMicas
                            .FirstOrDefault(x => x.IdMicaGraduacion == existingGraduation.Id);

                        var stockLabel = new Label
                        {
                            Text = existingGraduationLote?.Cantidad.ToString() ?? "0",
                            BackgroundColor = Colors.LightGray,
                            TextColor = Colors.Black,
                            Padding = new Thickness(0),
                            FontSize = 14,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalOptions = LayoutOptions.Fill,
                            VerticalOptions = LayoutOptions.Fill,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center
                        };

                        var stockEntry = new Entry
                        {
                            BackgroundColor = Colors.White,
                            TextColor = Color.FromArgb("#525CEB"),
                            FontSize = 12,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalOptions = LayoutOptions.Fill,
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Start
                        };

                        stockEntry.TextChanged += (s, e) => TextChanged_Event(s, e, sphereValue, cylinderValue);

                        var innerGrid = new Grid
                        {
                            RowDefinitions =
                        {
                            new RowDefinition { Height = GridLength.Star },
                            new RowDefinition { Height = GridLength.Star }
                        }
                        };
                        innerGrid.Children.Add(stockLabel);
                        Grid.SetRow(stockLabel, 0);
                        innerGrid.Children.Add(stockEntry);
                        Grid.SetRow(stockEntry, 1);

                        cellContent = innerGrid;
                    }
                    else
                    {
                        var emptyEntry = new Entry
                        {
                            BackgroundColor = Colors.White,
                            TextColor = Colors.Black,
                            FontSize = 15,
                            HorizontalOptions = LayoutOptions.Fill,
                            VerticalOptions = LayoutOptions.Fill,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center,
                            FontAttributes = FontAttributes.Bold
                        };

                        emptyEntry.TextChanged += (s, e) => TextChanged_Event(s, e, sphereValue, cylinderValue);

                        cellContent = emptyEntry;
                    }

                    framesToAdd.Add((new Frame
                    {
                        BorderColor = Colors.Black,
                        BackgroundColor = Colors.White,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        HeightRequest = 50,
                        WidthRequest = 70,
                        Content = cellContent,
                        Padding = 0,
                        Margin = new Thickness(5),
                        HasShadow = false
                    }, row, col));
                }
            }

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
            if (ViewModel.MicasGraduacion.Count == 0)
            {
                await DisplayAlert("Error", "No se ha seleccionado ninguna graduacion", "Aceptar");
                await Shell.Current.Navigation.PopAsync();
                return;
            }

            await ViewModel.EnsureGraduacionesExist();

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
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void TextChanged_Event(object? sender, EventArgs e, double sphereValue, double cylinderValue)
    {


        try
        {
            if (sender is Entry entry)
            {
                if (int.TryParse(entry.Text, out int cantidad) && cantidad > 0)
                {
                    var caputredGraduacionObj = new MicaGraduacion
                    {
                        IdMica = ViewModel.Mica.Id,
                        Graduacionesf = (float)sphereValue,
                        Graduacioncil = (float)cylinderValue
                    };

                    await ViewModel.AddSelectedMicaGraduacion(caputredGraduacionObj, cantidad);
                }
                else
                {
                    entry.Text = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void BtnEliminarGraduacion_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.BindingContext is DisplayMicaGraduacionAndDetails graduacionMostrada)
            {
                ViewModel.MicasGraduacion.Remove(graduacionMostrada);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }

    private void MinGraduacion_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(MinGraduacion.Text))
            return;

        string newText = MinGraduacion.Text;

        if (Regex.IsMatch(newText, @"^-?\d*$"))
        {
            if (int.TryParse(newText, out int minGraduacion))
            {
                _minGraduacion = minGraduacion;
            }
        }
        else
        {
            MinGraduacion.Text = e.OldTextValue;
        }
    }

    private void MaxGraduacion_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(MaxGraduacion.Text))
            return;

        string newText = MaxGraduacion.Text;

        if (Regex.IsMatch(newText, @"^-?\d*$"))
        {
            if (int.TryParse(newText, out int maxGraduacion))
            {
                _maxGraduacion = maxGraduacion;
            }
        }
        else
        {
            MaxGraduacion.Text = e.OldTextValue;
        }
    }

}