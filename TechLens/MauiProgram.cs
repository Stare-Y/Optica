using Application.Services;
using Application.ViewModels;
using Domain.Interfaces;
using CommunityToolkit.Maui;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
#endif

#if ANDROID
            logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TechLens", "Logs", "TechLens.log");
#endif

            var logger = new Logger(logFilePath);
            builder.Services.AddSingleton<Domain.Interfaces.Services.ILogger>(provider => logger);

            try
            {
                ConfigureServices(builder);
            }
            catch (Exception ex)
            {
                logger.Log("Error iniciando el programa: " + ex.Message + "Inner: " + ex.InnerException);
            }

            ServiceProvider = builder.Services.BuildServiceProvider();

#if DEBUG
            builder.Logging.AddDebug();
#endif


            return builder.Build();
        }

        private static void ConfigureServices(MauiAppBuilder builder)
        {
            builder.Services.AddDbContext<OpticaDbContext>(options =>
            {
                options.UseNpgsql("Host=26.115.67.153;Database=techlens;Username=admin;Password=staremedic1");
            });

            builder.Services.AddSingleton<IMicaGraduacionRepo, MicaGraduacionRepo>();
            builder.Services.AddSingleton<ILoteMicaRepo, LoteMicaRepo>();
            builder.Services.AddSingleton<IMicaRepo, MicaRepo>();
            builder.Services.AddSingleton<ILoteRepo, LoteRepo>();
            builder.Services.AddSingleton<IPedidoMicaRepo, PedidoMicaRepo>();
            builder.Services.AddSingleton<IPedidoRepo, PedidoRepo>();
            builder.Services.AddSingleton<IUsuarioRepo, UsuarioRepo>();
            builder.Services.AddSingleton<ViewModelMainPage>();

            builder.Services.AddTransient<ViewModelCapturas>();
            builder.Services.AddTransient<ViewModelMicas>();
            builder.Services.AddTransient<VMSeleccionMicas>();
            builder.Services.AddTransient<ViewModelReportes>();
            builder.Services.AddTransient<ViewModelUsuario>();
            builder.Services.AddTransient<ViewModelEditarUsuario>();
            builder.Services.AddTransient<VMLogin>();
            builder.Services.AddTransient<ViewModelCrearPedido>();
            builder.Services.AddTransient<VMSeleccionarMicasPedido>();
        }
    }
}
