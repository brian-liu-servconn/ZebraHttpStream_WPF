using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZebraHttpStream
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpListener _listener;
        int Port = 8080;
        string token = "";

        public MainWindow()
        {
            InitializeComponent();
            ParamText.Text = "{'timestamp':'2024-05-25T17:41:49.816827043Z','eventType':'tagInventory','tagInventoryEvent':{'epc':'11CVO212w10zy5WNOMxyDww3AMPU2345wrwwwwxh','epcHex':'D750953B6D76C35D33CB958D38CC720F0C3700C3D4DB7E39C2BC30C30C61','tid':'4oARwCAAD70dcAM6','tidHex':'E28011C020000FBD1D70033A','pc':'faQ=','antennaPort':4,'peakRssiCdbm':-6200,'frequency':923750,'transmitPowerCdbm':3000}}";
        }


        private void Receive()
        {
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        public void Stop()
        {
            if (_listener != null)
            {
                if (_listener.IsListening)
                {
                    _listener.Stop();
                    _listener.Close();
                }
            }

        }

        private void ListenerCallback(IAsyncResult result)
        {
            try
            {
                HttpListener _listener = result.AsyncState as HttpListener;
                if (_listener.IsListening)
                {
                    HttpListenerContext context = _listener.EndGetContext(result);
                    _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);

                    HttpListenerRequest request = context.Request;
                    string content = "";

                    switch (request.HttpMethod)
                    {
                        case "POST":
                            {
                                Stream stream = context.Request.InputStream;

                                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                                content = HttpUtility.UrlDecode(reader.ReadToEnd());
                                reader.Close();
                                stream.Close();
                            }
                            break;
                        case "GET":
                            {
                                var data = request.QueryString;
                            }
                            break;
                    }

                    //回傳給USER
                    var abcOject = new
                    {
                        code = "200",
                        description = "success成功",
                        data = "time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    string responseString = JsonConvert.SerializeObject(abcOject,
                        new JsonSerializerSettings()
                        {
                            StringEscapeHandling = StringEscapeHandling.EscapeHtml
                            //StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                        });

                    HttpListenerResponse response = context.Response;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.ContentType = "application/json;charset=UTF-8";
                    response.ContentEncoding = Encoding.UTF8;
                    response.AppendHeader("Content-Type", "application/json;charset=UTF-8");
                    using (StreamWriter writer = new StreamWriter(response.OutputStream, Encoding.UTF8))
                    {
                        //writer.Write(HttpUtility.UrlEncode(responseString));
                        writer.Write(responseString);
                        writer.Close();
                        response.Close();
                    }

                    //寫在UI上
                    //setText2(content);
                    setListText(content);
                }
            }
            catch (Exception ex) { }
        }

        public string GetZEBRAToken(string host, string account, string password)
        {
            string url = "https://" + host + "/cloud/localRestLogin";
            string authenticationString = account + ":" + password;
            var base64String = Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes(authenticationString));
            try
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                var client = new HttpClient(handler);
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", "Basic " + base64String);
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                string res = response.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(res);
                ZebraReader.Login loginR = Newtonsoft.Json.JsonConvert.DeserializeObject<ZebraReader.Login>(res);
                if (loginR != null)
                    return loginR.message;
                else
                    return "";
            }
            catch (Exception ex)
            {
                return "";

            }
        }

        private ZebraReader.Status SetZEBRAInventory(string host, string token, bool start)
        {
            string url = "https://" + host + "/cloud/start";
            if (start == false)
                url = "https://" + host + "/cloud/stop";
            try
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                var client = new HttpClient(handler);
                var request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Headers.Add("Authorization", "Bearer " + token);
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                string res = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(res);
                ZebraReader.Status status = JsonConvert.DeserializeObject<ZebraReader.Status>(res);
                return status;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        private void setListText(string text)
        {
            Application.Current.Dispatcher.Invoke(new Action(delegate
            {
                string a = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + " - " + text;
                listBox1.Items.Add(a);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }));
        }

        public void setText2(string text)
        {
            Application.Current.Dispatcher.Invoke(new Action(delegate
            {
                textBox1.Text += text + "\r\n";
                return;
                List<myZdata.Inventory> tag = new List<myZdata.Inventory>();
                try
                {
                    tag = Newtonsoft.Json.JsonConvert.DeserializeObject<List<myZdata.Inventory>>(text);
                }
                catch
                {
                    textBox1.Text += text + "\r\n";
                }
                string a = "";
                for (int i = 0; i < tag.Count; i++)
                {
                    string v = rfmt.clsRFMT.getHEXtoEPC(tag[i].data.idHex.ToUpper());
                    if (v == "")
                        a += tag[i].data.idHex.ToUpper() + "\r\n";
                    else
                        a += v + "\r\n";
                }
                textBox1.Text += a;
            }));
        }

        private void BtnStartService_Click(object sender, RoutedEventArgs e)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:" + Port.ToString() + "/");
            _listener.Start();
            Receive();
        }

        private void BtnStopService_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void BtnStartRead_Click(object sender, RoutedEventArgs e)
        {
            string ipHost = ConfigurationManager.AppSettings["ipHost"].ToString();
            token = GetZEBRAToken(ipHost, "admin", "P@ssw0rd");
            SetZEBRAInventory(ipHost, token, true);
        }

        private void BtnStopRead_Click(object sender, RoutedEventArgs e)
        {
            string ipHost = ConfigurationManager.AppSettings["ipHost"].ToString();
            token = GetZEBRAToken(ipHost, "admin", "P@ssw0rd");
            SetZEBRAInventory(ipHost, token, false);
        }

        private void BtnSendData_Click(object sender, RoutedEventArgs e)
        {
            string Url = "http://127.0.0.1:8080";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.KeepAlive = true;
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json;charset=UTF-8";
            string param = ParamText.Text;
            byte[] bs = Encoding.UTF8.GetBytes(param);

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string theMessage = streamReader.ReadToEnd();
                setText2(HttpUtility.UrlDecode(theMessage));
            }

        }
    }
}
