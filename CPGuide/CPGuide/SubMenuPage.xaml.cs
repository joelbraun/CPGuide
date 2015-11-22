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
using CPGuide.DataModel;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238


namespace CPGuide
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SubMenuPage : Page
    {

        public ObservableCollection<LayoutDataItem> defaultViewModel = new ObservableCollection<LayoutDataItem>();

        public SubMenuPage()
        {
            this.InitializeComponent();
            defaultViewModel = App.LDG.Items;
            ItemsView.ItemsSource = defaultViewModel;
        }

        public SubMenuPage(LayoutDataGroup dGroup, SplitView MainSplitView) 
        {
            this.InitializeComponent();
            ItemsView.ItemsSource = dGroup.Items;
            defaultViewModel = dGroup.Items;

        }

        private void ItemsView_ItemClick(object sender, ItemClickEventArgs e)
        {
            LayoutDataItem toNav = e.ClickedItem as LayoutDataItem;

            if (toNav.PageType == "Web")
            {
                (App.MSV.Content as Frame).Navigate(typeof(WebViewPage), toNav.Data);
            }
       }
    }
}
