using System;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repos;
using Microsoft.EntityFrameworkCore;

namespace Testing.Injection_lol;

public class Injection
{
    public readonly OpticaDbContext DbContext;
    public readonly IUsuarioRepo UsuarioRepo;
    public readonly ILoteMicaRepo LoteMicaRepo;
    public readonly ILoteRepo LoteRepo;
    public readonly IMicaGraduacionRepo MicaGraduacionRepo;
    public readonly IMicaRepo MicaRepo;
    public readonly IPedidoMicaRepo PedidoMicaRepo;
    public readonly IPedidoRepo PedidoRepo;

    public Injection()
    {
        DbContextOptions<OpticaDbContext> options = new DbContextOptionsBuilder<OpticaDbContext>()
                                                    .UseNpgsql("Host=localhost;Database=techlens;Username=postgres;Password=Isee420.69&hear")
                                                    .Options;

        DbContext = new OpticaDbContext(options);

        UsuarioRepo = new UsuarioRepo(DbContext);
        LoteMicaRepo = new LoteMicaRepo(DbContext);
        MicaGraduacionRepo = new MicaGraduacionRepo(DbContext);
        LoteRepo = new LoteRepo(DbContext, LoteMicaRepo, MicaGraduacionRepo);
        PedidoMicaRepo = new PedidoMicaRepo(DbContext, LoteMicaRepo);
        MicaRepo = new MicaRepo(DbContext, LoteMicaRepo, PedidoMicaRepo, MicaGraduacionRepo);
        PedidoRepo = new PedidoRepo(DbContext, PedidoMicaRepo, UsuarioRepo, MicaRepo, MicaGraduacionRepo, LoteMicaRepo, LoteRepo);
    }

    //tesst the db connection
    public async Task<bool> TestConnection()
    {
        try
        {
            await DbContext.Database.OpenConnectionAsync();
            await DbContext.Database.CloseConnectionAsync();
            return true;
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error al conectar a la base de datos: {e.Message}.");
            return false;
        }
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}
