using BancoSA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleConta.Models
{
    [Table("ContaFilial", Schema = "dbo")]
    public class ContaFilial
    {
    
            [HiddenInput(DisplayValue = false)]
            public int id { get; set; }

            [Display(Name = "Nome")]
            [Column("Nome")]
            [Required(ErrorMessage = "*")]
            [MaxLength(20, ErrorMessage = "*")]
            public string Nome { get; set; }

         
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime DataCriacao { get; set; }
           
            [Required(ErrorMessage = "*")]
            [Display(Name = "Saldo")]
            public decimal Saldo { get; set; }

            public Pessoa Pessoa { get; set; }

            public string Status { get; set; }

            public Conta Conta { get; set; }
        }
    }
