using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClienteMercado.Models;

namespace ClienteMercado.Controllers
{ 
    public class TipoEmpresaUsuarioController : Controller
    {
        private cliente_mercadoEntities db = new cliente_mercadoEntities();

        //
        // GET: /TipoEmpresaUsuario/

        public ViewResult Index()
        {
            return View(db.tipo_empresa_usuario.ToList());
        }

        //
        // GET: /TipoEmpresaUsuario/Details/5

        public ViewResult Details(int id)
        {
            tipo_empresa_usuario tipo_empresa_usuario = db.tipo_empresa_usuario.Find(id);
            return View(tipo_empresa_usuario);
        }

        //
        // GET: /TipoEmpresaUsuario/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /TipoEmpresaUsuario/Create

        [HttpPost]
        public ActionResult Create(tipo_empresa_usuario tipo_empresa_usuario)
        {
            if (ModelState.IsValid)
            {
                db.tipo_empresa_usuario.Add(tipo_empresa_usuario);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(tipo_empresa_usuario);
        }
        
        //
        // GET: /TipoEmpresaUsuario/Edit/5
 
        public ActionResult Edit(int id)
        {
            tipo_empresa_usuario tipo_empresa_usuario = db.tipo_empresa_usuario.Find(id);
            return View(tipo_empresa_usuario);
        }

        //
        // POST: /TipoEmpresaUsuario/Edit/5

        [HttpPost]
        public ActionResult Edit(tipo_empresa_usuario tipo_empresa_usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_empresa_usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipo_empresa_usuario);
        }

        //
        // GET: /TipoEmpresaUsuario/Delete/5
 
        public ActionResult Delete(int id)
        {
            tipo_empresa_usuario tipo_empresa_usuario = db.tipo_empresa_usuario.Find(id);
            return View(tipo_empresa_usuario);
        }

        //
        // POST: /TipoEmpresaUsuario/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            tipo_empresa_usuario tipo_empresa_usuario = db.tipo_empresa_usuario.Find(id);
            db.tipo_empresa_usuario.Remove(tipo_empresa_usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}