using ChristianApi.Models;
using ChristianApi.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace ChristianApi.Services
{
	public class SermonService : ISermonService
	{
		private readonly IConfiguration _configuration;
		private readonly string apiKey;
		private readonly string baseURL;
		private readonly string basePrompt = "There are 4 questions to answer. response should be a bulleted short answer of the question. first, on a scale of 1 to 10, rate the clarity of the main point of the below sermon. second, on a scale of 1 to 10 how biblically accurate is the below sermon. third, how many points are in the sermon below. and fourth how long is the sermon? : \r\n";
		public SermonService(IConfiguration configuration)
		{
			_configuration = configuration;
			apiKey = _configuration.GetValue<string>("OpenAICredentials:ApiKey") ?? string.Empty;
			baseURL = _configuration.GetValue<string>("OpenAICredentials:ApiHost") ?? string.Empty;

		}

		internal OpenAIRequestDto CreateRequest(string prompt)
		{
			return  new OpenAIRequestDto
			{
				Model = "gpt-4o",
				Messages = new List<OpenAIMessageRequestDto>{
					new OpenAIMessageRequestDto
					{
						Role = "user",
						Content = prompt
					}
				},
				MaxTokens = 10000
			};

		}

		public async Task<string> TriggerOpenAI(string prompt)
		{

			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

			var request = CreateRequest(prompt);
			var json = JsonSerializer.Serialize(request);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await client.PostAsync(baseURL, content);
			var resjson = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
			{
				var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
				throw new System.Exception(errorResponse?.Error.Message);
			}
			var data = JsonSerializer.Deserialize<OpenAIResponseDto>(resjson);
			var responseText = data?.choices?.Count > 0 ?   data?.choices[0]?.message?.content : string.Empty;

			return responseText ?? string.Empty;
		}

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
			var result = TriggerOpenAI(basePrompt + content);
			
			return result.Result;
		}
	}
}
