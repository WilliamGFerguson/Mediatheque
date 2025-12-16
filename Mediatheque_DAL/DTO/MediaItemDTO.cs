namespace Mediatheque_DAL.DTO
{
    public class MediaItemDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Type { get; set; }
        public required int Year { get; set; }
        public required int LengthMinutes { get; set; }
        public required List<string> Genres { get; set; }
    }
}
