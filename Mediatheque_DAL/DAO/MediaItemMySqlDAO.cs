using Mediatheque_DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mediatheque_DAL.Models;
using Mediatheque_DAL.DTO;


namespace Mediatheque_DAL.DAO
{
    public class MediaItemMySqlDAO : IMediaItemDAO
    {
        private readonly MediaDbContext _context;
        private readonly IMediaGenreDAO _mediaGenreDAO;
        private readonly IGenreDAO _genreDAO;
       


        public MediaItemMySqlDAO(MediaDbContext context, IGenreDAO genreMySqlDAO, IMediaGenreDAO mediaGenreMySqlDAO)
        {
            _context = context;
            _genreDAO = genreMySqlDAO;
            _mediaGenreDAO = mediaGenreMySqlDAO;
        }

        public List<MediaItemDTO> GetAll()
        {
            return _context.MediaItems.Include(mg => mg.MediaGenres)
                    .ThenInclude(g => g.Genre)
                .Select(mi => new MediaItemDTO
                {
                    Id = mi.Id,
                    Title = mi.Title,
                    Year = mi.Year,
                    Type = mi.Type,
                    LengthMinutes = mi.LengthMinutes,
                    Genres = mi.MediaGenres
                        .Select(mg => mg.Genre.GenreName)
                        .ToList()
                })
                .ToList();
        }

        public List<MediaItemSimple> GetAllMediaYear()
        {
            return _context.MediaItems.Select(x => new MediaItemSimple( x.Title, x.Year )).ToList();
        }

        public List<MediaItem> GetByYear(int year)
        {
            return _context.MediaItems
                .Where(m => m.Year == year).ToList();
        }

        public MediaItem? GetByIdNormal(int id)
        {
            return _context.MediaItems.Find(id);
        }

        public MediaItemDTO? GetById(int id)
        {
            return _context.MediaItems.Include(mg => mg.MediaGenres)
                    .ThenInclude(g => g.Genre)
                .Select(mi => new MediaItemDTO
                {
                    Id = mi.Id,
                    Title = mi.Title,
                    Year = mi.Year,
                    Type = mi.Type,
                    LengthMinutes = mi.LengthMinutes,
                    Genres = mi.MediaGenres
                        .Select(mg => mg.Genre.GenreName)
                        .ToList()
                })
                .FirstOrDefault(m => m.Id == id);
        }

        public MediaItemSimple? GetSimpleById(int id)
        {
            MediaItem? mediaItem = _context.MediaItems.Find(id);
            MediaItemSimple? mediaItemSimple = null;
            if (mediaItem != null)
            {
                mediaItemSimple = new(mediaItem.Title, mediaItem.Year);
            }

            return mediaItemSimple;
        }

        public List<MediaItemDTO> GetByGenre(List<string> reqGenres, bool inclusive)
        {
            var query = _context.MediaItems
                .Include(mg => mg.MediaGenres)
                .ThenInclude(g => g.Genre)
                .AsQueryable();

            if (inclusive)
            {
                query = query.Where(mi => mi.MediaGenres.Any(mg => reqGenres.Contains(mg.Genre.GenreName)));
            }
            else
            {
                query = query.Where(mi => mi.MediaGenres.Any(mg => !reqGenres.Contains(mg.Genre.GenreName)));
            }

            return query.Select(mi => new MediaItemDTO
            {
                Id = mi.Id,
                Title = mi.Title,
                Year = mi.Year,
                Type = mi.Type,
                LengthMinutes = mi.LengthMinutes,
                Genres = mi.MediaGenres
                            .Select(mg => mg.Genre.GenreName)
                            .ToList()
            }).ToList();
        }

        public MediaItem Insert(MediaItem mediaItem)
        {  
            _context.MediaItems.Add(mediaItem);
            _context.SaveChanges();

            return mediaItem;
        }

