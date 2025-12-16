using Mediatheque_DAL.Models;

namespace Mediatheque_DAL.Interfaces
{
    public interface IGenreDAO
    {
        List<Genre> GetAll();
        Genre? GetById(int id);
        Genre? GetByGenre(string genre);
        Genre Insert (Genre genre);
        Genre? Update (Genre genre);
        bool Delete (int id);
    }
}
