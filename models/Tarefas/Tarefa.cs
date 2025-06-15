using iTasks.models.Enums;
using iTasks.models.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks.models.Tarefas
{
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrdemExecucao { get; set; }

        [Required]
        [MaxLength(500)]
        public string Descricao { get; set; }

        [Required]
        public DateTime DataPrevistaInicio { get; set; }

        [Required]
        public DateTime DataPrevistaFim { get; set; }

        [Required]
        public int StoryPoints { get; set; }

        public DateTime? DataRealInicio { get; set; }
        public DateTime? DataRealFim { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; }

        [Required]
        public EstadoTarefa EstadoAtual { get; set; }

        // Relacionamentos com chaves estrangeiras explícitas
        [Required]
        [ForeignKey("TipoTarefa")]
        public int TipoTarefaId { get; set; }
        public virtual TipoTarefa TipoTarefa { get; set; }

        [Required]
        [ForeignKey("Gestor")]
        public int GestorId { get; set; }
        public virtual Gestor Gestor { get; set; }

        [Required]
        [ForeignKey("Programador")]
        public int ProgramadorId { get; set; }
        public virtual Programador Programador { get; set; }

        public Tarefa()
        {
            DataCriacao = DateTime.Now;
            EstadoAtual = EstadoTarefa.ToDo;
        }
    }
    
}
