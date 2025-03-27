using ChristianApi.Models;

namespace ChristianApi.Data.Interfaces
{
    public interface IBibleVerseData
    {

        List<BibleVerse> ReadFile();
        void WriteFile(BibleVerse bibleVerse, bool append);

        List<BibleVerseRank> ReadRankFile();
        void WriteFile(BibleVerseRank bibleVerseRank, bool append);
    }
}
