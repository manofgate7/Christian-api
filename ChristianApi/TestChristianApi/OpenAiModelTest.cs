using ChristianApi.Models;

namespace TestChristianApi;

[TestClass]
public class OpenAiModelTest
{
    [TestMethod]
    public void Choice_Pass()
    {
        var test = new Choice()
        {
            index = 1,
        };
        Assert.IsNull(test.logprobs);
		Assert.IsNull(test.message);
		Assert.IsNull(test.finish_reason);
	}

    [TestMethod]
    public void OpenAIResponseDto_Pass()
    {
        var test = new OpenAIResponseDto()
        {
            created = 1,
            usage = new Usage()
            {
                prompt_tokens = 1,
                total_tokens = 1,
                completion_tokens = 1,
            }
        };
        Assert.AreEqual(1, test.created);
        Assert.IsNotNull(test.usage);
        Assert.AreEqual(1, test.usage.total_tokens);
        Assert.IsNull(test.id);
        Assert.IsNull(test.choices);
        Assert.IsNull(test.model);
        Assert.IsNull(test.@object);
    }

    [TestMethod]
    public void OpenAIErrorResponseDto_Pass()
    {
        var test = new OpenAIErrorResponseDto() {
            Error = new OpenAIError()
            {
				Message = "test"
			}
        };
        Assert.IsNotNull(test);
        Assert.AreEqual("test", test.Error.Message);
        Assert.IsNull(test.Error.Code);
        Assert.IsNull(test.Error.Type);
		Assert.IsNull(test.Error.Param);
	}
}
