using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TicketDeviceSelect : ContentPage
    {
        //private bool _isRunning;
        public static List<string> DevicesList = new List<string>();
        public static ObservableCollection<SetDevicesList> dt;
        public string DEP_ID;
        //  public static List<string> DevicesListName = new List<string>();
        public static string deviceId = "0";
        public static string GROUPNAME = "";

        public string remoteAccess_deviceID;
        private static readonly HttpClient client = new HttpClient();

        location loc = new location();

        public TicketDeviceSelect(string id, string groupName)
        {
            if (Common.NetworkStatus())
            {
                try
                {
                    InitializeComponent();
                    GROUPNAME = groupName;
                    lblGroupName.Text = frstWord(groupName) + " DEVICES";
                    DEP_ID = id;
                    GetDevices(id);
                    timer(0);
                }
                catch { }
            }
            else
            {
                DisplayAlert("Error", "No Internet Connection", "ok");
            }
        }

        public string frstWord(string val)
        {
            char[] a = val.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            // DisplayAlert("val", a.ToString(), "ok");
            return new string(a);
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new TicketRoomSelect());
            base.OnBackButtonPressed();
            return true;
        }

        private void timer(double time)
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                time += 1;
                TimeSpan t = TimeSpan.FromSeconds(time);
                if (time == 15)
                {
                    udpateDevices();
                    time = 0;
                }

                return true;
            });
        }

        public void udpateDevices()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Updating device", "Devices updated");
                getDataFromService();
            }
            catch { }
        }

        public async Task<bool> getDeviceOnlineStatus(string device_id)
        {
            bool status = false;
            try
            {
                var responseString = await client.GetStringAsync("http://remote.cloudschool.management/guapi/guapi.php?devstat=1&conn_names=" + device_id);
                string onlineDevices = responseString.ToString();
                string[] b = onlineDevices.Split('=');
                if (b[1] == "true")
                {
                    status = true;
                }
            }
            catch { }
            return status;
        }

        public async void getDataFromService()
        {
            try
            {
                string joined = string.Join(",", DevicesList);
                var responseString = await client.GetStringAsync("http://remote.cloudschool.management/guapi/guapi.php?devstat=1&conn_names=" + joined);
                string onlineDevices = responseString.ToString();

                List<string> onlined = null;
                string[] a = onlineDevices.Split(',');
                int iii = 0;
                foreach (string s in a)
                {
                    string[] b = s.Split('=');
                    if (b[1] == "true")
                    {
                        dt[iii].online_status = "Online";
                        dt[iii].online = "https://cloudschool.management/itcrm/media/images/online.png";
                    }
                    iii++;
                }
                foreach (SetDevicesList d in dt)
                {
                    //DisplayAlert("DT", d.online_status + " = " + d.name, "ok");
                    System.Diagnostics.Debug.WriteLine("Updating device", d.online_status + " = " + d.name);
                }
            }
            catch { }
        }

        private async void GetDevices(string dep_id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.api_url);
                var values = new Dictionary<string, string>();
                //values.Add("operation", "query");
                values.Add("session_string", App.session_string);
                values.Add("data", "{\"table\": \"devices\"}");
                values.Add("extra", "{\"department\": \"" + dep_id.ToString() + "\"}");
                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage response = await client.PostAsync("/itcrm/getElements/", content);
                var result = await response.Content.ReadAsStringAsync();
                // await DisplayAlert("Info!", result, "ok");
                DevicesResult device_list = JsonConvert.DeserializeObject<DevicesResult>(result);
                var detail = device_list.result[0];
                dt = new ObservableCollection<SetDevicesList>();
                lvDevicesList.ItemsSource = dt;

                var lst = device_list.result;
                try
                {
                    for (int i = 0; i < lst.Length; i++)
                    {
                        try
                        {
                            DevicesList.Add(lst[i].id);
                            // getDeviceOnlineStatus(lst[i].id);
                        }
                        catch (Exception e)
                        {
                            // await DisplayAlert("Error name!", e.Message, "ok");
                        }
                    }
                }
                catch { }
                try
                {
                    for (int i = 0; i < lst.Length; i++)
                    {
                        try
                        {
                            string ionline = "https://cloudschool.management/itcrm/media/images/offline.png";
                            string ionline_status = "Offline";
                            bool status = await getDeviceOnlineStatus(lst[i].id);
                            if (status)
                            {
                                //await DisplayAlert("device id", lst[i].id + "status true", "ok");
                                ionline = "https://cloudschool.management/itcrm/media/images/online.png";
                                ionline_status = "Online";
                            }
                            dt.Add(new SetDevicesList()
                            {
                                name = lst[i].name,
                                deviceID = lst[i].id,
                                operating_system = lst[i].operating_system,
                                icon = "https://cloudschool.management/itcrm/media/images/devices.png",
                                online = ionline,
                                online_status = ionline_status
                            });
                        }
                        catch (Exception e)
                        {
                            //   await DisplayAlert("Error device!", e.Message, "ok");
                        }
                    }
                }
                catch { }
            }
            catch (Exception e)
            { }
            udpateDevices();
        }

        private string getIP()
        {
            var client = new HttpClient();
            var response = client.GetStringAsync("http://remote.cloudschool.management/guapi/guapi.php?cndetail=1&conn_name=25002&param=hostname");
            // await DisplayAlert("response", response.ToString(), "ok");
            return response.ToString();
        }

        private bool checkStatus(string ipAddress)
        {
            bool status = false;
            return status;
        }

        private async void lvDevicesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var index = (lvDevicesList.ItemsSource as ObservableCollection<SetDevicesList>).IndexOf(e.SelectedItem as SetDevicesList);
                var a = new SetDevicesList();
                ObservableCollection<SetDevicesList> dt = new ObservableCollection<SetDevicesList>();
                AddNewTicket.DEVICE_ID = DevicesList[index].ToString();
                App.NavigateMasterDetail(new AddNewTicket());
            }
            catch (Exception ex)
            {
                await DisplayAlert("CloudSchool", "Error! " + ex.Message, "ok");
            }
        }
        
    }
}