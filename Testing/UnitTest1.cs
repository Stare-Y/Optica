using Domain.Entities;
using Testing.Injection_lol;

namespace Testing;

public class Tests
{
    private Injection _injection = null!;
    private Usuario _testUser = new Usuario
                                {
                                    NombreDeUsuario = "test",
                                    Password = "test",
                                    Rol = "test"
                                };
    private Mica _testMica = new Mica
                                {
                                    Tipo = "Test Mica",
                                    Fabricante = "Test Fabricante",
                                    Material = "test Material",
                                    Tratamiento = "Test Tratamiento",
                                    Proposito = "Test Proposito"
                                };
    private Lote _testLote = null!;
    private Pedido _testPedido = null!;
    private List<LoteMica> _lotesMicas = new();
    private List<MicaGraduacion> _micasGraduaciones = new();
    

    public Tests()
    {
        _injection = new Injection();
    }

    [SetUp]
    public void Setup()
    {

        Thread.Sleep(1000);
    }

    [Test]
    public async Task aRectifyTestEnviroment()
    {
        bool canConnect = await _injection.TestConnection();

        if (!canConnect)
        {
            Assert.Fail();
        }

        //search pedido test
        Pedido? garbagePedido = await _injection.PedidoRepo.GetPedidoByRazonSocial("Test Razon Social");

        if (garbagePedido != null)
        {
            Assert.That(garbagePedido.RazonSocial, Is.EqualTo("Test Razon Social"));
            await _injection.PedidoRepo.DeletePedido(garbagePedido.Id);
        }


        //search lote test
        Lote? garbageLote = await _injection.LoteRepo.GetLoteByReferencia("Test Lote");

        if (garbageLote != null)
            await _injection.LoteRepo.DeleteLote(garbageLote.Id);


        //if mica test exists, delete it, because it is from a previous iteration
        Mica? garbageMica = await _injection.MicaRepo.GetMicaByTipoTEST("Test Mica");
        if (garbageMica != null)
            await _injection.MicaRepo.DeleteMica(garbageMica.Id);

        //if user test exists, delete it, because it is from a previous iteration
        try
        {
            Usuario garbageUsuario = await _injection.UsuarioRepo.GetUsuarioByNombreDeUsuario("test");
            await _injection.UsuarioRepo.DeleteUsuario(garbageUsuario.Id);
        }
        catch
        {
            //do nothing, just continue
        }

        Assert.Pass();
    }

    [Test]
    public async Task bCanUserCRUD()
    {
        #region Create

        _testUser = await _injection.UsuarioRepo.AddUsuario(_testUser);

        Assert.That(_testUser.Id, Is.Not.EqualTo(0));

        #endregion

        #region Read

        Usuario usuarioLeido = await _injection.UsuarioRepo.GetUsuarioById(_testUser.Id);

        Assert.That(usuarioLeido.NombreDeUsuario, Is.EqualTo(_testUser.NombreDeUsuario));

        #endregion

        #region Update

        usuarioLeido.NombreDeUsuario = "admin";

        //this should actually throw an exception, if not throw fail
        try
        {
            await _injection.UsuarioRepo.UpdateUsuario(usuarioLeido);
            Assert.Fail();
        }
        catch
        {
        }

        //now, this should work
        usuarioLeido.NombreDeUsuario = "test69";

        await _injection.UsuarioRepo.UpdateUsuario(usuarioLeido);

        //take back
        await _injection.UsuarioRepo.UpdateUsuario(_testUser);

        #endregion

        Assert.Pass();
    }

    [Test]
    public async Task cCanUserAuth()
    {
        Usuario usuarioLeido = await _injection.UsuarioRepo.AutenticarUsuario("test", "test");

        Assert.That(usuarioLeido, Is.Not.Null);

        //this should throw an exception
        try{
            usuarioLeido = await _injection.UsuarioRepo.AutenticarUsuario("nonexistant", "nonexistant");
            Assert.Fail();
        }
        catch
        {
            Assert.Pass();
        }
    }

