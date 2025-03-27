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
            
            BibleVerse result =  _verseServices.GetBibleVerse(id, verseNumber ?? string.Empty);
            if (result.BibleVerseId == 0)
            {
               return NotFound();
            }

            return result;
        }

        [HttpPost("SaveBibleVerse")]
        public ActionResult SaveBibleVerse(string verseNumber, string verse)
        {
            _verseServices.SaveBibleVerse(new BibleVerse() { Verse = verse, VerseNumber = verseNumber });
            return Ok();
        }

        [HttpGet("GetBibleVerseRankById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BibleVerseRank> GetBibleVerseRankById(int Id)
        {
            BibleVerseRank result = _verseServices.GetBibleVerseRankById(Id);
			if (result.BibleVerseRankId == 0)
			{
				return NotFound();
			}

			return result;
		}

        [HttpGet("GetRanksByBibleVerse")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<BibleVerseRank>> GetRanksByBibleVerse(int bibleVerseId)
        {
            List<BibleVerseRank> result = _verseServices.GetBibleVerseRanksByVerseId(bibleVerseId);
            if(result.Count == 0)
            {
                return NotFound();
            }
            return result;
        }

		[HttpGet("GetAverageRankByBibleVerse")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<double?> GetAverageRankByBibleVerse(int bibleVerseId)
		{
			double result = _verseServices.GetAverageBibleVerseRankForVerse(bibleVerseId);
            if(result == 0)
            {
                return NotFound();
            }
			
			return result;
		}

		[HttpPost("SaveBibleVerseRank")]
		public ActionResult SaveBibleVerseRank(int? verseRankId, int verseId, int rank)
		{
			_verseServices.SaveBibleVerseRank(
                    new BibleVerseRank() { 
                        BibleVerseRankId = verseRankId ?? 0,
                        BibleVerseId = verseId,
                        RankNumber = rank
                    });
			return Ok();
		}


		[HttpGet("GetBibleVersesWithAverage")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<List<Tuple<BibleVerse, double>>> GetBibleVersesWithAverage()
		{
			var result = _verseServices.GetBibleVersesWithAverageRank();
			if (result.Count == 0)
			{
				return NotFound();
			}

			return result;
		}

	}
}
