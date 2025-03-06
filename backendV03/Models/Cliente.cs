using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backendV03.Models
{
    public class Cliente
    {
        // Usa [Key] para indicar que esta propiedad es la clave primaria
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincremental
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombres es obligatorio.")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo Apellidos es obligatorio.")]
        public string Apellidos { get; set; } = string.Empty;

        public string? Pais { get; set; } // No es obligatorio (acepta valores nulos)

        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo Email no tiene un formato válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo Password es obligatorio.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo Perfil es obligatorio.")]
        public string Perfil { get; set; } = "perfil.png"; // Valor por defecto
        public string rol { get; set; } = "cliente";
        public string? Telefono { get; set; } // No es obligatorio (acepta valores nulos)

        public string? Genero { get; set; } // No es obligatorio (acepta valores nulos)

        public string? F_Nacimiento { get; set; } // No es obligatorio (acepta valores nulos)

        public string? Dni { get; set; } // No es obligatorio (acepta valores nulos)

        [Required(ErrorMessage = "El campo CreatedAt es obligatorio.")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Valor por defecto
    }
}
