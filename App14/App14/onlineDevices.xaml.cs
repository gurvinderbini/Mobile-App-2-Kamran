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
    public partial class onlineDevices : ContentPage
    {
        ComClass comfun = new ComClass();
        public static List<string> DevicesList = new List<string>();
        public static ObservableCollection<SetDevicesList> dt;
        private static readonly HttpClient client = new HttpClient();


        public onlineDevices()
        {
            InitializeComponent();
            try
            {
                AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));
                GetDevices();
                timer(0);
            }
            catch { }
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
                    //App.NavigateMasterDetail(new onlineDevices());
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

        public async void getDataFromService()
        {
            try
            {
                string joined = string.Join(",", DevicesList);
                var responseString = await client.GetStringAsync("https://remote.cloudschool.management/musaError000.php?device=" + joined);
                string onlineDevices = responseString.ToString();

                //List<string> onlined = null;
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

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MainPage());
            base.OnBackButtonPressed();
            return true;
        }

        private async void GetDevices()
        {
            if (comfun.isConnected())
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);
                    var values = new Dictionary<string, string>();
                    //values.Add("operation", "query");
                    values.Add("session_string", App.session_string);
                    values.Add("data", "{\"table\": \"devices\"}");
                    //values.Add("extra", "{\"status\": \"1\"}");
                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/getElements/", content);
                    var result = await response.Content.ReadAsStringAsync();
                    //await DisplayAlert("Info!", result, "ok");
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
                                bool status = await getDeviceOnlineStatus(lst[i].id);
                                if (status)
                                {
                                    dt.Add(new SetDevicesList()
                                    {
                                        name = lst[i].name,
                                        deviceID = lst[i].id,
                                        operating_system = lst[i].operating_system,
                                        icon = "https://cloudschool.management/itcrm/media/images/devices.png",
                                        online = "https://cloudschool.management/itcrm/media/images/online.png",
                                        online_status = "Online"
                                    });
                                }
                            }
                            catch (Exception e)
                            {
                                // await DisplayAlert("Error name!", e.Message, "ok");
                            }
                        }
                    }
                    catch { }
                }
                catch (Exception e)
                {
                    await DisplayAlert("e", e.Message, "ok");
                }
                aiDevices.IsRunning = false;
                //udpateDevices();
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }

        public async Task<bool> getDeviceOnlineStatus(string device_id)
        {
            bool status = false;
            try
            {
                string responseString = await client.GetStringAsync("https://remote.cloudschool.management/musaError000.php?device=" + device_id);
                if (responseString == "true")
                {
                    status = true;
                }
            }
            catch { }
            return status;
        }

        private async void btnAccess_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = (Xamarin.Forms.Button)sender;
                SetDevicesList listitem = (from itm in dt where itm.deviceID == item.CommandParameter.ToString() select itm).FirstOrDefault<SetDevicesList>();
                string device_id = listitem.deviceID.ToString();
                try
                {
                    App.NavigateMasterDetail(new WebViewPage(device_id));
                }
                catch { }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "Ok");
            }
        }

    }
}
