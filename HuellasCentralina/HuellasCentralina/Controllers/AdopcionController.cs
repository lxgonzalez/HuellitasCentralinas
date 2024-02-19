using HuellasCentralina.Data;
using HuellasCentralina.Models;
using HuellasCentralina.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HuellasCentralina.Controllers
{
    public class AdopcionController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdopcionController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }
            Mascota mascota = _db.Mascotas.Include(item => item.Usuario).FirstOrDefault(m => m.IdMascota == id);

            if (mascota == null)
            {
                return NotFound();
            }

            var viewModel = new AdopcionViewModel
            {
                Mascota = mascota,
                Formulario = new Formulario()
            };

            return View(viewModel);

        }

        [HttpPost]
        public IActionResult Index(AdopcionViewModel viewModel)
        {
            Mascota mascota = _db.Mascotas
                        .Include(m => m.Usuario)
                        .FirstOrDefault(m => m.IdMascota == viewModel.Mascota.IdMascota);

            if (viewModel.Formulario != null)
            {
                _db.Formularios.Add(viewModel.Formulario);
                _db.SaveChanges();
                Mensaje mensaje = new Mensaje();
                mensaje.IdMascota = mascota.IdMascota;
                mensaje.IdRemitente = int.Parse(User.FindFirst("IdUsuario")?.Value);
                mensaje.IdDestinatario = (int)mascota.IdUsuario;
                mensaje.IdFormulario = viewModel.Formulario.IdFormulario;

                _db.Mensajes.Add(mensaje);
                _db.SaveChanges();
                TempData["success"] = "Mensaje Enviado!";
            }
            else
            {
                TempData["success"] = "Llenar todos los capos!";
                return View();
            }

            return RedirectToAction("Index");
        }

        public IActionResult MensajeAdoptante(int id)
        {
            List<Mensaje> mensajes = _db.RetornarMensajeAdoptante(id);
            if (mensajes.Count == 0)
            {
                TempData["info"] = "No existen Mensajes";
                return RedirectToAction("Index", "Home");
            }
            return View(mensajes);
        }

        public IActionResult MensajesIndividuales(int id)
        {
            Mensaje obj = _db.Mensajes
                .Include(item => item.Remitente)
                .Include(item => item.Destinatario)
                .Include(item => item.Mascota)
                .Include(item => item.Formulario)
                .FirstOrDefault(item => item.IdMensaje == id);

            return View(obj);
        }

        public IActionResult MensajeRescatador(int id)
        {
            List<Mensaje> mensajes = _db.RetornarMensajeRescatador(id);
            if (mensajes.Count == 0)
            {
                TempData["info"] = "No existen Mensajes";
                return RedirectToAction("Index", "Home");
            }
            return View(mensajes);
        }

        public IActionResult MensajesIndividualesRescatador(int id)
        {
            Mensaje obj = _db.Mensajes
                .Include(item => item.Remitente)
                .Include(item => item.Destinatario)
                .Include(item => item.Mascota)
                .Include(item => item.Formulario)
                .FirstOrDefault(item => item.IdMensaje == id);

            return View(obj);
        }

        [HttpPost]
        public IActionResult MensajesIndividualesRescatador(Mensaje msj, string accion)
        {
            Mensaje obj = _db.Mensajes
                .Include(item => item.Remitente)
                .Include(item => item.Destinatario)
                .Include(item => item.Mascota)
                .Include(item => item.Formulario)
                .FirstOrDefault(item => item.IdMensaje == msj.IdMensaje);

            if (obj != null)
            {
                Formulario formulario = obj.Formulario;

                if (formulario != null)
                {
                    if (accion == "Desaprobar")
                    {
                        formulario.Aprobada = "Desaprobada";
                        TempData["info"] = "Solicitud NO APROBADA";
                    }
                    else if (accion == "Aprobar")
                    {
                        TempData["info"] = "Solicitud APROBADA";
                        formulario.Aprobada = "Aprobada";
                    }

                    _db.Formularios.Update(formulario);
                    _db.SaveChanges();
                    return RedirectToAction("MensajeRescatador", new { id = @User.FindFirst("IdUsuario")?.Value });
                }
            }

            return View(obj);
        }
    }
}