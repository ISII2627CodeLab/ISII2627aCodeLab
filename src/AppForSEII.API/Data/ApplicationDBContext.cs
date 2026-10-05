using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.ApplicationUserDTO;

namespace AppForSEII.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);


    }



    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

// Tablas de tu caso de uso (Compra de Accesorios)
    public DbSet<Accesorio> Accesorios { get; set; }
    public DbSet<CompraAccesorios> ComprasAccesorios { get; set; }
    public DbSet<LineaCompraAccesorio> LineasCompraAccesorio { get; set; }



}