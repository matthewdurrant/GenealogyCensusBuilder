using CensusDatabase.Models;

namespace CensusBuilder.Models.FamilySearch
{
    internal class FamilySearchCensusPerson : CensusPerson
    {
        public string Name { get; set; }
        public string? Role { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Birthplace { get; set; }
        public string Occupation { get; set; }
        public int LineNumber { get; internal set; }

        public CensusPerson ToCensusPerson()
        {
            return new CensusPerson()
            {
                LineNumber = LineNumber,
                Name = Name,
                Age = Age,
                Birthplace = Birthplace,
                Role = Role,
                Occupation = Occupation,
                Sex = Sex switch
                {
                    "F" or "Female" => SexEnum.Female,
                    "M" or "Male" => SexEnum.Male,
                    "I" or "X" => SexEnum.Intersex,
                    _ => SexEnum.Unknown
                }
            };
        }
    }
}
