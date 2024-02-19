using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HuellasCentralina.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Clave { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [ValidateNever]
        [StringLength(500)]
        public string FotoUrl { get; set; }

        [ForeignKey("Rol")]
        public int IdRol { get; set; }

        [ValidateNever]
        public Rol Rol { get; set; }

        [ValidateNever]
        [StringLength(1)]
        [RegularExpression("[MF]")]
        public string Sexo { get; set; }

        [ValidateNever]
        [StringLength(100)]
        public string Inmueble { get; set; } = "";

        [ValidateNever]
        [StringLength(200)]
        public string Tamano { get; set; }

    }
}
