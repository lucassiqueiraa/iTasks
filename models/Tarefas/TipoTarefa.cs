using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks.models.Tarefas
{
    public class TipoTarefa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        // Relacionamento com Tarefas
        public virtual ICollection<Tarefa> Tarefas { get; set; }

        public TipoTarefa()
        {
            Tarefas = new HashSet<Tarefa>();
        }
    }
}
