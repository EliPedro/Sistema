using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControleConta.Models
{
    public class Deposito
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Conta { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal Valor { get; set; }
    }
}