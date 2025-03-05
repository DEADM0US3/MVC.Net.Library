namespace FernandoLibrary.Persistance.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public List<Prestamo> Prestamos { get; set; } = new();
    }

    public class Autor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; } = new();
    }

    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; } = new();
    }

    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; } = null;
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null;
        public int Cantidad { get; set; }
        public List<Prestamo> Prestamos { get; set; } = new();
    }

    public class Prestamo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int LibroId { get; set; }
        public Libro Libro { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
    }

}