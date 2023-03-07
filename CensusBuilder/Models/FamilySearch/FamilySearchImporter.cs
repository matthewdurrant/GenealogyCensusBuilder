using CensusBuilder.Extensions;
using CensusDatabase.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CensusBuilder.Models.FamilySearch
{
    public class FamilySearchImporter : IResourceImporter
    {
        public CensusRecord GetRecordFromText(List<string> textLines)
        {
            FamilySearchCensusRecord fsRecord = GetFSRecordFromString(textLines);
            CensusRecord censusRecord = fsRecord.ToCensusRecord();

            return censusRecord;
        }
       

        internal static FamilySearchCensusRecord GetFSRecordFromString(List<string> text)
        {
            //Map each line to a JSON element
            JsonObject jsonObjCensusRecord = new();
            List<FamilySearchCensusPerson> people = new List<FamilySearchCensusPerson>();

            for (int i = 0; i < text.Count; i++)
            {
                string line = text[i];
                if (i > 1 && text[i-1] == "Citing this Record") //If the previous line was "Citing this record", this is the citation
                {
                    jsonObjCensusRecord.Add(nameof(FamilySearchCensusRecord.Citation), line);
                }
                else if (line.Contains(':')) //normal Label: Value line
                {
                    jsonObjCensusRecord = jsonObjCensusRecord.AddTextToJsonObject(line, ":");
                }
                else if (line.Contains("\t")) //This is a table line
                {
                    FamilySearchCensusPerson? censusPerson = TableRowToCensusPerson(line);
                    if (censusPerson is not null)
                        people.Add(censusPerson);
                }
            }

            FamilySearchCensusRecord record = jsonObjCensusRecord.Deserialize<FamilySearchCensusRecord>();
            record.People = people;

            //Familysearch doesn't include the "main" record in the "secondary" table of people that comes with the main record
            //Add the original person from the "main" record to the table of people
            record.People.Add(new FamilySearchCensusPerson()
            {
                Name = record.Name,
                Age = record.Age,
                Birthplace = record.Birthplace,
                Role = record.RelationshipToHeadOfHousehold,
                Occupation = record.Occupation,
                Sex = record.Sex,
                LineNumber = record.LineNumber
            });

            return record;
        }

        private static FamilySearchCensusPerson? TableRowToCensusPerson(string text)
        {
            string[] values = text.Split(" \t");
            if (values[0] == "Household")
                //This is not a data row
                return null;

            FamilySearchCensusPerson person = new()
            {
                Name = values[0],
                Role = values[1],
                Sex = values[2],
                Age = int.Parse(values[3]),
                Birthplace = values[4]
            };

            return person;
        }
    }
}
