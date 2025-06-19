using iTasks.controller;
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

namespace iTasks
{
    public partial class frmKanban : Form
    {
        Manager manager { get; set; }
        private Utilizador user;
        public frmKanban(Manager manager, Utilizador user)
        {
            InitializeComponent();
            this.manager = manager;
            this.user = user;


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
        }

        private void gerirUtilizadoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.manager.ShowGereUtilzadoresForm(true);
        }
    }
}
