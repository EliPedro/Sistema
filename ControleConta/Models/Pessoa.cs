using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoSA.Models
{
    [Table("Pessoa", Schema = "dbo")]
    public  class Pessoa
    {
        [Key]
        public int PessoaId{ get; set; }

        [Required]
        [Column("Tipo")]  
        public string Tipo { get; set; }  
    }
}