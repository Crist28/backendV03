using backendV03.Models;
using Microsoft.EntityFrameworkCore;

namespace backendV03.Database
{
    public class Conexion: DbContext
    {
        public Conexion(DbContextOptions<Conexion> options): base(options)
        {}
        public DbSet<Cliente> Clientes { get; set; }
    }
}
