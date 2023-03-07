using Microsoft.VisualStudio.TestTools.UnitTesting;
using CensusBuilder.Models.FamilySearch;
using CensusDatabase.Models;

namespace CensusDatabase.Tests
{
    [TestClass()]
    public class CensusDatabaseTests
    {
        FamilySearchImporter importer = new();

        [TestMethod()]
        public void SaveCensusTest()
        {
            //string[] text = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\FamilySearch\\Census1841.txt");
            //var censusRecord = importer.GetRecordFromText(text.ToList());
            string path = @"C:\Temp\MyData1.db";

            CensusDatabase database = new CensusDatabase(path);

            Census census = new Census()
            {
                Name = "Test Census 1907"
            };

            database.SaveCensus(census);

            Census census2 = database.GetCensus(census.Name);

            Assert.IsNotNull(census2);
            Assert.AreEqual(census.Name, census2.Name);
        }

        [TestMethod()]
        public void PersistenceTest()
        {
            //string[] text = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\FamilySearch\\Census1841.txt");
            //var censusRecord = importer.GetRecordFromText(text.ToList());
            string path = @"C:\Temp\MyData1.db";

            CensusDatabase database = new CensusDatabase(path);

            Census census = new Census()
            {
                Name = "Test Census 1907"
            };

            Census census2 = database.GetCensus(census.Name);

            Assert.IsNotNull(census2);
            Assert.AreEqual(census.Name, census2.Name);
        }
    }
}