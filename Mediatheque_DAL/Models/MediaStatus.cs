using System.Text.Json.Serialization;

namespace Mediatheque_DAL.Models
{
    public class MediaStatus
    {
        public required int UserId { get; set; }
        public required int MediaId { get; set; }
        public required string Status { get; set; }
        public MediaItem? MediaItem { get; set; }
        public virtual User? User { get; set; }
    }
}
