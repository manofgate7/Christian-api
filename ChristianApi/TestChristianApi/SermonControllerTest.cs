using ChristianApi.Controllers;
using ChristianApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestChristianApi;

[TestClass]
public class SermonControllerTest
{
    private SermonController Setup()
    {
		Mock<ILogger<SermonController>> mLogger = new Mock<ILogger<SermonController>>();
        Mock<ISermonService> mSermonService = new Mock<ISermonService>();
        mSermonService.Setup(s => s.GetSermonAnalysis(It.IsAny<IFormFile>()))
            .Returns((IFormFile file) => file.FileName);
        return new SermonController(mLogger.Object, mSermonService.Object);
	}

	[TestMethod]
    public void GetSermonAnalysis_Pass()
    {
		SermonController sermonAnalysis = Setup();

        FormFile formFile = new FormFile(new MemoryStream(), 0, 0, "hello", "test");
        var result = sermonAnalysis.GetSermonAnalysis(formFile);
        Assert.IsNotNull(result);
        Assert.AreEqual("test", result.Value);
    }

	[TestMethod]
	public void GetSermonAnalysis_Empty()
	{
		SermonController sermonAnalysis = Setup();

		FormFile formFile = new FormFile(new MemoryStream(), 0, 0, "hello", "");
		var result = sermonAnalysis.GetSermonAnalysis(formFile);
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
		Assert.AreEqual(404, ((StatusCodeResult)result.Result).StatusCode);
	}
}
