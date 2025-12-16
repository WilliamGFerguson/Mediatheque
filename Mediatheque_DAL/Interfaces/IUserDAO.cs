using Mediatheque_DAL.DTO;
using Mediatheque_DAL.Models;

namespace Mediatheque_DAL.Interfaces
{
    public interface IUserDAO
    {
        List<User> GetAll();
        User? GetById(int id);
        User Insert(User user);
        User? Update(User user);
        List<GenreCountDTO> GetUserGenreCount(User user);
        bool Delete(int id);
    }
}
