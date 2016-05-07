﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class PreviewConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool ispreviewon;
        public bool IsPreviewOn
        {
            get { return ispreviewon; }
            set { ispreviewon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPreviewOn))); }
        }

        private bool isautoplayon;
        public bool IsAutoPlayOn
        {
            get { return isautoplayon; }
            set { isautoplayon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAutoPlayOn))); }
        }
    }
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public PreviewConfig currentConfig;
        public MainPage()
        {
            this.InitializeComponent();
            currentConfig = new PreviewConfig();
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

            temp = false;
            SS_T.GetRecordObject(NameManager.PreviewSettingString, ref temp);
            PreviewSettingButton.IsChecked = temp;
            currentConfig.IsPreviewOn = temp;

            temp = false;
            SS_T.GetRecordObject(NameManager.AutoPlaySettingString, ref temp);
            currentConfig.IsAutoPlayOn = temp;
        }

        public void PlayerShowOrNot(Visibility _vis)
        {
            PlayerFrame.Visibility = _vis;
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
            //PlayerFrame.RequestedTheme = RequestedTheme;
        }


        private void PreviewEnabledButton_Click(object sender, RoutedEventArgs e)
        {
            var theButton = sender as AppBarToggleButton;
            var Toggled = (theButton.IsChecked.Value);
            if (Toggled)
            {
                // warning
                theButton.Label="关闭预览";
            }
            else
            {
                theButton.Label = "打开预览";
            }

            var SS_T = new SettingSaver_Local();
            SS_T.AlterRecordObject(NameManager.PreviewSettingString, Toggled);
            currentConfig.IsPreviewOn = Toggled;
            TVInfoPanel.Refresh();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            LoadAllSettings();
        }
    }
}
