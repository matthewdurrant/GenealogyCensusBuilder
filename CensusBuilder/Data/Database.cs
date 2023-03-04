using CensusBuilder.Models;

namespace CensusBuilder.Data
{
    public class Database
    {
        public Database(string dbPath)
        {
            AddFromText(Census1841.Data);
            AddFromText(Census1851.Data);

        }

        public List<Census> Censuses { get; set; } = new();

        public List<Census> AddFromText(string text)
        {
            string[] textLines = text.Split("\r\n");
            Models.FamilySearch.CensusRecord fsRecord = Models.FamilySearch.CensusRecord.FromClipboardString(textLines);
            CensusRecord censusRecord = fsRecord.ToCensusRecord();

            Census? existingCensus = Censuses.SingleOrDefault(c => c.Name == censusRecord.Census.Name && c.Date == censusRecord.Census.Date);

            if (existingCensus == null)
            {
                Censuses.Add(censusRecord.Census);
                existingCensus = Censuses.Single(c => c.Name == censusRecord.Census.Name && c.Date == censusRecord.Census.Date);
            }

            existingCensus.Records.Add(censusRecord);

            return Censuses;
        }
    }
}
