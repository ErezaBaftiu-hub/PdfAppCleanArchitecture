using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IPdfService
    {
        Task<PdfModel> GetByIdAsync(int id);
        Task<PdfOutputModel> ConvertHtmlToPdfAsync(PdfInputModel pdfInputModel);
    }
}
