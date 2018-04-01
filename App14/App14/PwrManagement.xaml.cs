using App14.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PwrManagement : ContentPage
    {
        public string deviceID;
        location loc = new location();
        ComClass comfun = new ComClass();
        public static double btnLocationX;
        public static double btnLocationY;

        public PwrManagement(string device_id)
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));
            /*btnLocationX = location.btnMenuLocationX;
            btnLocationY = location.btnMenuLocationY;
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
            try
            {
                deviceID = device_id;

                string time = timePicker.Time.ToString();

                statusPicker.Items.Add("Pending");
                statusPicker.Items.Add("Complete");
                statusPicker.WidthRequest = 20;

                periodPicker.Items.Add("Immedialtely");
                periodPicker.Items.Add("Daily");
                periodPicker.Items.Add("Weekly");
                periodPicker.Items.Add("Monthly");
                periodPicker.Items.Add("Schedule(Once)");

                //  time.Items.Add(currentTime);

                dayPicker.Items.Add("Monday");
                dayPicker.Items.Add("Tuesday");
                dayPicker.Items.Add("Wednesday");
                dayPicker.Items.Add("Thursday");
                dayPicker.Items.Add("Friday");
                dayPicker.Items.Add("Saturday");
                dayPicker.Items.Add("Sunday");

                command.Items.Add("Shutdown");
                command.Items.Add("Restart");

                target.Items.Add("This device only");
                target.Items.Add("Whole Group");
            }
            catch { }
        }

        private void statusPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void periodPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void dayPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void time_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void target_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void command_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void date_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new Warnings());
        }

        private void btnCreate_Clicked(object sender, EventArgs e)
        {
            creatTicket();
        }

        private async void creatTicket()
        {
            if (comfun.isConnected())
            {
                try
                {
                    var statusIndex = statusPicker.SelectedIndex;
                    string status;
                    if (statusIndex.ToString() == "0")
                    {
                        status = "1";
                    }
                    else if (statusIndex.ToString() == "1")
                    {
                        status = "2";
                    }
                    else
                    {
                        status = "1";
                    }

                    var periodIndex = periodPicker.SelectedIndex;
                    string period;
                    if (periodIndex.ToString() == "0")
                    {
                        period = "1";
                    }
                    else if (periodIndex.ToString() == "1")
                    {
                        period = "2";
                    }
                    else if (periodIndex.ToString() == "2")
                    {
                        period = "3";
                    }
                    else if (periodIndex.ToString() == "3")
                    {
                        period = "4";
                    }
                    else if (periodIndex.ToString() == "4")
                    {
                        period = "5";
                    }
                    else
                    {
                        period = "1";
                    }

                    var date = datePicker.Date.ToString("yyyy-MM-dd");

                    date = date.Replace("12:00:00 AM", "");

                    var dayIndex = dayPicker.SelectedIndex;
                    string day;
                    if (dayIndex.ToString() == "0")
                    {
                        day = "1";
                    }
                    else if (dayIndex.ToString() == "1")
                    {
                        day = "2";
                    }
                    else if (dayIndex.ToString() == "2")
                    {
                        day = "3";
                    }
                    else if (dayIndex.ToString() == "3")
                    {
                        day = "4";
                    }
                    else if (dayIndex.ToString() == "4")
                    {
                        day = "5";
                    }
                    else if (dayIndex.ToString() == "5")
                    {
                        day = "6";
                    }
                    else if (dayIndex.ToString() == "6")
                    {
                        day = "7";
                    }
                    else
                    {
                        day = "1";
                    }

                    var time = timePicker.Time.ToString();

                    var cmdIndex = command.SelectedIndex;
                    string cmd;
                    if (cmdIndex.ToString() == "0")
                    {
                        cmd = "2";
                    }
                    else if (cmdIndex.ToString() == "1")
                    {
                        cmd = "3";
                    }
                    else
                    {
                        cmd = "2";
                    }

                    var trgtIndex = target.SelectedIndex;
                    string trgt;
                    if (trgtIndex.ToString() == "0")
                    {
                        trgt = "1";
                    }
                    else if (trgtIndex.ToString() == "1")
                    {
                        trgt = "2";
                    }
                    else
                    {
                        trgt = "1";
                    }

                    try
                    {
                        //submit ticket format
                        var client = new HttpClient();
                        client.BaseAddress = new Uri(App.api_url);
                        var values = new Dictionary<string, string>();

                        values.Add("module", "device_tasks");
                        values.Add("id", "207");
                        values.Add("data", "{\"device_id\":\"" + deviceID.ToString() + "\",\"iscompleted\":\"" + status.ToString() + "\",\"period\":\"" + period.ToString() + "\",\"date\":\"" + date.ToString() + "\",\"day\":\"" + day.ToString() + "\",\"time\":\"" + time.ToString() + "\",\"command\":\"" + cmd.ToString() + "\",\"delivery_method\":\"" + trgt.ToString() + "\"}");
                        //"[{\"tickets_type\": \"1\",\"topic_id\":\"" + selectedId.ToString() + "\",\"notify_user\": \"1\",\"source\": \"5\",\"full_name\":\"" + name + "\",\"email_address\":\"" + email + "\",\"device_id\":\"" + deviceID + "\",\"summary\":\"" + sumry + "\",\"detail\":\"" + detal + "\",\"status\": \"1\"}]"
                        var content = new FormUrlEncodedContent(values);
                        HttpResponseMessage response = await client.PostAsync("/itcrm/addRecord/", content);

                        var result = await response.Content.ReadAsStringAsync();
                        //await DisplayAlert("result!", result, "ok");
                        statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);
                        if (chk_status.status)
                        {
                            try
                            {
                                await DisplayAlert("Status", "Task Added", "ok");
                                App.NavigateMasterDetail(new Warnings());
                            }
                            catch { }
                        }
                        else
                        {
                            await DisplayAlert("Error!", "Task not Added", "ok");
                        }
                    }
                    catch { }
                }
                catch (Exception e)
                {
                    await DisplayAlert("Error!", "Task could not be submiited due to some technicle issue, " + e.Message, "ok");
                }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }
    }
}