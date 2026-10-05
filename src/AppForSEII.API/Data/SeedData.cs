using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity;

namespace AppForSEII.API.Data;

    public class SeedData{
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger)
        {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try
            {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try
            {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }
            try
            {
                SeedModelos3D(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Modelos3D in the Database.");
            }

            try
            {
                SeedMaterialesYPiezas(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding Materials and Piezas3D in the Database.");
            }

            try
            {
                var cliente = dbContext.Users.OfType<Cliente>().FirstOrDefault(u => u.UserName == "peter@uclm.es");
                SeedEncargoImpresion(dbContext, cliente);
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding an EncargoImpresion in the Database.");
                
            } try{
                SeedLicenciasModelo3D(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the LicenciasModelo3D in the Database.");
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles) {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null) {
                Cliente cliente = new Cliente("1", "Elena", "Navarro Martínez", "elena@uclm.es", "Calle Cervantes 1");
                cliente.EmailConfirmed = true;

                var result = userManager.CreateAsync(cliente, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //administrator role
                    userManager.AddToRoleAsync(cliente, roles[0]).Wait();
                }
            }


            if (userManager.FindByNameAsync("peter@uclm.es").Result == null) {
                //A customer class has been defined because it has different attributes (purchase, rental, etc.)
                Cliente cliente = new Cliente("3", "Peter", "Jackson", "peter@uclm.es", "Calle Dulcinea 2");
                cliente.EmailConfirmed = true;

                var result = userManager.CreateAsync(cliente, "OtherPass12$");

                result.Wait();

                if (result.IsCompletedSuccessfully) {
                    //customer role
                    userManager.AddToRoleAsync(cliente, roles[2]).Wait();

                }
            }
        }
        public static void SeedLicenciasModelo3D(ApplicationDbContext dbContext)
        {
            if (dbContext.LicenciasModelo3D.FirstOrDefault(l => l.Id == 1) == null)
            {
                var licencia = new LicenciaModelo3D
                {
                    Id = 1,
                    Nombre = "Licencia Estándar",
                    FechaExpiracion = DateTime.Now.AddYears(1)
                };
                dbContext.LicenciasModelo3D.Add(licencia);
            }
            dbContext.SaveChanges();
        }
        public static void SeedModelos3D(ApplicationDbContext dbContext)
        {
            if (dbContext.Modelos3D.FirstOrDefault(m => m.Nombre == "Robot") == null)
            {
                var modelo = new Modelo3D
                {   
                    Id = 1,
                    Nombre = "Robot",
                    Categoria = "Personajes",
                    Formato = FormatoModelo3D.OBJ,
                    Precio = 15
                };
                dbContext.Modelos3D.Add(modelo);
            }
        }

        public static void SeedMaterialesYPiezas(ApplicationDbContext dbContext)
        {
            // 1. Seed Materiales
            if (dbContext.Materiales.FirstOrDefault(m => m.Nombre == "PLA Premium") == null)
            {
                dbContext.Materiales.Add(new Material("PLA Premium", 0.05m, 5000m));
            }

            if (dbContext.Materiales.FirstOrDefault(m => m.Nombre == "PETG Resistente") == null)
            {
                dbContext.Materiales.Add(new Material("PETG Resistente", 0.08m, 3000m));
            }

            dbContext.SaveChanges();

            // 2. Seed Piezas3D
            var pla = dbContext.Materiales.FirstOrDefault(m => m.Nombre == "PLA Premium");
            var petg = dbContext.Materiales.FirstOrDefault(m => m.Nombre == "PETG Resistente");

            if (dbContext.Piezas3D.FirstOrDefault(p => p.Nombre == "Soporte Auriculares") == null)
            {
                var pieza = new Pieza3D("Soporte Auriculares", "HerramientasYAccesorios", 150.0, 15.0m);
                pieza.Material = pla;
                dbContext.Piezas3D.Add(pieza);
            }

            if (dbContext.Piezas3D.FirstOrDefault(p => p.Nombre == "Figura Dragon") == null)
            {
                var pieza = new Pieza3D("Figura Dragon", "Decoracion", 80.0, 10.0m);
                pieza.Material = petg;
                dbContext.Piezas3D.Add(pieza);
            }

            dbContext.SaveChanges();
        }

        public static void SeedEncargoImpresion(ApplicationDbContext dbContext, Cliente? cliente)
        {
            if (!dbContext.EncargosImpresion.Any())
            {
                var pieza = dbContext.Piezas3D.FirstOrDefault();
                var material = dbContext.Materiales.FirstOrDefault();

                if (pieza == null || material == null) return;

                var encargo = new EncargoImpresion
                {
                    FechaEncargo = DateTime.Now,
                    NombreCliente = cliente?.Name ?? "Peter",
                    ApellidosCliente = cliente?.Surname ?? "Jackson",
                    DireccionEnvio = "Avda. España s/n, Albacete",
                    NumeroTelefono = "600123456",
                    Descripcion = "Encargo de prueba inicial",
                    PrecioTotal = 15.0m,
                    MetodoPago = MetodoPago.Bizum
                };

                var linea = new LineaEncargo
                {
                    Cantidad = 1,
                    PrecioUnidad = 15.0m,
                    Subtotal = 15.0m,
                    Pieza = pieza,
                    MaterialSeleccionado = material,
                    EncargoImpresion = encargo
                };

                if (encargo.LineasEncargo != null)
                {
                    encargo.LineasEncargo.Add(linea);
                }

                dbContext.EncargosImpresion.Add(encargo);
            }

            dbContext.SaveChanges();
            if (dbContext.Modelos3D.FirstOrDefault(m => m.Nombre == "Castillo") == null)
            {
                var modelo = new Modelo3D
                {
                    Id = 2,
                    Nombre = "Castillo",
                    Categoria = "Arquitectura",
                    Formato = FormatoModelo3D.STL,
                    Precio = 20
                };
                dbContext.Modelos3D.Add(modelo);
            }

            if (dbContext.Modelos3D.FirstOrDefault(m => m.Nombre == "Nave Espacial") == null)
            {
                var modelo = new Modelo3D
                {
                    Id = 3,
                    Nombre = "Nave Espacial",
                    Categoria = "Vehículos",
                    Formato = FormatoModelo3D.TresMF,
                    Precio = 30.0m
                };
                dbContext.Modelos3D.Add(modelo);
            }

            dbContext.SaveChanges();
        }
        public static void SeedImpresora3D(ApplicationDbContext dbContext)
        {
            if (dbContext.Impresoras3Ds.FirstOrDefault(m => m.Nombre == "Robot") == null)
            {
                var impresora = new Impresora3D
                (   
                  "crusa",
        "CR-10",
        TipoImpresora.Filamento,
       "Impresora 3D de gran formato con una superficie de impresión de 300 x 300 x 400 mm. Ideal para proyectos grandes y detallados.",
        0.15m
                );
                dbContext.Impresoras3Ds.Add(impresora);
            }

            if (dbContext.Impresoras3Ds.FirstOrDefault(m => m.Nombre == "Castillo") == null)
            {
                var impresora = new Impresora3D
                (
                    "Elegoo Mars 2 Pro",
                    "Elegoo Mars 2 Pro",
                    TipoImpresora.Resina,
                    "Impresora 3D de resina con una resolución de 0.05 mm y una superficie de impresión de 129 x 80 x 160 mm. Ideal para modelos detallados y miniaturas.",
                    0.20m
                );
                dbContext.Impresoras3Ds.Add(impresora);
            }

            if (dbContext.Impresoras3Ds.FirstOrDefault(m => m.Nombre == "Nave Espacial") == null)
            {
                var impresora = new Impresora3D
                (
                    "Nave Espacial",
                    "Nave Espacial",
                    TipoImpresora.Resina,
                    "Impresora 3D de resina con una resolución de 0.05 mm y una superficie de impresión de 129 x 80 x 160 mm. Ideal para modelos detallados y miniaturas.",
                    30.0m
                );
                dbContext.Impresoras3Ds.Add(impresora);
            }

            dbContext.SaveChanges();
  
        }
    

} 
        
    