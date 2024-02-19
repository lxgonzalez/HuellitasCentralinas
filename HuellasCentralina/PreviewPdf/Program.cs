using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

// code in your main method
Document.Create(document =>
{
    document.Page(page =>
    {

        page.Margin(30);
        //page.Header().Height(100).Background(Colors.Blue.Medium);
        //page.Content().Background(Colors.Yellow.Medium);
        //page.Footer().Height(50).Background(Colors.Red.Medium);

        page.Header().ShowOnce().Row(row =>
        {
            row.ConstantItem(140).Height(60).Placeholder();


            row.RelativeItem().Column(col =>
            {
                col.Item().AlignCenter().Text("Huellitas Centralinas").Bold().FontSize(20);
                col.Item().AlignCenter().Text("Quito, Ecuador").FontSize(10);
                //col.Item().AlignCenter().Text("987 987 123 / 02 213232").FontSize(9);
                //col.Item().AlignCenter().Text("codigo@example.com").FontSize(9);

            });

            //row.RelativeItem().Column(col =>
            //{
            //    col.Item().Border(1).BorderColor("#257272")
            //    .AlignCenter().Text("RUC 21312312312");

            //    col.Item().Background("#257272").Border(1)
            //    .BorderColor("#257272").AlignCenter()
            //    .Text("Boleta de venta").FontColor("#fff");

            //    col.Item().Border(1).BorderColor("#257272").
            //    AlignCenter().Text("B0001 - 234");


            //});
        });

        page.Content().PaddingVertical(15).Column(col1 =>
        {
            col1.Item().Column(col2 =>
            {
                col2.Item().Text("Reporte de Usuarios").Underline().Bold().FontSize(15);

                col2.Item().Text(txt =>
                {
                    txt.Span("Hecho Por: ").SemiBold().FontSize(13);
                    txt.Span(".....").FontSize(13);
                });

                col2.Item().Text(txt =>
                {
                    txt.Span("Email: ").SemiBold().FontSize(13);
                    txt.Span("......").FontSize(13);
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
                    header.Cell().Background("#257272")
                    .Padding(2).Text("Nombre Completo").FontColor("#fff");

                    header.Cell().Background("#257272")
                   .Padding(2).Text("Email").FontColor("#fff");

                    header.Cell().Background("#257272")
                   .Padding(2).Text("Usuario").FontColor("#fff");

                    header.Cell().Background("#257272")
                   .Padding(2).Text("Rol").FontColor("#fff");
                });

                foreach (var item in Enumerable.Range(1, 20))
                {
                    var cantidad = Placeholders.Random.Next(1, 10);
                    var precio = Placeholders.Random.Next(5, 15);
                    var total = cantidad * precio;

                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                    .Padding(2).Text(Placeholders.Label()).FontSize(10);

                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
             .Padding(2).Text(cantidad.ToString()).FontSize(10);

                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
             .Padding(2).Text($"S/. {precio}").FontSize(10);

                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
             .Padding(2).AlignRight().Text($"S/. {total}").FontSize(10);
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
}).ShowInPreviewer();