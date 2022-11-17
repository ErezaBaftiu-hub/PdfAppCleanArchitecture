namespace Infrastructure.Models
{
    public class PdfInputModel
    {
        public string HtmlString { get; set; }
        public PdfOptionsModel? Options { get; set; }

        public PdfInputModel(string htmlString)
        {
            HtmlString = htmlString;
        }
    }
}
