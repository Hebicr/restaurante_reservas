using Microsoft.AspNetCore.Http;
using restaurante_reservas.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult AdminMenu()
        {
            try
            {
                var menu = db.sp_Seleccionar_Menu_Admin().ToList();
                return View(menu);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();
            }

        }

        public ActionResult MenuDetails(int id)
        {
            //LINQ
            sp_Seleccionar_Menu_Admin_Result myMenu = (from c in db.sp_Seleccionar_Menu_Admin() where id == c.id select c).First();
            return View(myMenu);
        }

        public ActionResult MenuCreate()
        {
            List<categoria_menu> categoriaList = db.categoria_menu.ToList();
            ViewBag.categoriaList = new SelectList(categoriaList, "id", "categoria");

            List<Estados> estadoList = db.Estados.Where(x => x.categoria == "menu").ToList();
            ViewBag.estadoList = new SelectList(estadoList, "id", "estado");

            return View();
        }

        [HttpPost]
        public ActionResult MenuCreate(string platillo,string descripcion, HttpPostedFileBase photo, int id_Categoria,int id_Estado)
        {

            try
            {
                string path,filePath;
                var stream = "";
                string fileName;

                Menu myMenu = new Menu();
                //myMenu.platillo = collection["platillo"];
                //myMenu.descripcion = collection["descripcion"];
                //myMenu.img =  collection["img"];
                //myMenu.id_Categoria = Convert.ToInt32(collection["id_Categoria"]);
                //myMenu.id_Estado = Convert.ToInt32(collection["id_Estado"]);

                myMenu.platillo = platillo;
                myMenu.descripcion = descripcion;
                myMenu.id_Categoria = id_Categoria;
                myMenu.id_Estado = id_Estado;

                if (photo != null)
                {
                    fileName = platillo + new FileInfo(photo.FileName).Extension;
                    path = Path.Combine(Server.MapPath("~/images"),Path.GetFileName(fileName));
                    filePath = Path.Combine("images/", fileName);
                    photo.SaveAs(path);


                    //using (stream = new FileStream(localFileName, FileMode.Create))
                    //{
                    //    photo.SaveAs(stream);
                    //}
                }
                else {
                    filePath = Path.Combine("images/", "default.jpg");
                }

                db.sp_Inserta_Menu(platillo, descripcion, filePath, id_Categoria, id_Estado);
                return RedirectToAction("AdminMenu");
            }
            catch(Exception ex)
            {
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();
            }
        }

        public ActionResult MenuDelete(int id)
        {
            sp_Seleccionar_Menu_Admin_Result myMenu = (from c in db.sp_Seleccionar_Menu_Admin() where id == c.id select c).First();
            return View(myMenu);
        }

        [HttpPost]
        public ActionResult MenuDelete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                sp_Seleccionar_Menu_Admin_Result myMenu = (from c in db.sp_Seleccionar_Menu_Admin() where id == c.id select c).First();
                db.sp_Delete_Menu(myMenu.id);
                return RedirectToAction("AdminMenu");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();
            }
        }
        public ActionResult MenuEdit(int id)
        {
            List<categoria_menu> categoriaList = db.categoria_menu.ToList();
            ViewBag.categoriaList = new SelectList(categoriaList, "id", "categoria");

            List<Estados> estadoList = db.Estados.Where(x => x.categoria == "menu").ToList();
            ViewBag.estadoList = new SelectList(estadoList, "id", "estado");

            sp_Seleccionar_Menu_Admin_Result myMenu = (from c in db.sp_Seleccionar_Menu_Admin() where id == c.id select c).First();
            TempData["myImgOld"] = myMenu.img;
            return View(myMenu);
        }

        [HttpPost]
        public ActionResult MenuEdit(int id, string platillo, string descripcion, HttpPostedFileBase photo, int id_Categoria, int id_Estado)
        {
            var menuImgOld = TempData["myImgOld"];
            string imgOld = (string)menuImgOld;
            try
            {
                string path, filePath;
                var stream = "";
                string fileName;

                Menu myMenu = new Menu();
                myMenu.id = id;
                myMenu.platillo = platillo;
                myMenu.descripcion = descripcion;
                myMenu.id_Categoria = id_Categoria;
                myMenu.id_Estado = id_Estado;

                if (photo != null)
                {
                    fileName = platillo + new FileInfo(photo.FileName).Extension;
                    path = Path.Combine(Server.MapPath("~/images"), Path.GetFileName(fileName));
                    filePath = Path.Combine("images/", fileName);
                    photo.SaveAs(path);


                    //using (stream = new FileStream(localFileName, FileMode.Create))
                    //{
                    //    photo.SaveAs(stream);
                    //}
                }
                else
                {
                    filePath = imgOld;
                }

                db.sp_Update_Menu(id,platillo, descripcion, filePath, id_Categoria, id_Estado);
                return RedirectToAction("AdminMenu");
            }
            catch (Exception ex)
            {
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

        public ActionResult ClienteInfo() {
            try
            {
                if (Session["usuario"] != null)
                {
                    var id = Session["usuario"];
                    var infoCliente = db.sp_Seleccionar_Cliente((int)id).ToList();
                    ViewBag.infoCliente = infoCliente;
                    return View();
                }
                else {
                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult ActualizarCliente(FormCollection myReserva) 
        {
            try {

                var id = Session["clientelogId"];
                db.sp_Actualizar_Cliente((int)id, myReserva["Nombre"], myReserva["Primer Apellido"], myReserva["Segundo Apellido"], myReserva["Telefono Principal"], myReserva["Correo Electronico"]);
                
                TempData["info"] = "Se Actualizo Correctamente ";
                
                return RedirectToAction("ClienteInfo", "Home");

            } catch (Exception ex) {

                TempData["error"] = "Ocurrio un error : " + ex.Message;
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
               
                return RedirectToAction("ClienteInfo", "Home");

            }
        }
    }
}