using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace CPGuide.DataModel
{
        public class MapDataItem
        {
            public MapDataItem(String uniqueId, String title, double latitude, double longitude)
            {
                this.UniqueId = uniqueId;
                this.Title = title;
                this.Latitude = latitude;
                this.Longitude = longitude;
            }

            public string UniqueId { get; private set; }
            public string Title { get; private set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public override string ToString()
            {
                return this.Title;
            }
        }

        /// <summary>
        /// Generic group data model.
        /// </summary>
        public class MapDataGroup
        {
            public MapDataGroup(String uniqueId)
            {
                this.UniqueId = uniqueId;
                this.Items = new ObservableCollection<MapDataItem>();
            }

            public string UniqueId { get; private set; }
            public ObservableCollection<MapDataItem> Items { get; private set; }

            public override string ToString()
            {
                return this.UniqueId;
            }
        }

        /// <summary>
        /// Creates a collection of groups and items with content read from a static json file.
        /// 
        /// SampleDataSource initializes with data read from a static json file included in the 
        /// project.  This provides sample data at both design-time and run-time.
        /// </summary>
        public sealed class MapDataSource
        {
            private static MapDataSource _mapDataSource = new MapDataSource();

            private ObservableCollection<MapDataGroup> _groups = new ObservableCollection<MapDataGroup>();
            public ObservableCollection<MapDataGroup> Groups
            {
                get { return this._groups; }
            }

            public static async Task<IEnumerable<MapDataGroup>> GetGroupsAsync()
            {
                await _mapDataSource.GetMapDataAsync();

                return _mapDataSource.Groups;
            }

            public static async Task<MapDataGroup> GetGroupAsync(string uniqueId)
            {
                await _mapDataSource.GetMapDataAsync();
                // Simple linear search is acceptable for small data sets
                var matches = _mapDataSource.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
                if (matches.Count() == 1) return matches.First();
                return null;
            }

            public static async Task<MapDataItem> GetItemAsync(string uniqueId)
            {
                await _mapDataSource.GetMapDataAsync();
                // Simple linear search is acceptable for small data sets
                var matches = _mapDataSource.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
                if (matches.Count() == 1) return matches.First();
                return null;
            }

            private async Task GetMapDataAsync()
            {
                if (this._groups.Count != 0)
                    return;

                Uri dataUri = new Uri("ms-appx:///DataModel/MapData.json");

                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
                String jsonText = await FileIO.ReadTextAsync(file);
                JsonObject jsonObject = JsonObject.Parse(jsonText);
                JsonArray jsonArray = jsonObject["Groups"].GetArray();

                foreach (JsonValue groupValue in jsonArray)
                {
                    JsonObject groupObject = groupValue.GetObject();
                    MapDataGroup group = new MapDataGroup(groupObject["UniqueId"].GetString());

                    foreach (JsonValue itemValue in groupObject["Items"].GetArray())
                    {
                        JsonObject itemObject = itemValue.GetObject();
                        group.Items.Add(new MapDataItem(itemObject["UniqueId"].GetString(),
                                                           itemObject["Title"].GetString(),
                                                           itemObject["Latitude"].GetNumber(),
                                                           itemObject["Longitude"].GetNumber()));
                    }
                    this.Groups.Add(group);
                }
            }
        }
    }
