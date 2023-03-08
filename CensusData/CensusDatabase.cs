using CensusData.Models;
using LiteDB;
using System.IO;
using System.Xml.Linq;

namespace CensusData
{
    public class CensusDatabase
    {
        private string dbPath { get; set; }

        public CensusDatabase(string path)
        {
            dbPath = path;
        }

        public void SaveCensus(Census census)
        {
            // Open database (or create if doesn't exist)
            using var db = new LiteDatabase(dbPath);

            var col = db.GetCollection<Census>("censuses");
            col.EnsureIndex(x => x.Name);

            col.Insert(census);
        }

        public void SaveCensusRecord(CensusRecord census)
        {
            // Open database (or create if doesn't exist)
            using var db = new LiteDatabase(dbPath);

            var col = db.GetCollection<CensusRecord>("records");
            
            col.Insert(census);
        }

        public Census? GetCensus(string name)
        {
            // Open database (or create if doesn't exist)
            using var db = new LiteDatabase(dbPath);

            var col = db.GetCollection<Census>("censuses");
            col.EnsureIndex(x => x.Name);

            Census? census = col.Query().Where(x => x.Name == name).SingleOrDefault();

            return census;
        }

        public List<Census> GetCensuses()
        {
            // Open database (or create if doesn't exist)
            using var db = new LiteDatabase(dbPath);

            var col = db.GetCollection<Census>("censuses");
            col.EnsureIndex(x => x.Name);

            List<Census> census = col.FindAll().ToList();

            return census;
        }
    }
}