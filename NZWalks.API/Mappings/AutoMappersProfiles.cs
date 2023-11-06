using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMappersProfiles :Profile 
    {
        public AutoMappersProfiles()
        {
            //map from Region to RegionDTO and vice versa
            CreateMap<Region, RegionDTO>().ReverseMap();

            //map from Region to AddRegionRequestDTO and vice versa
            CreateMap<Region, AddRegionRequestDTO>().ReverseMap();

            //map from Region to UpdateRegionRequestDTO and vice versa
            CreateMap<Region, UpdateRegionRequestDTO>().ReverseMap();

            //map from Walk to AddWalkRequestDTO and vice versa
            CreateMap<Walk, AddWalkRequestDTO>().ReverseMap();

            //map from Walk to UpdateWalkRequestDTO and vice versa
            CreateMap<Walk,WalkDTO>().ReverseMap();

            //map from difficulty to difficultyDTO and vice versa
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();

            //map from Walk to UpdateWalkRequestDTO and vice versa
            CreateMap<Walk, UpdateWalkRequestDTO>().ReverseMap();


        }
    }
}
