﻿using System;
using System.IO;
using Core2D;
using Core2D.Containers;
using Core2D.Data;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Core2D.Renderer.PdfSharp
{
    public partial class PdfSharpRenderer : IProjectExporter
    {
        public void Save(Stream stream, PageContainerViewModel containerViewModel)
        {
            using var pdf = new PdfDocument();
            Add(pdf, containerViewModel);
            pdf.Save(stream);
        }

        public void Save(Stream stream, DocumentContainerViewModel document)
        {
            using var pdf = new PdfDocument();
            var documentOutline = default(PdfOutline);

            foreach (var container in document.Pages)
            {
                var page = Add(pdf, container);

                if (documentOutline == null)
                {
                    documentOutline = pdf.Outlines.Add(
                        document.Name,
                        page,
                        true,
                        PdfOutlineStyle.Regular,
                        XColors.Black);
                }

                documentOutline.Outlines.Add(
                    container.Name,
                    page,
                    true,
                    PdfOutlineStyle.Regular,
                    XColors.Black);
            }

            pdf.Save(stream);
            ClearCache();
        }

        public void Save(Stream stream, ProjectContainerViewModel project)
        {
            using var pdf = new PdfDocument();
            var projectOutline = default(PdfOutline);

            foreach (var document in project.Documents)
            {
                var documentOutline = default(PdfOutline);

                foreach (var container in document.Pages)
                {
                    var page = Add(pdf, container);

                    if (projectOutline == null)
                    {
                        projectOutline = pdf.Outlines.Add(
                            project.Name,
                            page,
                            true,
                            PdfOutlineStyle.Regular,
                            XColors.Black);
                    }

                    if (documentOutline == null)
                    {
                        documentOutline = projectOutline.Outlines.Add(
                            document.Name,
                            page,
                            true,
                            PdfOutlineStyle.Regular,
                            XColors.Black);
                    }

                    documentOutline.Outlines.Add(
                        container.Name,
                        page,
                        true,
                        PdfOutlineStyle.Regular,
                        XColors.Black);
                }
            }

            pdf.Save(stream);
            ClearCache();
        }

        private PdfPage Add(PdfDocument pdf, PageContainerViewModel containerViewModel)
        {
            // Create A3 page size with Landscape orientation.
            var pdfPage = pdf.AddPage();
            pdfPage.Size = PageSize.A3;
            pdfPage.Orientation = PageOrientation.Landscape;

            var dataFlow = _serviceProvider.GetService<DataFlow>();
            var db = (object)containerViewModel.Properties;
            var record = (object)containerViewModel.RecordViewModel;

            dataFlow.Bind(containerViewModel.Template, db, record);
            dataFlow.Bind(containerViewModel, db, record);

            using (XGraphics gfx = XGraphics.FromPdfPage(pdfPage))
            {
                // Calculate x and y page scale factors.
                double scaleX = pdfPage.Width.Value / containerViewModel.Template.Width;
                double scaleY = pdfPage.Height.Value / containerViewModel.Template.Height;
                double scale = Math.Min(scaleX, scaleY);

                // Set scaling function.
                _scaleToPage = (value) => value * scale;

                // Draw container template contents to pdf graphics.
                Fill(gfx, 0, 0, pdfPage.Width.Value / scale, pdfPage.Height.Value / scale, containerViewModel.Template.Background);

                // Draw template contents to pdf graphics.
                DrawPage(gfx, containerViewModel.Template);

                // Draw page contents to pdf graphics.
                DrawPage(gfx, containerViewModel);
            }

            return pdfPage;
        }
    }
}
