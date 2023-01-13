using Chinook.Models;

namespace Chinook.Repository
{
    public interface IArtistsRepo
    {
        Task<Artist> GetArtistById(long artistId);
    }
}
