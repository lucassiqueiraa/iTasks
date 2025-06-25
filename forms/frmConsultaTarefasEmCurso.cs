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
        private TarefaController tarefaController = new TarefaController();

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
            var tarefas = tarefaController.ListarTarefasEmCursoParaGestor(gestorId);
            gvTarefasEmCurso.Columns.Clear();

            var colunas = new[]
            {
                new { Propriedade = "Descricao", Cabecalho = "Descrição", Formato = "" },
                new { Propriedade = "Programador", Cabecalho = "Programador", Formato = "" },
                new { Propriedade = "EstadoAtual", Cabecalho = "Estado", Formato = "" },
                new { Propriedade = "OrdemExecucao", Cabecalho = "Ordem", Formato = "" },
                new { Propriedade = "DataPrevistaInicio", Cabecalho = "Prev. Início", Formato = "dd/MM/yyyy HH:mm" },
                new { Propriedade = "DataPrevistaFim", Cabecalho = "Prev. Fim", Formato = "dd/MM/yyyy HH:mm" },
                new { Propriedade = "DataRealInicio", Cabecalho = "Real Início", Formato = "dd/MM/yyyy HH:mm" },
                new { Propriedade = "StoryPoints", Cabecalho = "Story Points", Formato = "" },
                new { Propriedade = "TempoEmFaltaStr", Cabecalho = "Tempo em Falta", Formato = "" },
                new { Propriedade = "AtrasoStr", Cabecalho = "Atraso", Formato = "" }
            };

            foreach (var c in colunas)
            {
                var coluna = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = c.Propriedade,
                    HeaderText = c.Cabecalho
                };
                if (!string.IsNullOrEmpty(c.Formato))
                    coluna.DefaultCellStyle.Format = c.Formato;
                gvTarefasEmCurso.Columns.Add(coluna);
            }

            gvTarefasEmCurso.AutoGenerateColumns = false;
            gvTarefasEmCurso.DataSource = tarefas;
        }

        private void btFechar_Click(object sender, EventArgs e)
        {
            manager.ShowKanbanForm();
        }

        private void frmConsultaTarefasEmCurso_FormClosing(object sender, FormClosingEventArgs e)
        {
            manager.ShowKanbanForm();   
        }
    }
}