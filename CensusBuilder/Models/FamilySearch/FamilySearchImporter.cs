using CensusBuilder.Extensions;
using CensusBuilder.Pages;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CensusBuilder.Models.FamilySearch
{
    public class FamilySearchImporter : IResourceImporter
    {
        public CensusRecord GetRecordFromText(string[] textLines)
        {
            FamilySearchCensusRecord fsRecord = GetFSRecordFromString(textLines);
            CensusRecord censusRecord = fsRecord.ToCensusRecord();

            return censusRecord;
        }
       

        internal static FamilySearchCensusRecord GetFSRecordFromString(string[] text)
        {
            //Map each line to a JSON element
            JsonObject jsonObj = new();
            List<FamilySearchCensusPerson> people = new List<FamilySearchCensusPerson>();

            for (int i = 0; i < text.Length; i++)
            {
                string line = text[i];
                if (i > 1 && text[i-1] == "Citing this Record") //If the previous line was "Citing this record", this is the citation
                {
                    jsonObj.Add(nameof(FamilySearchCensusRecord.Citation), line);
                }
                else if (line.Contains(':')) //normal Label: Value line
                {
                    jsonObj = jsonObj.AddTextToJsonObject(line);
                }
                else if (line.Contains("\t")) //This is a table line
                {
                    FamilySearchCensusPerson? censusPerson = TextToCensusPerson(line);
                    if (censusPerson is not null)
                        people.Add(censusPerson);
                }
            }

            FamilySearchCensusRecord record = jsonObj.Deserialize<FamilySearchCensusRecord>();
            record.People = people;

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

        private static FamilySearchCensusPerson? TextToCensusPerson(string text)
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
