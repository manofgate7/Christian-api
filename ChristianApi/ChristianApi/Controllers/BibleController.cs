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

        [HttpGet(Name = "GetBibleVerseById")]
        //[Route("/GetBibleVerseById/{id:int}")]
        public BibleVerse GetBibleVerse([FromQuery] int id )
        {
            return _verseServices.GetBibleVerseById(id);
        }
    }
}
