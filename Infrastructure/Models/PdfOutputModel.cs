using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class PdfOutputModel
    {
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
        public string? ErrorMessage { get; private set; }
        public string? PdfDocument { get; private set; }
        public int? PdfDocumentSize { get; private set; }

        public PdfOutputModel(string errorMessage) => ErrorMessage = errorMessage;

        public PdfOutputModel(string pdfDocument, int pdfDocumentSize)
        {
            PdfDocument = pdfDocument;
            PdfDocumentSize = pdfDocumentSize;
        }
    }
}
