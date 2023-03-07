using CensusDatabase.Models;

namespace CensusBuilder.Models
{
    public interface IResourceImporter
    {
        CensusRecord GetRecordFromText(List<string> textRows);
    }
}
