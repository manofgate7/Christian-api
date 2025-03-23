using ChristianApi.Models;

namespace ChristianApi.Data
{
    public interface IBibleVerseData
    {

        List<BibleVerse> ReadFile();
        void WriteFile(BibleVerse bibleVerse, bool append);
        void WriteAllToFile(List<BibleVerse> bibleVerses, bool append);

        List<BibleVerseRank> ReadRankFile();
        void WriteFile(BibleVerseRank bibleVerseRank, bool append);
        void WriteAllToFile(List<BibleVerseRank> bibleVerseRanks, bool append);
    }
}
