using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace BancoSA.Models
{
    [Table("PessoaJuridica", Schema = "dbo")]
    public  class PessoaJuridica 
    {  
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }

        [Display( Name ="CNPJ")]
        [Column("CNPJ")]
        [Required(ErrorMessage = "*")]
        public string CNPJ { get; set; }

        [Display(Name = "Razão Social")]
        [Column("RazaoSocial")]
        [Required(ErrorMessage = "*")]
        public string RazaoSocial { get; set; }

        [Column("NomeFantasia")]
        [Required]
        [Display(Name = "Nome Fantasia")]
        public string NomeFantasia { get; set; }
        
        public Pessoa Pessoa { get; set; }
    }
}