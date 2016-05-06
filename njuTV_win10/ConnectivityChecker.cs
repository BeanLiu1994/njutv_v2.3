using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace njuTV_win10
{
    public class ConnectivityChecker
    {
        public bool? IsConnectionAvaliable { get; set; }
        public string TestedUrl { get; set; }
        public string ReturnedData { get; set; }
        public ConnectivityChecker(string url = null)
        {
            TestedUrl = url;
        }
        public async Task<bool?> TestConnection(string url = null)
        {
            if (url != null)
                TestedUrl = url;
            else
                url = TestedUrl;

            IsConnectionAvaliable = null;
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2.5));//设置延时时间2.5s
            try
            {
                HttpClient myHC = new HttpClient();
                HttpResponseMessage response = await myHC.GetAsync(new Uri(url), cts.Token);
                IsConnectionAvaliable = response.IsSuccessStatusCode;
                ReturnedData = await response.Content.ReadAsStringAsync();
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine("连接超时");
                Debug.WriteLine(e.Message);
                Debug.WriteLine("可能出现的问题是: api修改 网络断开 没有使用校园网");
                ReturnedData = null;
                IsConnectionAvaliable = false; 
            }
            catch (Exception e)
            {
                Debug.WriteLine("exception!!");
                Debug.WriteLine(e.Message);
                Debug.WriteLine("可能出现的问题是: api修改 网络断开 没有使用校园网");
                ReturnedData = null;
                IsConnectionAvaliable = false;
            }
            //返回结果网页（html）代码
            return IsConnectionAvaliable;
        }
    }
}
