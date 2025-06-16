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

namespace iTasks
{
    public partial class frmLogin : Form
    {
        private Manager manager { get; set; }
        public frmLogin(Manager manager)
        {
            InitializeComponent();
            this.manager = manager;
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;

                if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Username and password cannot be empty.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if(!helpers.Validator.IsUsernameValid(username))
                {
                    MessageBox.Show("Username must be 3-64 characters long and can only contain letters, numbers, and underscores.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var db = new iTasksContext())
                {
                    var user = db.Utilizadores
                        .FirstOrDefault(u => u.Username == username && u.Password == password);

                    if(user == null)
                    {
                        MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (user is models.Usuarios.Gestor || user is models.Usuarios.Programador)
                    {
                        this.manager.ShowKanbanForm(user, true);
                        //this.manager.currentForm = new frmKanban(this.manager, user);
                    }
                    else
                    {
                        MessageBox.Show("User type not recognized.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }
    }
}
