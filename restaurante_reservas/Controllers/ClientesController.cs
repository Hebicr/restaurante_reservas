using restaurante_reservas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace restaurante_reservas.Controllers
{
    public class ClientesController : Controller
    {
        restauranteEntities db = new restauranteEntities();
        // GET: Clientes
        public ActionResult AdminClientes()
        {
            
            List<sp_Seleccionar_Usuarios_Admin_Result> usuariosList = db.sp_Seleccionar_Usuarios_Admin().ToList();
            return View(usuariosList);
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int id)
        {
            List<Estados> estadoList = db.Estados.Where(x => x.categoria == "usuario").ToList();
            ViewBag.estadoList = new SelectList(estadoList, "id", "estado");

            List<Roles> rolesList = db.Roles.ToList();
            ViewBag.rolesList = new SelectList(rolesList, "idRol", "Rol");

            sp_Seleccionar_Usuarios_Admin_Result myUsuario = (from c in db.sp_Seleccionar_Usuarios_Admin() where id == c.id select c).First();
            return View(myUsuario);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                db.sp_Actualizar_Usuarios_Admin(id,collection["contrasena"],Convert.ToInt32(collection["id_Estado"]),Convert.ToInt32(collection["id_Rol"]));

                return RedirectToAction("AdminClientes");
            }
            catch(Exception ex)
            {
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();
            }
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Clientes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
