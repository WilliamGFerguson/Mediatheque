using Mediatheque_DAL.Interfaces;
using Mediatheque_DAL.Models;

namespace Mediatheque_DAL.DAO
{
    public class MediaGenreMySqlDAO : IMediaGenreDAO
    {
        private readonly MediaDbContext _context;

        public MediaGenreMySqlDAO(MediaDbContext context)
        {
            _context = context;
        }

        public List<MediaGenre> GetAll()
        {
            return _context.Media_Genres.ToList();
        }

        public MediaGenre? GetById(int mediaId, int genreId)
        {
            return _context.Media_Genres.Find(genreId, mediaId);
        }

        public MediaGenre Insert(MediaGenre mediaGenre)
        {
            _context.Media_Genres.Add(mediaGenre);
            _context.SaveChanges();

            return mediaGenre;
        }

        public MediaGenre? Update(MediaGenre mediaGenre)
        {
            MediaGenre? existing = _context.Media_Genres.Find(mediaGenre.GenreId, mediaGenre.MediaId);
            if (existing == null)
            {
                return null;
            }

            existing.Genre = mediaGenre.Genre;
            existing.MediaItem = mediaGenre.MediaItem;
            _context.SaveChanges();

            return existing;
        }

        public bool Delete(int mediaId, int genreId)
        {
            MediaGenre? mediaGenre = _context.Media_Genres.Find(mediaId, genreId);
            if (mediaGenre != null)
            {
                _context.Media_Genres.Remove(mediaGenre);
                _context.SaveChanges();

                return true;
            }
            return false;
        }
    }
}
