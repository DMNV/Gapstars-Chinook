using Microsoft.EntityFrameworkCore;

namespace Chinook.Repository
{
    public class PlaylistsRepo : BaseRepo, IPlaylistsRepo
    {
        public PlaylistsRepo(ChinookContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<ClientModels.Playlist>> GetUserPlaylists(string userId)
        {
            var userPlaylists = await _dbContext.UserPlaylists
                .Where(x => x.UserId == userId)
                .Select(x => new ClientModels.Playlist
                {
                    Name = x.Playlist.Name,
                    PlaylistId = x.PlaylistId
                }).ToListAsync();

            if (userPlaylists != null)
                return userPlaylists;
            else
                return new List<ClientModels.Playlist>();
        }

        public async Task<ClientModels.Playlist> GetUserPaylistAsync(long playlistId, string userId)
        {
            var playlists = await _dbContext.Playlists
           .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
           .Where(p => p.PlaylistId == playlistId)
           .Select(p => new ClientModels.Playlist()
           {
               Name = p.Name,
               Tracks = p.Tracks.Select(t => new ClientModels.PlaylistTrack()
               {
                   AlbumTitle = t.Album.Title,
                   ArtistName = t.Album.Artist.Name,
                   TrackId = t.TrackId,
                   TrackName = t.Name,
                   IsFavorite = t.Playlists.Any(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "Favorites"))
               }).ToList()
           }).FirstOrDefaultAsync();

            if (playlists != null)
                return playlists;
            else
                return new ClientModels.Playlist();
        }

        public async Task AddPlaylist(ClientModels.Playlist playlist, string userId)
        {
            long newPlaylistId = 1;

            var latestPlaylistId = _dbContext.Playlists.OrderByDescending(p => p.PlaylistId).FirstOrDefault();

            newPlaylistId += latestPlaylistId != null ? latestPlaylistId.PlaylistId : 0;

            //add new play list with tracks

            _dbContext.Playlists.Add(new Models.Playlist()
            {
                Name = playlist.Name,
                PlaylistId = newPlaylistId,
                Tracks = (ICollection<Models.Track>)playlist.Tracks
            });

            //add new play list to user play list
            _dbContext.UserPlaylists.Add(new Models.UserPlaylist()
            {
                UserId = userId,
                PlaylistId = newPlaylistId
            });

            await _dbContext.SaveChangesAsync();
        }

        public void RemovePlaylist(long playlistId)
        {
            //remove play list and user play list
            var playlist = _dbContext.Playlists.Where(p => p.PlaylistId == playlistId);
            var userPlaylist = _dbContext.UserPlaylists.Where(p => p.PlaylistId == playlistId);

            _dbContext.Remove(playlist);
            _dbContext.Remove(userPlaylist);
        }

        public ClientModels.Playlist UpdatePlaylist(ClientModels.Playlist playlist)
        {
            var currentPlaylist = _dbContext.Playlists.FirstOrDefault(p => p.PlaylistId == playlist.PlaylistId);

            if (currentPlaylist != null)
            {
                currentPlaylist.Name = playlist.Name;
                _dbContext.Update(currentPlaylist);
            }

            return playlist;
        }
    }
}
