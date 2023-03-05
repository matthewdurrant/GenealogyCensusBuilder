using Blazored.LocalStorage;
using CensusBuilder.Models;
using CensusBuilder.Models.FamilySearch;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CensusBuilder.Data
{
    public class Database
    {
        [JsonIgnore]
        public Blazored.LocalStorage.ILocalStorageService localStore { get; set; }

        public string LocalStorageKey => "Census-DB";

        public async Task SetLocalStorage(ILocalStorageService _store)
        {
            localStore = _store;

            Censuses = await LoadFromLocalStorage();

            await Save();
        }

        private async Task<List<Census>> LoadFromLocalStorage()
        {
            //await localStore.RemoveItemAsync(LocalStorageKey);

            var data = await localStore.GetItemAsync<List<Census>>(LocalStorageKey);
            //await AddFromText(Census1841.Data);
            //await AddFromText(Census1851.Data);
            if (data is null) return new();
            return data;
        }

        public List<Census> Censuses { get; set; } = new();

        public async Task AddRecord(CensusRecord censusRecord)
        {
            Census? existingCensus = Censuses.SingleOrDefault(c => c.Name == censusRecord.Census.Name && c.Date == censusRecord.Census.Date);

            if (existingCensus == null)
            {
                Censuses.Add(censusRecord.Census);
                existingCensus = Censuses.Single(c => c.Name == censusRecord.Census.Name && c.Date == censusRecord.Census.Date);
            }

            //Check if household has been added
            CensusRecord? existingHousehold = existingCensus.Records.SingleOrDefault(r => r.CitationInfo.HouseholdIdentifier == censusRecord.CitationInfo.HouseholdIdentifier);

            if (existingHousehold is null)
            {
                existingCensus.Records.Add(censusRecord);
            }
            else
            {
                //Update the existing person.
                existingHousehold.UpdateFromRecord(censusRecord);
            }

            await Save();
        }

        public async Task Save()
        {
            if (this.Censuses is not null && this.Censuses.Any())
            {

                //Make sure any census records don't have a census property
                //TODO better way to do this
                this.Censuses.ForEach(c => c.Records.ForEach(r => r.Census = null));

                await localStore.SetItemAsync(LocalStorageKey, this.Censuses);
            }
        }
    }
}
