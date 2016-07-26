using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingCraft_07.Models
{
    [Table("Estados")]
    public class Estado
    {
        [Key]
        public long EstadoId { get; set; }

        [Required]
        public string Nome { get; set; }
        [Required]
        public string Sigla { get; set; }

        public virtual ICollection<Filial> Filiais { get; set; }
    }
}