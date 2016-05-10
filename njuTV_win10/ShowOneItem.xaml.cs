using System;
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
                //相当于oneway的绑定
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(URL)));
            }
        }

        private bool? avaliable;
        public bool? Avaliable
        {
            get { return avaliable; }
            set
            {
                avaliable = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Avaliable)));
                // 相当于一个binding
                if(value.HasValue)
                    if(value.Value)
                    {
                        SubTitleUI.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        SubTitleUI.Visibility = Visibility.Visible;
                    }
            }
        }

        private void FailedToOpen(object sender, ExceptionRoutedEventArgs e)
        {
            Avaliable = false;
        }
    }
}
