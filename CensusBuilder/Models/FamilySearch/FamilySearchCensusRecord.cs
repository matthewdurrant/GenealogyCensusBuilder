using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CensusBuilder.Models.FamilySearch
{
    public class CensusRecord
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
        public List<CensusPerson> People { get; private set; }

        public class CensusPerson
        {
            public string Name { get; set;}
            public string? Role { get; set; }
            public string Sex { get; set; }
            public int Age { get; set; }
            public string Birthplace { get; set; }
            public string Occupation { get; set; }


            internal Models.CensusPerson ToCensusPerson()
            {
                return new Models.CensusPerson()
                {
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

        public static CensusRecord FromClipboardString(string[] text)
        {
            //Map each line to a JSON element
            JsonObject jsonObj = new();
            List<CensusPerson> people = new List<CensusPerson>();

            for (int i = 0; i < text.Length; i++)
            {
                string line = text[i];
                if (i > 1 && text[i-1] == "Citing this Record") //If the previous line was "Citing this record", this is the citation
                {
                    jsonObj.Add(nameof(Citation), line);
                }
                else if (line.Contains(':')) //normal Label: Value line
                {
                    jsonObj = AddTextToJsonObject(jsonObj, line);
                }
                else if (line.Contains("\t")) //This is a table line
                {
                    CensusPerson? censusPerson = TextToCensusPerson(line);
                    if (censusPerson is not null)
                        people.Add(censusPerson);
                }
            }

            CensusRecord record = jsonObj.Deserialize<CensusRecord>();

            people.Add(new CensusPerson()
            {
                Name = record.Name,
                Age = record.Age,
                Birthplace = record.Birthplace,
                Role = record.RelationshipToHeadOfHousehold,
                Occupation = record.Occupation,
                Sex = record.Sex
            });

            record.People = people;
            return record;
        }

        private static JsonObject AddTextToJsonObject(JsonObject obj, string line)
        {
            string[] keyValuePair = line.Split(':');

            if (keyValuePair.Length == 2)
            {
                keyValuePair[0] = keyValuePair[0].Replace(" ", string.Empty);
                keyValuePair[0] = keyValuePair[0].Replace("/", string.Empty);
                keyValuePair[0] = keyValuePair[0].Trim();
                keyValuePair[1] = keyValuePair[1].Trim();

                if (int.TryParse(keyValuePair[1], out int value))
                    obj.Add(keyValuePair[0], value);
                else
                    obj.Add(keyValuePair[0], keyValuePair[1]);
            }

            return obj;
        }

        private static CensusPerson? TextToCensusPerson(string text)
        {
            string[] values = text.Split(" \t");
            if (values[0] == "Household")
                //This is not a data row
                return null;

            CensusPerson person = new()
            {
                Name = values[0],
                Role = values[1],
                Sex = values[2],
                Age = int.Parse(values[3]),
                Birthplace = values[4]
            };

            return person;
        }

        public Models.CensusRecord ToCensusRecord()
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
                LineNumber = LineNumber,
                PageNumber = PageNumber,
                PieceFolio = PieceFolio,
                RegistrationNumber = RegistrationNumber,
            };

            List<Models.CensusPerson> censusPeople = People
                .Select(p => p.ToCensusPerson())
                .OrderByDescending(p => p.Age)
                .ToList();

            //Assign IDs
            for (int i = 0; i < censusPeople.Count; i++)
            {
                censusPeople[i].Id = i;
            }

            Models.CensusRecord record = new()
            {
                Census = census,
                District = this.RegistrationDistrict,
                Place = this.EventPlace,
                ResidenceNote = this.ResidenceNote,
                CitationInfo = citation,
                People = censusPeople
            };

            return record;
        }
    }
}
