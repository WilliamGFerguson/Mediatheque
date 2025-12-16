using Mediatheque_DAL.DTO;
using Mediatheque_DAL.Interfaces;
using Mediatheque_DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Mediatheque_API.Controllers;

[ApiController]
[Route("mediaItems")]

public class MediaItemsController : ControllerBase
{
    private readonly MediaDbContext _context;
    private readonly IMediaItemDAO _mediaItemDAO;

    public MediaItemsController(MediaDbContext context, IMediaItemDAO mediaItemDAO)
    {
        _context = context;
        _mediaItemDAO = mediaItemDAO;
    }

    [HttpGet("GetAll")]
    public ActionResult<List<MediaItemDTO>> GetAll()
    {
        return Ok(_mediaItemDAO.GetAll());
    }

    [HttpGet("GetAll/MediaYear")]
    public ActionResult<List<MediaItemSimple>> GetAllMediaYear()
    {
        return Ok(_mediaItemDAO.GetAllMediaYear());
    }

    [HttpGet("Get/{id}")]
    public ActionResult<MediaItemDTO> GetItem(int id)
    {
        MediaItemDTO? mediaItem = _mediaItemDAO.GetById(id);
        if (mediaItem == null)
        {
            return NotFound();
        }
        return Ok(mediaItem);
    }

    [HttpGet("Get/Genre/{inclusive}")]
    public ActionResult<List<MediaItemDTO>> GetByGenre([FromQuery] List<string> genres, bool inclusive = true)
    {
        List<MediaItemDTO>? mediaItems = _mediaItemDAO.GetByGenre(genres, inclusive);

        if (mediaItems == null || mediaItems.Count == 0)
        {
            return NotFound($"No medias found");
        }

        return Ok(mediaItems);
    }

    [HttpGet("Get/MediaYear/{id}")]
    public ActionResult<MediaItemSimple> GetItemSimple(int id)
    {
        MediaItemSimple? mediaItemSimple = _mediaItemDAO.GetSimpleById(id);
        if (mediaItemSimple == null)
        {
            return NotFound();
        }
        return Ok(mediaItemSimple);
    }

    [HttpPost("Add/Media")]
    public ActionResult<MediaItemDTO> AddMedia(MediaItemDTO mediaItem)
    {
        string validMedia = _mediaItemDAO.IsValidMedia(mediaItem);

        if (validMedia != "valid_media")
        {
            return BadRequest(validMedia);
        }

        if (_mediaItemDAO.IsTitleInDb(mediaItem.Title))
        {
            return Conflict("A media with the same title already exist");
        }

        return Ok(_mediaItemDAO.AddNewMedia(mediaItem));
    }

    [HttpPut("Update")]
    public ActionResult<MediaItemDTO> Update(MediaItemDTO mediaItem)
    {
        string validMedia = _mediaItemDAO.IsValidMedia(mediaItem);

        if (validMedia != "valid_media")
        {
            return BadRequest(validMedia);
        }

        MediaItemDTO? updatedMedia = _mediaItemDAO.UpdateMedia(mediaItem);

        if (updatedMedia != null)
        {
            return Ok(updatedMedia);
        }
        return NotFound();
    }
}
