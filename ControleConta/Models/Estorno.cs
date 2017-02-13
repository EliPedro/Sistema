using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ControleConta.Models
{
   // [NotMapped]
    public class Estorno
    {
        public int Id { get; set; }

        [Display(Name = "Conta")]
        [Required(ErrorMessage = "*")]
        public string ContaFavorecido { get; set; }

        [Required(ErrorMessage = "*")]
        public string Valor { get; set; }


        [Display(Name = "Chave de Segurança")]
        public string Token { get; set; }

    }
}