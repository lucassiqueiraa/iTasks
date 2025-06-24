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

                //Se passarmos um idProgramador, filtramos por ele
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

        public bool CriarTarefa(Tarefa tarefa, out string erro)
        {
            erro = "";
            using (var db = new iTasksContext())
            {
                bool ordemRepetida = db.Tarefas.Any(t =>
                    t.ProgramadorId == tarefa.ProgramadorId &&
                    t.OrdemExecucao == tarefa.OrdemExecucao &&
                    t.EstadoAtual != EstadoTarefa.Done);

                if (ordemRepetida)
                {
                    erro = "Já existe uma tarefa não concluída com essa ordem para este programador.";
                    return false;
                }

                tarefa.DataCriacao = DateTime.Now;
                tarefa.EstadoAtual = EstadoTarefa.ToDo;
                tarefa.DataRealInicio = null;
                tarefa.DataRealFim = null;
                db.Tarefas.Add(tarefa);
                db.SaveChanges();
                return true;
            }
        }

        public bool AtualizarTarefa(Tarefa tarefa, out string erro, out string infoSwap)
        {
            erro = "";
            infoSwap = "";
            using (var db = new iTasksContext())
            {
                var tarefaExistente = db.Tarefas.Find(tarefa.Id);
                if (tarefaExistente == null)
                {
                    erro = "Tarefa não encontrada.";
                    return false;
                }

                // Verifica se mudou a ordem
                if (tarefaExistente.OrdemExecucao != tarefa.OrdemExecucao)
                {
                    var outraTarefa = db.Tarefas.FirstOrDefault(t =>
                        t.ProgramadorId == tarefa.ProgramadorId &&
                        t.OrdemExecucao == tarefa.OrdemExecucao &&
                        t.EstadoAtual != EstadoTarefa.Done &&
                        t.Id != tarefa.Id);

                    if (outraTarefa != null)
                    {
                        // Faz o swap de ordens
                        int ordemOriginal = tarefaExistente.OrdemExecucao;
                        outraTarefa.OrdemExecucao = ordemOriginal;

                        // Prepara a mensagem de swap
                        infoSwap = $"Tarefa '{tarefaExistente.Descricao}' trocou de ordem com '{outraTarefa.Descricao}' (ID {outraTarefa.Id}).";
                    }
                }

                // Atualiza os outros campos
                tarefaExistente.Descricao = tarefa.Descricao;
                tarefaExistente.TipoTarefaId = tarefa.TipoTarefaId;
                tarefaExistente.StoryPoints = tarefa.StoryPoints;
                tarefaExistente.DataPrevistaInicio = tarefa.DataPrevistaInicio;
                tarefaExistente.DataPrevistaFim = tarefa.DataPrevistaFim;
                tarefaExistente.OrdemExecucao = tarefa.OrdemExecucao;

                db.SaveChanges();
                return true;
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
                var ordemMinima = db.Tarefas.Where(t => t.ProgramadorId == programadorId && (t.EstadoAtual == EstadoTarefa.ToDo)).Min(t => t.OrdemExecucao);
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

                if (tarefa.EstadoAtual != EstadoTarefa.Doing)
                {
                    erro = "A tarefa só pode ser concluída a partir do estado Doing.";
                    return false;
                }

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

        public int ProximaOrdemDisponivel(int programadorId)
        {
            using (var db = new iTasksContext())
            {
                // Pega o maior número de ordem entre tarefas NÃO DONE
                var maiorOrdem = db.Tarefas
                    .Where(t => t.ProgramadorId == programadorId && t.EstadoAtual != EstadoTarefa.Done)
                    .Select(t => (int?)t.OrdemExecucao)
                    .Max();

                return (maiorOrdem ?? 0) + 1; // Começa em 1 se não existir nenhuma
            }
        }
    }
}