using iTasks.controller;
using iTasks.models.Usuarios;
using System;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmConsultarTarefasConcluidas : Form
    {
        private readonly TarefaController tarefaController = new TarefaController();
        private readonly Utilizador usuario;
        private readonly FormManager manager;

        public frmConsultarTarefasConcluidas(FormManager manager, Utilizador usuario)
        {
            InitializeComponent();
            this.manager = manager;
            this.usuario = usuario;
        }

        private void frmConsultarTarefasConcluidas_Load(object sender, EventArgs e)
        {
            // Exemplo: se Utilizador tem propriedade IsGestor
            if (usuario is models.Usuarios.Gestor)
                CarregarTarefasConcluidasParaGestor();
            else
                CarregarTarefasConcluidasParaProgramador();
        }

        private void CarregarTarefasConcluidasParaGestor()
        {
            var tarefas = tarefaController.ListarTarefasConcluidasDoGestor(usuario.Id);
            gvTarefasConcluidas.Columns.Clear();

            gvTarefasConcluidas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Descricao",
                HeaderText = "Descrição"
            });
            gvTarefasConcluidas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Programador",
                HeaderText = "Programador"
            });
            gvTarefasConcluidas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DataPrevistaInicio",
                HeaderText = "Prev. Início",
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });
            gvTarefasConcluidas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DataPrevistaFim",
                HeaderText = "Prev. Fim",
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });
            gvTarefasConcluidas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TipoTarefa",
                HeaderText = "Tipo"
            });
            gvTarefasConcluidas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DataRealInicio",
                HeaderText = "Real Início",
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });
            gvTarefasConcluidas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DataRealFim",
                HeaderText = "Real Fim",
                DefaultCellStyle = { Format = "dd/MM/yyyy HH:mm" }
            });
            gvTarefasConcluidas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DiasPrevistos",
                HeaderText = "Dias Previstos",
                DefaultCellStyle = { Format = "N2" }
            });
            gvTarefasConcluidas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DiasGastos",
                HeaderText = "Dias Gastos",
                DefaultCellStyle = { Format = "N2" }
            });

            gvTarefasConcluidas.AutoGenerateColumns = false;
            gvTarefasConcluidas.DataSource = tarefas;
        }

        private void CarregarTarefasConcluidasParaProgramador()
        {
            var tarefas = tarefaController.ListarTarefasConcluidasDoProgramador(usuario.Id);
            gvTarefasConcluidas.Columns.Clear();

            gvTarefasConcluidas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Descricao",
                HeaderText = "Descrição"
            });
            gvTarefasConcluidas.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DiasGastos",
                HeaderText = "Dias Gastos",
                DefaultCellStyle = { Format = "N2" }
            });

            gvTarefasConcluidas.AutoGenerateColumns = false;
            gvTarefasConcluidas.DataSource = tarefas;
        }

        private void btFechar_Click(object sender, EventArgs e)
        {
            this.manager.ShowKanbanForm(usuario, true);
        }

        private void frmConsultarTarefasConcluidas_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.manager.ShowKanbanForm(usuario, true);
        }
    }
}