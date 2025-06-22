using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using iTasks.models.Tarefas;
using iTasks.models.Enums;

namespace iTasks.controller
{
    public class TarefaController
    {
        public List<Tarefa> ListarPorEstado(EstadoTarefa estado, int? idProgramador = null)
        {
            using (var db = new iTasksContext())
            {
                var query = db.Tarefas.Include("TipoTarefa").Include("Programador").Include("Gestor")
                    .Where(t => t.EstadoAtual == estado);

                if (idProgramador.HasValue)
                    query = query.Where(t => t.ProgramadorId == idProgramador.Value);

                return query.OrderBy(t => t.OrdemExecucao).ToList();
            }
        }

        public Tarefa ObterPorId(int id)
        {
            using (var db = new iTasksContext())
            {
                return db.Tarefas.Include("TipoTarefa").Include("Programador").Include("Gestor")
                    .FirstOrDefault(t => t.Id == id);
            }
        }

        public void CriarTarefa(Tarefa tarefa)
        {
            using (var db = new iTasksContext())
            {
                tarefa.DataCriacao = DateTime.Now;
                tarefa.EstadoAtual = EstadoTarefa.ToDo;
                tarefa.DataRealInicio = null;
                tarefa.DataRealFim = null;
                db.Tarefas.Add(tarefa);
                db.SaveChanges();
            }
        }

        public void AtualizarTarefa(Tarefa tarefa)
        {
            using (var db = new iTasksContext())
            {
                db.Entry(tarefa).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // Transição de estado: ToDo -> Doing
        public bool ExecutarTarefa(int tarefaId, int programadorId, out string erro)
        {
            erro = "";
            using (var db = new iTasksContext())
            {
                var tarefa = db.Tarefas.Find(tarefaId);
                if (tarefa == null) { erro = "Tarefa não encontrada."; return false; }

                if (tarefa.ProgramadorId != programadorId) { erro = "Apenas o programador responsável pode executar esta tarefa."; return false; }
                if (tarefa.EstadoAtual != EstadoTarefa.ToDo) { erro = "A tarefa deve estar em ToDo para ser executada."; return false; }

                // Verifica ordem (deve ser a menor ordem entre as pendentes)
                var ordemMinima = db.Tarefas.Where(t => t.ProgramadorId == programadorId && (t.EstadoAtual == EstadoTarefa.ToDo || t.EstadoAtual == EstadoTarefa.Doing)).Min(t => t.OrdemExecucao);
                if (tarefa.OrdemExecucao != ordemMinima) { erro = "Execute primeiro a tarefa de menor ordem."; return false; }

                // Máximo 2 Doing
                int doingCount = db.Tarefas.Count(t => t.ProgramadorId == programadorId && t.EstadoAtual == EstadoTarefa.Doing);
                if (doingCount >= 2) { erro = "Você já possui 2 tarefas em Doing."; return false; }

                // Transição
                tarefa.EstadoAtual = EstadoTarefa.Doing;
                tarefa.DataRealInicio = DateTime.Now;
                db.SaveChanges();
                return true;
            }
        }

        // Transição de estado: Doing -> Done
        public bool TerminarTarefa(int tarefaId, int programadorId, out string erro)
        {
            erro = "";
            using (var db = new iTasksContext())
            {
                var tarefa = db.Tarefas.Find(tarefaId);
                if (tarefa == null) { erro = "Tarefa não encontrada."; return false; }
                if (tarefa.ProgramadorId != programadorId) { erro = "Apenas o programador responsável pode terminar esta tarefa."; return false; }
                if (tarefa.EstadoAtual != EstadoTarefa.Doing) { erro = "A tarefa deve estar em Doing para ser concluída."; return false; }

                // Ordem
                var ordemMinima = db.Tarefas.Where(t => t.ProgramadorId == programadorId && t.EstadoAtual == EstadoTarefa.Doing).Min(t => t.OrdemExecucao);
                if (tarefa.OrdemExecucao != ordemMinima) { erro = "Execute primeiro a tarefa de menor ordem."; return false; }

                // Transição
                tarefa.EstadoAtual = EstadoTarefa.Done;
                tarefa.DataRealFim = DateTime.Now;
                db.SaveChanges();
                return true;
            }
        }

        // Transição de estado: Doing -> ToDo (Reiniciar)
        public bool ReiniciarTarefa(int tarefaId, int programadorId, out string erro)
        {
            erro = "";
            using (var db = new iTasksContext())
            {
                var tarefa = db.Tarefas.Find(tarefaId);
                if (tarefa == null) { erro = "Tarefa não encontrada."; return false; }
                if (tarefa.ProgramadorId != programadorId) { erro = "Apenas o programador responsável pode reiniciar esta tarefa."; return false; }
                if (tarefa.EstadoAtual != EstadoTarefa.Doing) { erro = "A tarefa deve estar em Doing para ser reiniciada."; return false; }

                tarefa.EstadoAtual = EstadoTarefa.ToDo;
                tarefa.DataRealInicio = null; // Limpa data real de início
                db.SaveChanges();
                return true;
            }
        }
    }
}