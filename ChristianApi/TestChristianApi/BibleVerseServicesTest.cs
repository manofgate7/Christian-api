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
}
