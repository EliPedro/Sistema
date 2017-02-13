using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace BancoSA.Models
{
    [Table("PessoaFisica", Schema = "dbo")]
    public class PessoaFisica 
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "CPF")]
        [Column("CPF")]
        [Required(ErrorMessage = "*")]
        [StringLength(11, ErrorMessage = "*")]
        public string CPF { get; set; }

        [MaxLength(50)]
        [Display(Name = "Nome")]
        [Column("Nome")]
        [Required(ErrorMessage = "*")]
        public string Nome { get; set; }
        
        
        [Display(Name = "Data de Nascimento")]
        [Column("DataNascimento")]
        [Required(ErrorMessage = "*")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        
        public  Pessoa Pessoa { get; set; }

    }
}