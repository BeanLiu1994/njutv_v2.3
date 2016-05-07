﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Streaming.Adaptive;
using Windows.UI;
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
    public sealed partial class ShowOneItem : UserControl, INotifyPropertyChanged
    {
        public ShowOneItem()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
                TitleUI.Text = value;
            }
        }

        private string url;
        public string URL
        {
            get { return url; }
            set
            {
                url = value;
                SetSource(new Uri(value));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(URL)));
            }
        }
        private bool avaliable;
        public bool Avaliable
        {
            get { return avaliable; }
            set
            {
                avaliable = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Avaliable)));
                if(value)
                {
                    //之后改成一个visualstatemanager
                    ErrorPanel.Visibility = Visibility.Collapsed;
                    TitleUI.Foreground = new SolidColorBrush(Colors.DarkGreen);
                    ContentPresenterGrid.BorderThickness = new Thickness(0);
                }
                else
                {
                    ErrorPanel.Visibility = Visibility.Visible;
                    TitleUI.Foreground = new SolidColorBrush(Colors.Crimson);
                    ContentPresenterGrid.BorderThickness = new Thickness(1);
                    ContentPresenterGrid.BorderBrush = new SolidColorBrush(Colors.Crimson);
                }
            }
        }
        public async void SetSource(Uri inputUri)
        {
            var SS_L = new SettingSaver_Local();
            bool AutoPlay = false;//默认值
            SS_L.GetRecordObject(NameManager.AutoPlaySettingString, ref AutoPlay);
            if (!await MediaElementThumbnail.PreViewLoad(inputUri, AutoPlay))
                Avaliable = false;
        }
    }
}
