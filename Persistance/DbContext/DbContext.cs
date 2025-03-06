using FernandoLibrary.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace FernandoLibrary.Persistance
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }

        
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=library.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Usuario>()
                .HasKey(p => p.Id);
        
            modelBuilder.Entity<Usuario>()
                .HasOne(p => p.Role);
            modelBuilder.Entity<Usuario>()
                .HasOne(p => p.Role)
                .WithMany()
                .HasForeignKey(p => p.RoleId);

            modelBuilder.Entity<Roles>().HasKey(p => p.Id);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Permissions)
                .WithMany(e => e.Roles)
                .UsingEntity<RolePermissions>(
                    l => l.HasOne<Permissions>().WithMany().HasForeignKey(e => e.PermissionId),
                    r => r.HasOne<Roles>().WithMany().HasForeignKey(e => e.RoleId)
                ); 

            modelBuilder.Entity<Permissions>().HasKey(p => p.Id);
        
            modelBuilder.Entity<Permissions>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Permissions)
                .UsingEntity<RolePermissions>(
                    l => l.HasOne<Roles>().WithMany().HasForeignKey(e => e.RoleId),
                    r => r.HasOne<Permissions>().WithMany().HasForeignKey(e => e.PermissionId)
                ); 
        
            modelBuilder.Entity<RolePermissions>().HasKey(p => p.Id);
            
            // Relaciones
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Libros)
                .HasForeignKey(l => l.AutorId);

            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Categoria)
                .WithMany(c => c.Libros)
                .HasForeignKey(l => l.CategoriaId);


            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Prestamos)
                .HasForeignKey(p => p.UsuarioId);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro)
                .WithMany(e => e.Prestamos)
                .HasForeignKey(p => p.LibroId);
        }
    }
}