using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iTasks.models.Usuarios
{
    public class Utilizador
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(50)]
        [Index(IsUnique = true)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
