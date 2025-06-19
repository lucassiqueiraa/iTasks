using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTasks.controller;
using iTasks.models.Usuarios;

namespace iTasks
{
    public partial class frmGereUtilizadores : Form
    {
        private Manager manager { get; set; }

        public frmGereUtilizadores(Manager manager)
        {
            InitializeComponent();
            this.manager = manager;
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
    }
}
