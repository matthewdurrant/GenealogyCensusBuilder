namespace CensusBuilder.Models
{
    public interface IResourceImporter
    {
        CensusRecord GetRecordFromText(string[] textRows);
    }
}
