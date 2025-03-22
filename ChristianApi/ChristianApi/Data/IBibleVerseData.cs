using ChristianApi.Models;

namespace ChristianApi.Data
{
    public interface IBibleVerseData
    {

        List<BibleVerse> ReadFile();
        void WriteFile(BibleVerse bibleVerse, bool append);
        void WriteAllToFile(List<BibleVerse> bibleVerses, bool append);
    }
}
