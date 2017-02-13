using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoSA.Models
{
    [Table("Conta", Schema = "dbo")]
    public  class Conta
    {
        [Key]
        public int ContaId { get; set; }

        [Required]
        [Column("Tipo")]
        public string Tipo { get; set; }
        
    }
}