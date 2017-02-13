using BancoSA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ControleConta.Models
{
    [Table("Aporte", Schema = "dbo")]
    public class Aporte
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Conta Inválida")]    
        [Display(Name = "Conta")]
        public string Agencia   { get; set; }

        public decimal Valor { get; set; }

        [Display(Name = "Definir Código")]
        public string Token { get; set; }

        public Conta Conta { get; set; }
    }
}