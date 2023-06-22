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
using System.Management;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

namespace tracker_OS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {

        public string name { get; set; } = System.Environment.MachineName;
        public int ram { get; set; }
        public string serialNo { get; set; }
        public string brandName { get; set; }
        public string processor { get; set; }
        public string model { get; set; }
        public string ip { get; set; }
        public string dnsIp { get; set; }
        public string macAddress { get; set; }

        public MainWindow()
        {
            
            InitializeComponent();


            serialNo = GetBoardSerNo();
            brandName = GetManuName();
            macAddress = getMacAddress();
            GetModel();
            ram = getRam();
            model = GetModelName();
            processor = GetProcessor();
            ip = GetLocalIPAddress();
            dnsIp = getDNSAddress();

            this.DataContext = this;
        }

        public string GetBoardSerNo() {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return "Serial Number: " + wmi.GetPropertyValue("SerialNumber").ToString();
                }

                catch{}
            }
            return "Serial Number: Unknown";
        }

        public string GetBIOSstatus() {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return "Status: " + wmi.GetPropertyValue("Status").ToString();
                }
                catch { }
            }
            return "BIOS Caption: Unknown";
        }

        public string GetManuName()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return "Brand Name: " + wmi.GetPropertyValue("Manufacturer").ToString();
                }
                catch { }
            }
            return "BIOS Caption: Unknown";
        }

        public string GetProcessor()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return "Processor Name: " + wmi.GetPropertyValue("Name").ToString();
                }
                catch { }
            }
            return "Unknown";
        }

        public string GetModelName()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return "Model Name: " + wmi.GetPropertyValue("Model").ToString();
                }
                catch { }
            }
            return "Unknown";
        }

        public string GetOS()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");

            foreach (ManagementObject wmi in searcher.Get())
            {
                try
                {
                    return "Operating System: " + wmi.GetPropertyValue("Caption").ToString();
                }
                catch { }
            }
            return "Unknown";
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return "IP Address: " + ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string getDNSAddress()
        {
            string dnsIp = "";
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
                    IPAddressCollection dnsAddresses = ipProperties.DnsAddresses;

                    foreach (IPAddress dns in dnsAddresses)
                    {
                        dnsIp = Convert.ToString(dns);
                        return "DNS IP Address: " + dnsIp;
                    }
                }
            }
            return "Unknown";
        }

        public static int getRam()
        {
            int ram = 0;
            ManagementObjectSearcher Search = new ManagementObjectSearcher("Select * From Win32_ComputerSystem");

            foreach (ManagementObject Mobject in Search.Get())
            {
                double Ram_Bytes = (Convert.ToDouble(Mobject["TotalPhysicalMemory"]));
                ram = (int)(Ram_Bytes / 1073741824);
                Console.WriteLine("RAM Size in Bytes: {0}", Ram_Bytes);
            }
            return ram;
        }

        public static string getMacAddress()
        {
            string addr = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.OperationalStatus == OperationalStatus.Up)
                {
                    var mac = string.Join(":", n.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));
                    addr += mac.ToString();
                    break;
                }
            }
            return "Mac Address: " + addr;
        }

    }
}
