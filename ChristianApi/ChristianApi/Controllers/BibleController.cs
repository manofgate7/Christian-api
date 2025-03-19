using ChristianApi.Models;
using ChristianApi.Services;
using ChristianApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ChristianApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BibleController : ControllerBase
    {
        private readonly ILogger<BibleController> _logger;
        private readonly IBibleVerseService _verseServices;
        public BibleController(ILogger<BibleController> logger, IBibleVerseService bibleVerseServices)
        {
            _logger = logger;
            _verseServices = bibleVerseServices;
        }

        [HttpGet("GetBibleVerse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BibleVerse> GetBibleVerse([FromQuery] int? id, [FromQuery] string? verseNumber )
        {
            
            BibleVerse result =  _verseServices.GetBibleVerse(id, verseNumber);
            if (result.BibleVerseId == 0)
            {
               return NotFound();
            }

            return result;
        }
    }
}
