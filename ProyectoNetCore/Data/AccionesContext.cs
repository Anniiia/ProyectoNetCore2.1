using Microsoft.EntityFrameworkCore;
using ProyectoNetCore.Models;
using System.Collections.Generic;

namespace ProyectoNetCore.Data
{
    public class AccionesContext : DbContext
    {
        public AccionesContext(DbContextOptions<AccionesContext> options) : base(options)
        { }

        public DbSet<Accion> Acciones { get; set; }
    }
}