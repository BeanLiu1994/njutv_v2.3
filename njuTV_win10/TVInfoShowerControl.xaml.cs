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
        public static TVInfoShowerControl Current;
        public TVurlFetcher WebFetcher { get; private set; }
        public TVInfoShowerControl()
        {
            this.InitializeComponent();
            Current = this;
            WebFetcher = new TVurlFetcher();
            TVInfoItems = WebFetcher.TVFetchedInfo;
        }
        public void SetPreviewMode(bool Previewsetting)
        {
            if(Previewsetting)
            {
                HasPreview.IsEnabled = true;
                HasPreview.Visibility = Visibility.Visible;
                NoPreview.IsEnabled = false;
                NoPreview.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoPreview.IsEnabled = true;
                NoPreview.Visibility = Visibility.Visible;
                HasPreview.IsEnabled = false;
                HasPreview.Visibility = Visibility.Collapsed;
            }
        }
        
        public async void Refresh()
        {
            TVInfoItems.Clear();
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
    }
}
