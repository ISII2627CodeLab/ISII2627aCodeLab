using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII.API.Data
{
     public class SeedData {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger) {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex) {
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
                SeedLicenciasModelo3D(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the LicenciasModelo3D in the Database.");
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles) {

            foreach (string roleName in roles) {
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result) {
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
        public static void SeedLicenciasModelo3D(ApplicationDbContext dbcontext)
        {
            if (dbcontext.LicenciasModelo3D.FirstOrDefault(l => l.Id == 1) == null)
            {
                var licencia = new LicenciaModelo3D
                {
                    Id = 1,
                    Nombre = "Licencia Estándar",
                    FechaExpiracion = DateTime.Now.AddYears(1)
                };
                dbcontext.LicenciasModelo3D.Add(licencia);
            }
            dbcontext.SaveChanges();
        }
        public static void SeedModelos3D(ApplicationDbContext dbcontext)
        {
            if (dbcontext.Modelos3D.FirstOrDefault(m => m.Nombre == "Robot") == null)
            {
                var modelo = new Modelo3D
                {   
                    Id = 1,
                    Nombre = "Robot",
                    Categoria = "Personajes",
                    Formato = FormatoModelo3D.OBJ,
                    Precio = 15
                };
                dbcontext.Modelos3D.Add(modelo);
            }

            if (dbcontext.Modelos3D.FirstOrDefault(m => m.Nombre == "Castillo") == null)
            {
                var modelo = new Modelo3D
                {
                    Id = 2,
                    Nombre = "Castillo",
                    Categoria = "Arquitectura",
                    Formato = FormatoModelo3D.STL,
                    Precio = 20
                };
                dbcontext.Modelos3D.Add(modelo);
            }

            if (dbcontext.Modelos3D.FirstOrDefault(m => m.Nombre == "Nave Espacial") == null)
            {
                var modelo = new Modelo3D
                {
                    Id = 3,
                    Nombre = "Nave Espacial",
                    Categoria = "Vehículos",
                    Formato = FormatoModelo3D.TresMF,
                    Precio = 30.0m
                };
                dbcontext.Modelos3D.Add(modelo);
            }

            dbcontext.SaveChanges();
        }
    }

} 
        
    