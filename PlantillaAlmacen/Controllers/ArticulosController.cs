using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantillaAlmacen.Models;

namespace PlantillaAlmacen.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly AlmacenDbContext _context;
        public ArticulosController(AlmacenDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string ordenacion, string buscador, string Buscador, int? numeroPagina, int? tamanoPagina)
        {
            ViewData["Ordenacion"] = ordenacion;
            ViewData["OrdenacionProducto"] = string.IsNullOrEmpty(ordenacion) ? "producto_desc" : "";

            if (buscador != null)
                numeroPagina = 1;
            else
                buscador = Buscador;

            buscador = (buscador ?? "").Trim();
            ViewData["Buscador"] = buscador;

            int tamPag = tamanoPagina ?? 10;
            ViewData["TamanoPagina"] = tamPag;

            var q = _context.Articulos
                .AsNoTracking()
                .Include(a => a.CatalogoArticulo).ThenInclude(c => c.Categoria)
                .Include(a => a.EstatusArticulo)
                .Include(a => a.Almacen)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscador))
            {
                q = q.Where(a =>
                    EF.Functions.Like(a.CatalogoArticulo.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(a.CatalogoArticulo.Categoria.Nombre, $"%{buscador}%") ||
                    EF.Functions.Like(a.Observacion, $"%{buscador}%")
                );
            }

            switch (ordenacion)
            {
                case "producto_desc":
                    q = q.OrderByDescending(a => a.CatalogoArticulo.Nombre);
                    break;
                default:
                    q = q.OrderBy(a => a.CatalogoArticulo.Nombre);
                    break;
            }

            ViewBag.Categorias = new SelectList(
                _context.Categorias
                    .Where(c => c.Estatus)
                    .OrderBy(c => c.Nombre)
                    .ToList(),
                "IdCategoria",
                "Nombre"
            );

            ViewBag.Estatuses = new SelectList(
                _context.EstatusArticulos
                    .Where(e => e.Estatus)
                    .OrderBy(e => e.Nombre)
                    .ToList(),
                "IdEstatusArticulo",
                "Nombre"
            );

            var pagina = await Paginacion<Articulo>.CreateAsync(q, numeroPagina ?? 1, tamPag);
            return View(pagina);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .FirstOrDefaultAsync(m => m.IdArticulo == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }
        public IActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias.Where(c => c.Estatus).ToList(), "IdCategoria", "Nombre");
            ViewBag.Estatuses = new SelectList(_context.EstatusArticulos.Where(e => e.Estatus).ToList(), "IdEstatusArticulo", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCatalogoArticulo,IdEstatusArticulo,Marca,Modelo,NumeroSerie,Caracterisiticas,Observacion")] Articulo nuevo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos inválidos.");
            }

            nuevo.FechaAlta = DateTime.Now;

            try
            {
                _context.Add(nuevo);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException)
            {
                return BadRequest("No se pudo crear el artículo. Intenta nuevamente.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTecnologia(ArticuloTecnologia model)
        {
            ModelState.Remove("CatalogoArticulo");
            ModelState.Remove("EstatusArticulo");
            ModelState.Remove("NumeroInventario");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!Prefijos.TryGetValue(model.IdCatalogoArticulo, out var prefijo))
                return BadRequest("No existe prefijo definido para este tipo de artículo.");

            var idsMismoPrefijo = Prefijos
                .Where(kv => kv.Value == prefijo)
                .Select(kv => kv.Key)
                .ToList();

            async Task<string> GenerarNumeroInventarioAsync()
            {
                var ultimo = await _context.Articulos
                    .Where(a => idsMismoPrefijo.Contains(a.IdCatalogoArticulo)
                             && a.NumeroInventario.StartsWith(prefijo))
                    .OrderByDescending(a => a.NumeroInventario)
                    .Select(a => a.NumeroInventario)
                    .FirstOrDefaultAsync();

                int consecutivo = 1;
                if (!string.IsNullOrEmpty(ultimo))
                {
                    var numeroStr = new string(ultimo.Skip(prefijo.Length).ToArray());
                    if (int.TryParse(numeroStr, out int num))
                        consecutivo = num + 1;
                }

                return $"{prefijo}{consecutivo:D3}";
            }

            model.IdEstatusArticulo = 2;

            for (int intento = 0; intento < 2; intento++)
            {
                model.NumeroInventario = await GenerarNumeroInventarioAsync();
                _context.Articulos.Add(model);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    _context.Entry(model).State = EntityState.Detached;

                    if (intento == 0)
                    {
                        model.IdArticulo = 0;
                        continue;
                    }
                    return BadRequest("Error al guardar: el número de inventario se duplicó. Inténtalo de nuevo.");
                }
            }
            return BadRequest("No se pudo crear el artículo.");
        }

        private static readonly Dictionary<int, string> Prefijos = new Dictionary<int, string>
        {
            { 2008, "C" },
            { 2009, "CPU" },
            { 2010, "IM" },
            { 3008, "M" },
            { 3009, "T" },
            { 3010, "DIS" },
            { 3011, "DIS" },
            { 3012, "DIS" },
            { 3013, "USB" },
            { 3014, "USB" },
            { 3015, "MOUSE" }
        };

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehiculo(ArticuloVehiculo model)
        {
            ModelState.Remove("CatalogoArticulo");
            ModelState.Remove("EstatusArticulo");
            ModelState.Remove("NumeroInventario");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            string prefijo;
            int consecutivoInicial;

            if (model.Combustible != null && model.Combustible.ToUpper().Contains("GASOLINA"))
            {
                prefijo = "G";
                consecutivoInicial = 101;
            }
            else if (model.Combustible != null && model.Combustible.ToUpper().Contains("DIESEL"))
            {
                prefijo = "#";
                consecutivoInicial = 1;
            }
            else
            {
                return BadRequest("El tipo de combustible no es válido para generar número de inventario.");
            }

            async Task<string> GenerarNumeroInventarioAsync()
            {
                var ultimo = await _context.Articulos
                    .Where(a => a is ArticuloVehiculo && a.NumeroInventario.StartsWith(prefijo))
                    .OrderByDescending(a => a.NumeroInventario)
                    .Select(a => a.NumeroInventario)
                    .FirstOrDefaultAsync();

                int consecutivo = consecutivoInicial;

                if (!string.IsNullOrEmpty(ultimo))
                {
                    var numeroStr = new string(ultimo.Skip(prefijo.Length).ToArray());
                    if (int.TryParse(numeroStr, out int num))
                        consecutivo = num + 1;
                }

                return prefijo == "G"
                    ? $"{prefijo}{consecutivo}"
                    : $"{prefijo}{consecutivo:D2}";
            }

            model.IdEstatusArticulo = 2;

            for (int intento = 0; intento < 2; intento++)
            {
                model.NumeroInventario = await GenerarNumeroInventarioAsync();
                _context.Articulos.Add(model);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    _context.Entry(model).State = EntityState.Detached;

                    if (intento == 0)
                    {
                        model.IdArticulo = 0;
                        continue;
                    }
                    return BadRequest("Error: el número de inventario ya existe. Intenta de nuevo.");
                }
            }

            return BadRequest("No se pudo crear el vehículo.");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCombustible(ArticuloCombustible model)
        {
            ModelState.Remove("CatalogoArticulo");
            ModelState.Remove("EstatusArticulo");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.IdEstatusArticulo = 2;
            _context.Articulos.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHerramienta(ArticuloHerramienta model)
        {
            ModelState.Remove("CatalogoArticulo");
            ModelState.Remove("EstatusArticulo");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.IdEstatusArticulo = 2;
            _context.Articulos.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComunicaciones(ArticuloComunicaciones model)
        {
            ModelState.Remove("CatalogoArticulo");
            ModelState.Remove("EstatusArticulo");
            ModelState.Remove("NumeroInventario");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string prefijo = "R";
            int consecutivoInicial = 1;

            async Task<string> GenerarNumeroInventarioAsync()
            {
                var ultimo = await _context.Articulos
                    .Where(a => a is ArticuloComunicaciones && a.NumeroInventario.StartsWith(prefijo))
                    .OrderByDescending(a => a.NumeroInventario)
                    .Select(a => a.NumeroInventario)
                    .FirstOrDefaultAsync();

                int consecutivo = consecutivoInicial;

                if (!string.IsNullOrEmpty(ultimo))
                {
                    var numeroStr = new string(ultimo.Skip(prefijo.Length).ToArray());
                    if (int.TryParse(numeroStr, out int num))
                        consecutivo = num + 1;
                }

                // R001, R002, etc.
                return $"{prefijo}{consecutivo:D3}";
            }

            model.IdEstatusArticulo = 2;

            for (int intento = 0; intento < 2; intento++)
            {
                model.NumeroInventario = await GenerarNumeroInventarioAsync();
                _context.Articulos.Add(model);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    _context.Entry(model).State = EntityState.Detached;

                    if (intento == 0)
                    {
                        model.IdArticulo = 0;
                        continue;
                    }
                    return BadRequest("Error: el número de inventario ya existe. Intenta de nuevo.");
                }
            }

            return BadRequest("No se pudo crear el artículo de comunicaciones.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePapeleria(ArticuloPapeleria model)
        {
            ModelState.Remove("CatalogoArticulo");
            ModelState.Remove("EstatusArticulo");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.IdEstatusArticulo = 2;
            _context.Articulos.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEnfermeria(ArticuloEnfermeria model)
        {
            ModelState.Remove("CatalogoArticulo");
            ModelState.Remove("EstatusArticulo");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.IdEstatusArticulo = 2;
            _context.Articulos.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest("Falta id.");

            var art = await _context.Articulos
                .Include(a => a.CatalogoArticulo)
                    .ThenInclude(c => c.Categoria)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.IdArticulo == id);

            if (art == null) return NotFound();

            var estatus = await _context.EstatusArticulos
                .Where(e => new[] { 2, 8, 6, 5, 4 }.Contains(e.IdEstatusArticulo))
                .Select(e => new
                {
                    idEstatusArticulo = e.IdEstatusArticulo,
                    nombre = e.Nombre ?? string.Empty
                })
                .ToListAsync();

            var result = new Dictionary<string, object?>
            {
                ["idArticulo"] = art.IdArticulo,
                ["idCatalogoArticulo"] = art.IdCatalogoArticulo,
                ["producto"] = art.CatalogoArticulo?.Nombre,
                ["categoria"] = art.CatalogoArticulo?.Categoria?.Nombre,
                ["idEstatusArticulo"] = art.IdEstatusArticulo,
                ["observacion"] = art.Observacion,
                ["estatus"] = estatus
            };

            switch (art)
            {
                case ArticuloTecnologia tec:
                    result["marca"] = tec.Marca;
                    result["modelo"] = tec.Modelo;
                    result["numeroSerie"] = tec.NumeroSerie;
                    result["caracteristicas"] = tec.Caracteristicas;
                    break;

                case ArticuloVehiculo veh:
                    result["marca"] = veh.Marca;
                    result["modelo"] = veh.Modelo;
                    result["numeroSerie"] = veh.NumeroSerie;
                    result["placas"] = veh.Placas;
                    result["tarjetaCirculacion"] = veh.TarjetaCirculacion;
                    result["polizaSeguro"] = veh.PolizaSeguro;
                    break;

                case ArticuloCombustible comb:
                    result["tipo"] = comb.Tipo;
                    result["cantidad"] = comb.Cantidad;
                    result["destino"] = comb.Destino;
                    result["fechaSalida"] = comb.FechaSalida.ToString("yyyy-MM-dd");
                    break;

                case ArticuloHerramienta her:
                    result["marca"] = her.Marca;
                    result["modelo"] = her.Modelo;
                    result["numeroSerie"] = her.NumeroSerie;
                    break;

                case ArticuloPapeleria pap:
                    result["unidadMedida"] = pap.UnidadMedida;
                    result["cantidadPaquete"] = pap.CantidadPaquete;
                    break;

                case ArticuloEnfermeria enf:
                    result["fechaCaducidad"] = enf.FechaCaducidad.ToString("yyyy-MM-dd");
                    result["lote"] = enf.Lote;
                    break;
                case ArticuloComunicaciones com:
                    result["marca"] = com.Marca;
                    result["modelo"] = com.Modelo;
                    result["numeroSerie"] = com.NumeroSerie;
                    break;
            }

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, [FromForm] IFormCollection form)
        {
            var art = await _context.Articulos.FindAsync(id);
            if (art == null) return NotFound("Artículo no encontrado.");

            art.IdEstatusArticulo = int.Parse(form["IdEstatusArticulo"]);
            art.Observacion = form["Observacion"];

            switch (art)
            {
                case ArticuloTecnologia tec:
                    tec.Marca = form["Marca"];
                    tec.Modelo = form["Modelo"];
                    tec.NumeroSerie = form["NumeroSerie"];

                    var extras = form.Keys
                        .Where(k => k.StartsWith("extra["))
                        .ToDictionary(
                            k => k.Replace("extra[", "").Replace("]", ""),
                            k => form[k].ToString()
                        );

                    if (extras.Count > 0)
                    {
                        tec.Caracteristicas = JsonSerializer.Serialize(extras);
                    }
                    else if (!string.IsNullOrWhiteSpace(form["Caracteristicas"]))
                    {
                        tec.Caracteristicas = form["Caracteristicas"];
                    }
                    else
                    {
                        _context.Entry(tec).Property(x => x.Caracteristicas).IsModified = false;
                    }
                    break;

                case ArticuloVehiculo veh:
                    veh.Marca = form["Marca"];
                    veh.Modelo = form["Modelo"];
                    veh.NumeroSerie = form["NumeroSerie"];
                    veh.Placas = form["Placas"];
                    veh.TarjetaCirculacion = form["TarjetaCirculacion"];
                    veh.PolizaSeguro = form["PolizaSeguro"];
                    break;

                case ArticuloCombustible comb:
                    comb.Tipo = form["Tipo"];
                    comb.Cantidad = decimal.Parse(form["Cantidad"]);
                    comb.Destino = form["Destino"];
                    comb.FechaSalida = DateTime.Parse(form["FechaSalida"]);
                    break;

                case ArticuloHerramienta her:
                    her.Marca = form["Marca"];
                    her.Modelo = form["Modelo"];
                    her.NumeroSerie = form["NumeroSerie"];
                    break;

                case ArticuloPapeleria pap:
                    pap.UnidadMedida = form["UnidadMedida"];
                    pap.CantidadPaquete = int.Parse(form["CantidadPaquete"]);
                    break;

                case ArticuloEnfermeria enf:
                    enf.FechaCaducidad = DateTime.Parse(form["FechaCaducidad"]);
                    enf.Lote = form["Lote"];
                    break;
                case ArticuloComunicaciones com:
                    com.Marca = form["Marca"];
                    com.Modelo = form["Modelo"];
                    com.NumeroSerie = form["NumeroSerie"];
                    break;

            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .FirstOrDefaultAsync(m => m.IdArticulo == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo != null)
            {
                _context.Articulos.Remove(articulo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticuloExists(int id)
        {
            return _context.Articulos.Any(e => e.IdArticulo == id);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsJson(int? id)
        {
            if (id == null) return BadRequest();

            var art = await _context.Articulos
                .AsNoTracking()
                .Include(a => a.CatalogoArticulo)
                    .ThenInclude(p => p.Categoria)
                .Include(a => a.EstatusArticulo)
                .Include(a => a.Almacen)
                .Include(a => a.Factura) // 👈 IMPORTANTE
                .FirstOrDefaultAsync(a => a.IdArticulo == id);

            if (art == null) return NotFound();

            var fechaAltaStr = art.FechaAlta != default(DateTime)
                ? art.FechaAlta.ToString("yyyy-MM-dd")
                : "";

            var result = new Dictionary<string, object?>
            {
                ["idArticulo"] = art.IdArticulo,
                ["producto"] = art.CatalogoArticulo?.Nombre,
                ["categoria"] = art.CatalogoArticulo?.Categoria?.Nombre,
                ["observacion"] = art.Observacion,
                ["fechaAlta"] = fechaAltaStr,
                ["estatusNombre"] = art.EstatusArticulo?.Nombre,
                ["precio"] = art.Precio,
                ["fechaCompra"] = art.FechaCompra.ToString("yyyy-MM-dd"),
                ["idFactura"] = art.IdFactura, // 👈 Para saber si tiene factura
                ["numeroFactura"] = art.Factura?.NumeroFactura // 👈 Para mostrar
            };

            switch (art)
            {
                case ArticuloTecnologia tec:
                    result["marca"] = tec.Marca;
                    result["modelo"] = tec.Modelo;
                    result["numeroSerie"] = tec.NumeroSerie;
                    result["caracteristicas"] = tec.Caracteristicas;
                    result["almacen"] = tec.Almacen?.Nombre;
                    break;

                case ArticuloVehiculo veh:
                    result["marca"] = veh.Marca;
                    result["modelo"] = veh.Modelo;
                    result["numeroSerie"] = veh.NumeroSerie;
                    result["placas"] = veh.Placas;
                    result["tarjetaCirculacion"] = veh.TarjetaCirculacion;
                    result["polizaSeguro"] = veh.PolizaSeguro;
                    break;

                case ArticuloCombustible comb:
                    result["tipo"] = comb.Tipo;
                    result["cantidad"] = comb.Cantidad;
                    result["destino"] = comb.Destino;
                    result["fechaSalida"] = comb.FechaSalida.ToString("yyyy-MM-dd");
                    break;

                case ArticuloHerramienta her:
                    result["marca"] = her.Marca;
                    result["modelo"] = her.Modelo;
                    result["numeroSerie"] = her.NumeroSerie;
                    result["almacen"] = her.Almacen?.Nombre;
                    break;

                case ArticuloPapeleria pap:
                    result["unidadMedida"] = pap.UnidadMedida;
                    result["cantidadPaquete"] = pap.CantidadPaquete;
                    result["almacen"] = pap.Almacen?.Nombre;
                    break;

                case ArticuloEnfermeria enf:
                    result["fechaCaducidad"] = enf.FechaCaducidad.ToString("yyyy-MM-dd");
                    result["lote"] = enf.Lote;
                    result["almacen"] = enf.Almacen?.Nombre;
                    break;
                case ArticuloComunicaciones com:
                    result["marca"] = com.Marca;
                    result["modelo"] = com.Modelo;
                    result["numeroSerie"] = com.NumeroSerie;
                    result["almacen"] = com.Almacen?.Nombre;
                    break;
            }

            return Json(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportarExcel(int IdCategoria, IFormFile ArchivoExcel)
        {
            if (ArchivoExcel == null || ArchivoExcel.Length == 0)
                return BadRequest("Debe seleccionar un archivo Excel.");

            using var stream = new MemoryStream();
            await ArchivoExcel.CopyToAsync(stream);
            using var workbook = new XLWorkbook(stream);
            var ws = workbook.Worksheet(1);

            var firstRow = 8;
            var firstCol = 2;
            var lastRow = ws.LastRowUsed().RowNumber();
            var lastCol = ws.LastColumnUsed().ColumnNumber();

            var rows = ws.Range(firstRow + 1, firstCol, lastRow, lastCol).RowsUsed();

            var errores = new List<(int fila, string mensaje)>();

            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var row in rows)
                {
                    switch (IdCategoria)
                    {
                        case 1:
                            string numInventario = row.Cell(1).GetString()?.Trim();
                            string valorCelda = row.Cell(5).GetString().Trim().ToUpper();

                            if (string.IsNullOrEmpty(numInventario))
                            {
                                errores.Add((row.RowNumber(), "No tiene número de inventario"));
                                continue;
                            }

                            if (await _context.Articulos.AnyAsync(a => a.NumeroInventario == numInventario))
                            {
                                errores.Add((row.RowNumber(), $"El número de inventario '{numInventario}' ya existe"));
                                continue;
                            }

                            var catalogo = await _context.CatalogoArticulos
                                .FirstOrDefaultAsync(c => valorCelda.Contains(c.Nombre.ToUpper()));

                            if (catalogo == null)
                            {
                                errores.Add((row.RowNumber(), $"No existe catálogo para '{valorCelda}'"));
                                continue;
                            }

                            var marcasTec = new List<string> { "HP", "DELL", "LENOVO", "ACER", "ASUS", "APPLE", "SAMSUNG", "KONICA", "ASPIRE", "TVF", "MSI", "VICTUS", "LG", "AOC", "GETECH", "LOGITECH", "BROTHER", "EBRA", "DJI", "ADATA", "STF", "NOVOTEK", "GREEN LEAF" };
                            string marca = marcasTec.FirstOrDefault(m => valorCelda.Contains(m)) ?? "DESCONOCIDA";

                            string modelo = "";
                            if (marca != "DESCONOCIDA")
                            {
                                int index = valorCelda.IndexOf(marca);
                                if (index >= 0)
                                    modelo = valorCelda.Substring(index + marca.Length).Trim();
                            }

                            var artTec = new ArticuloTecnologia
                            {
                                IdCatalogoArticulo = catalogo.IdCatalogoArticulo,
                                Marca = marca,
                                Modelo = modelo,
                                NumeroInventario = numInventario,
                                NumeroSerie = row.Cell(6).GetString(),
                                Caracteristicas = modelo,
                                Precio = 0,
                                FechaCompra = DateTime.Now,
                                IdEstatusArticulo = 2,
                                IdAlmacen = 1
                            };

                            _context.Articulos.Add(artTec);
                            break;

                        case 3:
                            string numInvVeh = row.Cell(1).GetString()?.Trim();
                            string valorVeh = row.Cell(4).GetString().Trim().ToUpper();

                            if (string.IsNullOrEmpty(numInvVeh))
                            {
                                errores.Add((row.RowNumber(), "No tiene número de inventario"));
                                continue;
                            }

                            if (await _context.Articulos.AnyAsync(a => a.NumeroInventario == numInvVeh))
                            {
                                errores.Add((row.RowNumber(), $"El número de inventario '{numInvVeh}' ya existe"));
                                continue;
                            }

                            var catalogoVeh = await _context.CatalogoArticulos
                                .FirstOrDefaultAsync(c => valorVeh.Contains(c.Nombre.ToUpper()));

                            if (catalogoVeh == null)
                            {
                                errores.Add((row.RowNumber(), $"No existe catálogo para '{valorVeh}' regístrelo en el sistema"));
                                continue;
                            }

                            var marcasVeh = new List<string> { "NISSAN", "TOYOTA", "DODGE", "FORD", "CHRYSLER", "CHEVROLET", "VELOCI", "ITALIKA", "MITSUBISHI" };
                            string marcaV = marcasVeh.FirstOrDefault(m => valorVeh.Contains(m)) ?? "DESCONOCIDA";

                            string modeloV = "";
                            if (marcaV != "DESCONOCIDA")
                            {
                                int index = valorVeh.IndexOf(marcaV);
                                if (index >= 0)
                                    modeloV = valorVeh.Substring(index + marcaV.Length).Trim();
                            }

                            var artVeh = new ArticuloVehiculo
                            {
                                IdCatalogoArticulo = catalogoVeh.IdCatalogoArticulo,
                                Marca = marcaV,
                                Modelo = modeloV,
                                NumeroInventario = numInvVeh,
                                NumeroSerie = row.Cell(5).GetString(),
                                Placas = row.Cell(6).GetString(),
                                Color = row.Cell(7).GetString(),
                                Anio = int.TryParse(row.Cell(8).GetString(), out var anio) ? anio : 0,
                                Combustible = row.Cell(9).GetString(),
                                Transmision = row.Cell(10).GetString(),
                                Precio = decimal.TryParse(row.Cell(11).GetString(), out var precioVeh) ? precioVeh : 0,
                                FechaCompra = DateTime.Now,
                                Observacion = row.Cell(12).GetString(),
                                IdEstatusArticulo = 2
                            };

                            _context.Articulos.Add(artVeh);
                            break;

                        case 2:
                            string numInvHerr = row.Cell(1).GetString()?.Trim();

                            if (string.IsNullOrEmpty(numInvHerr))
                            {
                                errores.Add((row.RowNumber(), "No tiene número de inventario"));
                                continue;
                            }

                            if (await _context.Articulos.AnyAsync(a => a.NumeroInventario == numInvHerr))
                            {
                                errores.Add((row.RowNumber(), $"El número de inventario '{numInvHerr}' ya existe"));
                                continue;
                            }

                            int idCatalogoHerr;
                            if (!int.TryParse(row.Cell(2).GetString(), out idCatalogoHerr) ||
                                !await _context.CatalogoArticulos.AnyAsync(c => c.IdCatalogoArticulo == idCatalogoHerr))
                            {
                                errores.Add((row.RowNumber(), "Catálogo de herramienta no válido"));
                                continue;
                            }

                            var artHerr = new ArticuloHerramienta
                            {
                                IdCatalogoArticulo = idCatalogoHerr,
                                NumeroInventario = numInvHerr,
                                Marca = row.Cell(3).GetString(),
                                Modelo = row.Cell(4).GetString(),
                                NumeroSerie = row.Cell(5).GetString(),
                                Precio = decimal.TryParse(row.Cell(6).GetString(), out var precioHerr) ? precioHerr : 0,
                                FechaCompra = DateTime.Now,
                                Observacion = row.Cell(7).GetString(),
                                IdEstatusArticulo = 2
                            };

                            _context.Articulos.Add(artHerr);
                            break;
                    }
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                return BadRequest("No se pudo completar la importación.");
            }

            if (errores.Any())
            {
                return Json(new
                {
                    ok = false,
                    mensaje = "Algunos artículos no se registraron",
                    errores = errores.Select(e => new { fila = e.fila, mensaje = e.mensaje })
                });
            }

            return Ok(new { ok = true, mensaje = "Artículos importados correctamente" });
        }


        [HttpGet]
        public async Task<IActionResult> ConsultaVehiculos(string buscador, int? numeroPagina, int? tamanoPagina)
        {
            buscador = (buscador ?? "").Trim();
            ViewData["Buscador"] = buscador;

            int tamPag = tamanoPagina ?? 10;
            ViewData["TamanoPagina"] = tamPag;

            var q = _context.Articulos
                .OfType<ArticuloVehiculo>()
                .AsNoTracking()
                .Include(v => v.CatalogoArticulo).ThenInclude(c => c.Categoria)
                .Include(v => v.EstatusArticulo)
                .Include(v => v.Asignaciones).ThenInclude(a => a.Personal)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscador))
            {
                q = q.Where(v =>
                    EF.Functions.Like(v.Marca, $"%{buscador}%") ||
                    EF.Functions.Like(v.Modelo, $"%{buscador}%") ||
                    EF.Functions.Like(v.Placas, $"%{buscador}%") ||
                    EF.Functions.Like(v.NumeroSerie, $"%{buscador}%") ||
                    EF.Functions.Like(v.CatalogoArticulo.Nombre, $"%{buscador}%")
                );
            }

            q = q.OrderBy(v => v.Marca).ThenBy(v => v.Modelo);

            var pagina = await Paginacion<ArticuloVehiculo>.CreateAsync(q, numeroPagina ?? 1, tamPag);
            return View(pagina);
        }

        [HttpGet]
        public IActionResult ExportarVehiculos()
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Vehículos");

            var rutaLogoIzq = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoDefensa.png");
            var rutaLogoDer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoFrente.png");

            if (System.IO.File.Exists(rutaLogoIzq))
                ws.AddPicture(rutaLogoIzq).MoveTo(ws.Cell("A1")).Scale(0.2);

            if (System.IO.File.Exists(rutaLogoDer))
                ws.AddPicture(rutaLogoDer).MoveTo(ws.Cell("M1")).Scale(0.6);

            var titulo = ws.Range("C2:M2");
            titulo.Merge().Value = "INVENTARIO DE VEHÍCULOS GASOLINA";
            titulo.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            titulo.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            titulo.Style.Font.Bold = true;
            titulo.Style.Font.FontSize = 16;
            titulo.Style.Fill.BackgroundColor = XLColor.FromArgb(0, 0, 128);
            titulo.Style.Font.FontColor = XLColor.White;

            var subtitulo = ws.Range("C3:M3");
            subtitulo.Merge().Value = "REUBICACIÓN DE LAS VÍAS FÉRREAS EN NOGALES, SONORA";
            subtitulo.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            subtitulo.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            subtitulo.Style.Font.Bold = true;
            subtitulo.Style.Font.FontSize = 12;
            subtitulo.Style.Fill.BackgroundColor = XLColor.FromArgb(25, 25, 112);
            subtitulo.Style.Font.FontColor = XLColor.White;

            var headers = new string[]
            {
              "N° INVENTARIO", "NOMBRE", "MARCA", "MODELO", "COLOR", "AÑO",
              "TIPO", "PLACAS", "SERIE", "PRECIO", "FECHA COMPRA", "ESTATUS", "ASIGNADO A"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(5, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.DarkBlue;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.OutsideBorderColor = XLColor.Black;
            }

            var vehiculos = _context.Articulos
                .OfType<ArticuloVehiculo>()
                .Include(v => v.EstatusArticulo)
                .Include(v => v.CatalogoArticulo).ThenInclude(c => c.Categoria)
                .Include(v => v.Asignaciones).ThenInclude(a => a.Personal)
                .ToList();

            int fila = 6;
            foreach (var v in vehiculos)
            {
                ws.Cell(fila, 1).Value = v.NumeroInventario;
                ws.Cell(fila, 2).Value = v.CatalogoArticulo?.Nombre ?? "—";
                ws.Cell(fila, 3).Value = v.Marca;
                ws.Cell(fila, 4).Value = v.Modelo;
                ws.Cell(fila, 5).Value = v.Color;
                ws.Cell(fila, 6).Value = v.Anio;
                ws.Cell(fila, 7).Value = v.CatalogoArticulo?.Categoria?.Nombre ?? "—";
                ws.Cell(fila, 8).Value = v.Placas;
                ws.Cell(fila, 9).Value = v.NumeroSerie;

                ws.Cell(fila, 10).Value = v.Precio;
                ws.Cell(fila, 10).Style.NumberFormat.Format = "$ #,##0.00";

                ws.Cell(fila, 11).Value = v.FechaCompra;
                ws.Cell(fila, 11).Style.DateFormat.Format = "dd/MM/yyyy";

                ws.Cell(fila, 12).Value = v.EstatusArticulo?.Nombre ?? "—";

                var asignacion = v.Asignaciones?.OrderByDescending(a => a.FechaAsignacion).FirstOrDefault(a => a.FechaDevolucion == null);
                ws.Cell(fila, 13).Value = asignacion?.Personal?.Nombre ?? "No asignado";

                if (fila % 2 == 0)
                {
                    ws.Range(fila, 1, fila, headers.Length).Style.Fill.BackgroundColor = XLColor.White;
                }
                else
                {
                    ws.Range(fila, 1, fila, headers.Length).Style.Fill.BackgroundColor = XLColor.FromArgb(245, 245, 245); // gris claro
                }

                fila++;
            }

            var rangoTabla = ws.Range(5, 1, fila - 1, headers.Length);
            rangoTabla.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rangoTabla.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            rangoTabla.Style.Border.OutsideBorderColor = XLColor.Black;
            rangoTabla.Style.Border.InsideBorderColor = XLColor.Gray;

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Vehiculos.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> ConsultaTecnologia(string buscador, int? numeroPagina, int? tamanoPagina)
        {
            buscador = (buscador ?? "").Trim();
            ViewData["Buscador"] = buscador;

            int tamPag = tamanoPagina ?? 10;
            ViewData["TamanoPagina"] = tamPag;

            var q = _context.Articulos
                .OfType<ArticuloTecnologia>()
                .AsNoTracking()
                .Include(t => t.CatalogoArticulo).ThenInclude(c => c.Categoria)
                .Include(t => t.EstatusArticulo)
                .Include(t => t.Asignaciones).ThenInclude(a => a.Personal)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscador))
            {
                q = q.Where(t =>
                    EF.Functions.Like(t.Marca, $"%{buscador}%") ||
                    EF.Functions.Like(t.Modelo, $"%{buscador}%") ||
                    EF.Functions.Like(t.NumeroSerie, $"%{buscador}%") ||
                    EF.Functions.Like(t.Caracteristicas, $"%{buscador}%") ||
                    EF.Functions.Like(t.CatalogoArticulo.Nombre, $"%{buscador}%")
                );
            }
            q = q.OrderBy(t => t.Marca).ThenBy(t => t.Modelo);
            var pagina = await Paginacion<ArticuloTecnologia>.CreateAsync(q, numeroPagina ?? 1, tamPag);
            return View(pagina);
        }

        [HttpGet]
        public IActionResult ExportarTecnologia()
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Inventario");

            ws.Rows(2, 7).Height = 24;
            ws.Row(8).Height = 22;

            ws.Column("A").Width = 2;
            ws.Column("B").Width = 16;
            ws.Column("C").Width = 16;
            ws.Column("D").Width = 12;
            ws.Column("E").Width = 18;
            ws.Column("F").Width = 5;
            ws.Column("G").Width = 18;
            ws.Column("H").Width = 18;
            ws.Column("I").Width = 12;
            ws.Column("J").Width = 18;
            ws.Column("K").Width = 2;

            var marcoLogoIzq = ws.Range("B2:C7");
            marcoLogoIzq.Merge();
            marcoLogoIzq.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            var rutaLogoIzq = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoFrente.png");
            if (System.IO.File.Exists(rutaLogoIzq))
            {
                var picIzq = ws.AddPicture(rutaLogoIzq)
                               .MoveTo(ws.Cell("B2"), 8, 8)
                               .Scale(0.9);
            }

            var marcoCentral = ws.Range("D2:H6");
            marcoCentral.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            var titulo = ws.Range("D3:H3");
            titulo.Merge().Value = "INVENTARIO DE DISPOSITIVOS DE COMPUTO";
            titulo.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            titulo.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            titulo.Style.Font.Bold = true;
            titulo.Style.Font.FontSize = 14;

            var subtitulo = ws.Range("D4:H4");
            subtitulo.Merge().Value = "REUBICACIÓN DE LAS VÍAS FERREAS EN NOGALES, SONORA.";
            subtitulo.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            subtitulo.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            subtitulo.Style.Font.Bold = true;
            subtitulo.Style.Font.FontSize = 12;

            var marcoDer = ws.Range("I2:J7");
            marcoDer.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            var bloqueLogoDer = ws.Range("I2:J3");
            bloqueLogoDer.Merge();
            bloqueLogoDer.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            var rutaLogoDer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoDefensa.png");
            if (System.IO.File.Exists(rutaLogoDer))
            {
                var picDer = ws.AddPicture(rutaLogoDer)
                               .MoveTo(ws.Cell("I2"), 6, 6)
                               .Scale(0.5);
            }

            var banda1 = ws.Range("I5:I5");
            banda1.Merge().Value = "ETIQUETADO CUMPLE CON TODO";
            banda1.Style.Fill.BackgroundColor = XLColor.Yellow;
            banda1.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            banda1.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            banda1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            var banda2 = ws.Range("I6:I6");
            banda2.Merge().Value = "ACTIVIDAD PENDIENTE";
            banda2.Style.Fill.BackgroundColor = XLColor.Red;
            banda2.Style.Font.FontColor = XLColor.White;
            banda2.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            banda2.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            banda2.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            var banda3 = ws.Range("I7:I7");
            banda3.Merge().Value = "DISPOSITIVO INHABIL";
            banda3.Style.Fill.BackgroundColor = XLColor.Orange;
            banda3.Style.Font.FontColor = XLColor.White;
            banda3.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            banda3.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            banda3.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            ws.Cell("J5").Value = "CÓDIGO:";
            ws.Cell("J5").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("J6").Value = $"FECHA: {DateTime.Now:dd/MM/yyyy}";
            ws.Cell("J6").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell("J7").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range("J5:J7").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            ws.Range("J5:J7").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            string[] headers = {
            "NÚMERO DE INVENTARIO", "FRENTE", "ENCARGADO", "FECHA",
            "MODELO", "NUMERO DE SERIE", "ESTADO DEL DISPOSITIVO",
            "FACTURA", "PRECIO"
             };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(8, i + 2);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromArgb(0, 51, 102);
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                cell.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            var tecnologia = _context.Articulos
                .OfType<ArticuloTecnologia>()
                .Include(t => t.EstatusArticulo)
                .Include(t => t.CatalogoArticulo).ThenInclude(c => c.Categoria)
                .Include(t => t.Asignaciones).ThenInclude(a => a.Personal)
                .ToList();

            int fila = 9;
            foreach (var t in tecnologia)
            {
                ws.Cell(fila, 2).Value = t.NumeroInventario;
                ws.Cell(fila, 3).Value = t.CatalogoArticulo?.Categoria?.Nombre ?? "—";
                ws.Cell(fila, 4).Value = t.Asignaciones?.OrderByDescending(a => a.FechaAsignacion)
                                        .FirstOrDefault(a => a.FechaDevolucion == null)?.Personal?.Nombre ?? "No asignado";
                ws.Cell(fila, 5).Value = t.FechaCompra.ToString("dd/MM/yyyy");
                ws.Cell(fila, 6).Value = t.Modelo;
                ws.Cell(fila, 7).Value = t.NumeroSerie;

                var estadoTexto = t.EstatusArticulo?.Nombre ?? "—";
                var estadoCell = ws.Cell(fila, 8);
                estadoCell.Value = estadoTexto;

                if (estadoTexto.Contains("FALLA", StringComparison.OrdinalIgnoreCase) ||
                    estadoTexto.Contains("ROBO", StringComparison.OrdinalIgnoreCase) ||
                    estadoTexto.Contains("INHABIL", StringComparison.OrdinalIgnoreCase))
                {
                    estadoCell.Style.Fill.BackgroundColor = XLColor.Red;
                    estadoCell.Style.Font.FontColor = XLColor.White;
                }
                else if (estadoTexto.Contains("ETIQUETA", StringComparison.OrdinalIgnoreCase) ||
                         estadoTexto.Contains("PENDIENTE", StringComparison.OrdinalIgnoreCase))
                {
                    estadoCell.Style.Fill.BackgroundColor = XLColor.Yellow;
                    estadoCell.Style.Font.FontColor = XLColor.Black;
                }
                else
                {
                    estadoCell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                    estadoCell.Style.Font.FontColor = XLColor.Black;
                }

                //ws.Cell(fila, 9).Value = t.FacturaArchivo ?? "—";
                ws.Cell(fila, 10).Value = t.Precio;
                ws.Cell(fila, 10).Style.NumberFormat.Format = "$ #,##0.00";

                fila++;
            }

            var rangoTabla = ws.Range(8, 2, fila - 1, 10);
            rangoTabla.Style.Alignment.WrapText = true;
            rangoTabla.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rangoTabla.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            ws.SheetView.FreezeRows(8);
            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Tecnologia.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> VerFactura(int id)
        {
            var art = await _context.Articulos
                .Include(a => a.Factura)
                .FirstOrDefaultAsync(a => a.IdArticulo == id);

            if (art?.Factura == null)
                return NotFound("No hay factura asociada.");

            string bucket = "almacen-d9e8e.firebasestorage.app";
            var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "firebase-adminsdk.json");

            var signer = await UrlSigner.FromCredentialFileAsync(credentialPath);

            string objectName = $"facturas/{art.Factura.ArchivoFactura}";

            var url = await signer.SignAsync(
                bucket,
                art.Factura.ArchivoFactura,
                TimeSpan.FromHours(1),
                HttpMethod.Get
            );

            return Json(new { url });
        }

    }
}
