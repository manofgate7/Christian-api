using ChristianApi.Controllers;
using ChristianApi.Models;
using ChristianApi.Services;
using ChristianApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection.Metadata.Ecma335;

namespace TestChristianApi;

[TestClass]
public class BibleControllerTest
{
    internal BibleController SetUpBibleController(List<BibleVerse> bibleVerses, List<BibleVerseRank> bibleVerseRanks)
    {
        Mock<IBibleVerseService> mBibleVerseServices = new Mock<IBibleVerseService>();
        mBibleVerseServices.Setup(s => s.GetBibleVerse(It.IsAny<int?>(), It.IsAny<string>()))
            .Returns((int? id, string vN) =>
            {
                if (id.HasValue)
                {
                    return bibleVerses[id.Value];
                }
                else
                {
                    return bibleVerses.Where(b => b.VerseNumber == vN).FirstOrDefault()?? new BibleVerse();
                }
            });
        mBibleVerseServices.Setup(s => s.GetBibleVerseRankById(It.IsAny<int>()))
            .Returns((int id) => bibleVerseRanks.Where(b => b.BibleVerseRankId == id).FirstOrDefault() ?? new BibleVerseRank());

		mBibleVerseServices.Setup(s => s.GetBibleVerseRanksByVerseId(It.IsAny<int>()))
			.Returns((int id) => bibleVerseRanks.Where(b => b.BibleVerseId == id).ToList());
		mBibleVerseServices.Setup(s => s.GetAverageBibleVerseRankForVerse(It.IsAny<int>()))
			.Returns((int id) => bibleVerseRanks.Where(b => b.BibleVerseId == id).FirstOrDefault()?.RankNumber ?? 0);
		mBibleVerseServices.Setup(s => s.GetBibleVersesWithAverageRank())
			.Returns(() => bibleVerses.Select(bl => new Tuple<BibleVerse, double>(bl, 1)).ToList());

		mBibleVerseServices.Setup(s => s.SaveBibleVerseRank(It.IsAny<BibleVerseRank>()));
			
        Mock<ILogger<BibleController>> mLogger = new Mock<ILogger<BibleController>>();
        return new BibleController(mLogger.Object, mBibleVerseServices.Object);

	}
    [TestMethod]
    public void GetBibleVerse_Id ()
    {
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>();

        BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);

        Assert.IsNotNull(bibleController);
        var result = bibleController.GetBibleVerse(0, null);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Value?.BibleVerseId);
	}

	[TestMethod]
	public void GetBibleVerse_VerseNumber_NotExists()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>();

		BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);

		Assert.IsNotNull(bibleController);
		var result = bibleController.GetBibleVerse(null, "Romans 3:10-11");
		Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
        Assert.AreEqual(404, ((StatusCodeResult)result.Result).StatusCode);
	}

    [TestMethod]
    public void GetBibleVerseRankById_Pass()
    {
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
		};

		BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);

		var result = bibleController.GetBibleVerseRankById(1);
		Assert.IsNotNull(result);
		Assert.AreEqual(1, result.Value?.BibleVerseRankId);
		Assert.AreEqual(1, result.Value?.RankNumber);
	}

	[TestMethod]
	public void GetBibleVerseRankById_Empty()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
		};

		BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);

		var result = bibleController.GetBibleVerseRankById(3);
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
		Assert.AreEqual(404, ((StatusCodeResult)result.Result).StatusCode);
	}

	[TestMethod]
	public void GetRanksByBibleVerse_Pass()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseRankId= 3, BibleVerseId= 1, RankNumber= 2},
		};

		BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);

		var result = bibleController.GetRanksByBibleVerse(1);
		Assert.IsNotNull(result.Value);
		Assert.AreEqual(2, result.Value.Count);
		Assert.AreEqual(2, result.Value.LastOrDefault()?.RankNumber);
	}
	[TestMethod]
	public void GetRanksByBibleVerse_Empty()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseRankId= 3, BibleVerseId= 1, RankNumber= 2},
		};

		BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);

		var result = bibleController.GetRanksByBibleVerse(3);
		Assert.IsNull(result.Value);
		Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
		Assert.AreEqual(404, ((StatusCodeResult)result.Result).StatusCode);
	}

	[TestMethod]
	public void GetAverageRankByBibleVerse_Pass()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseRankId= 3, BibleVerseId= 1, RankNumber= 2},
		};

		BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);

		var result = bibleController.GetAverageRankByBibleVerse(1);
		Assert.IsNotNull(result.Value);
	}

	[TestMethod]
	public void GetAverageRankByBibleVerse_Empty()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseRankId= 3, BibleVerseId= 1, RankNumber= 2},
		};

		BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);

		var result = bibleController.GetAverageRankByBibleVerse(3);
		Assert.IsNull(result.Value);
		Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
		Assert.AreEqual(404, ((StatusCodeResult)result.Result).StatusCode);
	}

	[TestMethod]
	public void GetBibleVersesWithAverage_Pass()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseRankId= 3, BibleVerseId= 1, RankNumber= 2},
		};

		BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);

		var result = bibleController.GetBibleVersesWithAverage();
		Assert.IsNotNull(result.Value);
		Assert.AreEqual(2, result.Value.Count);
	}

	[TestMethod]
	public void GetBibleVersesWithAverage_Empty()
	{
		var bibleVersese = new List<BibleVerse>();

		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseRankId= 3, BibleVerseId= 1, RankNumber= 2},
		};

		BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);

		var result = bibleController.GetBibleVersesWithAverage();
		Assert.IsNull(result.Value);
		Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
		Assert.AreEqual(404, ((StatusCodeResult)result.Result).StatusCode);
	}

	[TestMethod]
	public void SaveBibleVerseRank_Pass()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			
		};

		BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);
		var result = bibleController.SaveBibleVerseRank(null, 1, 5);
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
		Assert.AreEqual(200, ((StatusCodeResult)result).StatusCode);

	}

	
	[TestMethod]
	public void SaveBibleVerse_Pass()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},

		};

		BibleController bibleController = SetUpBibleController(bibleVersese, bibleVerseRanks);
		var result = bibleController.SaveBibleVerse("romans", "romans test");
		Assert.IsNotNull(result);
		Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
		Assert.AreEqual(200, ((StatusCodeResult)result).StatusCode);

	}
}
