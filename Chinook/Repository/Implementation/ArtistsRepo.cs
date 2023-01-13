using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Repository
{
    public class ArtistsRepo : BaseRepo, IArtistsRepo
    {
        public ArtistsRepo(ChinookContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Artist>> GetAllArtists()
        {
            return await _dbContext.Artists.Include(a => a.Albums).ToListAsync();
        }

        public async Task<Artist> GetArtistById(long artistId)
        {
            var artists = await _dbContext.Artists.SingleOrDefaultAsync(a => a.ArtistId == artistId);

            if (artists != null)
                return artists;
            else
                return new Artist();
        }
    }
}

