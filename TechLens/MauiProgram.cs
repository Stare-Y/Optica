using Application.Services;
using Application.ViewModels;
using Application.ViewModels.Lotes;
using CommunityToolkit.Maui;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace TechLens
{
    public static class MauiProgram
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SofiaSans-Regular.ttf", "SofiaSans");
                    fonts.AddFont("Montserrat-Regular.ttf", "Montserrat");
                });

            //logger setup
            string logFilePath = "";

#if WINDOWS
            logFilePath = "C:\\Stare-y\\TechLens\\log.txt";

            string? directoryPath = Path.GetDirectoryName(logFilePath);

            if(directoryPath != null && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            builder.ConfigureLifecycleEvents(events =>
            {
                // Make sure to add "using Microsoft.Maui.LifecycleEvents;" in the top of the file 
                events.AddWindows(windowsLifecycleBuilder =>
                {
                    windowsLifecycleBuilder.OnWindowCreated(window =>
                    {
                        window.ExtendsContentIntoTitleBar = false;
                        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);

                        if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter)
                        {
                            // Deshabilita la capacidad de redimensionar y maximizar
                            overlappedPresenter.IsResizable = false;
                            overlappedPresenter.IsMaximizable = false;
                            overlappedPresenter.IsMinimizable = true;
                            overlappedPresenter.Maximize();

                            // Configura el borde y la barra de título visibles
                            overlappedPresenter.SetBorderAndTitleBar(true, true);
                        }

                    });
                });
            });

           
#endif

#if ANDROID
            logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TechLens", "Logs", "TechLens.log");
#endif

            var logger = new Logger(logFilePath);
            builder.Services.AddSingleton<Domain.Interfaces.Services.ILogger>(provider => logger);

            try
            {
                IServiceCollection services = new ServiceCollection();
                ConfigureServices(services);
                ServiceProvider = services.BuildServiceProvider();

            }
            catch (Exception ex)
            {
                logger.Log("Error iniciando el programa: " + ex.Message + "Inner: " + ex.InnerException);
            }


#if DEBUG
            builder.Logging.AddDebug();
#endif


            return builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<OpticaDbContext>(options =>
            {
                options.UseNpgsql("Host=stare-y.com;Database=techlens;Username=techlens;Password=staremedic1");
            });

            services.AddSingleton<IMicaGraduacionRepo, MicaGraduacionRepo>();
            services.AddSingleton<ILoteMicaRepo, LoteMicaRepo>();
            services.AddSingleton<IMicaRepo, MicaRepo>();
            services.AddSingleton<ILoteRepo, LoteRepo>();
            services.AddSingleton<IPedidoMicaRepo, PedidoMicaRepo>();
            services.AddSingleton<IPedidoRepo, PedidoRepo>();
            services.AddSingleton<IUsuarioRepo, UsuarioRepo>();
            services.AddSingleton<ViewModelMainPage>();

            services.AddTransient<ViewModelCapturas>();
            services.AddTransient<ViewModelMicas>();
            services.AddTransient<VMSeleccionMicas>();
            services.AddTransient<ViewModelReportes>();
            services.AddTransient<ViewModelUsuario>();
            services.AddTransient<ViewModelEditarUsuario>();
            services.AddTransient<VMLogin>();
            services.AddTransient<ViewModelCrearPedido>();
            services.AddTransient<VMSeleccionarMicasPedido>();
            services.AddTransient<VMTablaGraduaciones>();
            services.AddTransient<VMNuevaMica>();
            services.AddTransient<VMConsultarStockMica>();
            services.AddTransient<VMLotesView>();
            services.AddTransient<VMDisplayLotes>();
            services.AddTransient<VMLoteRelationsDetails>();
        }
    }
}
