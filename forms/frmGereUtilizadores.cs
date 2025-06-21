using iTasks.controller;
using iTasks.models.Enums;
using iTasks.models.Usuarios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using iTasks.helpers;

namespace iTasks
{
    public partial class frmGereUtilizadores : Form
    {
        private FormManager formManager { get; set; }
        private UsuarioController usuarioController = new UsuarioController();

        private int? selectedGestorId = null;
        private int? selectedProgramadorId = null;

        public frmGereUtilizadores(FormManager formManager)
        {
            InitializeComponent();
            this.formManager = formManager;
            LoadComboBoxes();
            LoadUsersFromDatabase();
        }

        #region Geral

        private void LoadUsersFromDatabase()
        {
            var gestores = chkMostrarDesativadosGestores.Checked
                ? usuarioController.ListarGestoresDesativados()
                : usuarioController.ListarGestores();

            var programadores = chkMostrarDesativadosProgramadores.Checked
                ? usuarioController.ListarProgramadoresDesativados()
                : usuarioController.ListarProgramadores();

            PopulateList(lstListaGestores, gestores);
            PopulateList(lstListaProgramadores, programadores);

            AtualizarComboGestores();
        }

        private void PopulateList<T>(ListBox listBox, List<T> usuarios) where T : Utilizador
        {
            listBox.Items.Clear();
            foreach (var usuario in usuarios)
            {
                listBox.Items.Add($"{usuario.Id} - {usuario.Nome} ({usuario.Username})");
            }
        }

        private void LoadComboBoxes()
        {
            cbDepartamento.DataSource = Enum.GetValues(typeof(Departamento));
            cbNivelProg.DataSource = Enum.GetValues(typeof(NivelExperiencia));
            AtualizarComboGestores();
        }

        private void AtualizarComboGestores()
        {
            var gestoresCombo = usuarioController.ListarGestores();
            cbGestorProg.DataSource = null;
            cbGestorProg.DataSource = gestoresCombo;
            cbGestorProg.DisplayMember = "Nome";
            cbGestorProg.ValueMember = "Id";
        }

        private void SelecionarNaLista(ListBox listBox, int usuarioId, ref int? selectedId)
        {
            foreach (var item in listBox.Items)
            {
                int id = StringHelper.ExtractIdFromFormattedText(item.ToString());
                if (id == usuarioId)
                {
                    listBox.SelectedItem = item;
                    selectedId = usuarioId;
                    break;
                }
            }
        }

        private void AtivarOuDesativarUsuario(
            int? selectedId, string tipoUsuario, bool ativar,
            Action<int> ativarAction, Action<int> desativarAction,
            Action reloadAction, Action clearFormAction)
        {
            if (!selectedId.HasValue)
            {
                MessageHelper.ShowError($"Nenhum {tipoUsuario} selecionado.");
                return;
            }
            if (!MessageHelper.ShowConfirmation($"Tem certeza que deseja {(ativar ? "ativar" : "desativar")} este {tipoUsuario}?"))
                return;

            try
            {
                if (ativar)
                    ativarAction(selectedId.Value);
                else
                    desativarAction(selectedId.Value);

                reloadAction();
                clearFormAction();

                MessageHelper.ShowSuccess($"{tipoUsuario} {(ativar ? "ativado" : "desativado")} com sucesso!");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError($"Erro ao {(ativar ? "ativar" : "desativar")} {tipoUsuario}: {ex.Message}");
            }
        }

        private void frmGereUtilizadores_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.formManager.ShowKanbanForm();
        }

        #endregion

        #region Gestores

        private void btGravarGestor_Click(object sender, EventArgs e)
        {
            if (!CamposGestorValidos())
                return;

            Gestor gestor;
            try
            {
                if (selectedGestorId.HasValue)
                {
                    gestor = usuarioController.ObterGestorPorId(selectedGestorId.Value);
                    if (gestor == null)
                    {
                        MessageHelper.ShowError("Gestor não encontrado.");
                        return;
                    }
                }
                else
                {
                    gestor = new Gestor();
                }

                gestor.Nome = txtNomeGestor.Text;
                gestor.Username = txtUsernameGestor.Text;
                gestor.Password = txtPasswordGestor.Text;
                gestor.Departamento = (Departamento)cbDepartamento.SelectedItem;
                gestor.GereUtilizadores = chkGereUtilizadores.Checked;

                usuarioController.SalvarGestor(gestor);
                LoadUsersFromDatabase();

                SelecionarNaLista(lstListaGestores, gestor.Id, ref selectedGestorId);
                MessageHelper.ShowSuccess("Gestor salvo com sucesso!");
                LimparFormularioGestor();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError($"Erro ao salvar gestor: {ex.Message}");
            }
        }

