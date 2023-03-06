using Microsoft.VisualStudio.TestTools.UnitTesting;
using CensusBuilder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CensusBuilder.Pages;
using CensusBuilder.Models.FamilySearch;
using CensusBuilder.Models.Findmypast;

namespace CensusBuilder.Data.Tests
{
    [TestClass()]
    public class DatabaseTests
    {
        FamilySearchImporter fsImporter = new();
        FindmypastImporter fmpImporter = new();

        [TestMethod()]
        public async Task AddRecordTwice_1841Test()
        {
            string[] fsText = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\FamilySearch\\Census1841.txt");
            var fsRecord = fsImporter.GetRecordFromText(fsText.ToList());

            string[] fmpText = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\Findmypast\\Census1841.txt");
            var fmpRecord = fmpImporter.GetRecordFromText(fmpText.ToList());

            Database db = new();

            await db.AddRecord(fsRecord);
            await db.AddRecord(fmpRecord);

            Assert.IsTrue(db.Censuses.Count == 1);
            Assert.IsTrue(db.Censuses.First().Records.Count == 1);
        }


        [TestMethod()]
        public async Task AddRecordTwice_1851Test()
        {
            string[] fsText = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\FamilySearch\\Census1851.txt");
            var fsRecord = fsImporter.GetRecordFromText(fsText.ToList());

            string[] fmpText = File.ReadAllLines("C:\\Users\\sum01\\source\\repos\\CensusBuilder\\CensusBuilderTests\\bin\\Debug\\net7.0\\Models\\Findmypast\\Census1851.txt");
            var fmpRecord = fmpImporter.GetRecordFromText(fmpText.ToList());

            Database db = new();

            await db.AddRecord(fsRecord);
            await db.AddRecord(fmpRecord);

            Assert.IsTrue(db.Censuses.Count == 1);
            Assert.IsTrue(db.Censuses.First().Records.Count == 1);
        }
    }
}