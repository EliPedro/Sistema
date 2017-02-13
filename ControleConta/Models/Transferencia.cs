using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ControleConta.Models
{
    //[NotMapped]
    public class Transferencia
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        public string Conta { get; set; }
        
        [Display(Name = "Conta do Favorecido")]
        [Required(ErrorMessage = "*")]
        public string ContaFavorecido { get; set; }

        [Required(ErrorMessage = "*")]
        public string Valor { get; set; }

    }
}