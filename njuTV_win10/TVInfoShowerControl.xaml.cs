using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
            TVInfoItems.CollectionChanged += TVInfoItems_CollectionChanged;
            Refresh();
        }

        public async void Refresh()
        {
            TVInfoItems.Clear();
            //GC.Collect();
            bool Hresult = await WebFetcher.RefreshWebState();
            if (Hresult)
                Debug.WriteLine("读取成功");
            else
                Debug.WriteLine("读取失败");
            LoadContent();
        }

        private int itemssize;
        public int ItemsSize
        {
            get { return itemssize; }
            set { itemssize = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemsSize))); }
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
            Player.Current.PlayingInfo = PlayerParam;
        }

        private void TVInfoItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ItemsSize = tvinfoitems.Count;
        }

        private void ControlLoaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("加载了列表框架");
        }

        public void LoadContent()
        {
            var saver = new SettingSaver_Local();
            string[] savedTVName = null;
            string[] savedTVURL = null;
            saver.GetRecordObject("TVNameSaved", ref savedTVName);
            saver.GetRecordObject("TVURLSaved", ref savedTVURL);
            if (savedTVName != null)
            {
                for (int i = 0; i < savedTVName.Count(); ++i)
                {
                    TVInfoItems.Add(new TVInfo() { Name = savedTVName.ElementAt(i), URL = savedTVURL.ElementAt(i), InSchoolTv = false });
                }
            }
        }
        public void SaveContent()
        {
            var DataToSave = TVInfoItems.Where((m)=>!m.InSchoolTv);
            var saver = new SettingSaver_Local();
            if (DataToSave.Count() != 0)
            {
                saver.AlterRecordObject("TVNameSaved", DataToSave.Select((m) => m.Name).ToArray());
                saver.AlterRecordObject("TVURLSaved", DataToSave.Select((m) => m.URL).ToArray());
            }
            else
            {
                saver.DeleteRecordObject("TVNameSaved");
                saver.DeleteRecordObject("TVURLSaved");
            }
        }

        private TVInfo NowDataContext;
        private void ItemMenuFlyout(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            NowDataContext = (sender as ShowOneItem).DataContext as TVInfo;
        }
        private void ItemMenuFlyout(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            NowDataContext = (sender as ShowOneItem).DataContext as TVInfo;
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            TVInfoItems.Remove(NowDataContext);
            SaveContent();
        }
    }
}