    [Test]
    public async Task dAddMicasUseCase()
    {

        _testMica = await _injection.MicaRepo.InsertMica(_testMica);

        Assert.That(_testMica.Id, Is.Not.EqualTo(0));

        Assert.Pass();
    }

    [Test]
    public async Task eAddMicaGraduacion()
    {
        _micasGraduaciones = new ([
            new MicaGraduacion
            {
                IdMica = _testMica.Id,
                Graduacionesf = 420.69f,
                Graduacioncil = 69.420f
            },
            new MicaGraduacion
            {
                IdMica = _testMica.Id,
                Graduacionesf = 45.69f,
                Graduacioncil = 66.420f
            },
            new MicaGraduacion
            {
                IdMica = _testMica.Id,
                Graduacionesf = 21.69f,
                Graduacioncil = 69.69f
            }
        ]);

        await _injection.MicaGraduacionRepo.InsertMicaGraduacion(_micasGraduaciones);
    }



    [Test]
    public async Task fAddNewLoteWithMicasUseCase()
    {
        //FISRST get lote info
        _testLote = new Lote
        {
            Referencia = "Test Lote",
            FechaEntrada = DateTime.Now.AddDays(30),
            IdUsuario = _testUser.Id
        };


        //SECOND get micas relation
        foreach (var micagraduacion in _micasGraduaciones)
        {
            _lotesMicas.Add(new LoteMica
            {
                IdLote = _testLote.Id,
                IdMicaGraduacion = micagraduacion.Id,
                Cantidad = 10
            });
        }

        _testLote.Existencias = _lotesMicas.Sum(lm => lm.Cantidad);

        //THIRD add lote with micas
        _testLote = await _injection.LoteRepo.AddLote(_testLote, _lotesMicas);

        Assert.That(_testLote.Id, Is.Not.EqualTo(0));

        Assert.That(_testLote.Existencias, Is.EqualTo(30));

        _lotesMicas = new (await _injection.LoteMicaRepo.GetLotesMicasByLoteIdAsync(_testLote.Id));

        Assert.That(_lotesMicas.Count, Is.EqualTo(3));

        foreach (var loteMica in _lotesMicas)
        {
            Assert.That(loteMica.Cantidad, Is.EqualTo(10));
        }

        Assert.Pass();
    }

    [Test]
    public async Task gGeneratePedidoUseCase()
    {
        //FIRST get pedido info
        Pedido pedido = new Pedido
        {
            IdUsuario = _testUser.Id,
            RazonSocial = "Test Razon Social",
            FechaSalida = DateTime.Now.AddDays(30)
        };

        //search lotes
        List<Lote> lotes = new (await _injection.LoteRepo.GetValidLotesAsync());

        //get micas from lote that contains id
        var targetLote = lotes.FirstOrDefault(lote => lote.IdUsuario == _testLote.IdUsuario);
        if (targetLote == null)
        {
            Assert.Fail();
            return;
        }

        //get lotesmicas from lote
        var lotesMicas = await _injection.LoteMicaRepo.GetLotesMicasByLoteIdAsync(targetLote.Id);

        //get micas from lotesmicas
        List<MicaGraduacion> micasGraduaciones = new (await _injection.MicaGraduacionRepo.GetMicaGraduacionesByMultipleIds(lotesMicas.Select(lm => lm.IdMicaGraduacion).ToList()));

        //get Micas to display, based on the lotesmicas cantidad, and the micasgraduaciones
        List<Mica> micas = new (await _injection.MicaRepo.GetMicasByIds(micasGraduaciones.Select(mg => mg.IdMica).ToList()));

        //make pedidosmicas
        List<PedidoMica> pedidoMicas = new ([
            new PedidoMica
            {
                IdPedido = pedido.Id,
                IdMicaGraduacion = micasGraduaciones[0].Id,
                Cantidad = 5,
                IdLoteOrigen = targetLote.Id
            },
            new PedidoMica
            {
                IdPedido = pedido.Id,
                IdMicaGraduacion = micasGraduaciones[1].Id,
                Cantidad = 5,
                IdLoteOrigen = targetLote.Id
            },
            new PedidoMica
            {
                IdPedido = pedido.Id,
                IdMicaGraduacion = micasGraduaciones[2].Id,
                Cantidad = 5,
                IdLoteOrigen = targetLote.Id
            }
        ]);

        //add pedido with micas
        _testPedido = await _injection.PedidoRepo.AddPedido(pedido, pedidoMicas);

        //validate pedido id
        Assert.That(_testPedido.Id, Is.Not.EqualTo(0));

        lotesMicas = await _injection.LoteMicaRepo.GetLotesMicasByLoteIdAsync(targetLote.Id);

        //confirm taken stock from lotemica
        foreach (var pedidoMica in pedidoMicas)
        {
            var loteMica = lotesMicas.FirstOrDefault(lm => lm.IdMicaGraduacion == pedidoMica.IdMicaGraduacion);

            if (loteMica == null)
            {
                Assert.Fail();
                return;
            }
            
            Assert.That(pedidoMica.Cantidad, Is.EqualTo(5));
            Assert.That(loteMica.Cantidad, Is.EqualTo(5));
        }

        var validateLote = await _injection.LoteRepo.GetLote(targetLote.Id);

        if (validateLote == null)
        {
            Assert.Fail();
            return;
        }

        Assert.That(validateLote.Existencias, Is.EqualTo(15));

        Assert.Pass();
    }

