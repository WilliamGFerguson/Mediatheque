namespace Mediatheque_DAL.Models;

internal class CsvMediaItem
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public int LengthMinutes { get; set; }

    public string MediaGenres { get; set; }
}
