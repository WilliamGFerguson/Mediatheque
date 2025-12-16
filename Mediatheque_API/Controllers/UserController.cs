using Mediatheque_DAL.DTO;
using Mediatheque_DAL.Interfaces;
using Mediatheque_DAL.Models;
using Mediatheque_DAL.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Mediatheque_API.Controllers;

[ApiController]
[Route("/user")]
public class UserController : ControllerBase
{
    private readonly IUserDAO _userDAO;
    private readonly IMediaItemDAO _mediaItemDAO;
    private readonly IMediaStatusDAO _statusDAO;

    public UserController(IUserDAO userDAO, IMediaItemDAO mediaItemDAO, IMediaStatusDAO mediaStatusDAO)
    {
        _userDAO = userDAO;
        _mediaItemDAO = mediaItemDAO;
        _statusDAO = mediaStatusDAO;
    }

    [HttpPost("signup")]
    public ActionResult<User> Signup(string username, string email)
    {
        User? user = _userDAO.Insert(new User { Username = username, Email = email });

        return Ok($"Welcome {user.Username} !");
    }

    [HttpPut("{userId}/{mediaId}/add/status/{status}")]
    public ActionResult<User> AddStatus(int mediaId, int userId, string status)
    {
        if (!ValidData.Status.Contains(status))
        {
            return BadRequest("Invalid media status");
        }

        User? user = _userDAO.GetById(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        MediaItemDTO? mediaItem = _mediaItemDAO.GetById(mediaId);
        if (mediaItem == null)
        {
            return NotFound("Media item not found");
        }

        MediaStatus? mediaStatus = _statusDAO.GetById(mediaId, userId);
        if (mediaStatus == null)
        {
            mediaStatus = new MediaStatus { UserId = userId, MediaId = mediaId, Status = status };
            mediaStatus = _statusDAO.Insert(mediaStatus);
        }
        else
        {
            mediaStatus.Status = status;
            mediaStatus = _statusDAO.Update(mediaStatus);
        }

        return Ok($"{user.Username} marked {mediaItem.Title} as '{status}'");
    }

    [HttpGet("{userId}/list/seen")]
    public ActionResult<List<MediaItemDTO>> GetSeenMedias(int userId)
    {
        if (_userDAO.GetById(userId) == null)
        {
            return NotFound($"User {userId} does not exist");
        }

        List<MediaStatus>? mediaStatusInfo = _statusDAO.GetByStatus(userId, "Seen");

        if (mediaStatusInfo == null)
        {
            return NotFound($"User {userId} has not seen any medias");
        }

        List<MediaItemDTO?> seenMedias = mediaStatusInfo.Select(ms =>
        {
            return _mediaItemDAO.GetById(ms.MediaId);
        }).ToList();

        return Ok(seenMedias);
    }

    [HttpGet("{userId}/stats/seen/byYear/{year}")]
    public ActionResult<string> SeenByYear(int userId, int year)
    {
        if (_userDAO.GetById(userId) == null)
        {
            return NotFound($"User {userId} does not exist");
        }

        int result = 0;

        List<MediaStatus>? mediaStatusInfo = _statusDAO.GetByStatus(userId, "Seen");

        if (mediaStatusInfo == null)
        {
            return NotFound();
        }

        foreach (MediaStatus ms in mediaStatusInfo)
        {
            MediaItemDTO? media = _mediaItemDAO.GetById(ms.MediaId);
            if (media != null && media.Year == year)
            {
                result++;
            }
        }
        
        return Ok($"User {userId} has seen {result} medias made in {year}");
    }

    [HttpGet("{userId}/stats/seen/averageLength")]
    public ActionResult<string> SeenAverageLength(int userId)
    {
        if (_userDAO.GetById(userId) == null)
        {
            return NotFound($"User {userId} does not exist");
        }

        List<MediaStatus>? mediaStatusInfo = _statusDAO.GetByStatus(userId, "Seen");

        if (mediaStatusInfo == null)
        {
            return NotFound();
        }

        int totalTime = 0;
        int seenMedias = 0;

        foreach (MediaStatus ms in mediaStatusInfo)
        {
            MediaItemDTO? media = _mediaItemDAO.GetById(ms.MediaId);
            if (media != null)
            {
                totalTime += media.LengthMinutes;
                seenMedias++;
            }
        }

        double averageLength = totalTime / seenMedias;

        return Ok($"User {userId}'s media length average (in minutes): {averageLength} minutes");
    }

    [HttpGet("/{userId}/FavoriteGenres")]
    public ActionResult<List<GenreCountDTO>> GetUserFavoriteGenres(int userId)
    {
        User? user = _userDAO.GetById(userId);
        if (user == null)
        {
            return NotFound($"User {userId} does not exist");
        }
        return Ok(_userDAO.GetUserGenreCount(user));
    }
}