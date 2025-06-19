using iTasks.controller;
using iTasks.models.Enums;
using iTasks.models.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTasks.helpers;   

namespace iTasks
{
    public partial class frmGereUtilizadores : Form
    {
        private Manager manager { get; set; }

        private int? selectedGestorId = null;
        private int? selectedProgramadorId = null;


        public frmGereUtilizadores(Manager manager)
        {
            InitializeComponent();
            this.manager = manager;
            LoadComboBoxes();
            LoadUsersFromDatabase();

        }

        // Method to load and separate users
        private void LoadUsersFromDatabase()
        {
            // Example: replace with your real database access method
            List<Utilizador> allUsers = GetAllUsersFromDatabase();

            var managers = allUsers
                .Where(u => u is Gestor)
                .ToList();

            var developers = allUsers
                .Where(u => u is Programador)
                .ToList();

            PopulateManagerList(managers);
            PopulateDeveloperList(developers);
        }

        // Helper methods to populate ListBox:
        private void PopulateManagerList(List<Utilizador> managers)
        {
            lstListaGestores.Items.Clear();
            foreach (var manager in managers)
            {
                lstListaGestores.Items.Add($"{manager.Id} - {manager.Nome} ({manager.Username})");
            }
        }

        private void PopulateDeveloperList(List<Utilizador> developers)
        {
            lstListaProgramadores.Items.Clear();
            foreach (var developer in developers)
            {
                lstListaProgramadores.Items.Add($"{developer.Id} - {developer.Nome} ({developer.Username})");
            }
        }

        // Example method to retrieve all users from the database
        // Replace with your actual implementation (Entity Framework, ADO.NET, etc.)
        private List<Utilizador> GetAllUsersFromDatabase()
        {
            using (var db = new iTasksContext())
            {
                return db.Utilizadores.ToList();
            }
        }

        private void frmGereUtilizadores_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.manager.ShowKanbanForm();
        }

        private void LoadComboBoxes()
        {
            // Preencher enum
            cbDepartamento.DataSource = Enum.GetValues(typeof(Departamento));
            cbNivelProg.DataSource =  Enum.GetValues(typeof(NivelExperiencia));

            using (var db = new iTasksContext())
            {
                // Preencher gestores
                cbGestorProg.DataSource = db.Gestores.ToList();
                cbGestorProg.DisplayMember = "Nome";
                cbGestorProg.ValueMember = "Id";
            }

        }

        private void btGravarGestor_Click(object sender, EventArgs e)
        {
            using (var db = new iTasksContext())
            {
                Gestor gestor;

                if (selectedGestorId.HasValue)
                {
                    gestor = db.Gestores.FirstOrDefault(g => g.Id == selectedGestorId.Value);
                    if (gestor == null) return; // Segurança
                }
                else
                {
                    gestor = new Gestor();
                    db.Gestores.Add(gestor);
                }

                gestor.Nome = txtNomeGestor.Text;
                gestor.Username = txtUsernameGestor.Text;
                gestor.Password = txtPasswordGestor.Text;
                gestor.Departamento = (Departamento)cbDepartamento.SelectedItem;
                gestor.GereUtilizadores = chkGereUtilizadores.Checked;

                db.SaveChanges();
                LoadUsersFromDatabase();
                ClearGestorForm();
            }
        }


        private void ClearGestorForm()
        {
            txtNomeGestor.Text = "";
            txtUsernameGestor.Text = "";
            txtPasswordGestor.Text = "";
            cbDepartamento.SelectedIndex = 0;
            chkGereUtilizadores.Checked = false;
            lstListaGestores.ClearSelected();
            selectedGestorId = null;
        }



        private void lstListaGestores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstListaGestores.SelectedItem is string selectedInfo)
            {
                int id = StringHelper.ExtractIdFromFormattedText(selectedInfo);
                selectedGestorId = id;

                using (var db = new iTasksContext())
                {
                    var gestor = db.Gestores.FirstOrDefault(g => g.Id == id);
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
        }




        private void lstListaProgramadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstListaProgramadores.SelectedItem is string selectedInfo)
            {
                int id = StringHelper.ExtractIdFromFormattedText(selectedInfo); // sua função para extrair o ID do texto da ListBox
                selectedProgramadorId = id;

                using (var db = new iTasksContext())
                {
                    var programador = db.Programadores.Include("Gestor").FirstOrDefault(p => p.Id == id);
                    if (programador != null)
                    {   txtNomeProg.Text = programador.Nome;
                        txtUsernameProg.Text = programador.Username;
                        txtPasswordProg.Text = programador.Password;

                        cbGestorProg.DataSource = db.Gestores.ToList();
                        cbGestorProg.DisplayMember = "Nome";
                        cbGestorProg.ValueMember = "Id";
                        cbGestorProg.SelectedValue = programador.GestorId;

                        cbNivelProg.DataSource = Enum.GetValues(typeof(NivelExperiencia));
                        cbNivelProg.SelectedItem = programador.NivelExperiencia;
                    }
                }
            }
        }

        private void btGravarProg_Click(object sender, EventArgs e)
        {
            using (var db = new iTasksContext())
            {
                Programador programador;
                if (selectedProgramadorId.HasValue)
                {
                    programador = db.Programadores.FirstOrDefault(p => p.Id == selectedProgramadorId.Value);
                    if (programador == null) return; // Segurança
                }
                else
                {
                    programador = new Programador();
                    db.Programadores.Add(programador);
                }
                programador.Nome = txtNomeProg.Text;
                programador.Username = txtUsernameProg.Text;
                programador.Password = txtPasswordProg.Text;
                programador.GestorId = (int)cbGestorProg.SelectedValue;
                programador.NivelExperiencia = (NivelExperiencia)cbNivelProg.SelectedItem;
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            MessageBox.Show($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }

                LoadUsersFromDatabase();
                ClearProgramadorForm();
            }

        }

        private void ClearProgramadorForm()
        {
            txtNomeProg.Text = "";
            txtUsernameProg.Text = "";
            txtPasswordProg.Text = "";
            cbNivelProg.SelectedIndex = 0;
            cbGestorProg.SelectedIndex = 0;

            lstListaProgramadores.ClearSelected();
            selectedProgramadorId = null;
        }



    }
}
