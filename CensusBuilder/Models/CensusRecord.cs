namespace CensusBuilder.Models
{
    public class CensusRecord
    {
        public Census Census { get; set; }
        public CitationInfo CitationInfo { get; set; }
        public string Place { get; set; }
        public string District { get; set; }
        public string ResidenceNote { get; set; }

        public List<CensusPerson> People { get; set; } = new();

    }
}
