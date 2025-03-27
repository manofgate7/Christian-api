using ChristianApi.Data.Interfaces;
using ChristianApi.Models;
using ChristianApi.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("TestChristianApi")]
namespace ChristianApi.Services
{
    
    public class BibleVerseServices : IBibleVerseService
    {
        private List<BibleVerse> bibleVerseList = new List<BibleVerse>();
        private List<BibleVerseRank> bibleVerseRanks = new List<BibleVerseRank>();

        private readonly IBibleVerseData _bibleVerseData;

       

        public BibleVerseServices(IBibleVerseData bibleVerseData)
        {
            _bibleVerseData = bibleVerseData;
            bibleVerseList = _bibleVerseData.ReadFile();
            bibleVerseRanks = _bibleVerseData.ReadRankFile();
        }
        
        public BibleVerse GetBibleVerseById(int bibleVerseId)
        {
            return bibleVerseList.FirstOrDefault(b => b.BibleVerseId == bibleVerseId) ?? new BibleVerse();
        }
        public BibleVerse GetBibleVerseByVerseNumber(string verseNumber)
        {
            return bibleVerseList.FirstOrDefault(b=> b.VerseNumber == verseNumber) ?? new BibleVerse();
        }

        public BibleVerse GetBibleVerse(int? bibleVerseId, string verseNumber)
        {
            BibleVerse bibleVerse = new();
            if (bibleVerseId.HasValue)
            {
                bibleVerse = GetBibleVerseById(bibleVerseId.Value);
            }
            if (bibleVerse.BibleVerseId == 0 && !string.IsNullOrEmpty(verseNumber))
            {
                bibleVerse = GetBibleVerseByVerseNumber(verseNumber);
            }
            return bibleVerse;
        }

        public void SaveBibleVerse(BibleVerse bibleVerse)
        {
            SetBibleVerseId(bibleVerse);
           
            //do an update
            if(bibleVerse.BibleVerseId > 0)
            {
                bibleVerseList.RemoveAll(bl => bl.BibleVerseId == bibleVerse.BibleVerseId);
            }
            else
            {
                var max = bibleVerseList.Max(bl => bl.BibleVerseId);
                bibleVerse.BibleVerseId = max + 1;
            }
            //insert new one
            bibleVerseList.Add(bibleVerse);
            _bibleVerseData.WriteFile(bibleVerse, true);
        }

        internal void SetBibleVerseId(BibleVerse bibleVerse)
        {
            var verses = bibleVerseList.Where(bl => bl.VerseNumber == bibleVerse.VerseNumber || bl.Verse == bibleVerse.Verse);
            if(verses.Count() > 1)
            {
                throw new Exception("Duplicate verses or versenumbers");
            }
            if(verses.Any())
            {
                bibleVerse.BibleVerseId = verses.First().BibleVerseId;
            }
            
        }

		public BibleVerseRank GetBibleVerseRankById(int bibleVerseRankId)
		{
			return bibleVerseRanks.FirstOrDefault(b => b.BibleVerseRankId == bibleVerseRankId) ?? new BibleVerseRank();
		}

		public List<BibleVerseRank> GetBibleVerseRanksByVerseId(int BibleVerseId)
		{
            return bibleVerseRanks.Where(b => b.BibleVerseId == BibleVerseId).ToList();
		}

		public double GetAverageBibleVerseRankForVerse(int bibleVerseId)
		{
            double result = 0;
            var ranks = bibleVerseRanks
                    .Where(b => b.BibleVerseId == bibleVerseId).ToList();
            if(ranks.Count > 0)
                result =  ranks.Average(b => b.RankNumber);
            return result;
		}

        internal bool ValidateBibleVerseRank(BibleVerseRank bibleVerseRank)
        {
            if(!bibleVerseRank.ValidateBibleVerseId(bibleVerseList.Select(bl => bl.BibleVerseId).ToList()))
            {
                throw new ArgumentException("bad bible verse");
            }
            if (!bibleVerseRank.ValidateRankNumber())
            {
                throw new ArgumentException("Rank number is not between 1 and 5");
            }

            return true;
        }

		public void SaveBibleVerseRank(BibleVerseRank bibleVerseRank)
		{
            ValidateBibleVerseRank(bibleVerseRank);

			//do an update
			if (bibleVerseRank.BibleVerseRankId > 0)
			{
				bibleVerseRanks.RemoveAll(bl => bl.BibleVerseRankId == bibleVerseRank.BibleVerseRankId);
			}
			else
			{
				var max = bibleVerseRanks.Count > 0 ? bibleVerseRanks.Max(bl => bl.BibleVerseRankId) : 0;
				bibleVerseRank.BibleVerseRankId = max + 1;
			}
			//insert new one
			bibleVerseRanks.Add(bibleVerseRank);
			_bibleVerseData.WriteFile(bibleVerseRank, true);
		}

		public List<Tuple<BibleVerse, double>> GetBibleVersesWithAverageRank()
		{
			return bibleVerseList.Select(bl => new Tuple<BibleVerse, double>(bl, GetAverageBibleVerseRankForVerse(bl.BibleVerseId))).ToList();
		}
	}
}
