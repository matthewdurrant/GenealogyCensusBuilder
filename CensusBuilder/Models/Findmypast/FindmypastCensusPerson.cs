using CensusDatabase.Models;

namespace CensusBuilder.Models.Findmypast
{
    internal class FindmypastCensusPerson : CensusPerson
    {
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string Sex { get; internal set; }
        public int Age { get; internal set; }
        public string Birthplace { get; internal set; }

        public string Relationship { get; internal set; }
        public string Occupation { get; internal set; }
        public string MaritalStatus { get; internal set; }

        public CensusPerson ToCensusPerson()
        {
            return new CensusPerson()
            {
                Name = $"{FirstName} {LastName}",
                Age = Age,
                Birthplace = Birthplace,
                Role = Relationship,
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