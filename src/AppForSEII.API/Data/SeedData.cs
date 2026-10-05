using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity;

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
                ApplicationUser user = new ApplicationUser("3", "Peter", "Jackson", "peter@uclm.es");
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
        }
    }
}