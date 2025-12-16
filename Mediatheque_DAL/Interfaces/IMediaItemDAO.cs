using Mediatheque_DAL.DTO;
using Mediatheque_DAL.Models;

namespace Mediatheque_DAL.Interfaces
{
    public interface IMediaItemDAO
    {
        List<MediaItemDTO> GetAll();
        List<MediaItemSimple> GetAllMediaYear();
        List<MediaItem> GetByYear(int year);
        MediaItem? GetByIdNormal(int id);
        MediaItemDTO? GetById(int id);
        MediaItemSimple? GetSimpleById(int id);
        List<MediaItemDTO> GetByGenre(List<string> genres, bool inclusive);
        MediaItem Insert(MediaItem mediaItem);
        MediaItemDTO AddNewMedia(MediaItemDTO mediaItem);
        MediaItemDTO? UpdateMedia(MediaItemDTO mediaItem);
        MediaItem? Update(MediaItem mediaItem);
        bool Delete(int id);
        bool IsTitleInDb(string title);
        string IsValidMedia(MediaItemDTO mediaItem);
    }
}
