using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// 依照步驟 1a 或 1b 執行，然後執行步驟 2，以便在 XAML 檔中使用此自訂控制項。
    ///
    /// 步驟 1a) 於存在目前專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    ///要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:ZebraHttpStream"
    ///
    ///
    /// 步驟 1b) 於存在其他專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    ///要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:ZebraHttpStream;assembly=ZebraHttpStream"
    ///
    /// 您還必須將 XAML 檔所在專案的專案參考加入
    /// 此專案並重建，以免發生編譯錯誤: 
    ///
    ///     在 [方案總管] 中以滑鼠右鍵按一下目標專案，並按一下
    ///     [加入參考]->[專案]->[瀏覽並選取此專案]
    ///
    ///
    /// 步驟 2)
    /// 開始使用 XAML 檔案中的控制項。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class ZebraReader : Control
    {
        static ZebraReader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZebraReader), new FrameworkPropertyMetadata(typeof(ZebraReader)));
        }

        public class Login
        {
            public int code { get; set; }
            public string message { get; set; }
        }

        public class Status
        {
            public Antennas antennas { get; set; }
            public Cpu cpu { get; set; }
            public Flash flash { get; set; }
            public Interfaceconnectionstatus interfaceConnectionStatus { get; set; }
            public Ntp ntp { get; set; }
            public string powerNegotiation { get; set; }
            public string powerSource { get; set; }
            public string radioActivity { get; set; }
            public string radioConnection { get; set; }
            public Ram ram { get; set; }
            public DateTime systemTime { get; set; }
            public int temperature { get; set; }
            public string uptime { get; set; }
        }

        public class Antennas
        {
            public string _1 { get; set; }
            public string _2 { get; set; }
            public string _3 { get; set; }
            public string _4 { get; set; }
            public string _5 { get; set; }
            public string _6 { get; set; }
            public string _7 { get; set; }
            public string _8 { get; set; }
        }
        public class Cpu
        {
            public int system { get; set; }
            public int user { get; set; }
        }

        public class Readerconfig
        {
            public int free { get; set; }
            public int total { get; set; }
            public int used { get; set; }
        }

        public class Interfaceconnectionstatus
        {
            public List<Datum> data { get; set; }
        }

        public class Datum
        {
            public string connectionError { get; set; }
            public string connectionStatus { get; set; }
            public string description { get; set; }
            public string _interface { get; set; }
        }

        public class Ntp
        {
            public float offset { get; set; }
            public int reach { get; set; }
        }

        public class Mode
        {
            public List<int> antennas { get; set; }
            public Filter filter { get; set; }
            public Modespecificsettings modeSpecificSettings { get; set; }
            public List<string> tagMetaData { get; set; }
            public List<float> transmitPower { get; set; }
            public string type { get; set; }
        }

        public class Filter
        {
            public string match { get; set; }
            public string operation { get; set; }
            public string value { get; set; }
        }

        public class Modespecificsettings
        {
            public Interval interval { get; set; }
        }

        public class Interval
        {
            public string unit { get; set; }
            public int value { get; set; }
        }

        public class Event
        {
            public string component { get; set; }
            public Data data { get; set; }
            public int eventNum { get; set; }
            public DateTime timestamp { get; set; }
            public string type { get; set; }
        }

        public class Data
        {
            public Radio_Control radio_control { get; set; }
            public Reader_Gateway reader_gateway { get; set; }
            public System system { get; set; }
            public object[] userapps { get; set; }
        }

        public class Radio_Control
        {
            public Antennas antennas { get; set; }
            public float cpu { get; set; }
            public int numDataMessagesTxed { get; set; }
            public int numErrors { get; set; }
            public int numRadioPacketsRxed { get; set; }
            public int numTagReads { get; set; }
            public Numtagreadsperantenna numTagReadsPerAntenna { get; set; }
            public int numWarnings { get; set; }
            public string radioActivity { get; set; }
            public string radioConnection { get; set; }
            public float ram { get; set; }
            public string status { get; set; }
            public string uptime { get; set; }
        }

        public class Numtagreadsperantenna
        {
            public int _1 { get; set; }
            public int _2 { get; set; }
            public int _3 { get; set; }
            public int _4 { get; set; }
            public int _5 { get; set; }
            public int _6 { get; set; }
            public int _7 { get; set; }
            public int _8 { get; set; }
        }

        public class Reader_Gateway
        {
            public float cpu { get; set; }
            public Datapathstatistic[] dataPathStatistics { get; set; }
            public Interfaceconnectionstatus interfaceConnectionStatus { get; set; }
            public int numDataMessagesRxedFromExt { get; set; }
            public int numErrors { get; set; }
            public int numManagementEventsTxed { get; set; }
            public int numWarnings { get; set; }
            public float ram { get; set; }
            public string uptime { get; set; }
        }

        public class Datapathstatistic
        {
            public int numDataMessagesDropped { get; set; }
            public int numDataMessagesRetained { get; set; }
            public int numDataMessagesRxed { get; set; }
            public int numDataMessagesTxed { get; set; }
        }

        public class System
        {
            public GPI GPI { get; set; }
            public GPO GPO { get; set; }
            public Cpu cpu { get; set; }
            public Flash flash { get; set; }
            public Ntp ntp { get; set; }
            public string powerNegotiation { get; set; }
            public string powerSource { get; set; }
            public Ram ram { get; set; }
            public DateTime systemtime { get; set; }
            public Temperature temperature { get; set; }
            public string uptime { get; set; }
        }

        public class GPI
        {
            public string _1 { get; set; }
            public string _2 { get; set; }
            public string _3 { get; set; }
            public string _4 { get; set; }
        }

        public class GPO
        {
            public string _1 { get; set; }
            public string _2 { get; set; }
            public string _3 { get; set; }
            public string _4 { get; set; }
        }

        public class Flash
        {
            public Platform platform { get; set; }
            public Readerconfig readerConfig { get; set; }
            public Readerdata readerData { get; set; }
            public Rootfilesystem rootFileSystem { get; set; }
        }

        public class Platform
        {
            public int free { get; set; }
            public int total { get; set; }
            public int used { get; set; }
        }

        public class Readerdata
        {
            public int free { get; set; }
            public int total { get; set; }
            public int used { get; set; }
        }

        public class Rootfilesystem
        {
            public int free { get; set; }
            public int total { get; set; }
            public int used { get; set; }
        }

        public class Ram
        {
            public int free { get; set; }
            public int total { get; set; }
            public int used { get; set; }
        }

        public class Temperature
        {
            public int ambient { get; set; }
            public int pa { get; set; }
        }

        public class Inventory
        {
            public TagData data { get; set; }
            public DateTime timestamp { get; set; }
            public string type { get; set; }
        }

        public class TagData
        {
            public string CRC { get; set; }
            public string PC { get; set; }
            public string TID { get; set; }
            public string USER { get; set; }
            public int antenna { get; set; }
            public float channel { get; set; }
            public int eventNum { get; set; }
            public string format { get; set; }
            public string idHex { get; set; }
            public int peakRssi { get; set; }
            public float phase { get; set; }
            public int reads { get; set; }
        }
    }
}

