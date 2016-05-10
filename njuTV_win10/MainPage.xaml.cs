using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
            Window.Current.SetTitleBar(MyTitlePart);
            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.BackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            view.TitleBar.InactiveBackgroundColor = Colors.Transparent;
            
            Current = this;
            PlayerFrame.Navigate(typeof(Player));
        }
        public void LoadAllSettings()
        {
            var SS_T = new SettingSaver_Local();
            bool temp = false;
            SS_T.GetRecordObject(NameManager.ThemeSettingString, ref temp);
            if (temp)
            {
                RequestedTheme = ElementTheme.Light;
            }
            else
            {
                RequestedTheme = ElementTheme.Dark;
            }
            Splitter.IsPaneOpen = true;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            TVInfoPanel.Refresh();
        }
        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            var SS_T = new SettingSaver_Local();
            switch(RequestedTheme)
            {
                case ElementTheme.Default:
                case ElementTheme.Light:
                    RequestedTheme = ElementTheme.Dark;
                    SS_T.AlterRecordObject(NameManager.ThemeSettingString, false);
                    break;
                case ElementTheme.Dark:
                    RequestedTheme = ElementTheme.Light;
                    SS_T.AlterRecordObject(NameManager.ThemeSettingString, true);
                    break;
            }
        }
        
        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            LoadAllSettings();
        }
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }
        public void SetTitleOfPlaying(string info)
        {
            TitleOfPlaying.Text = info;
        }

        private int i = 1;
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(!TVurlFetcher.VerifyURL(InputURL.Text))
            {
                AddButtonFlyoutOKButton.Content = "URL格式有误";
                return;
            }
            else
            {
                AddButtonFlyoutOKButton.Content = "确定";
            }

            var CurrentName = InputName.Text;
            if (CurrentName == "")
            {
                CurrentName = "Item " + i++.ToString();
            }

            TVInfoPanel.TVInfoItems.Add(new TVInfo() { Name = CurrentName, URL = InputURL.Text });
            AddButtonFlyout.Hide();
        }
    }
}