        private void lstListaGestores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstListaGestores.SelectedItem is string selectedInfo)
            {
                int id = StringHelper.ExtractIdFromFormattedText(selectedInfo);
                selectedGestorId = id;

                var gestor = usuarioController.ObterGestorPorId(id);
                if (gestor != null)
                {
                    txtNomeGestor.Text = gestor.Nome;
                    txtUsernameGestor.Text = gestor.Username;
                    txtPasswordGestor.Text = gestor.Password;
                    cbDepartamento.SelectedItem = gestor.Departamento;
                    chkGereUtilizadores.Checked = gestor.GereUtilizadores;
                }
            }
        }

        private void LimparFormularioGestor()
        {
            txtNomeGestor.Text = "";
            txtUsernameGestor.Text = "";
            txtPasswordGestor.Text = "";
            cbDepartamento.SelectedIndex = 0;
            chkGereUtilizadores.Checked = false;
            lstListaGestores.SelectedItem = null;
            selectedGestorId = null;
        }

        private bool CamposGestorValidos()
        {
            if (string.IsNullOrWhiteSpace(txtNomeGestor.Text) ||
                string.IsNullOrWhiteSpace(txtUsernameGestor.Text) ||
                string.IsNullOrWhiteSpace(txtPasswordGestor.Text) ||
                cbDepartamento.SelectedItem == null)
            {
                MessageHelper.ShowError("Preencha todos os campos obrigatórios do gestor.");
                return false;
            }
            return true;
        }

        private void chkMostrarDesativadosGestores_CheckedChanged(object sender, EventArgs e)
        {
            LoadUsersFromDatabase();
            btnAtivarDesativarGestor.Text = chkMostrarDesativadosGestores.Checked ? "Ativar Gestor" : "Desativar Gestor";
        }

        private void btnAtivarDesativarGestor_Click(object sender, EventArgs e)
        {
            AtivarOuDesativarUsuario(
                selectedGestorId,
                "gestor",
                chkMostrarDesativadosGestores.Checked,
                usuarioController.AtivarUsuario,
                usuarioController.DesativarUsuario,
                LoadUsersFromDatabase,
                LimparFormularioGestor
            );
        }

        #endregion

        #region Programadores

        private void btGravarProg_Click(object sender, EventArgs e)
        {
            if (!CamposProgramadorValidos())
                return;

            Programador programador;
            try
            {
                if (selectedProgramadorId.HasValue)
                {
                    programador = usuarioController.ObterProgramadorPorId(selectedProgramadorId.Value);
                    if (programador == null)
                    {
                        MessageHelper.ShowError("Programador não encontrado.");
                        return;
                    }
                }
                else
                {
                    programador = new Programador();
                }

                programador.Nome = txtNomeProg.Text;
                programador.Username = txtUsernameProg.Text;
                programador.Password = txtPasswordProg.Text;
                programador.GestorId = (int)cbGestorProg.SelectedValue;
                programador.NivelExperiencia = (NivelExperiencia)cbNivelProg.SelectedItem;

                usuarioController.SalvarProgramador(programador);

                LoadUsersFromDatabase();

                SelecionarNaLista(lstListaProgramadores, programador.Id, ref selectedProgramadorId);
                MessageHelper.ShowSuccess("Programador salvo com sucesso!");
                LimparFormularioProgramador();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        MessageHelper.ShowError($"Campo: {validationError.PropertyName} Erro: {validationError.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError($"Erro ao salvar programador: {ex.Message}");
            }
        }

        private void lstListaProgramadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstListaProgramadores.SelectedItem is string selectedInfo)
            {
                int id = StringHelper.ExtractIdFromFormattedText(selectedInfo);
                selectedProgramadorId = id;

                var programador = usuarioController.ObterProgramadorPorId(id);
                if (programador != null)
                {
                    txtNomeProg.Text = programador.Nome;
                    txtUsernameProg.Text = programador.Username;
                    txtPasswordProg.Text = programador.Password;
                    cbGestorProg.SelectedValue = programador.GestorId;
                    cbNivelProg.SelectedItem = programador.NivelExperiencia;
                }
            }
        }

        private void LimparFormularioProgramador()
        {
            txtNomeProg.Text = "";
            txtUsernameProg.Text = "";
            txtPasswordProg.Text = "";
            cbNivelProg.SelectedIndex = 0;
            cbGestorProg.SelectedIndex = 0;
            lstListaProgramadores.SelectedItem = null;
            selectedProgramadorId = null;
        }

        private bool CamposProgramadorValidos()
        {
            if (string.IsNullOrWhiteSpace(txtNomeProg.Text) ||
                string.IsNullOrWhiteSpace(txtUsernameProg.Text) ||
                string.IsNullOrWhiteSpace(txtPasswordProg.Text) ||
                cbGestorProg.SelectedItem == null ||
                cbNivelProg.SelectedItem == null)
            {
                MessageHelper.ShowError("Preencha todos os campos obrigatórios do programador.");
                return false;
            }
            return true;
        }

        private void chkMostrarDesativadosProgramadores_CheckedChanged(object sender, EventArgs e)
        {
            LoadUsersFromDatabase();
            btnAtivarDesativarProg.Text = chkMostrarDesativadosProgramadores.Checked
                ? "Ativar Programador"
                : "Desativar Programador";
        }

        private void btnAtivarDesativarProg_Click(object sender, EventArgs e)
        {
            AtivarOuDesativarUsuario(
                selectedProgramadorId,
                "programador",
                chkMostrarDesativadosProgramadores.Checked,
                usuarioController.AtivarUsuario,
                usuarioController.DesativarUsuario,
                LoadUsersFromDatabase,
                LimparFormularioProgramador
            );
        }

        #endregion
    }
}