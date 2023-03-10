namespace CensusData.Models
{
    public class CitationInfo
    {
        public int PageNumber { get; set; }
        public int Piece { get; set; }
        public int Folio { get; set; }
        public string RegistrationNumber { get; set; }
        public int HouseholdIdentifier { get; set; }
        public int BookNumber { get; set; }
        public int DigitalFolderNumber { get; set; }
        public int ImageNumber { get; set; }
        public string Citation { get; set; }
        public int Schedule { get; set; }
    }
}