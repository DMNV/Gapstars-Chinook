using Chinook.ClientModels;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Repository
{
    public class TracksRepo : BaseRepo, ITracksRepo
    {
        public TracksRepo(ChinookContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<PlaylistTrack>> GetTracksByArtistIdAsync(long artistId, string userId)
        {
            var track = await _dbContext.Tracks.Where(a => a.Album.ArtistId == artistId)
          .Include(a => a.Album)
          .Select(t => new PlaylistTrack()
          {
              AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
              TrackId = t.TrackId,
              TrackName = t.Name,
              IsFavorite = t.Playlists.Any(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "Favorites"))
          }).ToListAsync();

            return track;
        }
    }
}
