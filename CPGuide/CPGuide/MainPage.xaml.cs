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
        private ObservableCollection<LayoutDataGroup> defaultViewModel = new ObservableCollection<LayoutDataGroup>();
        //private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public MainPage(Frame frame)
        {
            this.InitializeComponent();
            MainSplitView.Content = frame;
            App.MSV = MainSplitView;
            (MainSplitView.Content as Frame).Navigate(typeof(HomePage));
            getData();
            HamburgerList.ItemsSource = DefaultViewModel;
            HamburgerListItemCommand = new Command<object>(HamburgerListButtonClick);
        }

        public ICommand HamburgerListItemCommand
        {
            get;
            private set;
        }

        private async void getData()
        {
            var cpDataGroups = await LayoutDataSource.GetGroupsAsync();

            foreach (LayoutDataGroup c in cpDataGroups)
            {
                DefaultViewModel.Add(c);
            }
            
        }

        public ObservableCollection<LayoutDataGroup> DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }


        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void HamburgerList_ItemClick(object sender, ItemClickEventArgs e)
        {
            LayoutDataGroup item = e.ClickedItem as LayoutDataGroup;
            App.LDG = item;
            (MainSplitView.Content as Frame).Navigate(typeof(SubMenuPage), item);

        }
        
        private void Item1Click(object sender, RoutedEventArgs e)
        {
            (MainSplitView.Content as Frame).Navigate(typeof(HomePage));
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        public void HamburgerListButtonClick(object parameter)
        {
            LayoutDataGroup item = parameter as LayoutDataGroup;
            int index = DefaultViewModel.IndexOf(item);
            HamburgerList.SelectedIndex = index;
            App.LDG = item;
            (MainSplitView.Content as Frame).Navigate(typeof(SubMenuPage), item);
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
