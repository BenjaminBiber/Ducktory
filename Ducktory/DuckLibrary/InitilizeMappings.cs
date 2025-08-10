using AutoMapper;
using DuckLibrary.MappingModels;
using DuckLibrary.Models;

namespace DuckLibrary;

public class InitilizeMappings : Profile
{
    public InitilizeMappings()
    {
        CreateMap<DuckPart, DuckPartDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Folder))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.EntenBilds, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.SortierIndex, opt => opt.MapFrom(src => src.SortOrder))
            .ForMember(dest => dest.IstSichtbar, opt => opt.MapFrom(src => src.IsVisible))
            .ReverseMap();
        
        CreateMap<DuckPartImage, DuckPartImageDto>()
            .ForMember(dest => dest.Bild, opt => opt.MapFrom(src => src.Bild))
            .ForMember(dest => dest.IstDefault, opt => opt.MapFrom(src => src.IsDefault))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();

        CreateMap<ImageDto, Image>().ReverseMap();

    }
}