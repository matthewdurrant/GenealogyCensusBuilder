using CensusDatabase.Models;
using LiteDB;
using System.IO;

namespace CensusDatabase
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

        public Census? GetCensus(string name)
        {
            // Open database (or create if doesn't exist)
            using var db = new LiteDatabase(dbPath);

            var col = db.GetCollection<Census>("censuses");
            col.EnsureIndex(x => x.Name);

            Census? census = col.Query().Where(x => x.Name == name).SingleOrDefault();

            return census;
        }
    }
}