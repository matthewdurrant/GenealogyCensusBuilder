using CensusDatabase.Models;

namespace CensusBuilder.Models.FamilySearch
{
    internal class FamilySearchCensusRecord : ICensusRecord
    {
        public string Name { get; set; }
        public string Sex { get; set; }

        public int Age { get; set; }
        public int EventDate { get; set; }
        public string EventPlace { get; set; }
        public string RegistrationDistrict { get; set; }
        public string EventType { get; set; }
        public string ResidenceNote { get; set; }
        public string Birthplace { get; set; }
        public string RelationshipToHeadOfHousehold { get; set; }
        public string Occupation { get; set; }
        public int PageNumber { get; set; }
        public string PieceFolio { get; set; }
        public string RegistrationNumber { get; set; }
        public int HouseholdIdentifier { get; set; }
        public int BookNumber { get; set; }
        public int LineNumber { get; set; }
        public int DigitalFolderNumber { get; set; }
        public int ImageNumber { get; set; }
        public string Citation { get; set; }
        public List<FamilySearchCensusPerson> People { get; set; } = new();

        public CensusRecord ToCensusRecord()
        {
            Census? census = this.EventDate switch
            {
                1841 => Census.EnglandAndWales1841,
                1851 => Census.EnglandAndWales1851,
                _ => null
            };

            CitationInfo citation = new()
            {
                DigitalFolderNumber = DigitalFolderNumber,
                BookNumber = BookNumber,
                Citation = Citation,
                HouseholdIdentifier = HouseholdIdentifier,
                ImageNumber = ImageNumber,
                PageNumber = PageNumber,
                Piece = int.Parse(PieceFolio.Split('/')[0]),
                Folio = int.Parse(PieceFolio.Split('/')[1]),
                RegistrationNumber = RegistrationNumber,
            };

            List<CensusPerson> censusPeople = People
                .Select(p => p.ToCensusPerson())
                .OrderByDescending(p => p.Age)
                .ToList();

            //Assign IDs
            for (int i = 0; i < censusPeople.Count; i++)
            {
                censusPeople[i].Id = i;
            }

            CensusRecord record = new()
            {
                Census = census,
                District = this.RegistrationDistrict,
                FullAddress = $"{ResidenceNote}, {this.EventPlace}",
                Street = this.ResidenceNote,
                CitationInfo = citation,
                People = censusPeople
            };

            return record;
        }
    }
}

