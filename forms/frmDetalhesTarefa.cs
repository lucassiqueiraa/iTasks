using iTasks.controller;
using iTasks.models.Tarefas;
using iTasks.models.Usuarios;
using iTasks.models.Enums;
using System;
using System.Windows.Forms;
using System.Linq;

namespace iTasks
{
    public partial class frmDetalhesTarefa : Form
    {
        private FormManager manager { get; set; }
        private Utilizador usuarioLogado;
        private int? tarefaId = null; // null para novo, valor para edição
        private TarefaController tarefaController = new TarefaController();
        private TipoTarefaController tipoTarefaController = new TipoTarefaController();
        private UsuarioController usuarioController = new UsuarioController();
        private Tarefa tarefaAtual = null;

        // Construtor para criação de tarefa (dois argumentos)
        public frmDetalhesTarefa(FormManager manager, Utilizador user)
            : this(manager, user, null) // Chama o construtor principal com tarefaId = null
        {
        }

        // Construtor principal (três argumentos)
        public frmDetalhesTarefa(FormManager manager, Utilizador user, int? tarefaId)
        {
            InitializeComponent();
            this.manager = manager;
            this.usuarioLogado = user;
            this.tarefaId = tarefaId;
            CarregarCombos();
            if (tarefaId.HasValue)
                CarregarDadosTarefa(tarefaId.Value);
            else
                InicializarNovo();
            AplicarModoReadOnlySeProgramador();
        }


        private void CarregarCombos()
        {
            // Tipos de tarefa
            cbTipoTarefa.DataSource = tipoTarefaController.ListarTipos();
            cbTipoTarefa.DisplayMember = "Nome";
            cbTipoTarefa.ValueMember = "Id";
            cbTipoTarefa.SelectedIndex = -1;
            
            // Programadores (apenas gestores podem escolher)
            if (usuarioLogado is Gestor gestor)
            {
                var programadoresDoGestor = usuarioController.ListarProgramadores()
                .Where(p => p.GestorId == gestor.Id)
                .ToList();

                cbProgramador.DataSource = programadoresDoGestor;
                cbProgramador.DisplayMember = "Nome";
                cbProgramador.ValueMember = "Id";
                cbProgramador.SelectedIndex = -1;
                cbProgramador.Enabled = true;
            }
            else // Programador não escolhe, campo desabilitado
            {
                cbProgramador.DataSource = null;
                cbProgramador.Items.Clear();
                cbProgramador.Enabled = false;
            }
        }

        private void CarregarDadosTarefa(int tarefaId)
        {
            tarefaAtual = tarefaController.ObterPorId(tarefaId);
            if (tarefaAtual == null)
            {
                MessageBox.Show("Tarefa não encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            txtId.Text = tarefaAtual.Id.ToString();
            txtDescricao.Text = tarefaAtual.Descricao;
            txtEstado.Text = tarefaAtual.EstadoAtual.ToString();
            txtDataCriacao.Text = tarefaAtual.DataCriacao.ToString("dd/MM/yyyy HH:mm");
            txtDataRealini.Text = tarefaAtual.DataRealInicio?.ToString("dd/MM/yyyy HH:mm") ?? "";
            txtdataRealFim.Text = tarefaAtual.DataRealFim?.ToString("dd/MM/yyyy HH:mm") ?? "";
            cbTipoTarefa.SelectedValue = tarefaAtual.TipoTarefaId;
            cbProgramador.SelectedValue = tarefaAtual.ProgramadorId;
            txtOrdem.Text = tarefaAtual.OrdemExecucao.ToString();
            txtStoryPoints.Text = tarefaAtual.StoryPoints.ToString();
            dtInicio.Value = tarefaAtual.DataPrevistaInicio;
            dtFim.Value = tarefaAtual.DataPrevistaFim;

            // Só gestor pode mudar programador
            cbProgramador.Enabled = usuarioLogado is Gestor;
        }

        private void InicializarNovo()
        {
            txtId.Text = "";
            txtEstado.Text = EstadoTarefa.ToDo.ToString();
            txtDataCriacao.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            txtDataRealini.Text = "";
            txtdataRealFim.Text = "";
            txtDescricao.Text = "";
            cbTipoTarefa.SelectedIndex = -1;
            cbProgramador.SelectedIndex = -1;
            txtOrdem.Text = "";
            txtStoryPoints.Text = "";
            dtInicio.Value = DateTime.Today;
            dtFim.Value = DateTime.Today;

            // Preencher programador automaticamente se não for gestor
            if (usuarioLogado is Programador prog)
            {
                cbProgramador.Items.Clear();
                cbProgramador.Items.Add(prog);
                cbProgramador.SelectedIndex = 0;
                cbProgramador.Enabled = false;
            }
        }

        private void btGravar_Click(object sender, EventArgs e)
        {
            // Se for criação de tarefa (tarefaAtual == null)
            if (tarefaAtual == null && !(usuarioLogado is Gestor))
            {
                MessageBox.Show("Apenas gestores podem criar tarefas.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validações
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                MessageBox.Show("Descrição obrigatória.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbTipoTarefa.SelectedItem == null)
            {
                MessageBox.Show("Selecione o tipo de tarefa.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbProgramador.SelectedItem == null)
            {
                MessageBox.Show("Selecione o programador.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtOrdem.Text, out int ordem) || ordem < 1)
            {
                MessageBox.Show("Ordem de execução inválida.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtStoryPoints.Text, out int storyPoints) || storyPoints < 1)
            {
                MessageBox.Show("Story points inválido.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtFim.Value <= dtInicio.Value)
            {
                MessageBox.Show("Data prevista de fim deve ser após a de início.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Monta a tarefa
            var tarefa = tarefaAtual ?? new Tarefa();

            tarefa.Descricao = txtDescricao.Text.Trim();
            tarefa.TipoTarefaId = (int)cbTipoTarefa.SelectedValue;
            tarefa.ProgramadorId = (cbProgramador.SelectedItem is Programador p) ? p.Id : (int)cbProgramador.SelectedValue;
            tarefa.OrdemExecucao = ordem;
            tarefa.StoryPoints = storyPoints;
            tarefa.DataPrevistaInicio = dtInicio.Value;
            tarefa.DataPrevistaFim = dtFim.Value;

            if (tarefaAtual == null)
            {
                // Nova tarefa: estado ToDo, gestor é o usuário logado (gestor)
                tarefa.EstadoAtual = EstadoTarefa.ToDo;
                tarefa.DataCriacao = DateTime.Now;
                tarefa.GestorId = (usuarioLogado is Gestor g) ? g.Id : 0; 
                tarefaController.CriarTarefa(tarefa);
                MessageBox.Show("Tarefa criada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Atualização (só certos campos editáveis)
                tarefaController.AtualizarTarefa(tarefa);
                MessageBox.Show("Tarefa atualizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Close();
        }

        private void AplicarModoReadOnlySeProgramador()
        {
            if (usuarioLogado is Programador)
            {
                // Torna todos os campos somente leitura/desabilitados
                txtDescricao.ReadOnly = true;
                txtOrdem.ReadOnly = true;
                txtStoryPoints.ReadOnly = true;
                cbTipoTarefa.Enabled = false;
                cbProgramador.Enabled = false;
                dtInicio.Enabled = false;
                dtFim.Enabled = false;
                btGravar.Enabled = false;
               
            }
        }

        private void frmDetalhesTarefa_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.manager.ShowKanbanForm(usuarioLogado, true);
        }
    }
}