namespace ChristianApi.Models
{
    public class BibleVerseRank
    {
        public int BibleVerseRankId { get; set; }
        public int BibleVerseId { get; set; }
        public int RankNumber { get; set; }
        

        public string ToFileString()
        {
            return BibleVerseRankId + "|" + BibleVerseId + "|" + RankNumber;
        }

        public bool ValidateRankNumber()
        {
            return RankNumber > 0 && RankNumber < 6;
        }

        public bool ValidateBibleVerseId(List<int> verses)
        {
            return verses.Any(verse => verse == BibleVerseId);
        }
    }
}
