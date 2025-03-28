using ChristianApi.Services.Interfaces;

namespace ChristianApi.Services
{
	public class SermonService : ISermonService
	{
		public string GetSermonAnalysis(IFormFile file)
		{
			//4 points
			//1) rate 1-10 scale, bible acuracy
			//2) rate 1-10 scale, clarity of main point
			//3) how many points in the sermon
			//4) how long it is
			string content;
			using (var sr = new StreamReader(file.OpenReadStream()))
			{
				content = sr.ReadToEnd();
				
			}


			return content;
		}
	}
}
