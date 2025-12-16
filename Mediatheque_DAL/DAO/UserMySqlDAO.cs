using Mediatheque_DAL.DTO;
using Mediatheque_DAL.Interfaces;
using Mediatheque_DAL.Models;

namespace Mediatheque_DAL.DAO
{
    public class UserMySqlDAO : IUserDAO
    {
        private readonly MediaDbContext _context;

        public UserMySqlDAO(MediaDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User Insert(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User? Update(User user)
        {
            User? existing = _context.Users.Find(user.Id);
            if (existing == null)
            {
                return null;
            }

            existing.Username = user.Username;
            existing.Email = user.Email;
            existing.MediaStatuses = user.MediaStatuses;
            _context.SaveChanges();

            return existing;
        }

        public bool Delete(int id)
        {
            User? user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public List<GenreCountDTO> GetUserGenreCount(User user)
        {
            return _context.MediaItems
                .Where(mi => mi.MediaStatuses.Any(ms => ms.UserId == user.Id))
                .SelectMany(mi => mi.MediaGenres.Select(mg => mg.Genre.GenreName))
                .GroupBy(genreName => genreName)
                .Select(g => new GenreCountDTO
                {
                    GenreName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToList();
        }
    }
}