using CsvHelper;
using System.Globalization;
using Mediatheque_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Mediatheque_DAL.Interfaces;
using Mediatheque_DAL.Utils;

namespace Mediatheque_DAL.DAO;

public class DbInitializer
{
    private readonly MediaDbContext _context;
    private readonly IMediaItemDAO _mediaItemDAO;
    private readonly IGenreDAO _genreDAO;
    private readonly IMediaGenreDAO _mediaGenreDAO;
    private readonly IUserDAO _userDAO;

    public DbInitializer(MediaDbContext context, IMediaItemDAO mediaItemDAO, IGenreDAO genreDAO, IMediaGenreDAO mediaGenreDAO, IUserDAO userDAO)
    {
        _context = context;
        _mediaItemDAO = mediaItemDAO;
        _genreDAO = genreDAO;
        _mediaGenreDAO = mediaGenreDAO;
        _userDAO = userDAO;
    }

    public void SeedMedias()
    {
        _context.Database.Migrate();
        if (_context.MediaItems.Any()) { return; }

        string filePath = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
            "Mediatheque_DAL", "Ressources", "csv_media.csv");

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var mediaItemsData = csv.GetRecords<CsvMediaItem>().ToList();
        var genreTracker = new HashSet<string>();

        foreach (var item in mediaItemsData)
        {
            var media = new MediaItem
            {
                Title = item.Title,
                Type = item.Type,
                Year = item.Year,
                LengthMinutes = item.LengthMinutes
            };

            media = _mediaItemDAO.Insert(media);

            Genre? genre;
            var genreNames = item.MediaGenres.Split(',', StringSplitOptions.RemoveEmptyEntries);
            media.MediaGenres = genreNames.Select(genreName =>
            {
                if (genreTracker.Add(genreName))
                {
                    genre = _genreDAO.Insert(new Genre { GenreName = genreName });
                }
                else
                {
                    genre = _genreDAO.GetByGenre(genreName);
                }

                MediaGenre mediaGenre = new MediaGenre { GenreId = genre.Id, MediaId = media.Id };
                return _mediaGenreDAO.Insert(mediaGenre);
            }).ToList();
        }
    }

    public void SeedUsers()
    {
        string filePath = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).FullName,
            "Mediatheque_DAL", "Ressources", "users.csv");

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var usersData = csv.GetRecords<CsvUser>().ToList();
        Random rnd = new();

        foreach (var user in usersData)
        {
            var newUser = new User { Username = user.Username, Email = user.Email };
            newUser = _userDAO.Insert(newUser);

            int mediasToAdd = rnd.Next(5, 15);
            AddMediaStatus(newUser.Id, mediasToAdd);
        }
    }

    private void AddMediaStatus(int id, int mediasToAdd)
    {
        int mediaAmount = _context.MediaItems.Count();

        Random rnd = new();
        HashSet<int> mediaIds = new();

        int index = 0;
        while (index < mediasToAdd)
        {
            int mediaId = rnd.Next(1, mediaAmount);
            if (!mediaIds.Add(mediaId))
            {
                continue;
            }

            int statusId = rnd.Next(ValidData.Status.Count());
            var status = ValidData.Status.ElementAt(statusId);

            _context.Media_Statuses.Add(new MediaStatus
            {
                UserId = id,
                MediaId = mediaId,
                Status = status
            });

            index++;
        }
        _context.SaveChanges();
    }
}
