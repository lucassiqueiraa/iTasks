using iTasks;
using iTasks.models.Enums;
using System;
using System.Data.Entity;
using System.Linq;

public class TarefaService
{
    private readonly iTasksContext _context;
    public TarefaService(iTasksContext context)
    {
        _context = context;
    }

    public double CalcularTempoPrevistoParaToDos(int gestorId)
    {
        using (var db = new iTasksContext())
        {
            // Pega tarefas concluídas agrupadas por StoryPoints
            // 1. Busque todas as tarefas concluídas do gestor já filtradas
            var concluidas = db.Tarefas
                .Where(t => t.GestorId == gestorId &&
                            t.EstadoAtual == EstadoTarefa.Done &&
                            t.DataRealInicio != null &&
                            t.DataRealFim != null)
                .Select(t => new {
                    t.StoryPoints,
                    t.DataRealInicio,
                    t.DataRealFim
                })
                .ToList(); // Agora está em memória

            // 2. Agrupe e calcule a média em C#
            var concluidasPorSP = concluidas
                .GroupBy(t => t.StoryPoints)
                .ToDictionary(
                    g => g.Key,
                    g => g.Average(t => (t.DataRealFim.Value - t.DataRealInicio.Value).TotalMinutes)
                );

            // Lista de todos SPs disponíveis nas tarefas concluídas
            var storyPointsExistentes = concluidasPorSP.Keys.ToList();

            // Para cada tarefa ToDo, calcula o tempo previsto
            var tarefasToDo = db.Tarefas
                .Where(t => t.GestorId == gestorId && t.EstadoAtual == EstadoTarefa.ToDo)
                .ToList();

            double totalMinutos = 0;

            foreach (var tarefa in tarefasToDo)
            {
                // Procura se tem média para o SP exato
                if (concluidasPorSP.TryGetValue(tarefa.StoryPoints, out var mediaMinutos))
                {
                    totalMinutos += mediaMinutos;
                }
                else if (storyPointsExistentes.Count > 0)
                {
                    // Busca o SP mais próximo
                    int maisProximo = storyPointsExistentes
                        .OrderBy(sp => Math.Abs(sp - tarefa.StoryPoints))
                        .First();

                    totalMinutos += concluidasPorSP[maisProximo];
                }
            }

            // Retorne em horas, se quiser
            return totalMinutos / 60.0;
        }
    }
}