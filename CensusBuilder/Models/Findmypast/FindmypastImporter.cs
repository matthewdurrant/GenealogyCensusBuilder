using System.Text.Json.Nodes;
using CensusBuilder.Extensions;
using System.Text.Json;
using CensusDatabase.Models;

namespace CensusBuilder.Models.Findmypast
{
    public class FindmypastImporter : IResourceImporter
    {
        public CensusRecord GetRecordFromText(List<string> textLines)
        {
            FindmypastCensusRecord fsRecord = GetFMPRecordFromString(textLines);
            CensusRecord censusRecord = fsRecord.ToCensusRecord();

            return censusRecord;
        }

        private FindmypastCensusRecord GetFMPRecordFromString(List<string> text)
        {
            //Map each line to a JSON element
            JsonObject jsonObjCensusRecord = new();
            List<FindmypastCensusPerson> people = new List<FindmypastCensusPerson>();

            for (int i = 0; i < text.Count; i++)
            {
                string line = text[i];
                string[] lineParts = line.Split("\t");
                if (lineParts.Length > 2) //This is a table line with a person
                {
                    FindmypastCensusPerson? censusPerson = TableRowToCensusPerson(lineParts);
                    if (censusPerson is not null)
                        people.Add(censusPerson);
                }

                else if (lineParts.Length == 2) //This is a normal line
                {
                    jsonObjCensusRecord = jsonObjCensusRecord.AddTextToJsonObject(line, "\t");
                }
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            FindmypastCensusRecord record = jsonObjCensusRecord.Deserialize<FindmypastCensusRecord>(options);
            record.People = people;

            return record;
        }

        private FindmypastCensusPerson? TableRowToCensusPerson(string[] values)
        {
            if (values[0] == "First name(s)")
                //This is the header row
                return null;

            FindmypastCensusPerson person = null;

            if (values.Length == 6) //1841 style census
            {
                person = new()
                {
                    FirstName = values[0],
                    LastName = values[1],
                    Sex = values[2],
                    Age = int.Parse(values[3]),
                    Birthplace = values[5]
                };
            }

            if (values.Length == 9) //1851 style census
            {
                person = new()
                {
                    FirstName = values[0],
                    LastName = values[1],
                    Relationship = values[2],
                    MaritalStatus = values[3],
                    Sex = values[4],
                    Age = int.Parse(values[5]),
                    Occupation = values[7],
                    Birthplace = values[8]
                };
            }

            return person;
        }
    }
}
