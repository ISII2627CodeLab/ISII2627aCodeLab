using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppForSEII.API.Data;

public class SeedData{
    public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger){
        List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        try { SeedRoles(roleManager, rolesNames); }
        catch (Exception ex) { logger.LogError(ex, "An error occurred seeding the roles in the Database."); }

        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        try { SeedUsers(userManager, rolesNames); }
        catch (Exception ex) { logger.LogError(ex, "An error occurred seeding the Users in the Database."); }

        // 1. Poner el Seed LicenciaModelo3D ANTES que el de Modelo3D (Cambio propuesto por el profesor)
        try { SeedLicenciasModelo3D(dbContext); }
        catch (Exception ex) { logger.LogError(ex, "An error occurred seeding the LicenciasModelo3D."); }

        try { SeedModelos3D(dbContext); }
        catch (Exception ex) { logger.LogError(ex, "An error occurred seeding the Modelos3D."); }

        try { SeedMaterialesYPiezas(dbContext); }
        catch (Exception ex) { logger.LogError(ex, "An error occurred seeding Materials and Piezas3D."); }

        // 2. Añadir Impresora3D a la inicialización (Cambio propuesto por el profesor)
        try { SeedImpresora3D(dbContext); }
        catch (Exception ex) { logger.LogError(ex, "An error occurred seeding Impresoras3D."); }

        try { SeedAccesorios(dbContext); }
        catch (Exception ex) { logger.LogError(ex, "An error occurred seeding Accesorios."); }

        // Operaciones dependientes de los clientes y las entidades anteriormente creadas
        try{
            var cliente = dbContext.Users.OfType<Cliente>().FirstOrDefault(u => u.UserName == "peter@uclm.es");
            if (cliente != null){
                SeedEncargoImpresion(dbContext, cliente);
                SeedReservaImpresora(dbContext, cliente);
                SeedCompraModelo3D(dbContext, cliente);
                SeedCompraAccesorios(dbContext, cliente);
            }
        }
        catch (Exception ex){
            logger.LogError(ex, "An error occurred seeding customer transactions (Encargos, Reservas, Compras).");
        }
    }

    public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles){
        foreach (string roleName in roles){
            if (!roleManager.RoleExistsAsync(roleName).Result){
                IdentityRole role = new IdentityRole { Name = roleName, NormalizedName = roleName };
                roleManager.CreateAsync(role).Wait();
            }
        }
    }

    public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles){
        if (userManager.FindByNameAsync("elena@uclm.es").Result == null){
            Cliente cliente = new Cliente("1", "Elena", "Navarro Martínez", "elena@uclm.es", "Calle Cervantes 1") { EmailConfirmed = true };
            var result = userManager.CreateAsync(cliente, "Password1234%").Result;
            if (result.Succeeded) userManager.AddToRoleAsync(cliente, roles[0]).Wait();
        }

        if (userManager.FindByNameAsync("peter@uclm.es").Result == null){
            Cliente cliente = new Cliente("3", "Peter", "Jackson", "peter@uclm.es", "Calle Dulcinea 2") { EmailConfirmed = true };
            var result = userManager.CreateAsync(cliente, "OtherPass12$").Result;
            if (result.Succeeded) userManager.AddToRoleAsync(cliente, roles[2]).Wait();
        }
    }

