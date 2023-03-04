using CensusBuilder.Models;

namespace CensusBuilder.Data
{
    public class Database
    {
        public Database(string dbPath)
        {
            AddFromText(Census1841.Data);
            AddFromText(Census1851.Data);



            string[] text1851 = Census1851.Data.Split("\r\n");
            Models.FamilySearch.CensusRecord fsRecord1851 = Models.FamilySearch.CensusRecord.FromClipboardString(text1851);
            CensusRecord censusRecord1851 = fsRecord1851.ToCensusRecord();

            Census census1841 = Census.EnglandAndWales1841;
            census1841.Records = new() { censusRecord1841 };
            Census census1851 = Census.EnglandAndWales1851;
            census1851.Records = new() { censusRecord1851 };

            Censuses = new()
            {
                census1841,
                census1851
            };
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
