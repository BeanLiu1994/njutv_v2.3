using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(URL)));
                MedaElementThumbnail.Source = new Uri(value);
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
    }
}
