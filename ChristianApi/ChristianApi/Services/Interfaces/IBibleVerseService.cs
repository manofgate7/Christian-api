using ChristianApi.Models;

namespace ChristianApi.Services.Interfaces
{
    public interface IBibleVerseService
    {
        BibleVerse GetBibleVerseById(int bibleVerseId);
        BibleVerse GetBibleVerseByVerseNumber(string verseNumber);
    }
}
