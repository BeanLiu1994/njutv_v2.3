using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Streaming.Adaptive;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace njuTV_win10
{
    public sealed partial class MediaThumbNail : UserControl
    {
        public MediaThumbNail()
        {
            this.InitializeComponent();
        }
        public Uri CurrentUri { get; private set; }
        public async Task<bool> PreViewLoad(Uri tvurl)
        {
            CurrentUri = tvurl;
            var URLsourceResult = await AdaptiveMediaSource.CreateFromUriAsync(tvurl);
            if (URLsourceResult.Status == AdaptiveMediaSourceCreationStatus.Success)
            {
                Thumbnail.Source = null;
                video.SetMediaStreamSource(URLsourceResult.MediaSource);
                video.Visibility = Visibility.Visible;
                return true;
            }
            else
            {
                Thumbnail.Visibility = Visibility.Collapsed;
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

        //这段没法用了 RenderTargetBitmap不能给MediaElement截图
        //private bool BeginToMakeThumbnail = false;
        //private async Task MakeThumbnail()
        //{
        //    var CacheFolder = ApplicationData.Current.LocalCacheFolder;
        //    var CacheFile = await CacheFolder.CreateFileAsync(CurrentUri.Segments.Last() + ".png", CreationCollisionOption.ReplaceExisting);
        //    var CacheFileWriteStream = await CacheFile.OpenAsync(FileAccessMode.ReadWrite);
        //    RenderTargetBitmap rtb = new RenderTargetBitmap();
        //    await rtb.RenderAsync(video);
        //    var Buffer = await rtb.GetPixelsAsync();
        //    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, CacheFileWriteStream);
        //    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
        //              (uint)rtb.PixelWidth,
        //              (uint)rtb.PixelHeight,
        //              96d,
        //              96d,
        //              Buffer.ToArray());
        //    await encoder.FlushAsync();
        //    CacheFileWriteStream.Dispose();
        //    video.Stop();
        //    video.Source = new Uri(CacheFile.Path);
        //    Thumbnail.Source = new BitmapImage(new Uri(CacheFile.Path));
        //    video.Visibility = Visibility.Collapsed;
        //    BeginToMakeThumbnail = false;
        //}
        //private async void StateChanged(object sender, RoutedEventArgs e)
        //{
        //    switch (video.CurrentState)
        //    {
        //        case MediaElementState.Paused:
        //            if (!BeginToMakeThumbnail)
        //            {
        //                BeginToMakeThumbnail = true;
        //                await MakeThumbnail();
        //            }
        //            break;
        //    }
        //}
    }
}
