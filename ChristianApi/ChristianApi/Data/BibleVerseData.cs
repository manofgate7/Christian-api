using ChristianApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristianApi.Data
{
	public class BibleVerseData : IBibleVerseData
	{
		private readonly string currentPath = Directory.GetCurrentDirectory();
		private readonly string fileVerseLocation = "/BibleVerseDB.txt";
		private readonly string fileRankLocation = "/BibleVerseRankDB.txt";

		public List<BibleVerse> ReadFile()
		{
			return ReadFile(currentPath + fileVerseLocation);
		}

		public List<BibleVerseRank> ReadRankFile()
		{
			return ReadRankFile(currentPath + fileRankLocation);
		}

		public void WriteAllToFile(List<BibleVerse> bibleVerses, bool append)
		{
			WriteAllToFile(currentPath + fileVerseLocation, bibleVerses, append);
		}

		public void WriteAllToFile(List<BibleVerseRank> bibleVerseRanks, bool append)
		{
			WriteAllToFile(currentPath + fileRankLocation, bibleVerseRanks, append);
		}

		public void WriteFile(BibleVerse bibleVerse, bool append)
		{
			WriteFile(currentPath + fileVerseLocation, bibleVerse, append);
		}

		public void WriteFile(BibleVerseRank bibleVerseRank, bool append)
		{
			WriteFile(currentPath + fileRankLocation, bibleVerseRank, append);
		}

		internal List<BibleVerse> ReadFile(string fileLocation)
		{
			List<BibleVerse> bibleVerses = new();
			using StreamReader streamReader = new StreamReader(fileLocation, Encoding.UTF8);
			while (!streamReader.EndOfStream)
			{
				string? line = streamReader.ReadLine();
				if (line == null)
					continue;
				var verseList = line.Split("|");
				if (verseList.Length == 3)
					bibleVerses.Add(new BibleVerse()
					{
						BibleVerseId = Int32.Parse(verseList[0]),
						VerseNumber = verseList[1],
						Verse = verseList[2]
					});
			}
			return bibleVerses;
		}

		internal List<BibleVerseRank> ReadRankFile(string fileLocation)
		{
			List<BibleVerseRank> bibleVerseRanks = new();
			using StreamReader streamReader = new StreamReader(fileLocation, Encoding.UTF8);
			while (!streamReader.EndOfStream)
			{
				string? line = streamReader.ReadLine();
				if (line == null)
					continue;
				var verseList = line.Split("|");
				if (verseList.Length == 3)
					bibleVerseRanks.Add(new BibleVerseRank()
					{
						BibleVerseRankId= Int32.Parse(verseList[0]),
						BibleVerseId = Int32.Parse(verseList[1]),
						RankNumber = Int32.Parse(verseList[2])
					});
			}
			return bibleVerseRanks;
		}

		internal void WriteAllToFile(string fileLocation, List<BibleVerse> bibleVerses, bool append)
		{
			using StreamWriter writetext = new StreamWriter(fileLocation, append);
			foreach (var verse in bibleVerses)
			{
				var bibleVerseString = verse.ToFileString();
				writetext.WriteLine(bibleVerseString);
			}
		}

		internal void WriteAllToFile(string fileLocation, List<BibleVerseRank> bibleVerseRanks, bool append)
		{
			using StreamWriter writetext = new StreamWriter(fileLocation, append);
			foreach (var rank in bibleVerseRanks)
			{
				var bibleVerseRankString = rank.ToFileString();
				writetext.WriteLine(bibleVerseRankString);
			}
		}

		internal void WriteFile(string fileLocation, BibleVerse bibleVerse, bool append)
		{
			string[] arrLine = File.ReadAllLines(fileLocation);
			var lineNumber = Array.IndexOf(arrLine, arrLine.FirstOrDefault(l => l.StartsWith(bibleVerse.BibleVerseId + "|")));
			if (lineNumber > -1)
			{
				arrLine[lineNumber] = bibleVerse.ToFileString();
				File.WriteAllLines(fileLocation, arrLine);
				
			}
			else
			{
			   List<string> lines =  arrLine.ToList();
			   lines.Add(bibleVerse.ToFileString());
			   File.WriteAllLines(fileLocation, lines);
			}
			   
		}

		internal void WriteFile(string fileLocation, BibleVerseRank bibleVerseRank, bool append)
		{
			string[] arrLine = File.ReadAllLines(fileLocation);
			var lineNumber = Array.IndexOf(arrLine, arrLine.FirstOrDefault(l => l.StartsWith(bibleVerseRank.BibleVerseRankId + "|")));
			if (lineNumber > -1)
			{
				arrLine[lineNumber] = bibleVerseRank.ToFileString();
				File.WriteAllLines(fileLocation, arrLine);

			}
			else
			{
				List<string> lines = arrLine.ToList();
				lines.Add(bibleVerseRank.ToFileString());
				File.WriteAllLines(fileLocation, lines);
			}

		}
	}
}
