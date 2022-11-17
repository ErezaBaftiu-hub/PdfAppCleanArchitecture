using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Domain.Entities;
using Infrastructure.Models;
using WkHtmlToPdfDotNet;

namespace Infrastructure.AutoMapper
{
    public class PdfMapper : Profile
    {
        public PdfMapper()
        {
            CreateMap<Pdf, PdfModel>().ReverseMap();
            CreateMap<Margins, MarginSettings>().ReverseMap();
            CreateMap<PdfOptionsModel, GlobalSettings>()
                .ForMember(x => x.DocumentTitle, y => y.MapFrom(src => src.FileName))
                .ForMember(x => x.ColorMode, y => y.MapFrom(src => src.PageColorMode))
                .ForMember(x => x.Margins, y => y.MapFrom(src => src.Margins))
                .ForMember(x => x.PaperSize, y => y.MapFrom(src => src.PaperSize))
                .ForMember(x => x.Orientation, y => y.MapFrom(src => src.PageOrientation))
                .ReverseMap();
            CreateMap<Orientation, Domain.Enum.PageOrientation>().ReverseMap();
            CreateMap<ColorMode, Domain.Enum.ColorMode>().ReverseMap();
            CreateMap<PaperKind, Domain.Enum.PaperKind>().ReverseMap();
            CreateMap<PechkinPaperSize, PechkinPaperSizeModel>().ReverseMap();
            CreateMap<Unit, Domain.Enum.Unit>().ReverseMap();
        }
    }
}
