using Microsoft.VisualStudio.TestTools.UnitTesting;
using CensusBuilder.Models.FamilySearch;
namespace CensusBuilder.Models.FamilySearch.Tests
{
    [TestClass()]
    public class FamilySearchCensusRecordTests
    {
        [TestMethod()]
        public void WriteDBTest()
        {
            string[] text1841 = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\FamilySearch\\Census1841.txt");
            FamilySearch.CensusRecord fsRecord1841 = FamilySearch.CensusRecord.FromClipboardString(text1841);
            Models.CensusRecord censusRecord1841 = fsRecord1841.ToCensusRecord();

            string[] text1851 = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\FamilySearch\\Census1851.txt");
            FamilySearch.CensusRecord fsRecord1851 = FamilySearch.CensusRecord.FromClipboardString(text1851);
            Models.CensusRecord censusRecord1851 = fsRecord1851.ToCensusRecord();

            Census census1841 = Census.EnglandAndWales1841;
            census1841.Records = new() { censusRecord1841 };
            Census census1851 = Census.EnglandAndWales1851;
            census1851.Records = new() { censusRecord1851 };

            List<Census> censuses = new()
            {
                census1841,
                census1851
            };

            string json = System.Text.Json.JsonSerializer.Serialize(censuses);

            File.WriteAllText("C:\\temp\\db.json", json);

        }


        [TestMethod()]
        public void FromClipboardString1841Test()
        {
            string[] text = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\FamilySearch\\Census1841.txt");
            var result = FromClipboardStringTest(text);
        }

        [TestMethod()]
        public void ToCensusRecord1841Test()
        {
            string[] text = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\FamilySearch\\Census1841.txt");
            var result = ToCensusRecordTest(text);
        }

        [TestMethod()]
        public void FromClipboardString1851Test()
        {
            string[] text = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\FamilySearch\\Census1851.txt");
            var result = FromClipboardStringTest(text);

            Assert.IsFalse(result.People.All(p => string.IsNullOrEmpty(p.Occupation)));
            Assert.IsFalse(result.People.All(p => string.IsNullOrEmpty(p.Role)));


        }

        [TestMethod()]
        public void ToCensusRecord1851Test()
        {
            string[] text = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\FamilySearch\\Census1851.txt");
            var result = ToCensusRecordTest(text);

            Assert.IsFalse(result.People.All(p => string.IsNullOrEmpty(p.Occupation)));
            Assert.IsFalse(result.People.All(p => string.IsNullOrEmpty(p.Role)));
        }

        private static Models.CensusRecord ToCensusRecordTest(string[] text)
        {
            FamilySearch.CensusRecord fsRecord = FamilySearch.CensusRecord.FromClipboardString(text);

            Models.CensusRecord censusRecord = fsRecord.ToCensusRecord();

            Assert.IsNotNull(censusRecord);
            Assert.IsNotNull(censusRecord.Census.Name);
            Assert.IsFalse(censusRecord.Census.Date == default);
            Assert.IsNotNull(censusRecord.CitationInfo);
            Assert.IsFalse(string.IsNullOrEmpty(censusRecord.CitationInfo.Citation));
            Assert.IsFalse(string.IsNullOrEmpty(censusRecord.CitationInfo.PieceFolio));

            Assert.IsTrue(censusRecord.People.Any());

            Assert.IsFalse(censusRecord.People.Any(p => string.IsNullOrEmpty(p.Name)));
            Assert.IsFalse(censusRecord.People.Any(p => p.Sex == SexEnum.Unknown));
            Assert.IsFalse(censusRecord.People.Any(p => p.Age == 0));
            Assert.IsFalse(censusRecord.People.Any(p => string.IsNullOrEmpty(p.Birthplace)));

            Assert.IsFalse(censusRecord.People.Where(p => p.Id == 0).Count() > 1);

            return censusRecord;
        }

        private static FamilySearch.CensusRecord FromClipboardStringTest(string[] text)
        {
            FamilySearch.CensusRecord record = FamilySearch.CensusRecord.FromClipboardString(text);

            Assert.IsNotNull(record);
            Assert.IsNotNull(record.Name);
            Assert.IsTrue(record.EventDate > 0);
            Assert.IsTrue(record.LineNumber > 0);
            Assert.IsFalse(string.IsNullOrEmpty(record.Citation));
            Assert.IsFalse(string.IsNullOrEmpty(record.PieceFolio));


            Assert.IsTrue(record.People.Any());

            Assert.IsFalse(record.People.Any(p => string.IsNullOrEmpty(p.Name)));
            Assert.IsFalse(record.People.Any(p => string.IsNullOrEmpty(p.Sex)));
            Assert.IsFalse(record.People.Any(p => p.Age == 0));
            Assert.IsFalse(record.People.Any(p => string.IsNullOrEmpty(p.Birthplace)));

            return record;
        }
    }
}