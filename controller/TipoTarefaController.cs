using iTasks.models.Tarefas;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace iTasks.controller
{
    public class TipoTarefaController
    {
        public List<TipoTarefa> ListarTipos()
        {
            using (var db = new iTasksContext())
            {
                return db.TiposTarefa.OrderBy(x => x.Nome).ToList();
            }
        }

        public TipoTarefa ObterTipoPorId(int id)
        {
            using (var db = new iTasksContext())
            {
                return db.TiposTarefa.FirstOrDefault(x => x.Id == id);
            }
        }

        public void SalvarTipo(TipoTarefa tipo)
        {
            using (var db = new iTasksContext())
            {
                if (tipo.Id == 0)
                {
                    db.TiposTarefa.Add(tipo);
                }
                else
                {
                    db.Entry(tipo).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }

        public bool PodeEliminarTipo(int idTipo)
        {
            using (var db = new iTasksContext())
            {
                // Verifica se existe alguma tarefa associada a este tipo
                return !db.Tarefas.Any(t => t.TipoTarefaId == idTipo);
            }
        }

        public void EliminarTipo(int idTipo)
        {
            using (var db = new iTasksContext())
            {
                var tipo = db.TiposTarefa.Find(idTipo);
                if (tipo != null)
                {
                    db.TiposTarefa.Remove(tipo);
                    db.SaveChanges();
                }
            }
        }
    }
}