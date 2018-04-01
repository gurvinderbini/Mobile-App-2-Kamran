using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App14.Models;
using System.Collections.ObjectModel;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PCHealth : ContentPage
    {
        location loc = new location();
        ComClass comfun = new ComClass();
        public static double btnLocationX;
        public static double btnLocationY;
        public static string GROUPNAME = "";

        private string deviceId;
        private string DEP_ID;
        public string pc_detail = "";
        public static ObservableCollection<LvPcHealth> dt;

        public PCHealth()
        {
            InitializeComponent();
        }

        public PCHealth(string device_id, string dep_id, string groupName)
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY - 25));
            /*btnLocationX = location.btnMenuLocationX;
            btnLocationY = location.btnMenuLocationY;
            //DisplayAlert("loc", "x: " + btnLocationX + " y: " + btnLocationY,"ok");
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX , location.screenY -25));
            AbsoluteLayout.SetLayoutBounds(backlayout, new Rectangle(0, 0, location.screenX, location.screenY));
            AbsoluteLayout.SetLayoutBounds(btnlayout, new Rectangle(45, 45, location.screenX, location.screenY));
            AbsoluteLayout.SetLayoutBounds(mainStack, new Rectangle(btnLocationX, btnLocationY, 45, 45));

            Menu.ItemTapped += async (sender, e) =>
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

            lvPcHealth.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                ((ListView)sender).SelectedItem = null; // de-select the row
            };

            dt = new ObservableCollection<LvPcHealth>();
            lvPcHealth.ItemsSource = dt;
            try
            {
                deviceId = device_id;
                DEP_ID = dep_id;
                //await DisplayAlert("device_id", device_id, "ok");
                GetPCHealth(device_id);
            }
            catch { }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new Devices(DEP_ID, GROUPNAME));
           // Navigation.PushAsync(new Detail());
            base.OnBackButtonPressed();
            return true;
        }

        private async void GetPCHealth(string device_id)
        {
            if (comfun.isConnected())
            {
                try
                {
                    var clients = new HttpClient();
                    clients.BaseAddress = new Uri(App.api_url);

                    var values = new Dictionary<string, string>();
                    values.Add("session_string", App.session_string);
                    values.Add("data", "{\"table\": \"devices\"}");
                    values.Add("extra", "{\"id\": \"" + device_id + "\"}");

                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await clients.PostAsync("/itcrm/getElements/", content);
                    var result = await response.Content.ReadAsStringAsync();

                    HealthResult health_list = JsonConvert.DeserializeObject<HealthResult>(result);
                    var detail = health_list.result[0];

                    dt.Add(new LvPcHealth()
                    {
                        title = "Device ID: ",
                        value = detail.id
                    });
                    dt.Add(new LvPcHealth()
                    {
                        title = "Name: ",
                        value = detail.name
                    });
                    pc_detail = pc_detail + "Device Name: " + detail.name + ", ";

                    dt.Add(new LvPcHealth()
                    {
                        title = "Make: ",
                        value = detail.make
                    });

                    dt.Add(new LvPcHealth()
                    {
                        title = "HDD Status: ",
                        value = detail.hdd_status
                    });
                    pc_detail = pc_detail + "HDD Status: " + detail.hdd_status + ", ";

                    dt.Add(new LvPcHealth()
                    {
                        title = "RAM: ",
                        value = detail.ram
                    });
                    pc_detail = pc_detail + "RAM: " + detail.ram + ", ";

                    dt.Add(new LvPcHealth()
                    {
                        title = "Processor: ",
                        value = detail.processor
                    });
                    pc_detail = pc_detail + "Processor: " + detail.processor + ", ";

                    dt.Add(new LvPcHealth()
                    {
                        title = "Temp: ",
                        value = detail.temperature
                    });
                    pc_detail = pc_detail + "Temprature: " + detail.temperature + ", ";

                    dt.Add(new LvPcHealth()
                    {
                        title = "Antivirus: ",
                        value = detail.antivirus
                    });
                    pc_detail = pc_detail + "Antivirus: " + detail.antivirus + ", ";

                    dt.Add(new LvPcHealth()
                    {
                        title = "OS Installed: ",
                        value = detail.operating_system
                    });
                    pc_detail = pc_detail + "OS Installed: " + detail.operating_system + ", ";

                    dt.Add(new LvPcHealth()
                    {
                        title = "CPU Usage: ",
                        value = detail.cpu_usage
                    });
                    pc_detail = pc_detail + "CPU Usage: " + detail.cpu_usage;
                    activityIndicator.IsRunning = false;
                }
                catch (Exception e)
                {
                    //    await DisplayAlert("Error!", e.Message, "ok");
                }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }

        private async void letCSfixIt(bool result)
        {
            if (comfun.isConnected())
            {
                try
                {
                    if (result)
                    {
                        var email = App.user_email;
                        var user_name = App.logged_user_name;
                        var summary = "Device #" + deviceId.ToString() + " Health";
                        await DisplayAlert("detail", "email : " + email + " user name " + user_name + " summary " + summary, "ok");
                        if (pc_detail != "")
                        {
                            pc_detail = pc_detail + ", Details of device #" + deviceId;
                        }
                        await DisplayAlert("pc_detail", pc_detail, "ok");

                        var client = new HttpClient();
                        client.BaseAddress = new Uri(App.api_url);
                        var values = new Dictionary<string, string>();
                        values.Add("session_string", App.session_string);
                        values.Add("module", "tickets");
                        values.Add("id", "169");
                        values.Add("data", "{\"type\": \"1\",\"notify_user\": \"1\",\"source\": \"5\",\"lcsfi\": \"2\",\"full_name\":\"" + user_name + "\",\"email_address\":\"" + email + "\",\"device_id\":\"" + deviceId + "\",\"summary\":\"" + summary + "\",\"detail\":\"" + pc_detail + "\",\"status\": \"1\"}");
                        //  values.Add("data", "{\"type\": \"1\",\"notify_user\": \"1\",\"source\": \"5\",\"lcsfi\": \"2\",\"full_name\": \"Mk\",\"email_address\": \"muhammad.musa@creativerays.com\",\"device_id\":\"" + device_id + "\",\"summary\": \"Checking issue summary\",\"detail\": \"issue detail\",\"status\": \"1\"}");
                        await DisplayAlert("array", "{\"type\": \"1\",\"notify_user\": \"1\",\"source\": \"5\",\"lcsfi\": \"2\",\"full_name\":\"" + user_name.ToString() + "\",\"email_address\":\"" + email.ToString() + "\",\"device_id\":\"" + deviceId.ToString() + "\",\"summary\":\"" + summary.ToString() + "\",\"detail\":\"" + pc_detail + "\",\"status\": \"1\"}", "OK");
                        var content = new FormUrlEncodedContent(values);
                        HttpResponseMessage response = await client.PostAsync("/itcrm/addRecord/", content);
                        var res_result = await response.Content.ReadAsStringAsync();
                        await DisplayAlert("res_result!", res_result.ToString(), "ok");
                        statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(res_result);
                        // await DisplayAlert("status!", chk_status.status.ToString(), "ok");
                        if (chk_status.status)
                        {
                            try
                            {
                                await DisplayAlert("Status", "Ticket has been submitted", "ok");
                            }
                            catch { }
                        }
                        else
                        {
                            try
                            {
                                await DisplayAlert("Error!", "Ticket not submitted", "ok");
                            }
                            catch { }
                        }
                    }
                }
                catch (Exception e)
                {
                    await DisplayAlert("Error!", e.Message, "Ok");
                }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }


        private void btnRemote_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.NavigateMasterDetail(new WebViewPage(deviceId));
            }
            catch { }
        }

        private void tckt_create_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.NavigateMasterDetail(new TicketCreate(deviceId));
            }
            catch { }
        }

        private void inst_sw_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.NavigateMasterDetail(new InstallSoftware(deviceId));
            }
            catch { }
        }

        private void pwr_mgt_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.NavigateMasterDetail(new PwrManagement(deviceId));
            }
            catch { }
        }

        private async void btnCsFix_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool answer = await DisplayAlert("CloudSchool", "Do you want to let cloudSchool fix it!", "Yes", "No");
                letCSfixIt(answer);
            }
            catch
            { }
        }
    }

    public class LvPcHealth
    {
        public string title { get; set; }
        public string value { get; set; }
    }
    public class SetDeviceHealthList
    {
        public string id { get; set; }
        public string device_name { get; set; }
        public string hdd_status { get; set; }
        public string ram { get; set; }
        public string temperature { get; set; }
        public string antivirus { get; set; }
        public string os { get; set; }
        public string cpuUse { get; set; }
        public string processor { get; set; }
    }

}