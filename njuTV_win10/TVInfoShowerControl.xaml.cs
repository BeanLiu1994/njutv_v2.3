using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace njuTV_win10
{
    public sealed partial class TVInfoShowerControl : UserControl,INotifyPropertyChanged
    {
        public static TVInfoShowerControl Current;
        public TVurlFetcher WebFetcher { get; private set; }
        public TVInfoShowerControl()
        {
            this.InitializeComponent();
            Current = this;
            WebFetcher = new TVurlFetcher();
            TVInfoItems = WebFetcher.TVFetchedInfo;
        }
        private PreviewConfig currentConfig;
        public PreviewConfig CurrentConfig
        {
            get { return currentConfig; }
            set
            {
                currentConfig = value;
                if (currentConfig.IsPreviewOn)
                    VisualStateManager.GoToState(this, "PreviewOn", true);
                else
                    VisualStateManager.GoToState(this, "PreviewOff", true);

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentConfig)));
            }
        }

        public async void Refresh()
        {
            TVInfoItems.Clear();
            //GC.Collect();
            await WebFetcher.RefreshWebState();
        }
        private ObservableCollection<TVInfo> tvinfoitems;
        public ObservableCollection<TVInfo> TVInfoItems
        {
            get { return tvinfoitems; }
            set { tvinfoitems = value;PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TVInfoItems))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ItemClicked(object sender, ItemClickEventArgs e)
        {
            var container = sender as ItemsControl;
            var ItemIndex = container.Items.IndexOf(e.ClickedItem);
            var PlayerParam = TVInfoItems.ElementAt(ItemIndex);
            MainPage.Current.PlayerShowOrNot(Visibility.Visible);
            Player.Current.PlayingInfo = PlayerParam;
        }

        private void ControlLoaded(object sender, RoutedEventArgs e)
        {
            var temp = MainPage.Current?.currentConfig;
            temp.PropertyChanged+=PreviewSettingChangedHandler;
        }
        private void PreviewSettingChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            CurrentConfig = sender as PreviewConfig;
        }
    }
}
