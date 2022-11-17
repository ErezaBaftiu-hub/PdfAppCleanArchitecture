using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Serilog;
using System.Text;
using WkHtmlToPdfDotNet;

namespace Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        private readonly IRepository<Pdf> _pdfRepository;
        private readonly IMapper _mapper;

        public PdfService(IRepository<Pdf> pdfRepository, IMapper mapper)
        {
            _pdfRepository = pdfRepository;
            _mapper = mapper;
        }

        public async Task<PdfModel> GetByIdAsync(int id)
        {
            try
            {
                var pdf = await _pdfRepository.GetByIdAsync(id);

                if (pdf == null)
                    throw new Exception($"Pdf file not found with id {id}");

                var pdfModel = _mapper.Map<PdfModel>(pdf);

                return pdfModel;
            }
            catch (Exception ex)
            {
                Log.Error("Could not find Pdf file with {id} {exMessage}", id, ex.Message);
                return new PdfModel(ex.Message);
            }
        }

        public async Task<PdfOutputModel> ConvertHtmlToPdfAsync(PdfInputModel pdfInputModel)
        {
            try
            {
                var converter = new BasicConverter(new PdfTools());

                var globalSettings = _mapper.Map<GlobalSettings>(pdfInputModel.Options);
                globalSettings.PaperSize = Enum.Parse<PaperKind>(pdfInputModel.Options.PagePaperSize);
                globalSettings.ColorMode = Enum.Parse<ColorMode>(pdfInputModel.Options.PageColorMode);

                var htmlContentBase64 = Convert.FromBase64String(pdfInputModel.HtmlString);

                var htmlContent = Encoding.UTF8.GetString(htmlContentBase64);

                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = {
                        new ObjectSettings()
                        {
                           PagesCount = true,
                           HtmlContent = htmlContent,
                           WebSettings =
                           {
                               DefaultEncoding = "utf-8"
                           }
                        }
                    }
                };

                var pdfDoc = converter.Convert(doc);

                var fileName = $"pdf-{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}.pdf";

                var (pdfDocumentSize, pdfPath) = await GetPdfDocumentSizeAsync(pdfDoc, fileName);

                var pdfOutputDocumentBase64 = Convert.ToBase64String(pdfDoc);

                var pdf = await _pdfRepository.InsertAsync(new Pdf
                {
                    FileName = fileName,
                    Path = pdfPath,
                    HtmlString = htmlContent,
                    PdfDocumentSize = pdfDocumentSize,
                });

                if (!pdf)
                    throw new Exception("Could not insert pdf file into database");

                return new PdfOutputModel(pdfOutputDocumentBase64, pdfDocumentSize);
            }
            catch (Exception ex)
            {
                Log.Error("Could not convert HTML to PDF with exception message {ex.Message}", ex.Message);
                return new PdfOutputModel(ex.Message);
            }
        }

        private static async Task<(int, string)> GetPdfDocumentSizeAsync(byte[] pdf, string fileName)
        {
            int pdfDocumentSize;
            string path = Path.Combine(Path.GetTempPath(), $"PDFConverter");

            if (!File.Exists(path))
                Directory.CreateDirectory(path);

            var pdfPath = Path.Combine(path, fileName);

            await using (var fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(pdf, 0, pdf.Length);
                pdfDocumentSize = (int)fs.Length;
            }

            return (pdfDocumentSize, pdfPath);
        }
    }
}
