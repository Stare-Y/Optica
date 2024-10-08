// See https://aka.ms/new-console-template for more information

using Domain.Entities;
using Domain.Interfaces.Services.Reportes.Entities;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repos;
using Microsoft.EntityFrameworkCore;

DbContextOptions<OpticaDbContext> options = new DbContextOptionsBuilder<OpticaDbContext>()
    .UseNpgsql("Host=localhost;Database=techlens;Username=admin;Password=staremedic1")
    .Options;


try
{
    #region Crear instancias de repositorios
    var dbContext = new OpticaDbContext(options);
    Console.WriteLine("Conexión exitosa");

    //Crear instancias de repositorios
    var loteMicaRepo = new LoteMicaRepo(dbContext);
    var micaGraduacionRepo = new MicaGraduacionRepo(dbContext);
    var loteRepo = new LoteRepo(dbContext, loteMicaRepo, micaGraduacionRepo); // depende de lotemicarepo

    var pedidoMicaRepo = new PedidoMicaRepo(dbContext, loteMicaRepo); // depende de loteMicaRepo
    var usuariosRepo = new UsuarioRepo(dbContext);
    var micaRepo = new MicaRepo(dbContext, loteMicaRepo, pedidoMicaRepo, micaGraduacionRepo); //depende de loteMicaRepo y pedidoMicaRepo

    var pedidoRepo = new PedidoRepo(dbContext, pedidoMicaRepo, usuariosRepo, micaRepo,micaGraduacionRepo); // depende de pedidoMicaRepo



    Console.WriteLine("Repositorios creados\n");

    #endregion

    //passed
    #region Agregar usuario

    //var usuario = new Usuario
    //{
    //    Id = 1,
    //    NombreDeUsuario = "admin",
    //    Password = "admin",
    //    Rol = "admin"
    //};

    //usuario = await usuariosRepo.AddUsuario(usuario);

    //Console.WriteLine($"Usuario creado con id: {usuario.Id}");

    #endregion
    //passed
    #region Actualizar Usuario

    ////var usuario = new Usuario
    ////{
    ////    Id = 1,
    ////    NombreDeUsuario = "admin",
    ////    Password = "Admin",
    ////    Rol = "admin"
    ////};          USUARIO ANTIGUO

    //var usuario = new Usuario
    //{
    //    Id = 1,
    //    NombreDeUsuario = "admin",
    //    Password = "staremedic1",
    //    Rol = "admin"
    //};//usuario nuevo

    //await usuariosRepo.UpdateUsuario(usuario);

    //Console.WriteLine($"Usuario {usuario.NombreDeUsuario} actualizado (password)");

    #endregion
    //passed
    #region Validar para iniciar sesion
    //si el usuario y contraseña son correctos, se inicia sesion
    //try
    //{
    //    var usuario = await usuariosRepo.AutenticarUsuario("admin", "staremedic1");
    //    Console.WriteLine($"Usuario {usuario.NombreDeUsuario} autenticado correctamente");
    //}
    //catch (Exception e)
    //{
    //    Console.WriteLine("Error: " + e.Message);
    //}
    //finally
    //{
    //    Console.WriteLine("Comportamiento esperado: Usuario no encontrado\n");
    //}


    //try
    //{
    //    var usuario = await usuariosRepo.AutenticarUsuario("admin", "admin");
    //    Console.WriteLine($"Usuario {usuario.NombreDeUsuario} autenticado correctamente");
    //}
    //catch (Exception e)
    //{
    //    Console.WriteLine("Error: " + e.Message);
    //}
    //finally
    //{
    //    Console.WriteLine("Comportamiento esperado: Usuario encontrado\n");
    //}
    #endregion
    //passed
    #region Eliminar Usuario

    //try
    //{
    //    var usuario = await usuariosRepo.DeleteUsuario(1);
    //    Console.WriteLine($"Usuario {usuario.NombreDeUsuario} eliminado exitosamente");
    //}
    //catch (Exception e)
    //{
    //    Console.WriteLine("Error: " + e.Message);
    //}
    //finally
    //{
    //    Console.WriteLine("Comportamiento esperado: Usuario eliminado exitosamente\n");
    //}

    #endregion
    //passed
    #region Agregar Mica y MicaGraduacion
    //try
    //{
    //    var mica = new Mica
    //    {
    //        Tipo = "Antireflejante",
    //        Fabricante = "Uwu",
    //        Material = "Fierro",

    //        Tratamiento = "insano",
    //        Proposito = "transition"
    //    };

    //    var micagraduacion = new MicaGraduacion
    //    {
    //        IdMica = mica.Id,
    //        Graduacionesf = 2.1f,
    //        Graduacioncil = 1.5f,
    //        Precio = 100
    //    };
    //    var listamicagraduacion = new List<MicaGraduacion>
    //    {
    //        micagraduacion
    //    };

    //    //NOTA, si los detalles de la mica ya existen, no se puede crear
    //    var micaGuardada = await micaRepo.AddMica(mica, listamicagraduacion);

    //    Console.WriteLine($"Mica guardada con id: {micaGuardada.Id}");
    //}
    //catch (Exception e)
    //{
    //    Console.WriteLine("Error: " + e.Message);
    //}
    //finally
    //{
    //    Console.WriteLine("Comportamiento esperado: Mica guardada\n");
    //}

    #endregion
    //passed
    #region Caso de Uso: Agregar Lote
    //try
    //{
    //    var mica = await micaRepo.GetMica(1);
    //    Console.WriteLine("Mica obtenida");

    //    var lote = new Lote
    //    {
    //        Referencia = "Lote de prueba use case add lote",
    //        Extra1 = "Extra1",
    //        Extra2 = "Extra2",
    //        FechaEntrada = DateTime.Now,
    //        Proveedor = "Proveedor de Prueba",
    //        FechaCaducidad = DateTime.Now.AddDays(30)
    //    };

    //    var listaLoteMica = new List<LoteMica>
    //    {
    //        new LoteMica
    //        {
    //            IdLote = lote.Id,
    //            IdMicaGraduacion = 2,
    //            Stock = 100,
    //            FechaCaducidad = DateTime.Now.AddDays(30)
    //        },

    //    };

    //    var loteRegistrado = await loteRepo.AddLote(lote, listaLoteMica);

    //    Console.WriteLine("Lote guardado con id: " + loteRegistrado.Id);

    //}
    //catch (Exception e)
    //{
    //    Console.WriteLine("Error: " + e.Message);
    //}
    //finally
    //{
    //    Console.WriteLine("Comportamiento esperado: Lote guardado\n");

    //}
    //var stockMica1 = await micaRepo.GetStock(1);
    //Console.WriteLine("\nStock de la mica con id 1: " + stockMica1);
    //var stockMica2 = await micaRepo.GetStock(2);
    //Console.WriteLine("\nStock de la mica con id 2: " + stockMica2);
    #endregion
    //pased
    #region Caso de Uso: Eliminar Lote

    //await loteRepo.DeleteLote(1);

    #endregion
    //passed
    #region Caso de Uso: Agregar Pedido

    var pedido = new Pedido
    {
        FechaSalida = DateTime.Now,
        IdUsuario = 1,
        RazonSocial = "Razon social de prueba"
    };

    var listaPedidoMica = new List<PedidoMica>
    {
        new PedidoMica
        {
            IdPedido = pedido.Id,
            IdMicaGraduacion = 1,
            Cantidad = 10
        },
        new PedidoMica
        {
            IdPedido = pedido.Id,
            IdMicaGraduacion = 2,
            Cantidad = 10
        }
    };

    var pedidoRegistrado = await pedidoRepo.AddPedido(pedido, listaPedidoMica);

    Console.WriteLine("\nPedido guardado con id: " + pedidoRegistrado.Id);
    var stockMica1 = await micaRepo.GetStock(1);
    Console.WriteLine("\nStock de la mica con id 1: " + stockMica1);
    var stockMica2 = await micaRepo.GetStock(2);
    Console.WriteLine("\nStock de la mica con id 2: " + stockMica2);

    #endregion
    //passed
    #region Caso de Uso: Eliminar Pedido

    //await pedidoRepo.DeletePedido(3);

    #endregion

    #region Caso de Uso: Generar Reporte

    //Console.WriteLine("Generando reporte...");
    //var reporte = await pedidoRepo.GenerarReporte(DateTime.Now.AddDays(-30), DateTime.Now);
    //float total = 0;
    //foreach (var fila in reporte)
    //{
    //    Console.WriteLine($"ID pedido: {fila.IdPedido} | Usuario: {fila.Usuario} | Fabricante: {fila.Fabricante} | " +
    //        $"Tratamiento: {fila.Tratamiento} | FechaSalida: {fila.FechaSalida} | Graduacion: ({fila.GraduacionEsferica},{fila.GraduacionCilindrica}) | " +
    //        $"Cantidad: {fila.Cantidad}");
    //    total += fila.Cantidad * fila.Precio;
    //}
    //Console.WriteLine($"Reporte generado, Total vendido: {total}\n");

    #endregion
}
catch (Exception e)
{
    Console.WriteLine("Carajo: " + e.Message + "Inner Exception: " + e.InnerException);
}



