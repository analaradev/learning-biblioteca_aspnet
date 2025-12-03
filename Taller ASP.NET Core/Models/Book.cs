using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taller_ASP.NET_Core.Models
{
    public class Book
    {
        // El orden si importa, las restricciones deben colocarse antes de declarar las propiedades.


        // Llave primaria
        public int id { get; set; }

        // Título del libro
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(300, ErrorMessage = "El título no puede exceder 300 caracteres")]
        public string Title { get; set; } = string.Empty;

        // Autor del libro
        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(200, ErrorMessage = "El autor no puede exceder 200 caracteres")]
        public string Author { get; set; } = string.Empty;

        // ISBN del libro
        [StringLength(20, ErrorMessage = "El ISBN no puede exceder 20 caracteres")]
        public string? ISBN { get; set; }

        // Editorial
        [StringLength(200, ErrorMessage = "La editorial no puede exceder 200 caracteres")]
        public string? Publisher { get; set; }

        // Año de publicación
        [Range(1000, 2100, ErrorMessage = "El año debe estar entre 1000 y 2100")]
        public int? Year { get; set; }

        // Género literario
        [StringLength(100, ErrorMessage = "El género no puede exceder 100 caracteres")]
        public string? Genre { get; set; }

        // Descripción o sinopsis
        [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
        public string? Description { get; set; }

        // Estado de lectura
        public bool IsRead { get; set; } = false;

        // Orden para drag & drop
        public int Order { get; set; } = 0;

        // Propiedades para la portada del libro
        public byte[]? CoverImage { get; set; }
        public string? ImageContentType { get; set; }

        // Propiedad para subir portada (no se mapea a la BD)
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        // Auditoría
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign Key a AspNetUsers
        [BindNever]
        public string UserId { get; set; } = string.Empty;
    }
}