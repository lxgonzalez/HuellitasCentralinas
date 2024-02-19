using HuellasCentralina.Data;
using HuellasCentralina.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HuellasCentralina.Controllers
{
    public class MascotaUsuarioController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MascotaUsuarioController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "Adoptante")]
        public IActionResult MascotaAdoptante(string sexo, string tamano, string preferencias)
        {
            List<Mascota> list;
            if (!String.IsNullOrEmpty(preferencias))
            {
                Usuario obj = _db.Usuarios.Find(int.Parse(preferencias));
                if(obj.Sexo == null || obj.Tamano ==null || String.Equals(obj.Inmueble,""))
                {
                    TempData["error"] = "Registra tus preferencias";
                    return RedirectToAction("Preferencia", "Acceso", new { id = User.FindFirst("IdUsuario")?.Value });
                }
                list = _db.Mascotas.Include(m => m.Usuario)
                    .Where(m => m.Sexo == obj.Sexo && m.Tamano == obj.Tamano && m.Inmueble == obj.Inmueble).ToList();
            }
            else if (!string.IsNullOrEmpty(sexo) && !string.IsNullOrEmpty(tamano))
            {
                list = _db.Mascotas.Include(m => m.Usuario)
                    .Where(m => m.Sexo == sexo && m.Tamano == tamano).ToList();
            }
            else if (!string.IsNullOrEmpty(tamano))
            {
                list = _db.Mascotas
                    .Include(m => m.Usuario).Where(m => m.Tamano == tamano).ToList();
            }
            else if (!string.IsNullOrEmpty(sexo))
            {
                list = _db.Mascotas.Include(m => m.Usuario).Where(m => m.Sexo == sexo).ToList();
            }
            else
            {
                list = _db.Mascotas.Include(m => m.Usuario).ToList();
            }

            if (list.Count == 0)
            {
                TempData["info"] = "No existen caninos";
            }

            return View(list);
        }

        [Authorize(Roles = "Rescatador")]
        public IActionResult MascotaRescatador()
        {
            var id = int.Parse(@User.FindFirst("IdUsuario")?.Value);

            List<Mascota> list = _db.Mascotas.Where(item => item.IdUsuario == id).ToList();
            if (list.Count == 0)
            {
                TempData["info"] = "Aún no has publicado ningún canino";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(list);
            }
        }
        [Authorize(Roles = "Rescatador")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Rescatador")]
        [HttpPost]
        public IActionResult Create(Mascota obj, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string mascotaPath = Path.Combine(wwwRootPath, @"images/mascota");

                    using (var fileStream = new FileStream(Path.Combine(mascotaPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.FotoUrl = @"images\mascota\" + fileName;
                    obj.IdUsuario = int.Parse(User.FindFirst("IdUsuario")?.Value);
                    _db.Mascotas.Add(obj);
                    _db.SaveChanges();
                    TempData["success"] = "Mascota Registrada Exitosamente!";
                    return RedirectToAction("MascotaRescatador");
                }

            }
            TempData["error"] = "Mascota No Registrada";
            return View();
        }

    }
}