        public MediaItemDTO AddNewMedia(MediaItemDTO mediaItem)
        {
            MediaItem mediaToAdd = new()
            {
                Title = mediaItem.Title,
                Year = mediaItem.Year,
                Type = mediaItem.Type,
                LengthMinutes = mediaItem.LengthMinutes
            };

            mediaToAdd = Insert(mediaToAdd);

            List<Genre> genres = _genreDAO.GetAll();
            HashSet<string> genresName = new();
            foreach (Genre genre in genres)
            {
                genresName.Add(genre.GenreName.ToLower());
            }

            foreach (string mediaGenre in mediaItem.Genres)
            {
                Genre? genre;
                if(genresName.Add(mediaGenre.ToLower()))
                {
                    genre = new() { GenreName = mediaGenre };
                    genre = _genreDAO.Insert(genre);
                }
                else
                {
                    genre = _genreDAO.GetByGenre(mediaGenre);
                }

                _mediaGenreDAO.Insert(new MediaGenre { GenreId = genre.Id, MediaId = mediaToAdd.Id });
            }

            _context.SaveChanges();

            return GetById(mediaToAdd.Id);
        }

        public MediaItemDTO? UpdateMedia(MediaItemDTO mediaItem)
        {
            MediaItem? existing = _context.MediaItems.Find(mediaItem.Id);
            if (existing == null)
            {
                return null;
            }

            existing.Title = mediaItem.Title;
            existing.Type = mediaItem.Type;
            existing.Year = mediaItem.Year;
            existing.LengthMinutes = mediaItem.LengthMinutes;

            List<MediaGenre> oldMediaGenres = _context.Media_Genres
                .Where(mg => mg.MediaId == mediaItem.Id)
                .Select(mg => new MediaGenre
                { 
                    GenreId = mg.GenreId,
                    MediaId = mg.MediaId
                })
                .ToList();

            _context.Media_Genres.RemoveRange(oldMediaGenres);
            _context.SaveChanges();

            List<Genre> genres = _genreDAO.GetAll();
            HashSet<string> genresName = new();
            foreach (Genre genre in genres)
            {
                genresName.Add(genre.GenreName.ToLower());
            }

            foreach (string mediaGenre in mediaItem.Genres)
            {
                Genre? genre;
                if (genresName.Add(mediaGenre.ToLower()))
                {
                    genre = new() { GenreName = mediaGenre };
                    genre = _genreDAO.Insert(genre);
                }
                else
                {
                    genre = _genreDAO.GetByGenre(mediaGenre);
                }

                _mediaGenreDAO.Insert(new MediaGenre { GenreId = genre.Id, MediaId = existing.Id });
            }

            _context.SaveChanges();

            return GetById(existing.Id);
        }

        public MediaItem? Update(MediaItem mediaItem)
        {
            MediaItem? existing = _context.MediaItems.Find(mediaItem.Id);
            if (existing == null)
            {
                return null;
            }

            existing.Type = mediaItem.Type;
            existing.Title = mediaItem.Title;
            existing.LengthMinutes = mediaItem.LengthMinutes;
            existing.Year = mediaItem.Year;
            existing.MediaGenres = mediaItem.MediaGenres;
            existing.MediaStatuses = mediaItem.MediaStatuses;
            _context.SaveChanges();

            return existing;
        }

        public bool Delete(int id)
        {
            MediaItem? mediaItem = _context.MediaItems.Find(id);
            if (mediaItem != null)
            {
                _context.MediaItems.Remove(mediaItem);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool IsTitleInDb(string title)
        {
            MediaItem? mediaItem = _context.MediaItems.FirstOrDefault(x => x.Title == title);

            if (mediaItem != null)
            {
                return true;
            }
            return false;
        }

        public string IsValidMedia(MediaItemDTO mediaItem)
        {
            if (mediaItem.Title.Trim() == "" || mediaItem.Type.Trim() == "")
            {
                return "Cannot enter an empty title or type";
            }
            if (mediaItem.Year < 1900 || mediaItem.Year > DateTime.Now.Year)
            {
                return $"Year must be between 1900 and {DateTime.Now.Year}";
            }
            if (mediaItem.LengthMinutes < 1)
            {
                return "Length must be at least 1 minutes";
            }
            if (mediaItem.Genres.Count == 0)
            {
                return "Must contain at least one genre";
            }
            return "valid_media";
        }
    }
}
