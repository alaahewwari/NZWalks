namespace NZWalks.UI.Models.DTO
{
    public class RegionDTO
    {
        //properties as in regiondto in nzwalks.api
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }

    }
}
