using Blazored.LocalStorage;
using CensusBuilder.Models;
using CensusBuilder.Models.FamilySearch;
using CensusDatabase.Models;
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

        public async Task AddRecord(CensusRecord newRecord)

        {
            Census? existingCensus = Censuses.SingleOrDefault(c => c.Name == newRecord.Census.Name && c.Date == newRecord.Census.Date);

            if (existingCensus == null)
            {
                Censuses.Add(newRecord.Census);
                existingCensus = Censuses.Single(c => c.Name == newRecord.Census.Name && c.Date == newRecord.Census.Date);
            }

            //Check if household has been added
            CensusRecord? existingHousehold = existingCensus.Records.SingleOrDefault(r => 
                r.CitationInfo.PageNumber == newRecord.CitationInfo.PageNumber &&
                r.CitationInfo.Piece == newRecord.CitationInfo.Piece &&
                r.CitationInfo.Folio == newRecord.CitationInfo.Folio &&
                r.Description == r.Description
            );

            if (existingHousehold is null)
            {
                existingCensus.Records.Add(newRecord);
            }
            else
            {
                //Update the existing person.
                existingHousehold.UpdateFromRecord(newRecord);
            }

            await Save();
        }

        public async Task Save()
        {
            if (localStore is not null && this.Censuses is not null && this.Censuses.Any())
            {

                //Make sure any census records don't have a census property
                //TODO better way to do this
                this.Censuses.ForEach(c => c.Records.ForEach(r => r.Census = null));

                await localStore.SetItemAsync(LocalStorageKey, this.Censuses);
            }
        }
    }
}
