using iTasks.models.Enums;
using iTasks.models.Usuarios;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using iTasks.controller;

namespace iTasks
{
    public partial class frmConsultaTarefasEmCurso : Form
    {
        private int gestorId; // Receba o id do gestor logado
        private FormManager manager { get; set; }

        public frmConsultaTarefasEmCurso(FormManager manager, Utilizador user)
        {
            InitializeComponent();
            this.manager = manager;
            this.gestorId = user.Id;
        }

        private void frmConsultaTarefasEmCurso_Load(object sender, EventArgs e)
        {
            CarregarTarefasEmCurso();
        }

        private void CarregarTarefasEmCurso()
        {
            using (var db = new iTasksContext())
            {
                var tarefas = db.Tarefas
                    .Where(t => t.GestorId == gestorId && t.EstadoAtual != EstadoTarefa.Done)
                    .Select(t => new
                    {
                        t.Descricao,
                        Programador = t.Programador.Nome,
                        t.EstadoAtual,
                        t.OrdemExecucao,
                        t.DataPrevistaInicio,
                        t.DataPrevistaFim,
                        t.DataRealInicio,
                        t.StoryPoints
                    })
                    .ToList();

                gvTarefasEmCurso.DataSource = tarefas;
            }
        }

     
    }
}