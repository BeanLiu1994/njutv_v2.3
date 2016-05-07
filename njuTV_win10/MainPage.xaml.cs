using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace njuTV_win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
            PlayerFrame.Navigate(typeof(Player));
        }
        public void PlayerShowOrNot(Visibility _vis)
        {
            PlayerFrame.Visibility = _vis;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            TVInfoPanel.Refresh();
        }
    }
}
