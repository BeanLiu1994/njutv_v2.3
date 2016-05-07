using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Streaming.Adaptive;
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
    public sealed partial class MediaPreview : UserControl,INotifyPropertyChanged
    {
        public MediaPreview()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool autoplay;

        public bool AutoPlay
        {
            get { return autoplay; }
            set { autoplay = value; video.AutoPlay = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AutoPlay))); }
        }
        public Uri CurrentUri { get; private set; }
        public async Task<bool> PreViewLoad(Uri tvurl, bool _AutoPlay)
        {
            CurrentUri = tvurl;
            AutoPlay = _AutoPlay;
            var URLsourceResult = await AdaptiveMediaSource.CreateFromUriAsync(tvurl);
            if (URLsourceResult.Status == AdaptiveMediaSourceCreationStatus.Success)
            {
                video.SetMediaStreamSource(URLsourceResult.MediaSource);
                video.Visibility = Visibility.Visible;
                return true;
            }
            else
            {
                video.Visibility = Visibility.Collapsed;
                return false;
            }
        }

        private void StateChanged(object sender, RoutedEventArgs e)
        {
            switch (video.CurrentState)
            {
                case MediaElementState.Paused:
                    video.Stop();
                    //video.Source = new Uri("ms-appx://Assets/StoreLogo.png");
                    break;
            }
        }
    }
}
