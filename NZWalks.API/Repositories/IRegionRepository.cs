using NZWalks.API.Models.Domain;
namespace NZWalks.API.Repositories
{
    //interface is the contract between the repository and the controller
    //it is the definition of what the repository can do
    //we implement this interface in the SQLRegionRepository
    public interface IRegionRepository
    {
        public Task<List<Region>> GetAllAsync();

        public Task<Region?> GetByIdAsync(Guid id);

        public Task<Region> CreateAsync(Region region);

        public Task<Region?> UpdateAsync(Guid id, Region region);

        public Task<Region?> DeleteAsync(Guid id);
    }
}