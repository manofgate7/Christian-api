using ChristianApi.Data;
using ChristianApi.Models;
using ChristianApi.Services;
using Moq;
using System.Reflection.Metadata;

namespace TestChristianApi;

[TestClass]
public class BibleVerseServicesTest
{
	internal BibleVerseServices SetUpService(List<BibleVerse> bibleVerses, List<BibleVerseRank> bibleVerseRanks)
	{
		Mock<IBibleVerseData> mBibleVerseData = new Mock<IBibleVerseData>();
		mBibleVerseData.Setup(s => s.ReadFile()).Returns(bibleVerses);
		mBibleVerseData.Setup(s => s.ReadRankFile()).Returns(bibleVerseRanks);
		BibleVerseServices bibleVerseServices = new BibleVerseServices(mBibleVerseData.Object);
		return bibleVerseServices;
	}

	[TestMethod]
    public void Test_SetBibleVerseId()
    {
        var bibleVersese = new List<BibleVerse>()
        {
            new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
            new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
        };
        var bibleVerseRanks = new List<BibleVerseRank>();

		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		BibleVerse bibleVerse = new BibleVerse()
		{
			BibleVerseId = 0,
			Verse = "romans",
			VerseNumber = "Romans 3:10-12"
		};

		bibleVerseServices.SetBibleVerseId(bibleVerse);
		Assert.AreEqual(1, bibleVerse.BibleVerseId);

		bibleVerse.VerseNumber = "romans";
		bibleVerse.BibleVerseId = 0;
		bibleVerseServices.SetBibleVerseId(bibleVerse);
		Assert.AreEqual(0, bibleVerse.BibleVerseId);


		bibleVerse.VerseNumber = "John 3:16";
		bibleVerse.BibleVerseId = 0;
		bibleVerseServices.SetBibleVerseId(bibleVerse);
		Assert.AreEqual(2, bibleVerse.BibleVerseId);
	}

