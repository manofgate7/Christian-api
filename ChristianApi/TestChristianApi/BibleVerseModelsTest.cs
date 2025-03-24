using ChristianApi.Models;

namespace TestChristianApi
{
	[TestClass]
	public sealed class BibleVerseModelsTest
	{
		[TestMethod]
		public void Test_BibleVerse_ToFileString()
		{
			string verseNumber = "Romans 8:12";
			string verse = "here we go a test";
			
			BibleVerse BibleVerse = new BibleVerse()
			{
				BibleVerseId = 1,
				VerseNumber = verseNumber,
				Verse = verse
			};
			var result = BibleVerse.ToFileString();
			var expected = "1|"+ verseNumber+"|" + verse;
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void BibleVerseRank_ToFileString()
		{
			string expected = "1|1|4";
			BibleVerseRank bibleVerseRank = new BibleVerseRank()
			{
				BibleVerseRankId = 1,
				BibleVerseId = 1,
				RankNumber = 4,
			};
			var result = bibleVerseRank.ToFileString();
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void BibleVerseRank_Validate_RankNumber()
		{
			BibleVerseRank bibleVerseRank = new BibleVerseRank()
			{
				BibleVerseRankId = 1,
				BibleVerseId = 1,
				RankNumber = 1,
			};

			var result = bibleVerseRank.ValidateRankNumber();
			Assert.IsTrue(result);

			bibleVerseRank.RankNumber = 10;
			result = bibleVerseRank.ValidateRankNumber(); 
			Assert.IsFalse(result);

			bibleVerseRank.RankNumber = 0;
			result = bibleVerseRank.ValidateRankNumber();
			Assert.IsFalse(result);

			bibleVerseRank.RankNumber = -2;
			result = bibleVerseRank.ValidateRankNumber();
			Assert.IsFalse(result);

			bibleVerseRank.RankNumber = 5;
			result = bibleVerseRank.ValidateRankNumber();
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void BibleVerseRank_Validate_BibleVerseId()
		{
			BibleVerseRank bibleVerseRank = new BibleVerseRank()
			{
				BibleVerseRankId = 1,
				BibleVerseId = 1,
				RankNumber = 1,
			};
			List<int> verses = new List<int>() { 1, 2, 3 };

			var result = bibleVerseRank.ValidateBibleVerseId(verses);
			Assert.IsTrue(result);

			bibleVerseRank.BibleVerseId = 3;
			result = bibleVerseRank.ValidateBibleVerseId(verses);
			Assert.IsTrue(result);

			bibleVerseRank.BibleVerseId = 10;
			result = bibleVerseRank.ValidateBibleVerseId(verses);
			Assert.IsFalse(result);

			bibleVerseRank.BibleVerseId = -1;
			result = bibleVerseRank.ValidateBibleVerseId(verses);
			Assert.IsFalse(result);
		}
	}
}
