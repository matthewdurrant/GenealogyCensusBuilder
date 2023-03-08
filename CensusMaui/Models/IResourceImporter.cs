using CensusData.Models;

namespace CensusMaui.Models
{
    public interface IResourceImporter
    {
        CensusRecord GetRecordFromText(List<string> textRows);
    }
}
