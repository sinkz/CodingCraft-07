using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodingCraft_07.Models
{
    public class Empresa
    {
        [Key]
        public long EmpresaId { get; set; }

        [Required]
        public String NomeFantasia { get; set; }
        [Required]
        public String RazaoSocial { get; set; }
        [Required]
        public String Cnpj { get; set; }

        public virtual ICollection<Filial> Filiais { get; set; }
    }
}