using NzWalk.API.Model.Domain;

namespace NzWalk.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
    }
}
