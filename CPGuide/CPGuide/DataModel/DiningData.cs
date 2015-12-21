using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace CPGuide.DataModel
{
    public class CPDiningItem
    {
        public CPDiningItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, String hours, Double latitude, Double longitude, String openstate, String phoneno)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Content = content;
            this.Hours = hours;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.OpenState = openstate;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public string Content { get; private set; }
        public string Hours { get; private set; }
        public Double Latitude { get; private set; }
        public Double Longitude { get; private set; }
        public String OpenState { get; private set; }
        public String phoneno { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public class CPDiningGroup
    {
        public CPDiningGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Items = new ObservableCollection<CPDiningItem>();
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public ObservableCollection<CPDiningItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public sealed class CPDiningDataSource
    {
        private static CPDiningDataSource _diningDataSource = new CPDiningDataSource();

        private ObservableCollection<CPDiningGroup> _groups = new ObservableCollection<CPDiningGroup>();
        public ObservableCollection<CPDiningGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<IEnumerable<CPDiningGroup>> GetGroupsAsync()
        {
            await _diningDataSource.GetDiningDataAsync();

            return _diningDataSource.Groups;
        }

        public static async Task<CPDiningGroup> GetGroupAsync(string uniqueId)
        {
            await _diningDataSource.GetDiningDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _diningDataSource.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<CPDiningItem> GetItemAsync(string uniqueId)
        {
            await _diningDataSource.GetDiningDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _diningDataSource.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static String GetOpenState(string uniqueId)
        {

            if ("Metro" == uniqueId)
            {
                //figure out datetime issue
                if ((DateTime.Now.Hour > 10) && DateTime.Now.Hour < 21)
                    return "Open";
                else
                    return "Closed";

            }
            if ("VGs" == uniqueId)
            {
                if (DateTime.Now.Hour > 8 && DateTime.Now.Hour < 12)
                    return "Open";
                else
                    return "Closed";

            }
            if ("Name of place" == uniqueId)
            {
                if (DateTime.Now.Hour < 9)
                    return "Open";
                else
                    return "Closed";

            }

            else
                return "Closed";
        }

        private async Task GetDiningDataAsync()
        {
            if (this._groups.Count != 0)
                return;

            Uri dataUri = new Uri("ms-appx:///DataModel/DiningData.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Groups"].GetArray();

            foreach (JsonValue groupValue in jsonArray)
            {
                JsonObject groupObject = groupValue.GetObject();
                CPDiningGroup group = new CPDiningGroup(groupObject["UniqueId"].GetString(),
                                                            groupObject["Title"].GetString(),
                                                            groupObject["Subtitle"].GetString(),
                                                            groupObject["ImagePath"].GetString(),
                                                            groupObject["Description"].GetString());

                foreach (JsonValue itemValue in groupObject["Items"].GetArray())
                {
                    JsonObject itemObject = itemValue.GetObject();
                    group.Items.Add(new CPDiningItem(itemObject["UniqueId"].GetString(),
                                                       itemObject["Title"].GetString(),
                                                       itemObject["Subtitle"].GetString(),
                                                       itemObject["ImagePath"].GetString(),
                                                       itemObject["Description"].GetString(),
                                                       itemObject["Content"].GetString(),
                                                       itemObject["Hours"].GetString(),
                                                       itemObject["Latitude"].GetNumber(),
                                                       itemObject["Longitude"].GetNumber(),
                                                       GetOpenState(itemObject["UniqueId"].GetString()),
                                                       itemObject["Phoneno"].GetString()));
                    // Convert.ToDouble(itemObject["Latitude"].GetString()),
                    //Convert.ToDouble(itemObject["Longitude"].GetString()),false));
                    //GetOpenState(itemObject["UniqueID"].GetString())));
                }
                this.Groups.Add(group);
            }
        }
    }
}
