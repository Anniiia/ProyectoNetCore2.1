using Microsoft.EntityFrameworkCore;
using ProyectoNetCore.Models;

namespace ProyectoNetCore.Data
{
    public class UsuariosContext:DbContext
    {
        public UsuariosContext(DbContextOptions<UsuariosContext> options) : base(options)
        { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