	[TestMethod]
	public void Test_SetBibleVerseId_Duplicate()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>();

		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		BibleVerse bibleVerse = new BibleVerse()
		{
			BibleVerseId = 0,
			Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life",
			VerseNumber = "Romans 3:10-12"
		};
		Assert.ThrowsException<Exception>(() => bibleVerseServices.SetBibleVerseId(bibleVerse), "Duplicate verses or versenumbers");
		
	}

	[TestMethod]
	public void ValidateBibleVerseRank_Pass()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		BibleVerseRank bibleVerseRank = new BibleVerseRank() { BibleVerseRankId = 3, BibleVerseId = 1, RankNumber = 5 };

		var result = bibleVerseServices.ValidateBibleVerseRank(bibleVerseRank);
		Assert.IsTrue(result);

		bibleVerseRank.RankNumber = 3;
		bibleVerseRank.BibleVerseId = 2;

		result = bibleVerseServices.ValidateBibleVerseRank(bibleVerseRank);
		Assert.IsTrue(result);

	}

	[TestMethod]
	public void ValidateBibleVerseRank_Fail_ValidateBibleVerseId()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		BibleVerseRank bibleVerseRank = new BibleVerseRank() { BibleVerseRankId = 3, BibleVerseId = 3, RankNumber = 5 };

		Assert.ThrowsException<ArgumentException>(() => bibleVerseServices.ValidateBibleVerseRank(bibleVerseRank), "bad bible verse");

	}

	[TestMethod]
	public void ValidateBibleVerseRank_Fail_ValidateRankNumber()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		BibleVerseRank bibleVerseRank = new BibleVerseRank() { BibleVerseRankId = 3, BibleVerseId = 1, RankNumber = 6 };

		Assert.ThrowsException<ArgumentException>(() => bibleVerseServices.ValidateBibleVerseRank(bibleVerseRank), "Rank number is not between 1 and 5");

	}

	[TestMethod]
	public void GetBibleVerseByVerseNumber_Pass()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		BibleVerse bibleVerse = new BibleVerse()
		{
			BibleVerseId = 0,
			Verse = "romans",
			VerseNumber = "Romans 3:10-12"
		};

		var result = bibleVerseServices.GetBibleVerseByVerseNumber(bibleVerse.VerseNumber);
		Assert.AreEqual(1, result.BibleVerseId);
	}

	[TestMethod]
	public void GetBibleVerseByVerseNumber_Fail()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		BibleVerse bibleVerse = new BibleVerse()
		{
			BibleVerseId = 0,
			Verse = "romans",
			VerseNumber = "Romans 3:10"
		};

		var result = bibleVerseServices.GetBibleVerseByVerseNumber(bibleVerse.VerseNumber);
		Assert.AreEqual(0, result.BibleVerseId);
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
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		var result = bibleVerseServices.GetBibleVerseRankById(1);
		Assert.AreEqual(1, result.BibleVerseId);
	}

	[TestMethod]
	public void GetBibleVerseRankById_Fail()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		var result = bibleVerseServices.GetBibleVerseRankById(3);
		Assert.AreEqual(0, result.BibleVerseId);
	}

	[TestMethod]
	public void GetBibleVerseRanksByVerseId_Pass()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 3, RankNumber = 3},
			new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 4, RankNumber = 4},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		var result = bibleVerseServices.GetBibleVerseRanksByVerseId(1);
		Assert.AreEqual(3, result.Count);

		result = bibleVerseServices.GetBibleVerseRanksByVerseId(2);
		Assert.AreEqual(1, result.Count);
	}

	[TestMethod]
	public void GetBibleVerseRanksByVerseId_Fail()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 3, RankNumber = 3},
			new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 4, RankNumber = 4},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		var result = bibleVerseServices.GetBibleVerseRanksByVerseId(3);
		Assert.AreEqual(0, result.Count);

		result = bibleVerseServices.GetBibleVerseRanksByVerseId(0);
		Assert.AreEqual(0, result.Count);
	}

	[TestMethod]
	public void GetAverageBibleVerseRankForVerse_Pass()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 3, RankNumber = 3},
			new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 4, RankNumber = 4},
			new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 5, RankNumber = 3},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		var result = bibleVerseServices.GetAverageBibleVerseRankForVerse(1);
		Assert.AreEqual(2.75, result);

		result = bibleVerseServices.GetAverageBibleVerseRankForVerse(2);
		Assert.AreEqual(2.00, result);
	}

	[TestMethod]
	public void GetAverageBibleVerseRankForVerse_Fail()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 3, RankNumber = 3},
			new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 4, RankNumber = 4},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		var result = bibleVerseServices.GetAverageBibleVerseRankForVerse(3);
		Assert.AreEqual(0, result);
	}

	[TestMethod]
	public void SaveBibleVerse_Update()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>();

		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		BibleVerse bibleVerse = new BibleVerse()
		{
			BibleVerseId = 1,
			Verse = "romans",
			VerseNumber = "Romans 3:10-12"
		};
		bibleVerseServices.SaveBibleVerse(bibleVerse);

		var result = bibleVerseServices.GetBibleVerseById(1);
		Assert.AreEqual(bibleVerse.Verse, result.Verse);
	}

	[TestMethod]
	public void SaveBibleVerse_Save()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>();

		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		BibleVerse bibleVerse = new BibleVerse()
		{
			BibleVerseId = 0,
			Verse = "romans",
			VerseNumber = "Romans 3:11"
		};
		bibleVerseServices.SaveBibleVerse(bibleVerse);

		var result = bibleVerseServices.GetBibleVerseById(3);
		Assert.AreEqual(3, result.BibleVerseId);
	}

	[TestMethod]
	public void SaveBibleVerseRank_Update()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 3, RankNumber = 3},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		var verseRank = new BibleVerseRank() { BibleVerseId = 1, BibleVerseRankId = 3, RankNumber = 4 };

		bibleVerseServices.SaveBibleVerseRank(verseRank);

		var result = bibleVerseServices.GetBibleVerseRankById(3);
		Assert.AreEqual(4, result.RankNumber);
	}

	[TestMethod]
	public void SaveBibleVerseRank_Save()
	{
		var bibleVersese = new List<BibleVerse>()
		{
			new BibleVerse() {BibleVerseId = 1, VerseNumber = "Romans 3:10-12" , Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one." },
			new BibleVerse() { BibleVerseId = 2, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" }
		};
		var bibleVerseRanks = new List<BibleVerseRank>() { new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 1, RankNumber = 1 },
			new BibleVerseRank() {BibleVerseRankId= 2, BibleVerseId= 2, RankNumber= 2},
			new BibleVerseRank() {BibleVerseId = 1, BibleVerseRankId = 3, RankNumber = 3},
		};
		BibleVerseServices bibleVerseServices = SetUpService(bibleVersese, bibleVerseRanks);

		var verseRank = new BibleVerseRank() { BibleVerseId = 1, BibleVerseRankId = 0, RankNumber = 4 };

		bibleVerseServices.SaveBibleVerseRank(verseRank);

		var result = bibleVerseServices.GetBibleVerseRankById(4);
		Assert.AreEqual(4, result.RankNumber);
	}
}
