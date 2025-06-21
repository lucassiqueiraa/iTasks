using iTasks.controller;
using iTasks.models.Tarefas;
using iTasks.models.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace iTasks
{
    public partial class frmGereTiposTarefas : Form
    {
        private FormManager formManager { get; set; }
        private TipoTarefaController controller = new TipoTarefaController();
        private int? selectedTipoId = null;

        public frmGereTiposTarefas(FormManager formManager)
        {
            InitializeComponent();
            this.formManager = formManager;
            AtualizarLista();
            btnEliminar.Enabled = false;
            lstLista.SelectedIndexChanged += lstLista_SelectedIndexChanged;
        }

        private void AtualizarLista()
        {
            var tipos = controller.ListarTipos();
            lstLista.Items.Clear();
            foreach (var tipo in tipos)
            {
                lstLista.Items.Add($"{tipo.Id} - {tipo.Nome}");
            }
            LimparFormulario();
        }

        private void LimparFormulario()
        {
            txtId.Text = "";
            txtDesc.Text = "";
            selectedTipoId = null;
            lstLista.SelectedItem = null;
            btnEliminar.Enabled = false;
        }

        private void lstLista_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLista.SelectedItem is string selected)
            {
                int id = ObterIdSelecionado(selected);
                selectedTipoId = id;
                var tipo = controller.ObterTipoPorId(id);
                if (tipo != null)
                {
                    txtId.Text = tipo.Id.ToString();
                    txtDesc.Text = tipo.Nome;
                }
                btnEliminar.Enabled = true;
            }
        }

        private int ObterIdSelecionado(string item)
        {
            int id = 0;
            if (!string.IsNullOrEmpty(item))
            {
                int.TryParse(item.Split('-')[0].Trim(), out id);
            }
            return id;
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            string nome = txtDesc.Text.Trim();
            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("O nome do tipo de tarefa é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TipoTarefa tipo = new TipoTarefa
            {
                Id = selectedTipoId ?? 0,
                Nome = nome
            };

            controller.SalvarTipo(tipo);
            MessageBox.Show("Tipo de Tarefa salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AtualizarLista();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!selectedTipoId.HasValue)
            {
                MessageBox.Show("Selecione um tipo de tarefa para eliminar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!controller.PodeEliminarTipo(selectedTipoId.Value))
            {
                MessageBox.Show("Não é possível eliminar este tipo de tarefa pois está associado a uma ou mais tarefas.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Tem certeza que deseja eliminar este tipo de tarefa?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                controller.EliminarTipo(selectedTipoId.Value);
                MessageBox.Show("Tipo de tarefa eliminado.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AtualizarLista();
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparFormulario();
        }

        private void frmGereTiposTarefas_FormClosing(object sender, FormClosingEventArgs e)
        {
            formManager.ShowKanbanForm();
        }
    }
}