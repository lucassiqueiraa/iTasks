using iTasks.models.Enums;
using iTasks.models.Tarefas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks.models.Usuarios
{
    [Table("Programadores")]
    public class Programador : Utilizador
    {
        public NivelExperiencia NivelExperiencia { get; set; }

        // Chave estrangeira para Gestor
        [Required]
        [ForeignKey("Gestor")]
        public int GestorId { get; set; }
        public virtual Gestor Gestor { get; set; }

        // Relacionamento com Tarefas
        public virtual ICollection<Tarefa> TarefasAtribuidas { get; set; }

        public Programador()
        {
            TarefasAtribuidas = new HashSet<Tarefa>();
        }
    }
}
