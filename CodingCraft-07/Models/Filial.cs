using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingCraft_07.Models
{
    [Table("Filiais")]
    public class Filial
    {
        [Key]
        public long FilialId { get; set; }
        public long EmpresaId { get; set; }
        public long EstadoId { get; set; }

        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cidade { get; set; }

        [JsonIgnore]
        public virtual Empresa Empresa { get; set; }
        [JsonIgnore]
        public virtual Estado Estado { get; set; }
    }
}