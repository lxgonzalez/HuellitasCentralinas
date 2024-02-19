using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HuellasCentralina.Models
{
    public class Mascota
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMascota { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Edad { get; set; }

        [Required]
        [StringLength(1)]
        [RegularExpression("[MF]")]
        public string Sexo { get; set; }

        [ValidateNever]
        [StringLength(500)]
        public string FotoUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string Inmueble { get; set; }

        [Required]
        [StringLength(200)]
        public string Tamano { get; set; }

        [Required]
        [StringLength(1)]
        [RegularExpression("[SN]")]
        public string Esterilizado { get; set; }

        [ForeignKey("Usuario")]
        public int? IdUsuario { get; set; }

        public Usuario Usuario { get; set; }
    }
}
