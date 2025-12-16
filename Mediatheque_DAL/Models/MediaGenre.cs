using System.Text.Json.Serialization;

namespace Mediatheque_DAL.Models
{
    public class MediaGenre
    {
        public required int MediaId { get; set; }
        public required int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        public MediaItem MediaItem { get; set; }
    }
}
