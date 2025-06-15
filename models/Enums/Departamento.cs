using iTasks.models.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks.models.Enums
{
    public enum Departamento
    {
        IT,
        Marketing,
        Administracao
    }
}

//Se quiser exibir o nome ("IT", "Marketing", etc.) na interface, basta converter o valor do enum para string:
//string nomeDepartamento = gestor.Departamento.ToString(); Resultado : "IT"