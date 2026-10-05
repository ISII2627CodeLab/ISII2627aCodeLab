namespace AppForSEII.API.Data
{
    public class SeedData
    {
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
                // Inicializa los catalogos de Materiales y Piezas3D
                SeedMaterialesYPiezas(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding Materials and Piezas3D in the Database.");
            }

            try
            {
                var cliente = dbContext.Users.OfType<Cliente>().FirstOrDefault(u => u.UserName == "peter@uclm.es");

                // Inicializa el encargo de impresion de prueba
                SeedEncargoImpresion(dbContext, cliente);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding an EncargoImpresion in the Database.");
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

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles)
        {
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

            if (userManager.FindByNameAsync("peter@uclm.es").Result == null)
            {
                // Instanciado como Cliente según el diagrama UML
                Cliente user = new Cliente("3", "Peter", "Jackson", "peter@uclm.es", "Avda. España s/n, Albacete");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "OtherPass12$");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager.AddToRoleAsync(user, roles[2]).Wait();
                }
            }
        }

        public static void SeedMaterialesYPiezas(ApplicationDbContext dbContext)
        {
            Material materialPla;
            Material materialPetg;

            // 1. Seed Materiales
            var pla = dbContext.Materiales.FirstOrDefault(m => m.Nombre == "PLA Premium");
            if (pla == null)
            {
                materialPla = new Material("PLA Premium", 0.05m, 5000m);
                dbContext.Materiales.Add(materialPla);
            }
            else
            {
                materialPla = pla;
            }

            var petg = dbContext.Materiales.FirstOrDefault(m => m.Nombre == "PETG Resistente");
            if (petg == null)
            {
                materialPetg = new Material("PETG Resistente", 0.08m, 3000m);
                dbContext.Materiales.Add(materialPetg);
            }
            else
            {
                materialPetg = petg;
            }

            dbContext.SaveChanges();

            // 2. Seed Piezas3D
            if (dbContext.Piezas3D.FirstOrDefault(p => p.Nombre == "Soporte Auriculares") == null)
            {
                var pieza = new Pieza3D("Soporte Auriculares", 150m, CategoriaPieza.HerramientasYAccesorios);
                pieza.MaterialesValidos.Add(materialPla);
                pieza.MaterialesValidos.Add(materialPetg);

                dbContext.Piezas3D.Add(pieza);
            }

            if (dbContext.Piezas3D.FirstOrDefault(p => p.Nombre == "Figura Dragon") == null)
            {
                var pieza = new Pieza3D("Figura Dragon", 80m, CategoriaPieza.Decoracion);
                pieza.MaterialesValidos.Add(materialPla);

                dbContext.Piezas3D.Add(pieza);
            }

            dbContext.SaveChanges();
        }

        public static void SeedEncargoImpresion(ApplicationDbContext dbContext, Cliente cliente)
        {
            if (dbContext.EncargosImpresion.FirstOrDefault(e => e.Id == 1) == null)
            {
                var pieza = dbContext.Piezas3D.First();
                var material = dbContext.Materiales.First();

                var encargo = new EncargoImpresion
                {
                    FechaEncargo = DateTime.Now,
                    NombreCliente = cliente?.Name ?? "Peter",
                    ApellidosCliente = cliente?.Surname ?? "Jackson",
                    DireccionEnvio = "Avda. España s/n, Albacete",
                    NumeroTelefono = "600123456",
                    Descripcion = "Encargo de prueba inicial",
                    PrecioTotal = 15.0m,
                    MetodoPago = MetodoPago.Bizum,
                    Cliente = cliente
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

                encargo.LineaEncargos.Add(linea);

                dbContext.EncargosImpresion.Add(encargo);
            }

            dbContext.SaveChanges();
        }
    }
}