using System;

public class TarefaConcluidaDTO
{
    public string Descricao { get; set; }
    public string Programador { get; set; } // Só exibe para gestor
    public double DiasPrevistos { get; set; } // Só exibe para gestor
    public double DiasGastos { get; set; }   // Sempre exibe

    // Adicione as propriedades abaixo:
    public DateTime? DataPrevistaInicio { get; set; }
    public DateTime? DataPrevistaFim { get; set; }
    public string TipoTarefa { get; set; }
    public DateTime? DataRealInicio { get; set; }
    public DateTime? DataRealFim { get; set; }
}