using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class PdfModel
    {
        public string? HtmlString { get; set; }
        public string FileName { get; set; }
        public string? Path { get; set; }
        public string? UrlPath { get; set; }
        public int? PdfDocumentSize { get; set; }
    }
}
