using App14.Models;
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
using SlideOverKit;
using App14.iOS.RightSideMenu;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WarningDetail : MenuContainerPage
    {
        public string device_id;
        private string WARNINGID = "";
        public string warning_detail = "";
        public static ObservableCollection<WarningDetailLv> dt;

        location loc = new location();
        ComClass comfun = new ComClass();
        public static double btnLocationX;
        public static double btnLocationY;
        public WarningDetail(string warning_id)
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY - 25));
            this.SlideMenu = new RightSideMasterPage3();

            /*btnLocationX = location.btnMenuLocationX;
            btnLocationY = location.btnMenuLocationY;
            //DisplayAlert("loc", "x: " + btnLocationX + " y: " + btnLocationY,"ok");
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY - 25));
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
            lvWarningDetail.ItemTapped += (object sender, ItemTappedEventArgs e) =>
            {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                ((ListView)sender).SelectedItem = null; // de-select the row
            };

            dt = new ObservableCollection<WarningDetailLv>();
            lvWarningDetail.ItemsSource = dt;
            try
            {
                WARNINGID = warning_id;
                GetWarningDetails(warning_id);
            }
            catch { }
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            this.ShowMenu();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new Warnings());
            base.OnBackButtonPressed();
            return true;
        }

        private void btnRemote_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new WebViewPage(device_id));
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

        private async void letCSfixIt(bool result)
        {
            try
            {
                if (result)
                {
                    var email = App.user_email;
                    var user_name = App.logged_user_name;
                    var summary = "Device #" + device_id.ToString() + " Problem";
                    // await DisplayAlert("detail", "email : " + email + " user name " + user_name + " summary " + summary , "ok");
                    if (warning_detail == "")
                    {
                        warning_detail = "No warning in device #" + device_id;
                    }
                    else
                    {
                        warning_detail = warning_detail + ", issues found in device #" + device_id;
                    }
                    //await DisplayAlert("warning_detail", warning_detail, "ok");
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);
                    var values = new Dictionary<string, string>();
                    values.Add("session_string", App.session_string);
                    values.Add("module", "tickets");
                    values.Add("id", "169");
                    values.Add("data", "{\"type\": \"1\",\"notify_user\": \"1\",\"source\": \"5\",\"lcsfi\": \"2\",\"full_name\":\"" + user_name + "\",\"email_address\":\"" + email + "\",\"device_id\":\"" + device_id + "\",\"summary\":\"" + summary + "\",\"detail\":\"" + warning_detail + "\",\"status\": \"1\"}");
                  //  values.Add("data", "{\"type\": \"1\",\"notify_user\": \"1\",\"source\": \"5\",\"lcsfi\": \"2\",\"full_name\": \"Mk\",\"email_address\": \"muhammad.musa@creativerays.com\",\"device_id\":\"" + device_id + "\",\"summary\": \"Checking issue summary\",\"detail\": \"issue detail\",\"status\": \"1\"}");
                    await DisplayAlert("array", "{\"type\": \"1\",\"notify_user\": \"1\",\"source\": \"5\",\"lcsfi\": \"2\",\"full_name\":\"" + user_name.ToString() + "\",\"email_address\":\"" + email.ToString() + "\",\"device_id\":\"" + device_id.ToString() + "\",\"summary\":\"" + summary.ToString() + "\",\"detail\": \"issue detail\",\"status\": \"1\"}", "OK");
                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/addRecord/", content);
                    var res_result = await response.Content.ReadAsStringAsync();
                    // await DisplayAlert("res_result!", res_result.ToString(), "ok");
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

        private void war_tckt_create_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.NavigateMasterDetail(new TicketCreate(device_id));
            }
            catch { }
        }

        private void war_inst_sw_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.NavigateMasterDetail(new InstallSoftware(device_id));
            }
            catch { }
        }

        private void war_pwr_mgt_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.NavigateMasterDetail(new PwrManagement(device_id));
            }
            catch { }
        }

        private async void war_markas_fix_Clicked(object sender, EventArgs e)
        {
            bool val = true;
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.api_url);
                var values = new Dictionary<string, string>();
                values.Add("session_string", App.session_string);
                values.Add("id", WARNINGID);
                values.Add("module", "warnings");
                values.Add("data", "{\"has_warning\":0,\"fix_counter\":\"IFNULL(`fix_counter`, 0) + 1\", \"warnings_fix_count\": \"IFNULL(`warnings_fix_count`, 0) + `warnings_count`\",\"warnings_count\":0}");
                values.Add("method", "set");
                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage response = await client.PostAsync("/itcrm/updateRecordI/", content);
                var result = await response.Content.ReadAsStringAsync();
                //await DisplayAlert("result!", result, "ok");
                statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);
                if (chk_status.status)
                {
                    try
                    {
                        await DisplayAlert("Status", "warning fixed", "ok");
                        App.NavigateMasterDetail(new Warnings());
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        await DisplayAlert("Error!", "warning not fix, please try again", "ok");
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Response Error!", ex.Message, "ok");
            }
        }

        private async void GetWarningDetails(string warning_id)
        {
            if (comfun.isConnected())
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.api_url);
                var values = new Dictionary<string, string>();
                values.Add("operation", "query");
                values.Add("session_string", App.session_string);
                values.Add("query", "select * from warnings where id = " + warning_id);
                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage response = await client.PostAsync("/itcrm/webservices/", content);
                var result = await response.Content.ReadAsStringAsync();
                //await DisplayAlert("Info!", result, "ok");
                WarningsResult warningDetail = JsonConvert.DeserializeObject<WarningsResult>(result);

                var detail = warningDetail.result[0];
                device_id = detail.warnings_device_id;
                // Device name
                try
                {
                    var clientsID = new HttpClient();
                    clientsID.BaseAddress = new Uri(App.api_url);
                    var valuesID = new Dictionary<string, string>();
                    valuesID.Add("session_string", App.session_string);
                    valuesID.Add("data", "{\"table\": \"devices\"}");
                    valuesID.Add("extra", "{\"id\": \"" + device_id + "\"}");
                    var contentID = new FormUrlEncodedContent(valuesID);
                    HttpResponseMessage responseID = await clientsID.PostAsync("/itcrm/getElements/", contentID);

                    var resultID = await responseID.Content.ReadAsStringAsync();
                    // await DisplayAlert("Alert", "You have been alerted" + resultID, "OK");
                    HealthResult health_list = JsonConvert.DeserializeObject<HealthResult>(resultID);
                    var detailID = health_list.result[0];
                    //lblWarningID.Text = detailID.name;
                    dt.Add(new WarningDetailLv()
                    {
                        title = "Name: ",
                        value = detailID.name
                    });
                }
                catch (Exception e)
                {
                    //    await DisplayAlert("Error!", e.Message, "ok");
                }
                var lst = warningDetail.result[0];
                var warning_list = lst.warnings_warnings;
                // OS
                try
                {
                    var opSystem = warning_list.os;
                    bool os_status = opSystem.status;
                    if (os_status)
                    {
                        //lblWarningOS.Text = opSystem.warning;
                        dt.Add(new WarningDetailLv()
                        {
                            title = "Operating System: ",
                            value = opSystem.warning
                        });
                        warning_detail = warning_detail + opSystem.warning + ", ";
                    }
                    else
                    {
                        //lblWarningOS.Text = "No errors found";
                    }
                }
                catch (Exception r)
                {
                    //lblWarningOS.Text = "Something went wrong";
                }
                // hard disk
                try
                {
                    var hardDisk = warning_list.hdd_status;
                    var hd_status = hardDisk.status;
                    if (hd_status)
                    {
                        //lblWarningHDDStatus.Text = hardDisk.warning;
                        dt.Add(new WarningDetailLv()
                        {
                            title = "HDD Status: ",
                            value = hardDisk.warning
                        });
                        warning_detail = warning_detail + hardDisk.warning + ", ";
                    }
                    else
                    {
                        //lblWarningHDDStatus.Text = "No errors found";
                    }
                }
                catch (Exception w)
                {
                    //lblWarningHDDStatus.Text = "Something went wrong";
                }
                // temp
                try
                {
                    var temperature = warning_list.temperature;
                    bool temp_status = temperature.status;
                    if (temp_status)
                    {
                        //lblWarningTemperature.Text = temperature.warning;
                        dt.Add(new WarningDetailLv()
                        {
                            title = "Temprature: ",
                            value = temperature.warning
                        });
                        warning_detail = warning_detail + temperature.warning + ", ";
                    }
                    else
                    {
                        //lblWarningTemperature.Text = "No errors found";
                    }
                }
                catch (Exception r)
                {
                    //lblWarningTemperature.Text = "Something went wrong";
                }
                // ram
                try
                {
                    var memory = warning_list.ram;
                    bool ram_status = memory.status;
                    if (ram_status)
                    {
                        dt.Add(new WarningDetailLv()
                        {
                            title = "Momeory: ",
                            value = memory.warning
                        });
                        warning_detail = warning_detail + memory.warning + ", ";
                    }
                    else
                    {
                        //lblWarningRAM.Text = "No errors found";
                    }
                }
                catch (Exception r)
                {
                    //lblWarningRAM.Text = "Something went wrong";
                }
                // cpu
                try
                {
                    var cpu_use = warning_list.cpu;
                    bool cpu_status = cpu_use.status;
                    if (cpu_status)
                    {
                        //lblWarningCPUUsage.Text = cpu_use.warning;
                        dt.Add(new WarningDetailLv()
                        {
                            title = "CPU: ",
                            value = cpu_use.warning
                        });
                        warning_detail = warning_detail + cpu_use.warning + ", ";
                    }
                    else
                    {
                        //lblWarningCPUUsage.Text = "No errors found";
                    }
                }
                catch (Exception r)
                {
                    //lblWarningCPUUsage.Text = "Something went wrong";
                }
                // antivirus
                try
                {
                    var antivirus = warning_list.antivirus;
                    bool av_status = antivirus.status;
                    if (av_status)
                    {
                        //lblWarningAntivirus.Text = antivirus.warning;
                        dt.Add(new WarningDetailLv()
                        {
                            title = "Antivirus: ",
                            value = antivirus.warning
                        });
                        warning_detail = warning_detail + antivirus.warning + " ";
                    }
                    else
                    {
                        //lblWarningAntivirus.Text = "No errors found";
                    }
                }
                catch (Exception r)
                {
                    //lblWarningAntivirus.Text = "Something went wrong";
                }
                // created on 
                dt.Add(new WarningDetailLv()
                {
                    title = "Created on: ",
                    value = detail.warnings_created
                });
                activityIndicator.IsRunning = false;
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }
        // later work on it
        public async void test_fun(string warning_id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.api_url);
                var values = new Dictionary<string, string>();
                values.Add("session_string", App.session_string);
                values.Add("data", "{\"table\":\"warnings\",\"fields\":\"id,modified as date,warnings\"}");
                values.Add("extra", "{\"id\":\"" + warning_id + "\"}");
                values.Add("join", "[{\"table\":\"devices\",\"fields\":\"name\",\"where\":\"id\",\"is\":\"warnings.device_id\"}]");
                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage response = await client.PostAsync("/itcrm/getElements/", content);
                var result = await response.Content.ReadAsStringAsync();
                await DisplayAlert("result!", result, "ok");
                WarningsResult warningDetail = JsonConvert.DeserializeObject<WarningsResult>(result);
                var lst = warningDetail.result[0];
                var warning_list = lst.warnings_warnings;
                await DisplayAlert("warning_list!", warning_list.ToString(), "ok");
                // OS
                try
                {
                    var opSystem = warning_list.os;
                    bool os_status = opSystem.status;
                    if (os_status)
                    {
                        //lblWarningOS.Text = opSystem.warning;
                        dt.Add(new WarningDetailLv()
                        {
                            title = "Operating System",
                            value = opSystem.warning
                        });
                        warning_detail = warning_detail + opSystem.warning + ", ";
                    }
                    else
                    {
                        //lblWarningOS.Text = "No errors found";
                    }
                }
                catch (Exception r)
                {
                    await DisplayAlert("OS!", r.Message.ToString(), "ok");
                    //lblWarningOS.Text = "Something went wrong";
                }
            }
            catch (Exception r)
            {
                await DisplayAlert("whole!", r.Message.ToString(), "ok");
                //lblWarningOS.Text = "Something went wrong";
            }

        }
        public class WarningDetailLv
        {
            public string title { get; set; }
            public string value { get; set; }
        }
        public class SetWarningsDetailList
        {
            public string id { get; set; }
            public string antivirus { get; set; }
            public string temperature { get; set; }
            public string hdd_status { get; set; }
            public string os { get; set; }
            public string ram { get; set; }
            public string createDate { get; set; }
            public string cpu_use { get; set; }
            public string name { get; set; }
            public string date { get; set; }
        }
    }
}
