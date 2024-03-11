using Microsoft.EntityFrameworkCore;
using ProyectoNetCore.Models;
using System.Collections.Generic;

namespace ProyectoNetCore.Data
{
    public class ComprasContext : DbContext
    {
        public ComprasContext(DbContextOptions<ComprasContext> options) : base(options)
        { }

        public DbSet<Compra> Compras { get; set; }
    }
}
