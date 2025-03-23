using ChristianApi.Models;

namespace TestChristianApi
{
	[TestClass]
	public sealed class BibleVerseTest
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
	}
}
