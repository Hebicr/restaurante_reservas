using restaurante_reservas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace restaurante_reservas.Controllers
{
    public class HomeController : Controller
    {
        restauranteEntities db = new restauranteEntities();
        public ActionResult Index()
        {
            //var menu = db.sp_Seleccionar_Menu().ToList();
            try
            {

                //ViewBag.menu = menu;

                if (Session["usuario"] != null)
                {
                    var idUsuario = Session["usuario"];
                    var clienteLog = db.sp_Seleccionar_Cliente((int)idUsuario).ToList();
                    Session["clientelog"] = clienteLog[0];
                    Session["clientelognombre"] = clienteLog[0].Nombre;
                    Session["clientelogapellido1"] = clienteLog[0].PrimerApellido;
                    Session["clientelogapellido2"] = clienteLog[0].SegundoApellido;
                    Session["clientelogRol"] = clienteLog[0].id_Rol;
                    Session["clientelogId"] = clienteLog[0].id;

                    // ViewBag.menu = menu;
                    return View();
                }
                else {
                    //ViewBag.menu = menu;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                //ViewBag.menu = menu;
                return View();
            }
        }

        public ActionResult Menu()
        {
            var menu = db.sp_Seleccionar_Menu().ToList();
            try {
                //var menu = db.sp_Seleccionar_Menu().ToList();
                ViewBag.menu = menu;
                return View();
            } catch (Exception ex) {
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();
            }

        }


        public ActionResult LogOut()
        {
            Session["usuario"] = null;
            Session["clientelog"] = null;
            Session["clientelognombre"] = null;
            Session["clientelogapellido1"] = null;
            Session["clientelogapellido2"] = null;
            Session["clientelogRol"] = null;
            Session["clientelogId"] = null;
            return RedirectToAction("Login", "Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Reserva()
        {
            if (Session["usuario"] != null)
            {
                return View();
            }
            else {
                //ViewData["Error"] = "Debes Iniciar Seccion para poder Reservar";
                //return View();
                TempData["info"] = "Debes Iniciar Seccion para poder Reservar";
                return RedirectToAction("Login", "Login");
            }

        }

        [HttpPost]
        public ActionResult Reserva(FormCollection myReserva)
        {
            try {
                var id = Session["clientelogId"];
                db.sp_Inserta_Reserva((int)id, Convert.ToDateTime(myReserva["fecha"]), 5, Convert.ToInt32(myReserva["cantidad_personas"]));
                return RedirectToAction("ClienteReservas", "Home");
            } catch (Exception ex) {
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();
            }
            
        }

        public ActionResult ClienteReservas()
        {
            try
            {
                if (Session["clientelogId"] != null) {
                    var id = Session["clientelogId"];
                    var listReservasCliente = db.sp_Selecionar_Reservas_Cliente((int)id).ToList();
                    ViewBag.listaReservasCliente = listReservasCliente;
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();
            }

        }



    }
}