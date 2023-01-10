using AutoMapper;
using magicvilla_villaapi.models;
using magicvilla_villaapi.models.Dto;

namespace magicvilla_villaapi
{
    public class automapperconfig: Profile 
    {
        public automapperconfig()
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();
            CreateMap<Villa, VillacreateDTO>().ReverseMap();
            CreateMap<Villa, VillaupdateDTO>().ReverseMap();
            
        }
    }
}
