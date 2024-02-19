using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HuellasCentralina.Models
{
    public class Mensaje
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMensaje { get; set; }

        [ForeignKey("Mascota")]
        public int IdMascota { get; set; }

        [ValidateNever]
        public Mascota Mascota { get; set; }

        [ForeignKey("Remitente")]
        public int IdRemitente { get; set; }

        [ValidateNever]
        public Usuario Remitente { get; set; }

        [ForeignKey("Destinatario")]
        public int IdDestinatario { get; set; }

        [ValidateNever]
        public Usuario Destinatario { get; set; }

        [ForeignKey("Formulario")]
        public int IdFormulario { get; set; }

        [ValidateNever]
        public Formulario Formulario { get; set;  }
    }
}
