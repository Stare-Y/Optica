// See https://aka.ms/new-console-template for more information

using Domain.Entities;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

DbContextOptions<DbContext> options = new DbContextOptionsBuilder<DbContext>()
    .UseNpgsql("Host=localhost;Database=techlens;Username=postgres;Password=Isee420.69&hear")
    .Options;

var dbContext = new OpticaDbContext(options);
try
{
    dbContext.Micas.Add(new Mica
    {
        Id = 1,
        Tipo = "Monofocal",
        Fabricante = "Zeiss",
        Material = "CR-39",
        GraduacionESF = (float)-2.5,
        GraduacionCIL = (float)-1.5,
        Tratamiento = "Antirreflejante"
    });
    dbContext.SaveChanges();
    Console.WriteLine("Mica agregada");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}



