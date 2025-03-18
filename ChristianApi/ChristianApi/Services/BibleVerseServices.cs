using ChristianApi.Models;
using ChristianApi.Services.Interfaces;

namespace ChristianApi.Services
{
    public class BibleVerseServices : IBibleVerseService
    {
        private List<BibleVerse> bibleVerseList = new List<BibleVerse>();

        private void SetBibleVerses()
        {
            bibleVerseList.Add(new BibleVerse() { BibleVerseId = 1, VerseNumber = "John 3:16", Verse = "For God so loved the world that he gave his only begotten Son, that whosoever believeth in him should not perish, but have eternal life" });
            bibleVerseList.Add(new BibleVerse() { BibleVerseId = 2, VerseNumber = "Romans 3:10-12", Verse = "As it is written: 'There is no one righteous, not even one; there is no one who understands; there is no one who seeks God. All have turned away, they have together become worthless; there is no one who does good, not even one.'" });
            bibleVerseList.Add(new BibleVerse() { BibleVerseId = 3, VerseNumber = "Ephisans 5:21", Verse = "Submit to one another out of reverence for Christ" });
        }
        public BibleVerseServices()
        {
            SetBibleVerses();
        }
        
        public BibleVerse GetBibleVerseById(int bibleVerseId)
        {
            return bibleVerseList.FirstOrDefault(b => b.BibleVerseId == bibleVerseId) ?? new BibleVerse();
        }
        public BibleVerse GetBibleVerseByVerseNumber(string verseNumber)
        {
            return bibleVerseList.FirstOrDefault(b=> b.VerseNumber == verseNumber) ?? new BibleVerse();
        }
    }
}
