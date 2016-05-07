using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace njuTV_win10
{
    public class TVInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); }
        }

        private string url;
        public string URL
        {
            get { return url; }
            set
            {
                url = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(URL)));
                URLAvaliableCheck();
            }
        }

        public async void URLAvaliableCheck()
        {
            var WebChecker = new ConnectivityChecker();
            Avaliable = (await WebChecker.TestConnection(URL)).Value;
        }

        private bool avaliable;
        public bool Avaliable
        {
            get { return avaliable; }
            set { avaliable = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Avaliable))); }
        }
    }
    public class TVurlFetcher:INotifyPropertyChanged
    {
        public static bool VerifyURL(string InURL)
        {
            var myMatch = Regex.Matches(InURL, @"[h,t,p,H,T,P]{4}://.*[m3u8,pls,flv,mp4,avi,wmv,mkv]");
            if (myMatch.Count > 0)
                return true;
            else
                return false;
        }


        private ObservableCollection<TVInfo> tvfetchedinfo;
        public ObservableCollection<TVInfo> TVFetchedInfo
        {
            get { return tvfetchedinfo; }
            set { tvfetchedinfo = value;PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(TVFetchedInfo))); }
        }
        public TVurlFetcher()
        {
            TVFetchedInfo = new ObservableCollection<TVInfo>();
            RefreshWebState();
        }

        const string SchoolVerify = "http://tv.nju.edu.cn";
        const string Source = "http://tv.nju.edu.cn";
        private static ConnectivityChecker InSchoolChecker = new ConnectivityChecker(SchoolVerify);
        private static ConnectivityChecker WebChecker = new ConnectivityChecker(Source);

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CurrentWebState { get; private set; }
        public bool InSchoolState { get; private set; }
        public bool AnalyseState { get; private set; }
        private string WebReturnedData { get; set; }
        public async Task<bool> RefreshWebState()
        {
            Debug.Write("[TVurlFetcher: 检测网络...]");
            InSchoolState = (await InSchoolChecker.TestConnection()).Value;
            CurrentWebState = (await WebChecker.TestConnection()).Value;

            if (CurrentWebState)
            {
                WebReturnedData = WebChecker.ReturnedData;
                Debug.Write("[TVurlFetcher: 正则表达式处理中...]");
                var myMatch = Regex.Matches(WebReturnedData, @"<li>\s*<h4>.{2,18}</h4>\s*<a\s*href=""player.html\?channel=\w{1,9}"".*>\w{1,5}</a>\s*<a\s*href=""hls/\w{1,9}.m3u8"".*>\w{1,5}</a>\s*</li>");
                foreach (Match m in myMatch)
                {
                    string temp = m.Groups[0].ToString();
                    temp = Regex.Replace(temp, @"<li>\s*<h4>", @"");
                    temp = Regex.Replace(temp, @"</h4>\s*<a\s*href=""", @" http://tv.nju.edu.cn/");
                    temp = Regex.Replace(temp, @""".*>\w{1,5}</a>\s*<a\s*href=""", @" http://tv.nju.edu.cn/");
                    temp = Regex.Replace(temp, @""".*>\w{1,5}</a>\s*</li>", @"");
                    string[] s = temp.Split(new char[] { ' ' });
                    TVFetchedInfo.Add(new TVInfo() { Name = s[0], URL = s[2] });
                }
                Debug.WriteLine("  [处理结束]");
                if (TVFetchedInfo.Count == 0)
                {
                    AnalyseState = false;
                    Debug.WriteLine("[TVurlFetcher: 成功打开网页, 但没有内容]");
                }
                AnalyseState = true;
            }
            else
            {
                WebReturnedData = null;
                TVFetchedInfo.Clear();
                AnalyseState = false;
                Debug.WriteLine("  [不能连接]");
            }

            return CurrentWebState;
        }

        public override string ToString()
        {
            if (TVFetchedInfo.Count > 0)
            {
                string info = "";
                foreach (TVInfo t in TVFetchedInfo)
                {
                    info += t.Name + "\t";
                    info += t.URL;
                    info += "\n";
                }
                return info;
            }
            else
                return "Failed";
        }
    }
}
