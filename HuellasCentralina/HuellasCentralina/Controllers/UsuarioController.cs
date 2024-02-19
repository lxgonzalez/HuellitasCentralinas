using HuellasCentralina.Data;
using HuellasCentralina.Models;
using HuellasCentralina.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HuellasCentralina.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UsuarioController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Usuario> list = _db.Usuarios.Include(u => u.Rol).ToList();
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario obj, IFormFile file)
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

                    obj.FotoUrl = @"images\usuario\" + fileName;
                    obj.Clave = Encriptacion.Encriptar(obj.Clave);
                    _db.Usuarios.Add(obj);
                    _db.SaveChanges();
                    TempData["success"] = "Usuario Registrado Exitosamente!";
                    return RedirectToAction("Index");
                }

            }
            TempData["error"] = "Usuario No Registrado";
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Usuario obj = _db.Usuarios.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Usuario obj, IFormFile file)
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
                _db.Usuarios.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Usuario Modificado Exitosamente!";
                return RedirectToAction("Index", "Usuario");

            }
            TempData["error"] = "Usuario No Modificada";
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Usuario obj = _db.Usuarios.Find(id);

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (obj == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(obj.FotoUrl))
            {
                var imagenAntigua = Path.Combine(wwwRootPath, obj.FotoUrl.TrimStart('\\'));

                if (System.IO.File.Exists(imagenAntigua))
                {
                    System.IO.File.Delete(imagenAntigua);
                }

            }
            var mensajesAsociados = _db.Mensajes.Where(m => m.IdRemitente == obj.IdUsuario);
            _db.Mensajes.RemoveRange(mensajesAsociados);
            var mensajesAsociados2 = _db.Mensajes.Where(m => m.IdDestinatario == obj.IdUsuario);
            _db.Mensajes.RemoveRange(mensajesAsociados2);


            _db.Usuarios.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Usuario Eliminado Exitosamente!";
            return RedirectToAction("Index");
        }

        public IActionResult ReporteUsuario()
        {
            List<Usuario> list = _db.Usuarios.Include(u => u.Rol).ToList();

            var rutaImagen = Path.Combine(_webHostEnvironment.WebRootPath, "images/general/logo.png");
            byte[] imagenData = System.IO.File.ReadAllBytes(rutaImagen);
            var nombreUsuario = User.Identity.Name + " " + User.FindFirst("Apellido")?.Value;
            var emailUsuario = User.FindFirst("Email")?.Value;
            string huellitas = "Refugio de caninos \n \"Huellitas Centralinas\"";

            var data = Document.Create(document =>
            {
                document.Page(page =>
                {

                    page.Margin(30);
                   
                    page.Header().ShowOnce().Row(row =>
                    {
                        row.ConstantItem(140).PaddingTop(20).Image(imagenData);


                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text(huellitas).Bold().FontSize(20);
                            col.Item().AlignCenter().Text("Quito, Ecuador").FontSize(10);
                        });

                    });


                    page.Content().PaddingVertical(15).Column(col1 =>
                    {
                        col1.Item().LineHorizontal(0.5f);

                        col1.Item().Column(col2 =>
                        {
                            col2.Item().Text("Reporte de Usuarios").Bold().FontSize(15);

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Hecho Por: ").SemiBold().FontSize(11);
                                txt.Span(nombreUsuario).FontSize(11);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Email: ").SemiBold().FontSize(11);
                                txt.Span(emailUsuario).FontSize(11);
                            });

                        });

                        col1.Item().LineHorizontal(0.5f);

                        col1.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn();
                                columns.RelativeColumn();

                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#78c2ad")
                                .Padding(2).Text("Nombre Completo").FontColor("#fff");

                                header.Cell().Background("#78c2ad")
                               .Padding(2).Text("Email").FontColor("#fff");

                                header.Cell().Background("#78c2ad")
                               .Padding(2).Text("Usuario").FontColor("#fff");

                                header.Cell().Background("#78c2ad")
                               .Padding(2).Text("Rol").FontColor("#fff");
                            });

                            foreach (var item in list)
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text(item.Nombre.ToString() + item.Apellido.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text(item.Email.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text(item.NombreUsuario.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text(item.Rol.Descripcion.ToString()).FontSize(10);

                            }

                        });

                        col1.Spacing(10);
                    });


                    page.Footer()
                    .AlignRight()
                    .Text(txt =>
                    {
                        txt.Span("Pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf();
            Stream stream = new MemoryStream(data);

            return File(stream, "application/pdf", "ReporteUsuarios.pdf");
        }
    }
}
