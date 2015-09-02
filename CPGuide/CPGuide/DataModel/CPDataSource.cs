using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace CPGuide.DataModel
{
    public class CPDataItem
    {
        public CPDataItem(String id, String title)
        {
            this.ID = id;
            this.Title = title;
        }

        public String ID { get; set; }
        public String Title { get; set; }

        public override string ToString()
        {
            return Title;
        }

    }

    public sealed class CPDataSource
    {
        private static CPDataSource _cpDataSource = new CPDataSource();
        private ObservableCollectio<CPDataItem> _items = new ObservableCollectio<CPDataItem>();
        public ObservableCollection<CPDataItem> Items
        {
            get { return this._items; }
        }
/*
       private async Task<string> GetjsonStream()
        {
            string url = "";
            Http
        }
        */

        private async Task GetSampleDataAsync()
        {
            if (this._items.Count != 0)
            {
                return;
            }

            Uri dataUri = new Uri("ms-appx:///CPData.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Items"].GetArray();

            foreach (JsonValue itemValue in jsonArray)
            {
                JsonObject itemObject = itemValue.GetObject();
                Items.Add(new CPDataItem(itemObject["ID"].GetString(), itemObject["Title"].GetString()));
            }

        }
    }
}
