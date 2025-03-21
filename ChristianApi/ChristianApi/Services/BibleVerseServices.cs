﻿using ChristianApi.Data;
using ChristianApi.Models;
using ChristianApi.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using System.IO;
using System.Text;

namespace ChristianApi.Services
{
    public class BibleVerseServices : IBibleVerseService
    {
        private List<BibleVerse> bibleVerseList = new List<BibleVerse>();

        private readonly IBibleVerseData _bibleVerseData;

       

        public BibleVerseServices(IBibleVerseData bibleVerseData)
        {
            _bibleVerseData = bibleVerseData;
            bibleVerseList = _bibleVerseData.ReadFile();
        }
        
        public BibleVerse GetBibleVerseById(int bibleVerseId)
        {
            return bibleVerseList.FirstOrDefault(b => b.BibleVerseId == bibleVerseId) ?? new BibleVerse();
        }
        public BibleVerse GetBibleVerseByVerseNumber(string verseNumber)
        {
            return bibleVerseList.FirstOrDefault(b=> b.VerseNumber == verseNumber) ?? new BibleVerse();
        }

        public BibleVerse GetBibleVerse(int? bibleVerseId, string? verseNumber)
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

       
    }
}
