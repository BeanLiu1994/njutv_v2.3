using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace njuTV_win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public sealed partial class Player : Page, INotifyPropertyChanged
    {
        public static Player Current;
        public Player()
        {
            this.InitializeComponent();
            Current = this;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested +=
                (s, e) =>
                {
                    MainPage.Current.PlayerShowOrNot(Visibility.Collapsed);
                    MediaPlayer.Pause();
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    ApplicationView.GetForCurrentView().Title = "";
                };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private TVInfo playinginfo;
        public TVInfo PlayingInfo
        {
            get { return playinginfo; }
            set
            {
                playinginfo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlayingInfo)));
                var view = ApplicationView.GetForCurrentView();
                view.Title = playinginfo.Name;
                if(value.Avaliable)
                {
                    ErrorPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ErrorPanel.Visibility = Visibility.Visible;
                    var currentFetcher = TVInfoShowerControl.Current.WebFetcher;
                    if (!currentFetcher.InSchoolState)
                    {
                        ErrorInfo.Text = "没有校园网,无法读取 tv.nju.edu.cn ";
                    }
                    else
                    {
                        ErrorInfo.Text = "当前视频源不可用 请尝试刷新或等待学校修复";
                    }                        
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var info = e.Parameter as TVInfo;
            if(info!=null)
                PlayingInfo = info;
        }

        private void MediaPartTapped(object sender, TappedRoutedEventArgs e)
        {
            //定义简单动画的实例
            DoubleAnimation daY = new DoubleAnimation();
            
            daY.From = 0D;

            //指定终点
            daY.To = MediaPlayerTransportControl.ActualHeight;
            
            //daY.By = 100D;
            //指定时长300ms
            Duration duration = new Duration(TimeSpan.FromMilliseconds(300));
            daY.Duration = duration;

            //将动画添加到偏移变形的实例上面 和Binding的格式有点像
            //this.textBox.SetBinding（TextBox.TextProperty,binding）

            //让按钮发生改变作为动画
            //btn.BeginAnimation(Button.WidthProperty, daX);
            //btn.BeginAnimation(Button.HeightProperty, daY);

            //让 位置发生改变作为动画               
            var s = new Storyboard();
            //Storyboard.SetTarget(daY, _tt);
            //Storyboard.SetTargetProperty(daY, "X");
            //s.Children.Add(daY);
            //s.Begin();
        }
    }
}
