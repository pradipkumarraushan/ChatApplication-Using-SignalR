using DeviceId;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Web;

namespace groupChat.Models
{

    public class Networking
    {
        public string SystemIPAddress { get; set; }
        public string VistorIPAddress { get; set; }



        //this gets the ip address of the server pc
        public string GetIPAddress()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // Dns.Resolve() method is deprecated.
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            return ipAddress.ToString();
        }
        public string GetVistorIPAddress()
        {
            //while this gets the ip address of the visitor making the call
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            return ipAddress;
        }
        public string getPhysicalAddress()
        {
            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }
        public string getClientMachineName()
        {
            string clientMachineName = string.Empty;
            clientMachineName = (Dns.GetHostEntry(HttpContext.Current.Request.ServerVariables["remote_addr"]).HostName);

            return clientMachineName;
        }

        //A simple library providing functionality to generate a 'device ID' that can be used to uniquely identify a computer.
        public string getDeviceId()
        {
            string deviceId = new DeviceIdBuilder()
                .AddMachineName()
                //.AddProcessorId()
                //.AddMotherboardSerialNumber()
                //.AddSystemDriveSerialNumber()
                .ToString();
            return deviceId;
        }
        public string getCPUId()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }
        public string getUUID()
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "CMD.exe";

            //wmic DISKDRIVE get SerialNumber
            //wmic nic get MACAddress
            //wmic BIOS Get Manufacturer,Name,Version,SerialNumber
            //wmic os list brief
            startInfo.Arguments = "/C wmic csproduct get UUID";
            process.StartInfo = startInfo;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd();
            return output;
        }


    }
    //public class WebsiteVistorData
    //{
    //    public string ip { get; set; }
    //    public string city { get; set; }
    //    public string region { get; set; }
    //    public string country { get; set; }
    //    public string loc { get; set; }
    //    public string org { get; set; }
    //    public string postal { get; set; }
    //    public string timezone { get; set; }


    //    public WebsiteVistorData getClientIPData()
    //    {
    //        WebsiteVistorData vistorData = new WebsiteVistorData();
    //        try
    //        {
    //            using (var client = new HttpClient())
    //            {
    //                string apiUrl = "https://ipinfo.io?token=xxxxxxxxxxxx";
    //                //HTTP GET
    //                client.BaseAddress = new Uri(apiUrl);
    //                client.DefaultRequestHeaders.Accept.Clear();
    //                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

    //                var responseTask = client.GetAsync(apiUrl);
    //                responseTask.Wait();

    //                var result = responseTask.Result;
    //                if (result.IsSuccessStatusCode)
    //                {
    //                    var readTask = result.Content.ReadAsStringAsync();
    //                    readTask.Wait();

    //                    var alldata = readTask.Result;

    //                    //WebsiteVistorData vistorData = new WebsiteVistorData();
    //                   vistorData = Newtonsoft.Json.JsonConvert.DeserializeObject<WebsiteVistorData>(alldata);


    //                    return vistorData;


    //                }
    //                else //web api sent error response 
    //                {

    //                    return vistorData = new WebsiteVistorData();


    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return vistorData = new WebsiteVistorData();
    //        }

    //    }
    //}
    public class WebsiteVistorData
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("regionName")]
        public string RegionName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zip")]
        public long Zip { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("isp")]
        public string Isp { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("as")]
        public string As { get; set; }

        [JsonProperty("query")]
        public string Ip { get; set; }

        public string MacId { get; set; }
        public string ClientMachineName { get; set; }

        public WebsiteVistorData getClientIPData()
        {
            WebsiteVistorData vistorData = new WebsiteVistorData();
            try
            {
                using (var client = new HttpClient())
                {
                    string apiUrl = "http://ip-api.com/json";
                    //HTTP GET
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var responseTask = client.GetAsync(apiUrl);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();

                        var alldata = readTask.Result;

                        //WebsiteVistorData vistorData = new WebsiteVistorData();
                        vistorData = Newtonsoft.Json.JsonConvert.DeserializeObject<WebsiteVistorData>(alldata);


                        return vistorData;


                    }
                    else //web api sent error response 
                    {

                        return vistorData = new WebsiteVistorData();


                    }
                }
            }
            catch (Exception ex)
            {
                return vistorData = new WebsiteVistorData();
            }

        }
    }


}