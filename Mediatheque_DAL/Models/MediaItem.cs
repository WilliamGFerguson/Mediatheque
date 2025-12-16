using CsvHelper.Configuration.Attributes;
using System.Text.Json.Serialization;

namespace Mediatheque_DAL.Models
{
    public class MediaItem
    {
        public int Id { get; set; }

        public required string Type { get; set; }
        public required string Title { get; set; }
        public int Year { get; set; }
        public int LengthMinutes { get; set; }
        public ICollection<MediaGenre> MediaGenres { get; set; }
        public virtual ICollection<MediaStatus> MediaStatuses { get; set; }
    }
}
