using Mediatheque_DAL.Interfaces;
using Mediatheque_DAL.Models;

namespace Mediatheque_DAL.DAO
{
    public class GenreMySqlDAO : IGenreDAO
    {
        private readonly MediaDbContext _context;

        public GenreMySqlDAO(MediaDbContext context)
        {
            _context = context;
        }

        public List<Genre> GetAll()
        {
            return _context.Genres.ToList();
        }

        public Genre? GetById(int id)
        {
            return _context.Genres.Find(id);
        }

        public Genre? GetByGenre(string genre)
        {
            return _context.Genres.Where(g => g.GenreName == genre).FirstOrDefault();
        }

        public Genre Insert(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();

            return genre;
        }

        public Genre? Update(Genre genre)
        {
            Genre? existing = _context.Genres.Find(genre.Id);
            if (existing == null)
            {
                return null;
            }

            existing.GenreName = genre.GenreName;
            existing.MediaGenres = genre.MediaGenres;
            _context.SaveChanges();

            return existing;
        }

        public bool Delete(int id)
        {
            Genre? genre = _context.Genres.Find(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
