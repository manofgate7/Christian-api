using ChristianApi.Models;

namespace ChristianApi.Services.Interfaces
{
    public interface IBibleVerseService
    {
        BibleVerse GetBibleVerseById(int bibleVerseId);
        BibleVerse GetBibleVerseByVerseNumber(string verseNumber);
        BibleVerse GetBibleVerse(int? bibleVerseId, string? verseNumber);
        void SaveBibleVerse(BibleVerse bibleVerse);
    }
}
