using Microsoft.VisualStudio.TestTools.UnitTesting;
using CensusBuilder.Models.Findmypast;
using CensusData.Models;
using CensusData;

namespace CensusBuilder.Models.FamilySearch.Tests
{
    [TestClass()]
    public class FindmypastCensusRecordTests
    {
        FindmypastImporter importer = new();

        [TestMethod()]
        public void TextToRecord_1841Test()
        {
            string[] text = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\Findmypast\\Census1841.txt");
            CensusRecord? censusRecord = importer.GetRecordFromText(text.ToList());

            Assertions(censusRecord);
        }


        [TestMethod()]
        public void TextToRecord_1851Test()
        {
            string[] text = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\Findmypast\\Census1851.txt");
            CensusRecord? censusRecord = importer.GetRecordFromText(text.ToList());

            Assertions(censusRecord);

            Assert.IsFalse(censusRecord.People.All(p => string.IsNullOrEmpty(p.Occupation)));
            Assert.IsFalse(censusRecord.People.All(p => string.IsNullOrEmpty(p.Role)));
        }

        private static void Assertions(CensusRecord censusRecord)
        {
            Assert.IsNotNull(censusRecord);
            Assert.IsNotNull(censusRecord.Census.Name);
            Assert.IsFalse(censusRecord.Census.Date == default);
            Assert.IsNotNull(censusRecord.CitationInfo);
            Assert.IsFalse(string.IsNullOrEmpty(censusRecord.CitationInfo.Citation));
            Assert.IsFalse(censusRecord.CitationInfo.Piece == 0);
            Assert.IsFalse(censusRecord.CitationInfo.Folio == 0);

            Assert.IsTrue(censusRecord.People.Any());

            Assert.IsFalse(censusRecord.People.Any(p => string.IsNullOrEmpty(p.Name)));
            Assert.IsFalse(censusRecord.People.Any(p => p.Sex == SexEnum.Unknown));
            Assert.IsFalse(censusRecord.People.Any(p => p.Age == 0));
            Assert.IsFalse(censusRecord.People.Any(p => string.IsNullOrEmpty(p.Birthplace)));

            Assert.IsFalse(censusRecord.People.Where(p => p.Id == 0).Count() > 1);

            Assert.IsTrue(censusRecord.CitationInfo.PageNumber > 0);
        }
    }
}