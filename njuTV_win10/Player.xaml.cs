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
            MediaPlayer.TransportControls.Visibility = Visibility.Collapsed;
            CoreWindow.GetForCurrentThread().KeyUp += Player_KeyUp;
            MediaPlayerTransportControl.GotFocus += MediaPlayerTransportControl_GotFocus;
        }

        private void MediaPlayerTransportControl_GotFocus(object sender, RoutedEventArgs e)
        {
            FocusBlock.Focus(FocusState.Pointer);
        }

        private void Player_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Up:
                    FullScreen(true); break;
                case Windows.System.VirtualKey.Escape:
                case Windows.System.VirtualKey.Down:
                    FullScreen(false); break;
                case Windows.System.VirtualKey.Space:
                    PlayingStatusChange(); break;
            }
        }

        public void showInitInfo()
        {
            InitialPanel.Visibility = Visibility.Visible;
            return;
        }
        public void hideInitInfo()
        {
            InitialPanel.Visibility = Visibility.Collapsed;
            return;
        }

        public void FullScreen(bool? status = null)
        {
            if (PlayingInfo != null && MainPage.Current?.IsPaneOpen() == false)
                if (!status.HasValue)
                    MediaPlayer.IsFullWindow = !MediaPlayer.IsFullWindow;
                else
                    MediaPlayer.IsFullWindow = status.Value;
        }
        public void PlayingStatusChange(MediaElementState? status = null)
        {
            if (!status.HasValue)
            {
                switch (MediaPlayer.CurrentState)
                {
                    case MediaElementState.Playing:
                        MediaPlayer.Pause(); break;
                    case MediaElementState.Paused:
                        MediaPlayer.Play(); break;
                }
            }
            else
            {
                switch (status)
                {
                    case MediaElementState.Playing:
                        MediaPlayer.Play(); break;
                    case MediaElementState.Paused:
                        MediaPlayer.Pause(); break;
                    case MediaElementState.Stopped:
                        MediaPlayer.Stop(); break;
                }
            }
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

                MediaPlayer.TransportControls.Visibility = Visibility.Visible;
                InitialPanel.Visibility = Visibility.Collapsed;
                var view = ApplicationView.GetForCurrentView();
                view.Title = playinginfo.Name;
                MainPage.Current?.SetTitleOfPlaying(playinginfo.Name);
                if (value.Avaliable.HasValue)
                    if (value.Avaliable.Value)
                    {
                        ErrorPanel.Visibility = Visibility.Collapsed;
                        MediaPlayer.Play();
                    }
                    else
                    {
                        MediaFailedEventHandler(null, null);
                    }
            }
        }

        private void MediaFailedEventHandler(object sender, ExceptionRoutedEventArgs e)
        {
            ErrorPanel.Visibility = Visibility.Visible;
            var currentFetcher = TVInfoShowerControl.Current.WebFetcher;
            if (PlayingInfo.InSchoolTv)
            {
                if (!currentFetcher.InSchoolState)
                {
                    ErrorInfo.Text = "没有校园网,无法读取 tv.nju.edu.cn ";
                }
                else
                {
                    ErrorInfo.Text = "当前视频源不可用 请尝试刷新或等待学校修复";
                }
            }
            else
            {
                ErrorInfo.Text = "当前视频源不可用 请尝试刷新或输入新的视频源";
            }
        }

        private void EnterFullScreen(object sender, DoubleTappedRoutedEventArgs e)
        {
            FullScreen();
        }
    }
}
