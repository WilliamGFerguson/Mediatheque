using Mediatheque_DAL.Interfaces;
using Mediatheque_DAL.Models;

namespace Mediatheque_DAL.DAO
{
    public class MediaStatusMySqlDAO : IMediaStatusDAO
    {
        private readonly MediaDbContext _context;

        public MediaStatusMySqlDAO(MediaDbContext context)
        {
            _context = context;
        }

        public List<MediaStatus> GetAll()
        {
            return _context.Media_Statuses.ToList();
        }

        public MediaStatus? GetById(int mediaId, int userId)
        {
            return _context.Media_Statuses.Find(userId, mediaId);
        }

        public List<MediaStatus> GetByStatus(int userId, string status)
        {
            return _context.Media_Statuses
                .Where(ms => ms.UserId == userId && ms.Status == status).ToList();
        }

        public MediaStatus Insert(MediaStatus mediaStatus)
        {
            _context.Media_Statuses.Add(mediaStatus);
            _context.SaveChanges();

            return mediaStatus;
        }

        public MediaStatus? Update(MediaStatus mediaStatus)
        {
            MediaStatus? existing = _context.Media_Statuses.Find(mediaStatus.UserId, mediaStatus.MediaId);
            if (existing == null)
            {
                return null;
            }

            existing.Status = mediaStatus.Status;
            existing.User = mediaStatus.User;
            existing.MediaItem = mediaStatus.MediaItem;
            _context.SaveChanges();

            return existing;
        }

        public bool Delete(int mediaId, int userId)
        {
            MediaStatus? mediaStatus = _context.Media_Statuses.Find(userId, mediaId);
            if (mediaStatus != null)
            {
                _context.Media_Statuses.Remove(mediaStatus);
                _context.SaveChanges();

                return true;
            }
            return false;
        }
    }
}
