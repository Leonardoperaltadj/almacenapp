using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.IO;

namespace PlantillaAlmacen.Services.Pdf
{
    public class ActaResponsabilidadPdf
    {
        public byte[] Generar(
            string nombrePersonal,
            string marca,
            string modelo,
            string placas,
            string numeroSerie,
            int anio,
            string color,
            string combustible,
            string eco,
            string descripcion,
            string kilometraje,
            string tipoTransmision)
        {
            var hoy = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fecha = DateTime.ParseExact(hoy, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string fechaFormateada = fecha.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));

            var logoDefensa = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoDefensa.png");
            var logoFrente = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoFrente.png");

            var doc = Document.Create(c =>
            {
                c.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(140).Image(logoDefensa).FitWidth();
                        row.RelativeItem();
                        row.ConstantItem(80).Image(logoFrente).FitWidth();
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().AlignCenter().PaddingTop(10).Text("ACTA DE ENTREGA DE VEHÍCULO")
                           .Bold().FontSize(16).ParagraphSpacing(2);

                        col.Item().AlignCenter().PaddingTop(10).Text("PROYECTO DE REUBICACIÓN DE VÍAS FERREAS DE NOGALES, SON.")
                           .Bold().FontSize(11).ParagraphSpacing(2);



                        col.Item().PaddingVertical(10).PaddingTop(10).Text(txt =>
                        {
                            txt.DefaultTextStyle(x => x.FontSize(11));
                            txt.Justify();
                            txt.ParagraphSpacing(1);
                            txt.Span($"EN LA CIUDAD DE NOGALES SONORA SIENDO EL DÍA {fechaFormateada.ToUpper()}, SE HACE CONSTAR DE ENTREGADO VEHÍCULO REASIGNADO AL C. ");
                            txt.Span(nombrePersonal.ToUpper()).Bold();
                            txt.Span(" QUE UTILIZARÁ EL MISMO CON EL DEBIDO CUIDADO Y ENTREGARÁ AL FINALIZAR LA OBRA.");
                        });

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3);
                                cols.RelativeColumn(3);
                            });

                            table.Cell().Border(0.5f).Padding(4).AlignCenter().Text("DESCRIPCIÓN DEL VEHÍCULO").Bold().FontSize(10);
                            table.Cell().Border(0.5f).Padding(4).AlignCenter().Text("DETALLES OBSERVADOS").Bold().FontSize(10);

                            table.Cell().Border(0.5f).Padding(8).Column(col2 =>
                            {
                                col2.Item().Text($"ECO: {eco.ToUpper()}").FontSize(10);
                                col2.Item().Text($"DESCRIPCIÓN: {descripcion.ToUpper()}").FontSize(10);
                                col2.Item().Text($"MARCA: {marca.ToUpper()}").FontSize(10);
                                col2.Item().Text($"MODELO: {modelo.ToUpper()}").FontSize(10);
                                col2.Item().Text($"VE: {tipoTransmision.ToUpper()}").FontSize(10);
                                col2.Item().Text($"NÚMERO DE SERIE: {numeroSerie.ToUpper()}").FontSize(10);
                                col2.Item().Text($"PLACAS: {placas.ToUpper()}").FontSize(10);
                                col2.Item().Text($"COLOR: {color.ToUpper()}").FontSize(10);
                                col2.Item().Text($"AÑO: {anio}").FontSize(10);
                                col2.Item().Text($"COMBUSTIBLE: {combustible.ToUpper()}").FontSize(10);
                                col2.Item().Text($"KILOMETRAJE: {kilometraje.ToUpper()}").FontSize(10);
                            });

