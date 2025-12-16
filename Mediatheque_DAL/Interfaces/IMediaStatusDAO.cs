using Mediatheque_DAL.Models;

namespace Mediatheque_DAL.Interfaces
{
    public interface IMediaStatusDAO
    {
        List<MediaStatus> GetAll();
        MediaStatus? GetById(int mediaId, int userId);
        List<MediaStatus>? GetByStatus(int userId, string status);
        MediaStatus Insert(MediaStatus mediaStatus);
        MediaStatus? Update(MediaStatus mediaStatus);
        bool Delete(int mediaId, int userId);
    }
}
