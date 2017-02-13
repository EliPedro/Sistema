using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ControleConta.Models
{
    [Table("Historico", Schema = "dbo")]
    public class Historico
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Cedente { get; set; }
        public string Sacado { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }

    }
}