                            table.Cell().Border(0.5f).Padding(5).Text("");
                        });

                        col.Item().PaddingTop(14).Text($"POR LO TANTO, EL C. {nombrePersonal.ToUpper()} SE COMPROMETE A:").Bold();

                        col.Item().PaddingTop(6).Column(lista =>
                        {
                            lista.Spacing(6);
                            lista.Item().Text(t =>
                            {
                                t.DefaultTextStyle(x => x.FontSize(10));
                                t.Span($"A) A REVISAR DETALLES FÍSICOS, CONTROL DE SERVICIO AUTOMOTRIZ, LIMPIEZA Y LLENADO DE ");
                                t.Span("GASOLINA.").Bold();
                            });
                            lista.Item().Text("B) PONER TODA DILIGENCIA EN LA CONSERVACIÓN Y BUEN USO DEL VEHÍCULO, ASÍ COMO DAR AVISO A SUS ENCARGADOS SOBRE INCIDENCIAS FÍSICAS Y MECÁNICAS.").FontSize(10);
                            lista.Item().Text("C) CONCEDER A TERCEROS EL USO O GOCE TEMPORAL DE LOS VEHÍCULOS, PERO NUNCA PODRÁ OTORGAR DE MANERA PERMANENTE EL USO DE LOS VEHÍCULOS A TERCERAS PERSONAS. ").FontSize(10);
                        });

                        col.Item().PaddingTop(30).Row(r =>
                        {
                            r.RelativeItem().AlignCenter().Text("ENTREGA:").Bold();
                            r.RelativeItem().AlignCenter().Text("RECIBE:").Bold();
                        });

                        col.Item().PaddingTop(20).Row(r =>
                        {
                            r.RelativeItem().AlignCenter().Column(c =>
                            {
                                c.Item().Height(1).LineHorizontal(1);
                                c.Item().PaddingTop(4).Text("ARQ. REY DAVID BELTRÁN ÁLVAREZ").FontSize(10);
                            });

                            r.RelativeItem().AlignCenter().Column(c =>
                            {
                                c.Item().Height(1).LineHorizontal(1);
                                c.Item().PaddingTop(4).Text($"C. {nombrePersonal.ToUpper()}").FontSize(10);
                            });
                        });

                    });

                    page.Footer().AlignCenter().Text("Documento generado automáticamente por el sistema").FontSize(9);
                });
            });

            return doc.GeneratePdf();
        }


        public byte[] GenerarTecnologia(
            string nombrePersonal,
            string numeroInventario,
            string descripcion,
            string marca,
            string modelo,
            string numeroSerie,
            string procesador,
            string memoria,
            string discoDuro)
        {
            var hoy = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fecha = DateTime.ParseExact(hoy, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string fechaFormateada = fecha.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));

            var logoDefensa = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoDefensa.png");
            var logoFrente = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoFrente.png");

            var doc = Document.Create(c =>
            {
                c.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(140).Image(logoDefensa).FitWidth();
                        row.RelativeItem();
                        row.ConstantItem(80).Image(logoFrente).FitWidth();
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().AlignCenter().PaddingTop(10).Text("ACTA DE ENTREGA DE EQUIPO DE CÓMPUTO")
                           .Bold().FontSize(16).ParagraphSpacing(2);

                        col.Item().AlignCenter().PaddingTop(10).Text("PROYECTO DE REUBICACIÓN DE VÍAS FERREAS DE NOGALES, SON.")
                           .Bold().FontSize(11).ParagraphSpacing(2);

                        col.Item().PaddingVertical(10).PaddingTop(10).Text(txt =>
                        {
                            txt.DefaultTextStyle(x => x.FontSize(11));
                            txt.Justify();
                            txt.ParagraphSpacing(1);
                            txt.Span($"EN LA CIUDAD DE NOGALES SONORA, SIENDO EL DÍA {fechaFormateada.ToUpper()}, SE HACE CONSTAR DE ENTREGADO EQUIPO REASIGNADO DE CÓMPUTO, SIN DETALLES FÍSICOS Y EN COMPLETO FUNCIONAMIENTO CON CARGADOR AL C. ");
                            txt.Span(nombrePersonal.ToUpper()).Bold();
                            txt.Span(" QUIEN UTILIZARÁ EL EQUIPO CON EL DEBIDO CUIDADO Y ENTREGARÁ AL FINALIZAR LA OBRA.");
                        });

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3);
                                cols.RelativeColumn(3);
                            });

                            table.Cell().Border(0.5f).Padding(4).AlignCenter().Text("DESCRIPCIÓN DEL EQUIPO").Bold().FontSize(10);
                            table.Cell().Border(0.5f).Padding(4).AlignCenter().Text("OBSERVACIONES").Bold().FontSize(10);

                            table.Cell().Border(0.5f).Padding(8).Column(col2 =>
                            {
                                col2.Item().Text($"NÚMERO DE INVENTARIO: {numeroInventario.ToUpper()}").FontSize(10);
                                col2.Item().Text($"DESCRIPCIÓN: {descripcion.ToUpper()}").FontSize(10);
                                col2.Item().Text($"MARCA: {marca.ToUpper()}").FontSize(10);
                                col2.Item().Text($"MODELO: {modelo.ToUpper()}").FontSize(10);
                                col2.Item().Text($"NÚMERO DE SERIE: {numeroSerie.ToUpper()}").FontSize(10);
                                col2.Item().Text($"PROCESADOR: {procesador.ToUpper()}").FontSize(10);
                                col2.Item().Text($"MEMORIA: {memoria.ToUpper()}").FontSize(10);
                                col2.Item().Text($"DISCO DURO: {discoDuro.ToUpper()}").FontSize(10);
                            });

                            table.Cell().Border(0.5f).Padding(5).Text("");
                        });

                        col.Item().PaddingVertical(10).PaddingTop(10).Text(txt =>
                        {
                            txt.DefaultTextStyle(x => x.FontSize(11));
                            txt.Justify();
                            txt.ParagraphSpacing(1);
                            txt.Span($"POR LO TANTO, CON LA FIRMA DEL C. ");
                            txt.Span(nombrePersonal.ToUpper()).Bold();
                            txt.Span(" ESTA DE ACUERDO EN QUE LOS COMPONENTES Y EQUIPO AQUÍ SEÑALADOS SON PROPIEDAD DE DEFENSA Y SE COMPROMETE A:");
                        });

                        col.Item().PaddingTop(6).Column(lista =>
                        {
                            lista.Spacing(6);
                            lista.Item().Text("A) REVISIÓN DE DETALLES FÍSICOS, LIMPIEZA Y USO ÚNICO DE TRABAJO, Y EN CASO DE MAL USO O NEGLIGENCIA EN SU OPERACIÓN, SE TOMARÁN LAS MEDIDAS NECESARIAS PARA CUBRIR EL DAÑO O DESPERFECTO OCASIONADO POR EL USUARIO.").FontSize(10);
                            lista.Item().Text("B) SE RESPONSABILIZA EN CASO DE PERDIDA Y ROBO, EL USUARIO LEVANTARA LA DENUNCIA CORRESPONDIENTE Y REPORTARA POR ESCRITO AL MINISTERIO PÚBLICO Y A SUS ENCARGADOS.").FontSize(10);
                            lista.Item().Text("C) EL USUARIO ESTÁ DE ACUERDO EN QUE SE REALICE EN CUALQUIER MOMENTO Y SIN PREVIA NOTIFICACIÓN AUDITORÍAS DEL EQUIPO Y LA INFORMACIÓN CONTENIDA EN EL MISMO.").FontSize(10);
                            lista.Item().Text("D) EL USUARIO ESTÁ DE ACUERDO EN QUE NO INSTALARA PROGRAMAS INFORMÁTICOS, JUEGOS, MÚSICA, VIDEOS, O CUALQUIER OTRO SOFTWARE QUE NO SEA AUTORIZADO.").FontSize(10);
                            lista.Item().Text("E) EN CASO DE QUE EL TRABAJADOR INCURRA EN EL ARTÍCULO 47 DE LA LEY FEDERAL DEL TRABAJO, O BIEN DECIDA RENUNCIAR POR SU PROPIO DERECHO, DEBERÁ HACER ENTREGA DEL DISPOSITIVO Y LOS ACCESORIOS ENTREGADOS PREVIAMENTE DE FORMA INMEDIATA, EN CASO DE NO HACERLO INCURRIRÁ EN LA OMISIÓN DE UN DELITO SANCIONABLE.").FontSize(10);
                        });

                        col.Item().PaddingTop(30).Row(r =>
                        {
                            r.RelativeItem().AlignCenter().Text("ENTREGA:").Bold();
                            r.RelativeItem().AlignCenter().Text("RECIBE:").Bold();
                        });

                        col.Item().PaddingTop(20).Row(r =>
                        {
                            r.RelativeItem().AlignCenter().Column(c =>
                            {
                                c.Item().Height(1).LineHorizontal(1);
                                c.Item().PaddingTop(4).Text("ARQ. REY DAVID BELTRÁN ÁLVAREZ").FontSize(10);
                            });

                            r.RelativeItem().AlignCenter().Column(c =>
                            {
                                c.Item().Height(1).LineHorizontal(1);
                                c.Item().PaddingTop(4).Text($"C. {nombrePersonal.ToUpper()}").FontSize(10);
                            });
                        });
                    });

                    page.Footer().AlignCenter().Text("Documento generado automáticamente por el sistema").FontSize(9);
                });
            });

            return doc.GeneratePdf();
        }

        public byte[] GenerarComunicaciones(
    string nombrePersonal,
    string numeroInventario,
    string descripcion,
    string marca,
    string modelo,
    string numeroSerie)
        {
            var hoy = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fecha = DateTime.ParseExact(hoy, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string fechaFormateada = fecha.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));

            var logoDefensa = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoDefensa.png");
            var logoFrente = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoFrente.png");

            var doc = Document.Create(c =>
            {
                c.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(140).Image(logoDefensa).FitWidth();
                        row.RelativeItem();
                        row.ConstantItem(80).Image(logoFrente).FitWidth();
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().AlignCenter().PaddingTop(10).Text("ACTA DE ENTREGA DE EQUIPO DE COMUNICACIONES")
                           .Bold().FontSize(16).ParagraphSpacing(2);

                        col.Item().AlignCenter().PaddingTop(10).Text("PROYECTO DE REUBICACIÓN DE VÍAS FERREAS DE NOGALES, SON.")
                           .Bold().FontSize(11).ParagraphSpacing(2);

                        col.Item().PaddingVertical(10).Text(txt =>
                        {
                            txt.DefaultTextStyle(x => x.FontSize(11));
                            txt.Justify();
                            txt.Span($"EN LA CIUDAD DE NOGALES SONORA SIENDO EL DÍA {fechaFormateada.ToUpper()}, SE HACE CONSTAR ENTREGADO EQUIPO REASIGNADO DE COMUNICACIÓN SIN FALLA O DETALLES FÍSICOS Y EN COMPLETO FUNCIONAMIENTO CON CARGADOR AL C. ");
                            txt.Span(nombrePersonal.ToUpper()).Bold();
                            txt.Span(" QUIEN UTILIZARÁ EL EQUIPO CON EL DEBIDO CUIDADO Y ENTREGARÁ AL FINALIZAR LA OBRA.");
                        });

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3);
                                cols.RelativeColumn(3);
                            });

                            table.Cell().Border(0.5f).Padding(4).AlignCenter().Text("DESCRIPCIÓN DEL EQUIPO").Bold().FontSize(10);
                            table.Cell().Border(0.5f).Padding(4).AlignCenter().Text("OBSERVACIONES").Bold().FontSize(10);

                            table.Cell().Border(0.5f).Padding(8).Column(col2 =>
                            {
                                col2.Item().Text($"NÚMERO DE INVENTARIO: {numeroInventario.ToUpper()}").FontSize(10);
                                col2.Item().Text($"DESCRIPCIÓN: {descripcion.ToUpper()}").FontSize(10);
                                col2.Item().Text($"MARCA: {marca.ToUpper()}").FontSize(10);
                                col2.Item().Text($"MODELO: {modelo.ToUpper()}").FontSize(10);
                                col2.Item().Text($"NÚMERO DE SERIE: {numeroSerie.ToUpper()}").FontSize(10);
                            });

                            table.Cell().Border(0.5f).Padding(5).Text("");
                        });

                        col.Item().PaddingVertical(10).Text(txt =>
                        {
                            txt.DefaultTextStyle(x => x.FontSize(11));
                            txt.Justify();
                            txt.Span("POR LO TANTO, CON LA FIRMA DEL C. ");
                            txt.Span(nombrePersonal.ToUpper()).Bold();
                            txt.Span(" ESTA DE ACUERDO EN QUE LOS COMPONENTES Y EQUIPO AQUÍ SEÑALADO SON PROPIEDAD DE DEFENSA Y SE COMPROMETE A:");
                        });

                        col.Item().PaddingTop(6).Column(lista =>
                        {
                            lista.Spacing(6);
                            lista.Item().Text("A) REVISIÓN DE DETALLES FÍSICOS, LIMPIEZA Y USO ÚNICO DE TRABAJO Y EN CASO DE MAL USO O NEGLIGENCIA EN SU OPERACIÓN, SE TOMARÁN LAS MEDIDAS NECESARIAS PARA CUBRIR EL DAÑO O DESPERFECTO OCASIONADO POR EL USUARIO.").FontSize(10);
                            lista.Item().Text("B) EN CASO DE QUE EL TRABAJADOR INCURRA EN EL ARTÍCULO 47 DE LA LEY FEDERAL DEL TRABAJO, O BIEN DECIDA RENUNCIAR POR SU PROPIO DERECHO, DEBERÁ HACER ENTREGA DEL DISPOSITIVO Y LOS ACCESORIOS ENTREGADOS PREVIAMENTE DE FORMA INMEDIATA, EN CASO DE NO HACERLO INCURRIRÁ EN LA OMISIÓN DE UN DELITO SANCIONABLE.").FontSize(10);
                        });

                        col.Item().PaddingTop(30).Row(r =>
                        {
                            r.RelativeItem().AlignCenter().Text("ENTREGA:").Bold();
                            r.RelativeItem().AlignCenter().Text("RECIBE:").Bold();
                        });

                        col.Item().PaddingTop(20).Row(r =>
                        {
                            r.RelativeItem().AlignCenter().Column(c =>
                            {
                                c.Item().Height(1).LineHorizontal(1);
                                c.Item().PaddingTop(4).Text("ARQ. REY DAVID BELTRÁN ÁLVAREZ").FontSize(10);
                            });

                            r.RelativeItem().AlignCenter().Column(c =>
                            {
                                c.Item().Height(1).LineHorizontal(1);
                                c.Item().PaddingTop(4).Text($"C. {nombrePersonal.ToUpper()}").FontSize(10);
                            });
                        });
                    });

                    page.Footer().AlignCenter().Text("Documento generado automáticamente por el sistema").FontSize(9);
                });
            });

            return doc.GeneratePdf();
        }

        public byte[] GenerarActaDanios(
       string nombrePersonal,
       string numeroInventario,
       string descripcion,
       decimal precio,
       List<byte[]> evidencias
   )
        {
            var hoy = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fecha = DateTime.ParseExact(hoy, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string fechaFormateada = fecha.ToString("dd 'de' MMMM 'de' yyyy", new CultureInfo("es-ES"));

            var logoDefensa = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoDefensa.png");
            var logoFrente = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "LogoFrente.png");

            var doc = Document.Create(c =>
            {
                c.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(140).Image(logoDefensa).FitWidth();
                        row.RelativeItem();
                        row.ConstantItem(80).Image(logoFrente).FitWidth();
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().AlignCenter().PaddingTop(10).Text("ACTA DE DAÑOS DE EQUIPO")
                           .Bold().FontSize(16).ParagraphSpacing(2);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3);
                                cols.RelativeColumn(3);
                            });

                            table.Cell().Border(0.5f).Padding(4).AlignCenter().Text("DETALLE DEL EQUIPO").Bold();
                            table.Cell().Border(0.5f).Padding(4).AlignCenter().Text("VALOR ESTIMADO").Bold();

                            table.Cell().Border(0.5f).Padding(8).Column(col2 =>
                            {
                                col2.Item().Text($"NÚMERO DE INVENTARIO: {numeroInventario}").FontSize(10);
                                col2.Item().Text($"DESCRIPCIÓN: {descripcion}").FontSize(10);
                            });

                            table.Cell().Border(0.5f).Padding(8).Text($"${precio:N2} MXN").FontSize(10);
                        });

                        if (evidencias.Any())
                        {
                            col.Item().PaddingTop(15).Text("EVIDENCIAS FOTOGRÁFICAS:").Bold();
                            foreach (var imgBytes in evidencias)
                            {
                                col.Item().PaddingTop(10).Image(imgBytes).FitWidth();
                            }
                        }

                        col.Item().PaddingVertical(10).Text("POR LO TANTO, EL C. " + nombrePersonal.ToUpper() +
                            " DEBERÁ RESPONDER POR EL MONTO SEÑALADO O SU REPOSICIÓN.").Justify();
                    });

                    page.Footer().AlignCenter().Text("Documento generado automáticamente por el sistema").FontSize(9);
                });
            });

            return doc.GeneratePdf();
        }





    }
}
