using ChristianApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristianApi.Data
{
    public class BibleVerseData : IBibleVerseData
    {
        private readonly string currentPath = Directory.GetCurrentDirectory();
        private readonly string fileLocation = "/BibleVerseDB.txt";

        public List<BibleVerse> ReadFile()
        {
            return ReadFile(currentPath + fileLocation);
        }
        public void WriteAllToFile(List<BibleVerse> bibleVerses, bool append)
        {
            WriteAllToFile(currentPath + fileLocation, bibleVerses, append);
        }
        public void WriteFile(BibleVerse bibleVerse, bool append)
        {
            WriteFile(currentPath + fileLocation, bibleVerse, append);
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
                        BibleVerseId = Int32.Parse(verseList[0])
                        ,
                        VerseNumber = verseList[1]
                        ,
                        Verse = verseList[2]
                    });
            }
            return bibleVerses;
        }

        internal void WriteAllToFile(string fileLocation, List<BibleVerse> bibleVerses, bool append)
        {
            using (StreamWriter writetext = new StreamWriter(fileLocation, append))
            {
                foreach (var verse in bibleVerses)
                {
                    var bibleVerseString = verse.ToFileString();
                    writetext.WriteLine(bibleVerseString);
                }
               
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
    }
}
