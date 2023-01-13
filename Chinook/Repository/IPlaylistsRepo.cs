using Chinook.ClientModels;

namespace Chinook.Repository
{
    public interface IPlaylistsRepo
    {
        Task<Playlist> GetUserPaylistAsync(long playlistId, string userId);

        Task<List<Playlist>> GetUserPlaylists(string userId);

        Task AddPlaylist(Playlist playlist, string userId);

        void RemovePlaylist(long playlistId);

        Playlist UpdatePlaylist(Playlist playlist);
    }
}
