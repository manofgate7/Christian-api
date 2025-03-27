using ChristianApi.Data.Interfaces;
using ChristianApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristianApi.Data
{
	public class BibleVerseData(IFileManager fileManager) : IBibleVerseData
	{
		private readonly string currentPath = Directory.GetCurrentDirectory();
		private readonly string fileVerseLocation = "/BibleVerseDB.txt";
		private readonly string fileRankLocation = "/BibleVerseRankDB.txt";

		private readonly IFileManager _FileManager = fileManager;	

		public List<BibleVerse> ReadFile()
		{
			return ReadFile(currentPath + fileVerseLocation);
		}

		public List<BibleVerseRank> ReadRankFile()
		{
			return ReadRankFile(currentPath + fileRankLocation);
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
			using StreamReader streamReader = _FileManager.StreamReader(fileLocation, Encoding.UTF8);
			while (!streamReader.EndOfStream)
			{
				string? line = streamReader.ReadLine();
				if (string.IsNullOrEmpty(line))
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
			using StreamReader streamReader = _FileManager.StreamReader(fileLocation, Encoding.UTF8);
			while (!streamReader.EndOfStream)
			{
				string? line = streamReader.ReadLine();
				if (string.IsNullOrEmpty(line))
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

		internal void WriteFile(string fileLocation, BibleVerse bibleVerse, bool append)
		{
			string[] arrLine = File.ReadAllLines(fileLocation);
			List<string> lines = SetFileLines(arrLine, bibleVerse.BibleVerseId, bibleVerse.ToFileString());
			File.WriteAllLines(fileLocation, lines);
		}

		internal List<string> SetFileLines(string[] arrLine, int id, string newLine)
		{
			List<string> lines = arrLine.ToList();
			var lineNumber = Array.IndexOf(arrLine, arrLine.FirstOrDefault(l => l.StartsWith(id + "|")));
			if (lineNumber > -1)
			{
				arrLine[lineNumber] = newLine;
				lines = arrLine.ToList();
			}
			else
			{
				lines.Add(newLine);
			}
			return lines;
		}

		internal void WriteFile(string fileLocation, BibleVerseRank bibleVerseRank, bool append)
		{
			string[] arrLine = File.ReadAllLines(fileLocation);
			List<string> lines = SetFileLines(arrLine, bibleVerseRank.BibleVerseRankId, bibleVerseRank.ToFileString());
			File.WriteAllLines(fileLocation, lines);
		}
	}
}
