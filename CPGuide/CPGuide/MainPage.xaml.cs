using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CPGuide.Data;
using CPGuide.Common;
using CPGuide.DataModel;
using Windows.ApplicationModel.Resources;
using System.Collections.ObjectModel;
using System.Windows.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CPGuide
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<CPDataItem> defaultViewModel = new ObservableCollection<CPDataItem>();
        //private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");
        private 

         MainPage(Frame frame)
        {
            this.InitializeComponent();
            MainSplitView.Content = frame;
            (MainSplitView.Content as Frame).Navigate(typeof(HomePage));
            getData();
            HamburgerList.ItemsSource = defaultViewModel;
            HamburgerListItemCommand = new Command<object>(HamburgerListButtonClick);

            //this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

         ICommand HamburgerListItemCommand
        {
            get;
            private set;
        }

        private async void getData()
        {
            var cpDataItems = await CPDataSource.GetItemsAsync();

            foreach (CPDataItem c in cpDataItems)
            {
                this.defaultViewModel.Add(c);
            }
            
        }

        public ObservableCollection<CPDataItem> DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }


        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void HamburgerList_ItemClick(object sender, RoutedEventArgs e)
        {

        }
        
        private void Item1Click(object sender, RoutedEventArgs e)
        {
            (MainSplitView.Content as Frame).Navigate(typeof(HomePage));
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void HamburgerListButtonClick(object parameter)
        {
            CPDataItem item = parameter as CPDataItem;
            int index = DefaultViewModel.IndexOf(item);
            HamburgerList.SelectedIndex = index;
            MainSplitView.Content = new WebViewPage(DetermineURI(item));
        }

        private Uri DetermineURI(CPDataItem parameter)
        {
            if (parameter != null)
            {
                return new Uri("http://microsoft.com");
            }
            else
            {
                return new Uri("http://google.com");
            }
        }

        /*
        private void Item2Click(object sender, RoutedEventArgs e)
        {
            (MainSplitView.Content as Frame).Navigate(typeof(SecondPage));
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
            HamburgerList.SelectedItem = Map;
        }
        
       
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            //this.Frame.Navigate(typeof(ItemPage), itemId);
        }
        */

    }
}
