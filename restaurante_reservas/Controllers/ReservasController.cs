using restaurante_reservas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace restaurante_reservas.Controllers
{
    public class ReservasController : Controller
    {
        // GET: Reservas
        restauranteEntities db = new restauranteEntities();
        public ActionResult AdminReservas()
        {
            try
            {
                var reservas = db.sp_Selecionar_Todas_Reservas().ToList();
                return View(reservas);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();
            }
        }

        // GET: Reservas/Details/5
        public ActionResult Details(int id)
        {
            sp_Selecionar_Todas_Reservas_Result myReserva = (from c in db.sp_Selecionar_Todas_Reservas() where id == c.id select c).First();
            return View(myReserva);

        }

        // GET: Reservas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reservas/Create
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

        // GET: Reservas/Edit/5
        public ActionResult Edit(int id)
        {
            List<Estados> estadoList = db.Estados.Where(x => x.categoria == "reservas").ToList();
            ViewBag.estadoList = new SelectList(estadoList, "id", "estado");

            sp_Selecionar_Todas_Reservas_Result myReserva = (from c in db.sp_Selecionar_Todas_Reservas() where id == c.id select c ).First();
            return View(myReserva);
        }

        // POST: Reservas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                db.sp_Actualizar_Reserva(id, Convert.ToDateTime(collection["fecha"]),Convert.ToInt32(collection["id_Estado"]), Convert.ToInt32(collection["cantidad_personas"]));
                return RedirectToAction("AdminReservas");
            }
            catch(Exception ex)
            {
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();

            }
        }

        // GET: Reservas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reservas/Delete/5
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
