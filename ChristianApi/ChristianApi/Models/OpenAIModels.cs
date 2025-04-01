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
		public string? Message { get; set; }

		[JsonPropertyName("type")]
		public string? Type { get; set; }

		[JsonPropertyName("param")]
		public string? Param { get; set; }

		[JsonPropertyName("code")]
		public string? Code { get; set; }
	}

	public class OpenAIResponseDto
	{
		public string? id { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
		public string? @object { get; set; }
		public int created { get; set; }
		public string? model { get; set; }
		public List<Choice>? choices { get; set; }
		public Usage? usage { get; set; }
	}

	public class Choice
	{
		public int index { get; set; }
		public Message? message { get; set; }
		public object? logprobs { get; set; }
		public string? finish_reason { get; set; }
	}
	public class Usage
	{
		public int prompt_tokens { get; set; }
		public int completion_tokens { get; set; }
		public int total_tokens { get; set; }
	}

	public class Message
	{
		public required string role { get; set; }
		public required string content { get; set; }
	}

}
