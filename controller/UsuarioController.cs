using iTasks.models.Enums;
using iTasks.models.Usuarios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace iTasks.controller
{
    public class UsuarioController
    {
        private const EntityState modified = System.Data.Entity.EntityState.Modified;

        public List<Gestor> ListarGestores()
        {
            using (var db = new iTasksContext())
            {
                return db.Gestores.Where(g => g.Ativo).ToList();
            }
        }

        public List<Gestor> ListarGestoresDesativados()
        {
            using (var db = new iTasksContext())
            {
                return db.Gestores.Where(g => !g.Ativo).ToList();
            }
        }

        public List<Programador> ListarProgramadores()
        {
            using (var db = new iTasksContext())
            {
                return db.Programadores
                 .Include("Gestor")
                 .Where(p => p.Ativo)
                 .ToList();
            }
        }

        public List<Programador> ListarProgramadoresDesativados()
        {
            using (var db = new iTasksContext())
            {
                return db.Programadores
                 .Include("Gestor")
                 .Where(p => !p.Ativo)
                 .ToList();
            }
        }

        public List<Utilizador> ListarTodosUsuarios()
        {
            using (var db = new iTasksContext())
            {
                return db.Utilizadores.ToList();
            }
        }

        public Gestor ObterGestorPorId(int id)
        {
            using (var db = new iTasksContext())
            {
                return db.Gestores.FirstOrDefault(g => g.Id == id);
            }
        }

        public Programador ObterProgramadorPorId(int id)
        {
            using (var db = new iTasksContext())
            {
                return db.Programadores.Include("Gestor").FirstOrDefault(p => p.Id == id);
            }
        }

        public void SalvarGestor(Gestor gestor, Utilizador usuarioLogado)
        {
            if (!(usuarioLogado is Gestor))
                throw new UnauthorizedAccessException("Somente gestores podem criar ou editar gestores.");

            using (var db = new iTasksContext())
            {
                if (gestor.Id == 0)
                    db.Gestores.Add(gestor);
                else
                    db.Entry(gestor).State = modified;

                db.SaveChanges();
            }
        }

        public void SalvarProgramador(Programador programador, Utilizador usuarioLogado)
        {
            if (!(usuarioLogado is Gestor))
                throw new UnauthorizedAccessException("Somente gestores podem criar ou editar programadores.");

            using (var db = new iTasksContext())
            {
                if (programador.Id == 0)
                    db.Programadores.Add(programador);
                else
                    db.Entry(programador).State = modified;

                db.SaveChanges();
            }
        }

        public void DesativarUsuario(int id)
        {
            using (var db = new iTasksContext())
            {
                var usuario = db.Utilizadores.Find(id);
                if (usuario != null)
                {
                    usuario.Ativo = false;
                    db.Entry(usuario).State = modified;
                    db.SaveChanges();
                }
            }
        }

        public void AtivarUsuario(int id)
        {
            using (var db = new iTasksContext())
            {
                var usuario = db.Utilizadores.Find(id);
                if (usuario != null)
                {
                    usuario.Ativo = true;
                    db.Entry(usuario).State = modified;
                    db.SaveChanges();
                }

            }
        }
    }
}