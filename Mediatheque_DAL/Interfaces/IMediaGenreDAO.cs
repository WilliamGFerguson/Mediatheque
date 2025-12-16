using Mediatheque_DAL.Models;

namespace Mediatheque_DAL.Interfaces
{
    public interface IMediaGenreDAO
    {
        List<MediaGenre> GetAll();
        MediaGenre? GetById(int mediaId, int genreId);
        MediaGenre Insert(MediaGenre mediaGenre);
        MediaGenre? Update(MediaGenre mediaGenre);
        bool Delete(int mediaId, int genreId);
    }
}
