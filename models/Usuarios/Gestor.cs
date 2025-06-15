using iTasks.models.Enums;
using iTasks.models.Tarefas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks.models.Usuarios
{
    [Table("Gestores")]
    public class Gestor : Utilizador
    {   
        public Departamento Departamento { get; set; }

        public bool GereUtilizadores { get; set; }

        public ICollection<Programador> Programadores { get; set; }
        public ICollection<Tarefa> Tarefas{get;set;}

        public Gestor()
        {
            Programadores = new HashSet<Programador>();
            Tarefas = new HashSet<Tarefa>();
        }
    }
}
