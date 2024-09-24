using Application.Services;
using Application.ViewModels;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TechLens
{
    public static class MauiProgram
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SofiaSans-Regular.ttf", "SofiaSans");
                    fonts.AddFont("Montserrat-Regular.ttf", "Montserrat");
                });

            //logger setup

            string logFilePath = "C:\\Stare-y\\TechLens\\log.txt";

            string directoryPath = Path.GetDirectoryName(logFilePath);

            // Crea el directorio si no existe
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

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
                options.UseNpgsql("Host=localhost;Database=techlens;Username=admin;Password=staremedic1");
            });

            builder.Services.AddSingleton<IMicaGraduacionRepo, MicaGraduacionRepo>();
            builder.Services.AddSingleton<ILoteMicaRepo, LoteMicaRepo>();
            builder.Services.AddSingleton<IMicaRepo, MicaRepo>();
            builder.Services.AddSingleton<ILoteRepo, LoteRepo>();
            builder.Services.AddSingleton<IPedidoMicaRepo, PedidoMicaRepo>();
            builder.Services.AddSingleton<IPedidoRepo, PedidoRepo>();
            builder.Services.AddSingleton<IUsuarioRepo, UsuarioRepo>();

            builder.Services.AddTransient<ViewModelCapturas>();
            builder.Services.AddTransient<ViewModelMainPage>();
        }
    }
}
