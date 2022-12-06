using restaurante_reservas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;

namespace restaurante_reservas.Controllers
{
    public class LoginController : Controller
    {
        restauranteEntities db = new restauranteEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario,string password)
        {
            try
            {
                //var idUsuario = db.sp_Login(Usuario, Password).ToList();
                Nullable<int> myValue = db.sp_Login(usuario, password).FirstOrDefault();
                int idUsuario = myValue.Value;

                if (idUsuario != 0)
                {

                    Session["usuario"] = idUsuario;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["Error"] = "Credenciales NO Validas";
                    return View();
                }

                //return View();
            }
            catch (Exception ex)
            {
                //return Content("Ocurrio un error : " + ex.Message);
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();
            }
        }

        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(FormCollection myCliente) 
        {
            try
            {
                var result = db.sp_Inserta_Clientes(myCliente["Cedula"], Convert.ToDateTime(myCliente["FechadeNacimiento"]), myCliente["Nombre"], myCliente["PrimerApellido"], myCliente["SegundoApellido"], myCliente["TelefonoPrincipal"], myCliente["CorreoElectronico"]);
                var result2 = result.ToList();

                if (result2.Count() == 1)
                {
                    //EnviarCorreo(result2[0].usuario, result2[0].contrasena, result2[0].Nombre, result2[0].PrimerApellido, result2[0].SegundoApellido, result2[0].CorreoElectronico);
                    enviarCorreo(result2[0].usuario, result2[0].contrasena, result2[0].Nombre, result2[0].PrimerApellido, result2[0].SegundoApellido, result2[0].CorreoElectronico);
                    ViewData["Mensaje"] = "Registro Exitoso, se le enviara un email con su informacion";

                    return View();
                    //return RedirectToAction("Login", "Login");
                }
                else
                {
                    ViewData["Error"] = "Cedula Ya esta Registrada";
                    return View();
                }
            }
            catch (Exception ex)
            {
                //return Content("Ocurrio un error : " + ex.Message);
                ViewData["Error"] = "Ocurrio un error : " + ex.Message;
                return View();
            }
        }


        void enviarCorreo(string userU, string passU, string nombreU, string apellido1U, string apellido2U, string correoU)
        {
            var fromAddress = new MailAddress("stev.199279@gmail.com", "Sistema Reservas");
            var toAddress = new MailAddress(correoU, "");
            const string fromPassword = "qzcpixbhupavxjmk";
            const string subject = "Credenciales Sistema Reservas";
            string mensaje;
            mensaje = "Estimado cliente: " + apellido1U + " " + apellido2U + " " + nombreU + " " + "\n" +
                          "A continuación, sus credenciales para ingresar a la aplicación " + "\n" +
                          "Usuario: " + " (" + userU + ")" + "\n" + "Contraseña: (" + passU + ")" + "\n" + "https://localhost:44356/";
            string body = mensaje;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}