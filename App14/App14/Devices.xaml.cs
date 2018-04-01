using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Devices : ContentPage
    {
        //private bool _isRunning;
        public static List<string> DevicesList = new List<string>();
        public static ObservableCollection<SetDevicesList> dt;
        public string DEP_ID;
        //  public static List<string> DevicesListName = new List<string>();
        public static string deviceId = "0";
        public static string GROUPNAME= "";

        public string remoteAccess_deviceID;
        private static readonly HttpClient client = new HttpClient();

        location loc = new location();
        public static double btnLocationX;
        public static double btnLocationY;
        ComClass comfun = new ComClass();

        public Devices()
        {
            InitializeComponent();
        }
        public Devices(string id, string groupName)
        {
            if (comfun.isConnected())
            {
                try
                {
                    InitializeComponent();
                    AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));
                    /*
                    btnLocationX = location.btnMenuLocationX;
                    btnLocationY = location.btnMenuLocationY;
                    //DisplayAlert("loc", "x: " + btnLocationX + " y: " + btnLocationY,"ok");
                    AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));
                    AbsoluteLayout.SetLayoutBounds(backlayout, new Rectangle(0, 0, location.screenX, location.screenY));
                    AbsoluteLayout.SetLayoutBounds(btnlayout, new Rectangle(45, 45, location.screenX, location.screenY));
                    AbsoluteLayout.SetLayoutBounds(mainStack, new Rectangle(btnLocationX, btnLocationY, 45, 45));

                    Menu.ItemTapped += (sender, e) =>
                    {
                        var evnt = (SelectedItemChangedEventArgs)e;
                        string text = (string)evnt.SelectedItem;
                        if (text == "menucircle")
                        {
                            backlayout.IsVisible = true;
                            btnlayout.IsVisible = true;
                        }
                        else if (text == "closecircle")
                        {
                            btnlayout.IsVisible = false;
                            backlayout.IsVisible = false;
                        }
                    };
                    */
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
                DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
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
            Navigation.PushAsync(new Rooms());
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
                        aiDevices.IsRunning = false;
                    }
                    catch { }
                    aiDevices.IsRunning = false;
                }
                catch (Exception e)
                { }
                aiDevices.IsRunning = false;
                udpateDevices();
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
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
            lvDevicesList.SelectedItem = Color.Transparent;
            try
            {
                var index = (lvDevicesList.ItemsSource as ObservableCollection<SetDevicesList>).IndexOf(e.SelectedItem as SetDevicesList);
                var a = new SetDevicesList();
                ObservableCollection<SetDevicesList> dt = new ObservableCollection<SetDevicesList>();
                App.NavigateMasterDetail(new PCHealth(DevicesList[index], DEP_ID, GROUPNAME));
            }
            catch (Exception ex)
            {
                await DisplayAlert("CloudSchool", "Error! " + ex.Message, "ok");
            }
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

        private async void moveDevice_Clicked(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                string device_id = mi.CommandParameter.ToString();
                //  await DisplayAlert("device_id", device_id, "ok");
                App.NavigateMasterDetail(new selectGroup(device_id));
            }
            catch (Exception ers)
            {
                await DisplayAlert("CloudSchool", "Error! " + ers.Message, "ok");
            }

        }
    }

    public class SetDevicesList : INotifyPropertyChanged
    {
        string OnlineChanged = "";
        string OnlineStatusChanged = "";
        public event PropertyChangedEventHandler PropertyChanged;

        public string name { get; set; }
        public string deviceID { get; set; }
        public string icon { get; set; }

        public string make { get; set; }
        public string model { get; set; }
        public string purchased_on { get; set; }
        public string operating_system { get; set; }

        public string online_status
        {
            get
            {
                return OnlineStatusChanged;
            }
            set
            {
                OnlineStatusChanged = value;
                OnPropertyChanged("online_status");
            }
        }

        public string online
        {
            get
            {
                return OnlineChanged;
            }

            set
            {
                OnlineChanged = value;
                OnPropertyChanged("online");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}