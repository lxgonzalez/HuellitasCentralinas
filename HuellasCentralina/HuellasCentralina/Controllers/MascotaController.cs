using HuellasCentralina.Data;
using HuellasCentralina.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
namespace HuellasCentralina.Controllers
{

    public class MascotaController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MascotaController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Index(string sexo, string tamano)
        {
            List<Mascota> list;
            if (!string.IsNullOrEmpty(sexo) && !string.IsNullOrEmpty(tamano))
            {
                list = _db.Mascotas.Where(m => m.Sexo == sexo && m.Tamano == tamano).Include(m => m.Usuario).ToList();
            }
            else if (!string.IsNullOrEmpty(tamano))
            {
                list = _db.Mascotas.Where(m => m.Tamano == tamano).Include(m => m.Usuario).ToList();
            }
            else if (!string.IsNullOrEmpty(sexo))
            {
                list = _db.Mascotas.Where(m => m.Sexo == sexo).Include(m => m.Usuario).ToList();
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

        [Authorize(Roles = "Administrador, Rescatador")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Administrador, Rescatador")]
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
                    return RedirectToAction("Index");
                }

            }
            TempData["error"] = "Mascota No Registrada";
            return View();
        }
        [Authorize(Roles = "Administrador, Rescatador")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Mascota obj = _db.Mascotas.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //[HttpPost]
        //public IActionResult Edit(Mascota obj, IFormFile file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;
        //        if (file != null)
        //        {
        //            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            string mascotaPath = Path.Combine(wwwRootPath, @"images/mascota");


        //            if (!string.IsNullOrEmpty(obj.FotoUrl))
        //            {
        //                var imagenAntigua = Path.Combine(wwwRootPath, obj.FotoUrl.TrimStart('\\'));
        //                if (System.IO.File.Exists(imagenAntigua))
        //                {
        //                    System.IO.File.Delete(imagenAntigua);
        //                }

        //            }
        //            using (var fileStream = new FileStream(Path.Combine(mascotaPath, fileName), FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }

        //            obj.FotoUrl = @"images\mascota\" + fileName;
        //            _db.Mascotas.Update(obj);
        //            _db.SaveChanges();
        //            TempData["success"] = "Mascota Modificada Exitosamente!";
        //            return RedirectToAction("Index");
        //        }

        //    }
        //    TempData["error"] = "Mascota No Modificada";
        //    return View();
        //}
        [Authorize(Roles = "Administrador, Rescatador")]
        [HttpPost]
        public IActionResult Edit(Mascota obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var imagenAntigua1 = obj.FotoUrl;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string mascotaPath = Path.Combine(wwwRootPath, @"images/mascota");
                    var imagenAntigua = Path.Combine(wwwRootPath, obj.FotoUrl.TrimStart('\\'));

                    if (!string.IsNullOrEmpty(obj.FotoUrl))
                    {
                        if (System.IO.File.Exists(imagenAntigua))
                        {
                            System.IO.File.Delete(imagenAntigua);
                        }

                    }
                    using (var fileStream = new FileStream(Path.Combine(mascotaPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.FotoUrl = @"images\mascota\" + fileName;
                }
                else
                {
                    obj.FotoUrl = imagenAntigua1;
                }

                _db.Mascotas.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Mascota Modificada Exitosamente!";
                if (User.IsInRole("Administrador"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("MascotaRescatador", "MascotaUsuario");
                }

            }
            TempData["error"] = "Mascota No Modificada";
            return View();
        }

        [Authorize(Roles = "Administrador, Rescatador")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Mascota obj = _db.Mascotas.Find(id);
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
            _db.Mascotas.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Mascota Eliminada Exitosamente!";

            if (User.IsInRole("Administrador"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("MascotaRescatador", "MascotaUsuario");
            }
        }

        public IActionResult ReporteMascotas()
        {
            List<Mascota> list = _db.Mascotas.Include(u => u.Usuario).ToList();

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
                            col2.Item().Text("Reporte de Mascotas").Bold().FontSize(15);

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
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();

                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#f3969a")
                                .Padding(2).Text("Dueño").FontColor("#fff");

                                header.Cell().Background("#f3969a")
                                .Padding(2).Text("Nombre").FontColor("#fff");

                                header.Cell().Background("#f3969a")
                               .Padding(2).Text("Edad (Mes)").FontColor("#fff");

                                header.Cell().Background("#f3969a")
                               .Padding(2).Text("Sexo").FontColor("#fff");

                                header.Cell().Background("#f3969a")
                               .Padding(2).Text("Tamaño").FontColor("#fff");

                                header.Cell().Background("#f3969a")
                                .Padding(2).Text("Esterilizado").FontColor("#fff");
                            });

                            foreach (var item in list)
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text(item.Usuario.Nombre.ToString() + " " + item.Usuario.Apellido.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text(item.Nombre.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text(item.Edad.ToString()).FontSize(10);

                                if (item.Sexo == "M")
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text("Macho").FontSize(10);
                                }
                                else
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text("Hembra").FontSize(10);
                                }

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text(item.Tamano.ToString()).FontSize(10);

                                if (item.Esterilizado == "S")
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text("Si").FontSize(10);
                                }
                                else
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                         .Padding(2).Text("No").FontSize(10);
                                }

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

            return File(stream, "application/pdf", "ReporteMascotas.pdf");
        }

    }
}
