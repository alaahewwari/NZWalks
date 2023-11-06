namespace NZWalks.API.Models.DTO
{
    public class RegionDTO
    {
        //it includes all the properties from the domain model and want to expose to the client
        //DTO is subset of domain model and it is a good practice to use DTOs
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
