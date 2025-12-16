using System.Text.Json.Serialization;

namespace Mediatheque_DAL.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public required string GenreName { get; set; }
        public virtual ICollection<MediaGenre> MediaGenres { get; set; }
    }
}
