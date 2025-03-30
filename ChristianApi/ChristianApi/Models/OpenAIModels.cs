using System.Text.Json.Serialization;

namespace ChristianApi.Models
{
	public class OpenAIRequestDto
	{
		[JsonPropertyName("model")]
		public required string Model { get; set; }

		[JsonPropertyName("messages")]
		public required List<OpenAIMessageRequestDto> Messages { get; set; }

		[JsonPropertyName("temperature")]
		public float Temperature { get; set; }

		[JsonPropertyName("max_tokens")]
		public int MaxTokens { get; set; }
	}

	public class OpenAIMessageRequestDto
	{
		[JsonPropertyName("role")]
		public required string Role { get; set; }

		[JsonPropertyName("content")]
		public required string Content { get; set; }
	}

	public class OpenAIErrorResponseDto
	{
		[JsonPropertyName("error")]
		public required OpenAIError Error { get; set; }
	}
	public class OpenAIError
	{
		[JsonPropertyName("message")]
		public required string Message { get; set; }

		[JsonPropertyName("type")]
		public required string Type { get; set; }

		[JsonPropertyName("param")]
		public required string Param { get; set; }

		[JsonPropertyName("code")]
		public required string Code { get; set; }
	}

	public class OpenAIResponseDto
	{
		public required string Id { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
		public required string @object { get; set; }
		public int Created { get; set; }
		public required string Model { get; set; }
		public required List<Choice> Choices { get; set; }
		public required Usage Usage { get; set; }
	}

	public class Choice
	{
		public int Index { get; set; }
		public required Message Message { get; set; }
		public required object Logprobs { get; set; }
		public required string Finish_reason { get; set; }
	}
	public class Usage
	{
		public int prompt_tokens { get; set; }
		public int completion_tokens { get; set; }
		public int total_tokens { get; set; }
	}
	public class OpenAIChoice
	{
		public required string Text { get; set; }
		public float probability { get; set; }
		public required float[] Logprobs { get; set; }
		public required int[] Finish_reason { get; set; }
	}

	public class Message
	{
		public required string Role { get; set; }
		public required string Content { get; set; }
	}

}
