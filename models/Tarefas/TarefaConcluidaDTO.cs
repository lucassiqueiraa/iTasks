using iTasks.models.Enums;
using System;
using System.Collections.Generic;
using iTasks.helpers;

public class TarefaConcluidaDTO
{
    public string Descricao { get; set; }
    public string Programador { get; set; } // Só exibe para gestor
    public double DiasPrevistos { get; set; } // Só exibe para gestor
    public double DiasGastos { get; set; }   // Sempre exibe
}