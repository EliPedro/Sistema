using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace BancoSA.Models
{
    [Table("ContaMatriz", Schema = "dbo")]
    public class ContaMatriz
    {

        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }

        [Display(Name = "Nome")]
        [Column("Nome")]
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "*")]
        public string Nome { get; set; }

        //[Required]
        //[Display(Name = "Data de Criação")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DataCriacao { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Saldo Inicial")]
        //[DataType(DataType.Currency)]
        public decimal Saldo { get; set; }

        public Pessoa Pessoa { get; set; }

        //[Display(Name = "Status")]
        //[Required(ErrorMessage = "*")]
        public string Status { get; set; }
        
        public Conta Conta { get; set; }
    }
}