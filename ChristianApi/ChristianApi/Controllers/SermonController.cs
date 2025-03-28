using ChristianApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChristianApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SermonController: ControllerBase
	{
		private readonly ILogger<SermonController> _logger;
		private readonly ISermonService _sermonService;
		public SermonController(ILogger<SermonController> logger, ISermonService sermonService)
		{
			_logger = logger;
			_sermonService = sermonService;
		}

		[HttpPost("SermonAnalysis")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<string> GetSermonAnalysis( IFormFile file)
		{
			string result = _sermonService.GetSermonAnalysis(file);
			if(string.IsNullOrEmpty(result))
			{
				return NotFound();
			}
			
			return result;
		}
	}
}
