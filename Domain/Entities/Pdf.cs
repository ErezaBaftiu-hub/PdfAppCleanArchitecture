namespace Domain.Entities
{
    public class Pdf : BaseEntity
    {
        public string HtmlString { get; set; }
        public string FileName { get; set; }
        public string? Path { get; set; }
        public string? UrlPath { get; set; }
        public int? PdfDocumentSize { get; set; }
    }
}
