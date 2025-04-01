
using ChristianApi.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace TestChristianApi;

[TestClass]
public class SermonServiceTest
{

    private SermonService Setup()
    {
		var inMemorySettings = new Dictionary<string, string?>
		{
			{"OpenAICredentials:ApiKey", "TopLevelValue"},
			{"OpenAICredentials:ApiHost", "SectionValue"},
		};

		IConfiguration configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(inMemorySettings)
			.Build();
		return new SermonService(configuration);

	}
	[TestMethod]
    public void CreateRequest_Pass()
    {
        var service = Setup();
        string prompt = "hello test prompt 1";
        var result = service.CreateRequest(prompt);

        Assert.IsNotNull(result);
        Assert.AreEqual("gpt-4o", result.Model);
        Assert.AreEqual(1, result.Messages.Count);
        Assert.AreEqual(prompt, result.Messages[0].Content);
	}
}
