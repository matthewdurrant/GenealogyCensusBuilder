using CensusBuilder.Models.FamilySearch;

namespace CensusBuilder.Models.Findmypast
{
    public class FindmypastImporter : IResourceImporter
    {
        public Models.CensusRecord GetRecordFromText(string[] textLines)
        {
            FindmypastCensusRecord fsRecord = GetFMPRecordFromString(textLines);
            CensusRecord censusRecord = fsRecord.ToCensusRecord();

            return censusRecord;
        }

        private FindmypastCensusRecord GetFMPRecordFromString(string[] textLines)
        {
            throw new NotImplementedException();
        }
    }
}
