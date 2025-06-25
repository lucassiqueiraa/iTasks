using iTasks.models.Enums;
using System;
using System.Collections.Generic;
using iTasks.helpers;

public class TarefaKanbanDTO
{
    public string Descricao { get; set; }
    public string Programador { get; set; }
    public EstadoTarefa EstadoAtual { get; set; }
    public int OrdemExecucao { get; set; }
    public DateTime DataPrevistaInicio { get; set; }
    public DateTime DataPrevistaFim { get; set; }
    public DateTime? DataRealInicio { get; set; }
    public int StoryPoints { get; set; }
    public TimeSpan TempoEmFalta { get; set; }
    public TimeSpan Atraso { get; set; }
    public string TempoEmFaltaStr => StringHelper.FormatTimeSpanFriendly(TempoEmFalta);
    public string AtrasoStr => StringHelper.FormatTimeSpanFriendly(Atraso);

   
}