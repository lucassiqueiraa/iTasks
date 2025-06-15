using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks.models.Enums
{
    public enum EstadoTarefa
    {
        [Description("A Fazer")]
        ToDo,

        [Description("Em Progresso")]
        Doing,

        [Description("Concluído")]
        Done
    }
}
