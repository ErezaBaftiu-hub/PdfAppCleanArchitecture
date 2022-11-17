
namespace Infrastructure.Models
{
    public class PdfOptionsModel
    {
        public string PageOrientation { get; set; }
        public string PageColorMode { get; set; }
        public string PagePaperSize { get; set; }
        public Margins PageMargins { get; set; } 
    }
}
