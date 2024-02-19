using HuellasCentralina.Data;
using HuellasCentralina.Models;
using HuellasCentralina.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace HuellasCentralina.Controllers
{
    public class AccesoController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        

        public AccesoController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Usuario obj)
        {
            obj.Clave = Encriptacion.Encriptar(obj.Clave);

            var usuario = _db.ValidarUsuario(obj.NombreUsuario, obj.Clave);

            if (usuario != null)
            {
                _db.Entry(usuario).Reference(u => u.Rol).Load();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim("Apellido", usuario.Apellido),
                    new Claim("Email", usuario.Email),
                    new Claim("FotoUrl", usuario.FotoUrl),
                    new Claim("Inmueble", usuario.Inmueble),
                    new Claim("IdUsuario", usuario.IdUsuario.ToString())
                };

                if (usuario.Rol != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, usuario.Rol.Descripcion));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                TempData["success"] = "Ingreso Exitoso!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Ingreso Fallido!. Verifique el usuario y la clave";
                return View();
            }
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrarse(Usuario obj, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string usuarioPath = Path.Combine(wwwRootPath, @"images/usuario");

                    using (var fileStream = new FileStream(Path.Combine(usuarioPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Clave = Encriptacion.Encriptar(obj.Clave);
                    obj.FotoUrl = @"images\usuario\" + fileName;
                    _db.Usuarios.Add(obj);
                    _db.SaveChanges();
                    TempData["success"] = "Usuario Registrado Exitosamente!";
                    return RedirectToAction("Index");
                }

            }
            TempData["error"] = "Usuario No Registrado";
            return View();
        }

        public IActionResult Perfil(string id)
        {
            int id_int = int.Parse(id);
            if (id_int == 0)
            {
                return NotFound();
            }
            Usuario obj = _db.Usuarios.Find(id_int);
            if (obj == null)
            {
                return NotFound();
            }
            obj.Clave = Encriptacion.Desencriptar(obj.Clave);

            return View(obj);
        }

        [HttpPost]
        public IActionResult Perfil(Usuario obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var imagenAntigua1 = obj.FotoUrl;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(wwwRootPath, @"images/usuario");
                    var imagenAntigua = Path.Combine(wwwRootPath, obj.FotoUrl.TrimStart('\\'));

                    if (!string.IsNullOrEmpty(obj.FotoUrl))
                    {
                        if (System.IO.File.Exists(imagenAntigua))
                        {
                            System.IO.File.Delete(imagenAntigua);
                        }

                    }
                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.FotoUrl = @"images\usuario\" + fileName;
                }
                else
                {
                    obj.FotoUrl = imagenAntigua1;
                }
                obj.Clave = Encriptacion.Encriptar(obj.Clave);

                _db.Usuarios.Update(obj);
                _db.SaveChanges();
                TempData["info"] = "Vuelva a Ingresar!";
                TempData["success"] = "Usuario Modificado Exitosamente!";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Usuario No Modificada";
            return View();
        }

        public IActionResult RecuperarClave()
        {
            TempData["info"] = "Ingrese los datos para recuperar la clave";
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarClave(Usuario obj)
        {

            if (!String.IsNullOrEmpty(obj.Nombre) && !String.IsNullOrEmpty(obj.Apellido) && !String.IsNullOrEmpty(obj.Email) && !String.IsNullOrEmpty(obj.NombreUsuario))
            {
                Usuario u = _db.ValidarCampos(obj.Nombre, obj.Apellido, obj.Email, obj.NombreUsuario);
                if (u != null)
                {
                    obj.Clave = u.Clave;
                    obj.Clave = Encriptacion.Desencriptar(obj.Clave);

                    return View(obj);
                }
                else
                {
                    TempData["error"] = "Usuario No Encontrado";
                    return RedirectToAction("RecuperarClave");
                }
            }
            TempData["error"] = "Ingrese todos los datos";
            return View();

        }

        public IActionResult Preferencia(string id)
        {
            int id_int = int.Parse(id);
            if (id_int == 0)
            {
                return NotFound();
            }
            Usuario obj = _db.Usuarios.Find(id_int);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Preferencia(Usuario obj)
        {
            if (ModelState.IsValid)
            {
                _db.Usuarios.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Preferencias Modificadas!";
                return RedirectToAction("Index", "Home");

            }
            TempData["error"] = "Preferencias No Modificadas";
            return View();
        }

        
    }
}

