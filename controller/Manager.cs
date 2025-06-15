using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iTasks.controller
{
    public class Manager
    {
        public Form currentForm = null;

        private bool isClosing = false;

        private frmLogin frmLogin = null;
        private frmKanban frmKanban = null;

        private void ShowForm<T>(ref T form, bool toggle = true, params object[] args) where T : Form
        {
            if (isClosing)
                return;

            this.currentForm = form;

            this.DestroyCurrentForm();

            if (currentForm == null || currentForm.GetType() != typeof(T))
            {
                form = (T)Activator.CreateInstance(typeof(T), args);
            }

            if (toggle && form != null)
            {
                form.Show();
                this.currentForm = form;
            }
            else if (form != null)
            {
                form.Close();
            }
        }

        


        public void ShowLoginForm(bool toggle = true)
        {
            ShowForm(ref frmLogin, toggle, this);
        }

        public void ShowKanbanForm(bool toggle = true)
        {
            ShowForm(ref frmKanban, toggle, this);
        }


        private void DestroyCurrentForm()
        {
            if (isClosing)
                return;

            try
            {
                if (this.currentForm != null)
                {
                    isClosing = true;
                    this.currentForm.Close();
                    this.currentForm = null;
                    isClosing = false;
                }
            }
            catch (Exception ex)
            {
                isClosing = false;
                Console.WriteLine($"Error closing form: {ex.Message}");
            }
        }
    }
}