    [Test]
    public async Task hCleanUp()
    {
        #region Delete Pedido

        //search pedido test
        Pedido? garbagePedido = await _injection.PedidoRepo.GetPedidoByRazonSocial("Test Razon Social");

        if (garbagePedido != null)
        {
            Assert.That(garbagePedido.RazonSocial, Is.EqualTo("Test Razon Social"), "Found garbage pedido is not the one we are looking for");
            await _injection.PedidoRepo.DeletePedido(garbagePedido.Id);
        }
        else
        {
            Console.WriteLine("Test pedido not found on hCleanUp");
            Assert.Fail();
        }
        
        //make sur it returns null
        var deletedPedido = await _injection.PedidoRepo.GetPedido(_testPedido.Id);
        if (deletedPedido != null)
        {
            Console.WriteLine($"Test pedido not deleted on hCleanUp. {deletedPedido.ToString()}");
            Assert.Fail();
        }

        #endregion

        #region Delete Lote

        //make sure that lotemica have 30 existencias
        Lote? garbageLote = await _injection.LoteRepo.GetLoteByReferencia("Test Lote");

        if (garbageLote != null)
        {
            Assert.That(garbageLote.Existencias, Is.EqualTo(30));
            await _injection.LoteRepo.DeleteLote(garbageLote.Id);

            //make sure it returns null
            var deletedLote = await _injection.LoteRepo.GetLote(garbageLote.Id);
            if (deletedLote != null)
            {
                Console.WriteLine("Test lote not deleted on hCleanUp");
                Assert.Fail();
            }
        }
        else
        {
            Console.WriteLine("Test lote not found on hCleanUp");
            Assert.Fail();
        }

        #endregion

        #region Delete Mica

        await _injection.MicaRepo.DeleteMica(_testMica.Id);

        try
        {
            var deletedMica = await _injection.MicaRepo.GetMica(_testMica.Id);
            Console.WriteLine("Test mica not deleted on hCleanUp");
            Assert.Fail();
        }
        catch
        {
            Console.WriteLine("Test mica deleted on hCleanUp");
        }

        #endregion

        #region LAST Delete User

        await _injection.UsuarioRepo.DeleteUsuario(_testUser.Id);

        try
        {
            _testUser = await _injection.UsuarioRepo.GetUsuarioById(_testUser.Id);
            Console.WriteLine("Test user not deleted on hCleanUp");
            Assert.Fail();
        }
        catch
        {
            Console.WriteLine("Test user deleted on hCleanUp");
        }

        #endregion

        Assert.Pass();
    }
}