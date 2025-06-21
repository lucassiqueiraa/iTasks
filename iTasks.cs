using iTasks.controller;
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
    public partial class iTasks : Form
    {
        private Timer timer;
        public iTasks()
        {
            InitializeComponent();

            timer = new Timer();

            timer.Interval = new Random().Next(2001, 3001);

            timer.Tick += new EventHandler(HandleLoginForm);
        }

        private void iTasks_Load(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void HandleLoginForm(object sender, EventArgs e)
        {
            timer.Stop();
            this.Hide();

            FormManager manager = new FormManager();
            manager.ShowLoginForm(true);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Clean up the timer
            if (timer != null)
            {
                timer.Dispose();
            }
            base.OnFormClosed(e);
        }


    }
}
