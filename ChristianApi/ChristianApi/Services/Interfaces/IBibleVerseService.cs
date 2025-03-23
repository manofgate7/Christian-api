using ChristianApi.Models;

namespace ChristianApi.Services.Interfaces
{
    public interface IBibleVerseService
    {
        BibleVerse GetBibleVerseById(int bibleVerseId);
        BibleVerse GetBibleVerseByVerseNumber(string verseNumber);
        BibleVerse GetBibleVerse(int? bibleVerseId, string? verseNumber);
        void SaveBibleVerse(BibleVerse bibleVerse);

        BibleVerseRank GetBibleVerseRankById(int bibleVerseRankId);
        List<BibleVerseRank> GetBibleVerseRanksByVerseId(int BibleVerseId);
        List<Tuple<BibleVerse, double>> GetBibleVersesWithAverageRank();
		double GetAverageBibleVerseRankForVerse(int bibleVerseId);
        void SaveBibleVerseRank(BibleVerseRank bibleVerseRank);

    }
}
