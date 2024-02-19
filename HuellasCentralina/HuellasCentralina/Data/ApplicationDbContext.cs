using HuellasCentralina.Models;
using Microsoft.EntityFrameworkCore;

namespace HuellasCentralina.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Formulario> Formularios { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Mascota>()
            //    .Property(e => e.IdUsuario)
            //    .IsRequired(false);
            modelBuilder.Entity<Mascota>()
            .HasOne(m => m.Usuario)
            .WithMany()
            .HasForeignKey(m => m.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Mensaje>()
           .HasOne(m => m.Remitente)
           .WithMany()
           .HasForeignKey(m => m.IdRemitente)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Mensaje>()
                .HasOne(m => m.Destinatario)
                .WithMany()
                .HasForeignKey(m => m.IdDestinatario)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Mensaje>()
                .HasOne(m => m.Formulario)
                .WithMany()
                .HasForeignKey(m => m.IdFormulario)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Mensaje>()
                .HasOne(m => m.Mascota)
                .WithMany()
                .HasForeignKey(m => m.IdMascota)
                .OnDelete(DeleteBehavior.NoAction);
        }



        public Usuario ValidarUsuario(string usuario, string clave)
        {
            return Usuarios.ToList().Where(item => item.NombreUsuario == usuario && item.Clave == clave).FirstOrDefault();
        }

        public Usuario ValidarCampos(string nombre, string apellido, string email, string usuario)
        {
            return Usuarios.ToList().Where(item => item.NombreUsuario == usuario && item.Nombre == nombre && item.Apellido == apellido && item.Email == email).FirstOrDefault();
        }

        public List<Mensaje> RetornarMensajeAdoptante(int idAdoptante)
        {
            return Mensajes
                .Include(item => item.Remitente)
                .Include(item => item.Destinatario)
                .Include(item => item.Mascota)
                .Include(item => item.Formulario)
                .Where(item => item.IdRemitente == idAdoptante)
                .ToList();
        }

        public List<Mensaje> RetornarMensajeRescatador(int id)
        {
            return Mensajes
                .Include(item => item.Remitente)
                .Include(item => item.Destinatario)
                .Include(item => item.Mascota)
                .Include(item => item.Formulario)
                .Where(item => item.IdDestinatario == id)
                .ToList();
        }
    }
    
}

