using AutoMapper;
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
            CreateMap<Margins, MarginSettings>()
                .ReverseMap();
            CreateMap<PdfOptionsModel, GlobalSettings>()
                .ForMember(x => x.ColorMode, y => y.MapFrom(src => src.PageColorMode))
                .ForMember(x => x.Margins, y => y.MapFrom(src => src.PageMargins))
                .ForMember(x => x.Orientation, y => y.MapFrom(src => src.PageOrientation))
                .ReverseMap();
        }
    }
}
