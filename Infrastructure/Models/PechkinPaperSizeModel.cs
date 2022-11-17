using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class PechkinPaperSizeModel
    {

        public string Height { get; set; }

        public string Width { get; set; }

        public PechkinPaperSizeModel(string width, string height)
        {
            Width = width;
            Height = height;
        }

        private static readonly Dictionary<PaperKind, PechkinPaperSizeModel> dictionary = new Dictionary<PaperKind, PechkinPaperSizeModel>
        {
            {
                PaperKind.A3,
                new PechkinPaperSizeModel("297mm", "420mm")
            },
            {
                PaperKind.A4,
                new PechkinPaperSizeModel("210mm", "297mm")
            },
            {
                PaperKind.A5,
                new PechkinPaperSizeModel("148mm", "210mm")
            }
        };

        public static implicit operator PechkinPaperSizeModel(PaperKind paperKind)
        {
            return dictionary[paperKind];
        }
    }
}
