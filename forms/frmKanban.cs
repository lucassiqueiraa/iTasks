using iTasks.controller;
using iTasks.models.Tarefas;
using iTasks.models.Usuarios;
using iTasks.models.Enums;
using System;
using System.Linq;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmKanban : Form
    {
        private FormManager manager { get; set; }
        private Utilizador user;
        private TarefaController tarefaController = new TarefaController();

        public frmKanban(FormManager manager, Utilizador user)
        {
            InitializeComponent();
            this.manager = manager;
            this.user = user;

            // Esconde a opção "Tarefas em Curso" se o usuário NÃO for gestor
            if (!(user is Gestor))
            {
                tarefasEmCursoToolStripMenuItem.Visible = false;
            }

            if (user is Gestor)
            {
                labelNome.Text = $"{user.Nome} (Gestor)";
            }
            else if (user is Programador)
            {
                labelNome.Text = $"{user.Nome} (Programador)";
            }
            else
            {
                labelNome.Text = "Kanban - Utilizador: " + user.Nome;
            }

            AtualizarKanban();
        }

        private void AtualizarKanban()
        {
            //int? idProgramador = (user is Programador) ? user.Id : (int?)null;

            lstTodo.DataSource = tarefaController.ListarPorEstado(EstadoTarefa.ToDo, null);
            lstTodo.DisplayMember = "Descricao";

            lstDoing.DataSource = tarefaController.ListarPorEstado(EstadoTarefa.Doing, null);
            lstDoing.DisplayMember = "Descricao";

            lstDone.DataSource = tarefaController.ListarPorEstado(EstadoTarefa.Done, null);
            lstDone.DisplayMember = "Descricao";
        }

        private void gerirUtilizadoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.manager.ShowGereUtilzadoresForm(true);
        }

        private void gerirTiposDeTarefasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.manager.ShowGereTiposTarefasForm(true);
        }

        private void btNova_Click(object sender, EventArgs e)
        {
            if (!(this.user is Gestor))
            {
                MessageBox.Show("Apenas gestores podem criar tarefas.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.manager.ShowDetalhesTarefaForm(this.user, true);
            AtualizarKanban();
        }

        private void btSetDoing_Click(object sender, EventArgs e)
        {
            if (lstTodo.SelectedItem is Tarefa tarefa)
            {
                string erro;
                if (tarefaController.ExecutarTarefa(tarefa.Id, user.Id, out erro))
                    AtualizarKanban();
                else
                    MessageBox.Show(erro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Selecione a tarefa que deseja executar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btSetTodo_Click(object sender, EventArgs e)
        {
            if (lstDoing.SelectedItem is Tarefa tarefa)
            {
                string erro;
                if (tarefaController.ReiniciarTarefa(tarefa.Id, user.Id, out erro))
                    AtualizarKanban();
                else
                    MessageBox.Show(erro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Selecione a tarefa que deseja reiniciar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btSetDone_Click(object sender, EventArgs e)
        {
            if (lstDoing.SelectedItem is Tarefa tarefa)
            {
                string erro;
                if (tarefaController.TerminarTarefa(tarefa.Id, user.Id, out erro))
                    AtualizarKanban();
                else
                    MessageBox.Show(erro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Selecione a tarefa que deseja terminar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // (Opcional) Atualizar detalhes ao clicar em uma tarefa
        private void lstTodo_DoubleClick(object sender, EventArgs e)
        {
            if (lstTodo.SelectedItem is Tarefa tarefa)
            {
                this.manager.ShowDetalhesTarefaForm(this.user, true, tarefa.Id); // false = modo leitura/edição
                AtualizarKanban();
            }
        }
        private void lstDoing_DoubleClick(object sender, EventArgs e)
        {
            if (lstDoing.SelectedItem is Tarefa tarefa)
            {
                this.manager.ShowDetalhesTarefaForm(this.user, true, tarefa.Id);
                AtualizarKanban();
            }
        }
        private void lstDone_DoubleClick(object sender, EventArgs e)
        {
            if (lstDone.SelectedItem is Tarefa tarefa)
            {
                this.manager.ShowDetalhesTarefaForm(this.user, true, tarefa.Id);
                AtualizarKanban();
            }
        }

        private void tarefasEmCursoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.manager.ShowConsultaTarefasEmCursoForm(this.user, true);
        }
    }
}