﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTasks.models.Usuarios;

namespace iTasks.controller
{
    public class FormManager
    {
        public Form currentForm = null;

        public Utilizador usuarioLogado { get; set; }

        private bool isClosing = false;

        private frmLogin frmLogin = null;
        private frmKanban frmKanban = null;
        private frmGereUtilizadores frmGereUtilizadores = null;
        private frmGereTiposTarefas frmGereTiposTarefas = null;
        private frmDetalhesTarefa frmDetalhesTarefa = null;
        private frmConsultaTarefasEmCurso frmConsultaTarefasEmCurso = null;
        private frmConsultarTarefasConcluidas frmConsultaTarefasConcluidas = null;

        private void ShowForm<T>(ref T form, bool toggle = true, params object[] args) where T : Form
        {
            if (isClosing)
                return;

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

        public void ShowKanbanForm(Utilizador user = null, bool toggle = true)
        {
            ShowForm(ref frmKanban, toggle, this, user ?? this.usuarioLogado);
        }

        public void ShowGereUtilzadoresForm(bool toggle = true)
        {
            ShowForm(ref frmGereUtilizadores, toggle, this);
        }

        public void ShowGereTiposTarefasForm(bool toggle = true)
        {
            ShowForm(ref frmGereTiposTarefas, toggle, this);
        }

        public void ShowDetalhesTarefaForm(Utilizador user, bool toggle = true, int? tarefaId = null)
        {
            if (tarefaId.HasValue)
                ShowForm(ref frmDetalhesTarefa, toggle, this, user, tarefaId.Value);
            else
                ShowForm(ref frmDetalhesTarefa, toggle, this, user);
        }

        public void ShowConsultaTarefasEmCursoForm(Utilizador user, bool toggle = true)
        {
            ShowForm(ref frmConsultaTarefasEmCurso, toggle, this, user);
        }

        public void ShowConsultaTarefasConcluidasForm(Utilizador user, bool toggle = true)
        {
            if (frmConsultaTarefasConcluidas == null || frmConsultaTarefasConcluidas.IsDisposed)
                frmConsultaTarefasConcluidas = new frmConsultarTarefasConcluidas(this, user);

            ShowForm(ref frmConsultaTarefasConcluidas, toggle, this, user);
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
