using CensusData.Models;

namespace CensusMaui.Models.Findmypast
{
    internal class FindmypastCensusRecord : ICensusRecord
    {
        public string FirstNames { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public int BirthYear { get; set; }
        public string BirthCounty { get; set; }
        public string BirthCountyAsTranscribed { get; set; }
        public string BirthPlaceOther { get; set; }
        public string Occupation { get; set; }
        public string FullAddress { get; set; }
        public string Street { get; set; }
        public string ParishOrTownship { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string RegistrationDistrict { get; set; }
        public string SubDistrict { get; set; }
        public string FamilyMemberFirstNames { get; set; }
        public string FamilyMemberLastName { get; set; }
        public string ArchiveReference { get; set; }
        public int PieceNumber { get; set; }
        public int BookNumber { get; set; }
        public int FolioNumber { get; set; }
        public int PageNumber { get; set; }
        public int Schedule { get; set; }
        public string RecordSet { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string CollectionsFrom { get; set; }


        public List<FindmypastCensusPerson> People { get; internal set; }

        public CensusRecord ToCensusRecord()
        {
            string recordYear = this.RecordSet.Substring(0, 4);
            Census? census = null;
            if (int.TryParse(recordYear, out int year)) {
                census = year switch
                {
                    1841 => Census.EnglandAndWales1841,
                    1851 => Census.EnglandAndWales1851,
                    _ => null
                };
            }

            CitationInfo citation = new()
            {
                RegistrationNumber = ArchiveReference,
                Piece = PieceNumber,
                Folio = FolioNumber,
                BookNumber = BookNumber,
                PageNumber = PageNumber,
                Schedule = Schedule,
                Citation = "TODO Generate Citation",
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
                FullAddress = this.FullAddress,
                Street = this.Street,
                CitationInfo = citation,
                People = censusPeople
            };

            return record;
        }
    }
}