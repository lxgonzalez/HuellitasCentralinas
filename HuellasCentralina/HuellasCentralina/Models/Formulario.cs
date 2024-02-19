using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HuellasCentralina.Models
{
    public class Formulario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFormulario { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Persona1 { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Persona2 { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Persona3 { get; set; }
        [ValidateNever]
        public Boolean Bebe { get; set; }
        [ValidateNever]
        public Boolean Cerramiento { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string MalaExperiencia { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string TipoAnimal { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string SexoAnimal { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Fallecio { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string PorqueAdopta { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string QuePasaria { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string SiViaja { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string TiempoSolo { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Dia { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Noche { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Necesidades { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Comida { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Enferma { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Dinero { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Visita { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Municipal { get; set; }
        [ValidateNever]
        [StringLength(100)]
        public string Familia { get; set; }

        [ValidateNever]
        [StringLength(100)]
        public string Aprobada { get; set; }
    }
}
