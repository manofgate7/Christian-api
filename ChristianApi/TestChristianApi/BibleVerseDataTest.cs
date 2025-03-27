using ChristianApi.Data;
using ChristianApi.Data.Interfaces;
using ChristianApi.Models;
using Moq;
using System.Text;

namespace TestChristianApi;

[TestClass]
public class BibleVerseDataTest
{
    internal Mock<IFileManager> SetUpFileManager (string fileContents)
    {
		Mock<IFileManager> mockFileManager = new Mock<IFileManager>();
		byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fileContents);

		MemoryStream fakeMemoryStream = new MemoryStream(fakeFileBytes);

		mockFileManager.Setup(fileManager => fileManager.StreamReader(It.IsAny<string>(), It.IsAny<Encoding>()))
					   .Returns(() => new StreamReader(fakeMemoryStream, Encoding.UTF8));
		return mockFileManager;
	}

	[TestMethod]
    public void ReadFile_BibleVerse_Pass()
    {
		string bibleVerseContents = "3|Ephisans 5:21|Submit to one another out of reverence for Christ\r\n";
		Mock<IFileManager> fileManager = SetUpFileManager(bibleVerseContents);
		BibleVerseData bibleVerseData = new BibleVerseData(fileManager.Object);

		var result = bibleVerseData.ReadFile();
		Assert.AreEqual(1, result.Count);
		Assert.AreEqual(3, result[0].BibleVerseId);
		Assert.AreEqual("Ephisans 5:21", result[0].VerseNumber);
	}
	[TestMethod]
	public void ReadFile_BibleVerse_Pass_Empty()
	{
		string bibleVerseContents = "\r\n";
		Mock<IFileManager> fileManager = SetUpFileManager(bibleVerseContents);
		BibleVerseData bibleVerseData = new BibleVerseData(fileManager.Object);

		var result = bibleVerseData.ReadFile();
		Assert.AreEqual(0, result.Count);
	}

	[TestMethod]
	public void ReadRankFile_BibleVerseRank_Pass()
	{
		string bibleVerseRankContents = "1|2|3\r\n";
		Mock<IFileManager> fileManager = SetUpFileManager(bibleVerseRankContents);
		BibleVerseData bibleVerseData = new BibleVerseData(fileManager.Object);

		var result = bibleVerseData.ReadRankFile();
		Assert.AreEqual(1, result.Count);
		Assert.AreEqual(1, result[0].BibleVerseRankId);
		Assert.AreEqual(2, result[0].BibleVerseId);
		Assert.AreEqual(3, result[0].RankNumber);
	}

	[TestMethod]
	public void ReadRankFile_BibleVerseRank_Empty()
	{
		string bibleVerseRankContents = "\r\n";
		Mock<IFileManager> fileManager = SetUpFileManager(bibleVerseRankContents);
		BibleVerseData bibleVerseData = new BibleVerseData(fileManager.Object);

		var result = bibleVerseData.ReadRankFile();
		Assert.AreEqual(0, result.Count);
	}

	[TestMethod]
	public void SetFileLines_Update()
	{
		string bibleVerseContents = "3|Ephisans 5:21|Submit to one another out of reverence for Christ\r\n";
		string newVerse = "1|Romans 3:10|test romans";
		string verse = "1|Romans 3:10|test romans update";
		Mock<IFileManager> fileManager = SetUpFileManager(bibleVerseContents);

		BibleVerseData bibleVerseData = new BibleVerseData(fileManager.Object);
		string[] strings = {bibleVerseContents, newVerse};
		
		var result = bibleVerseData.SetFileLines(strings, 1, verse);
		Assert.AreEqual(2, result.Count);
		Assert.AreEqual(verse, result[1]);
	}

	[TestMethod]
	public void SetFileLines_Save()
	{
		string bibleVerseContents = "3|Ephisans 5:21|Submit to one another out of reverence for Christ\r\n";
		string newVerse = "1|Romans 3:10|test romans";
		string verse = "2|Romans 3:11|test romans update";
		Mock<IFileManager> fileManager = SetUpFileManager(bibleVerseContents);

		BibleVerseData bibleVerseData = new BibleVerseData(fileManager.Object);
		string[] strings = { bibleVerseContents, newVerse };

		var result = bibleVerseData.SetFileLines(strings, 2, verse);
		Assert.AreEqual(3, result.Count);
		Assert.AreEqual(verse, result[2]);
	}
}
