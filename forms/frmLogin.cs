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
          this.manager.ShowKanbanForm(true);
        }
    }
}
