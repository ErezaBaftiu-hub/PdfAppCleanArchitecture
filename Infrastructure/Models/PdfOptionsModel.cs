using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class PdfOptionsModel
    {
        public string FileName { get; set; }
        public PageOrientation PageOrientation { get; set; }
        public IEnumerable<OptionModel> PageOrientationOptions =>
               (Enum.GetValues(typeof(PageOrientation))
                    .Cast<PageOrientation>()
                    .Select(pageOrientation =>
                    new OptionModel { Value = pageOrientation.ToString(), Id = (int)pageOrientation })).ToList();

        public ColorMode PageColorMode { get; set; }
        public IEnumerable<OptionModel> ColorModeOptions =>
              (Enum.GetValues(typeof(ColorMode))
                   .Cast<ColorMode>()
                   .Select(colorMode =>
                   new OptionModel { Value = colorMode.ToString(), Id = (int)colorMode })).ToList();
        public PechkinPaperSizeModel PaperSize { get; set; }
        public IEnumerable<OptionModel> PaperKindOptions =>
            (Enum.GetValues(typeof(PaperKind))
                 .Cast<PaperKind>()
                 .Select(paperKind =>
                 new OptionModel { Value = paperKind.ToString(), Id = (int)paperKind })).ToList();
        public Margins Margins { get; set; } 
    }
}
