using System.Text.Json.Serialization;

namespace CensusBuilder.Models
{
    public class CensusRecord
    {
        [JsonIgnore]
        public Census Census { get; set; }
        public CitationInfo CitationInfo { get; set; }
        public string Place { get; set; }
        public string District { get; set; }
        public string ResidenceNote { get; set; }

        public string Description { get
            {
                string desc = $"Household of {People[0].Name} ({People[0].Age})";
                if (People.Count > 1)
                {
                    desc += $" and {People[1].Name} ({People[1].Age})";
                }
                return desc;
            } 
        }

        public List<CensusPerson> People { get; set; } = new();

        //TODO this should probably sit at the provider-specific level
        internal void UpdateFromRecord(CensusRecord newInformation)
        {
            foreach (CensusPerson person in newInformation.People)
            {
                var existingPeople = People.Where(ep =>
                    ep.Name == person.Name &&
                    ep.Age == person.Age &&
                    ep.Sex == person.Sex &&
                    ep.Birthplace == person.Birthplace &&
                    ep.LineNumber == 0 //Not enhanced yet
                );

                //Only update if exactly 1 match
                if (existingPeople.Count() == 1)
                {
                    var ep = existingPeople.First();
                    ep.LineNumber = person.LineNumber;
                    ep.Occupation = person.Occupation;
                }
            }
        }
    }
}
