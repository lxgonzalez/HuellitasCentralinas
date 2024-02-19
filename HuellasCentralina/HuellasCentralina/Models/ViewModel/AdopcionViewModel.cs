using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace HuellasCentralina.Models.ViewModel
{
    public class AdopcionViewModel
    {
        [ValidateNever]
        public Mascota Mascota { get; set; }
        public Formulario Formulario { get; set; }
    }
}
