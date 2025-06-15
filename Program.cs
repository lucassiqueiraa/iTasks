using iTasks.models.Data;
using iTasks.models.Enums;
using iTasks.models.Usuarios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iTasks
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var db = new iTasksContext())
            {
                // Verifica se já existe um utilizador com o username "admin"
                var utilizadorExistente = db.Utilizadores.FirstOrDefault(u => u.Username == "admin");

                if (utilizadorExistente == null)
                {

                    // Cria o Gestor que automaticamente cria o Utilizador, o EF entende o Table-Per-Type (TPT)
                    var gestor = new Gestor
                    {
                        Nome = "administrator", //Da tabela utilizador
                        Username = "admin", //Da tabela utilizador
                        Password = "admin", //Da tabela utilizador
                        Departamento = Departamento.IT,
                        GereUtilizadores = true
                    };
                    db.Gestores.Add(gestor);
                    db.SaveChanges();
                }
                else
                {
                    // Verifica se já é um gestor, se não for ele apaga o usuario existente e cria de novo com gestor
                    var gestorExistente = db.Gestores.FirstOrDefault(g => g.Id == utilizadorExistente.Id);
                    if (gestorExistente == null)
                    {
                        db.Utilizadores.Remove(utilizadorExistente);
                        var gestor = new Gestor
                        {
                            Id = utilizadorExistente.Id,
                            Nome = utilizadorExistente.Nome,
                            Username = utilizadorExistente.Username,
                            Password = utilizadorExistente.Password,
                            Departamento = Departamento.IT,
                            GereUtilizadores = true
                        };
                        db.Gestores.Add(gestor);
                        db.SaveChanges();
                    }
                    // Se já for gestor, não faz nada
                }
            }
            Application.Run(new frmLogin());


           
        }
    }
}

