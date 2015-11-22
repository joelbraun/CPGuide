using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.Storage;
using Windows.Data.Json;

namespace CPGuide.DataModel
{
    public class LayoutDataItem
    {
        public LayoutDataItem(String uniqueId, String title, String pageType, String data)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.PageType = pageType;
            this.Data = data;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string PageType { get; private set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public class LayoutDataGroup
    {
        public LayoutDataGroup(String uniqueId, String icon, String title)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Icon = icon;
            this.Items = new ObservableCollection<LayoutDataItem>();
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Icon { get; private set; }
        public ObservableCollection<LayoutDataItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public sealed class LayoutDataSource
    {
        private static LayoutDataSource _layoutDataSource = new LayoutDataSource();

        private ObservableCollection<LayoutDataGroup> _groups = new ObservableCollection<LayoutDataGroup>();
        public ObservableCollection<LayoutDataGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<IEnumerable<LayoutDataGroup>> GetGroupsAsync()
        {
            await _layoutDataSource.GetLayoutDataAsync();

            return _layoutDataSource.Groups;
        }

        public static async Task<LayoutDataGroup> GetGroupAsync(string uniqueId)
        {
            await _layoutDataSource.GetLayoutDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _layoutDataSource.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<LayoutDataItem> GetItemAsync(string uniqueId)
        {
            await _layoutDataSource.GetLayoutDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _layoutDataSource.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetLayoutDataAsync()
        {
            if (this._groups.Count != 0)
                return;

            Uri dataUri = new Uri("ms-appx:///DataModel/LayoutData.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Data"].GetArray();

            foreach (JsonValue groupValue in jsonArray)
            {
                JsonObject groupObject = groupValue.GetObject();
                LayoutDataGroup group = new LayoutDataGroup(groupObject["UniqueId"].GetString(),
                                                            groupObject["Icon"].GetString(),
                                                            groupObject["Title"].GetString());

                foreach (JsonValue itemValue in groupObject["Items"].GetArray())
                {
                    JsonObject itemObject = itemValue.GetObject();
                    group.Items.Add(new LayoutDataItem(itemObject["UniqueId"].GetString(),
                                                       itemObject["Title"].GetString(), 
                                                       itemObject["PageType"].GetString(),
                                                       itemObject["Data"].GetString()));
                }
                this.Groups.Add(group);
            }
        }
    }
}
