﻿using System;
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
        public TVurlFetcher WebFetcher { get; private set; }
        public TVInfoShowerControl()
        {
            this.InitializeComponent();
            WebFetcher = new TVurlFetcher();
            TVInfoItems = WebFetcher.TVFetchedInfo;
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
            var container = sender as GridView;
            var ItemIndex = container.Items.IndexOf(e.ClickedItem);
            var PlayerParam = TVInfoItems.ElementAt(ItemIndex);
            MainPage.Current.PlayerShowOrNot(Visibility.Visible);
            Player.Current.PlayingInfo = PlayerParam;
        }
    }
}
