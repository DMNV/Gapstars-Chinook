using Chinook.ClientModels;

namespace Chinook.Repository
{
    public interface ITracksRepo
    {
        Task<List<PlaylistTrack>> GetTracksByArtistIdAsync(long artistId, string userId);
    }
}