// 1. Poner el Seed LicenciaModelo3D ANTES que el de Modelo3D (Cambio propuesto por el profesor)
    public static void SeedLicenciasModelo3D(ApplicationDbContext dbContext){
        // 4. Hacer == a un nombre en el if y no usar id fijos (Cambio propuesto por el profesor)
        if (dbContext.LicenciasModelo3D.FirstOrDefault(l => l.Nombre == "Licencia Estándar") == null){
            // 4. No declarar el id manualmente
            dbContext.LicenciasModelo3D.Add(new LicenciaModelo3D("Licencia Estándar", DateTime.Now.AddYears(1)));
            dbContext.SaveChanges();
        }
    }

    public static void SeedModelos3D(ApplicationDbContext dbContext){
        // 4. Hacer == a un nombre en el if y no usar id fijos (Cambio propuesto por el profesor)
        if (dbContext.Modelos3D.FirstOrDefault(m => m.Nombre == "Robot") == null){
            var licencia = dbContext.LicenciasModelo3D.FirstOrDefault(l => l.Nombre == "Licencia Estándar");
            if (licencia == null) return; 

            // 4. No declarar el id manualmente
            dbContext.Modelos3D.AddRange(
                new Modelo3D("Robot", "Personajes", FormatoModelo3D.OBJ, 15m, licencia),
                new Modelo3D("Castillo", "Arquitectura", FormatoModelo3D.STL, 20m, licencia),
                new Modelo3D("Nave Espacial", "Vehículos", FormatoModelo3D.TresMF, 30.0m, licencia)
            );
            
            // 3. Añadir SaveChanges en Modelos3D
            dbContext.SaveChanges();
        }
    }

    public static void SeedMaterialesYPiezas(ApplicationDbContext dbContext){
        if (dbContext.Materiales.FirstOrDefault(m => m.Nombre == "PLA Premium") == null){
            dbContext.Materiales.AddRange(
                new Material("PLA Premium", 0.05m, 5000m),
                new Material("PETG Resistente", 0.08m, 3000m)
            );
            dbContext.SaveChanges();
        }

        if (dbContext.Piezas3D.FirstOrDefault(p => p.Nombre == "Soporte Auriculares") == null){
            var pla = dbContext.Materiales.FirstOrDefault(m => m.Nombre == "PLA Premium");
            var petg = dbContext.Materiales.FirstOrDefault(m => m.Nombre == "PETG Resistente");
            if(pla == null || petg == null) return;

            var pieza1 = new Pieza3D("Soporte Auriculares", "Soporte para escritorio", 150.0, 15.0m) { Material = pla, MaterialId = pla.Id };
            var pieza2 = new Pieza3D("Figura Dragon", "Decoracion fantástica", 80.0, 10.0m) { Material = petg, MaterialId = petg.Id };
            
            dbContext.Piezas3D.AddRange(pieza1, pieza2);
            dbContext.SaveChanges();
        }
    }

    public static void SeedImpresora3D(ApplicationDbContext dbContext){
        if (dbContext.Impresoras3Ds.FirstOrDefault(i => i.Nombre == "Prusa") == null){
            dbContext.Impresoras3Ds.AddRange(
                new Impresora3D("Prusa", "i3 MK3S+", TipoImpresora.Filamento, "Impresora 3D de gran formato.", 0.15m),
                new Impresora3D("Elegoo Mars", "2 Pro", TipoImpresora.Resina, "Impresora 3D de resina.", 0.20m)
            );
            dbContext.SaveChanges();
        }
    }

    public static void SeedAccesorios(ApplicationDbContext dbContext){
        if (dbContext.Accesorios.FirstOrDefault(a => a.Nombre == "Boquilla 0.4mm") == null){
            dbContext.Accesorios.AddRange(
                new Accesorio("Boquilla 0.4mm", CategoriaAccesorio.BOQUILLAS, "Prusa i3", 50, 5.99m),
                new Accesorio("Kit de limpieza", CategoriaAccesorio.KITS_LIMPIEZA, "Universal", 20, 15.50m)
            );
            dbContext.SaveChanges();
        }
    }

    public static void SeedEncargoImpresion(ApplicationDbContext dbContext, Cliente cliente){
        if (!dbContext.EncargosImpresion.Any()){
            var pieza = dbContext.Piezas3D.FirstOrDefault();
            var material = dbContext.Materiales.FirstOrDefault();
            if (pieza == null || material == null) return;

            var encargo = new EncargoImpresion(DateTime.Now, cliente.Name ?? "Peter", cliente.Surname ?? "Jackson", "Avda. España s/n, Albacete", "600123456", "Encargo de prueba", 15.0m, MetodoPago.Bizum);

            var linea = new LineaEncargo(1, 15.0m, 15.0m){
                Pieza = pieza,
                Pieza3DId = pieza.Id,
                MaterialSeleccionado = material,
                MaterialId = material.Id,
                EncargoImpresion = encargo
            };
            
            encargo.LineasEncargo.Add(linea);
            dbContext.EncargosImpresion.Add(encargo);
            dbContext.SaveChanges();
        }
    }

    public static void SeedReservaImpresora(ApplicationDbContext dbContext, Cliente cliente){
        if (!dbContext.ReservaImpresora.Any()){
            var impresora = dbContext.Impresoras3Ds.FirstOrDefault();
            if (impresora == null) return;

            var reserva = new ReservaImpresora{
                FechaReserva = DateTime.Now,
                NombreCliente = cliente.Name ?? "Peter",
                ApellidosCliente = cliente.Surname ?? "Jackson",
                DireccionFacturacion = cliente.DireccionFacturacion,
                PrecioTotal = 10.0m,
                MetodoPago = MetodoPago.TarjetaCredito,
                ClienteId = cliente.Id,
                Cliente = cliente
            };

            reserva.LineaReservas.Add(new LineaReserva{ 
                TiempoReserva = TiempoReserva.DosHoras, 
                PrecioSubtotal = 10.0m, 
                Impresora3DId = impresora.Id,
                Impresora3D = impresora 
            }); 
            
            dbContext.ReservaImpresora.Add(reserva);
            dbContext.SaveChanges();
        }
    }

    public static void SeedCompraModelo3D(ApplicationDbContext dbContext, Cliente cliente){
        if (!dbContext.ComprasModelo3D.Any()){
            var modelo = dbContext.Modelos3D.FirstOrDefault();
            if (modelo == null) return;

            var compra = new CompraModelo3D(DateTime.Now, cliente.Name ?? "Peter", cliente.Surname ?? "Jackson", cliente.Email ?? "peter@uclm.es", cliente.DireccionFacturacion, null, modelo.Precio, MetodoPago.Paypal, cliente);
            
            dbContext.ComprasModelo3D.Add(compra);
            dbContext.SaveChanges();

            var linea = new LineaCompraModelo(1, modelo.Precio, compra, modelo);
            dbContext.Add(linea);
            dbContext.SaveChanges();
        }
    }

    public static void SeedCompraAccesorios(ApplicationDbContext dbContext, Cliente cliente){
        if (!dbContext.ComprasAccesorios.Any()){
            var accesorio = dbContext.Accesorios.FirstOrDefault();
            if (accesorio == null) return;

            var compra = new CompraAccesorios(DateTime.Now, cliente.Name ?? "Peter", cliente.Surname ?? "Jackson", cliente.DireccionFacturacion, "600123456", accesorio.Precio * 2, MetodoPago.TarjetaCredito, cliente);

            var linea = new LineaCompraAccesorio(2, accesorio.Precio, accesorio.Precio * 2, accesorio, compra);
            compra.LineasCompra.Add(linea);
            
            dbContext.ComprasAccesorios.Add(compra);
            dbContext.SaveChanges();
        }
    }
}