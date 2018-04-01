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

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InstallSoftware : ContentPage
    {
        ComClass comfun = new ComClass();
        public string device_id;
        location loc = new location();
        public static double btnLocationX;
        public static double btnLocationY;
        public InstallSoftware(string deviceID)
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));
            /*
             btnLocationX = location.btnMenuLocationX;
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
                device_id = deviceID;
                statusPicker.Items.Add("Pending");
                statusPicker.Items.Add("Complete");
                statusPicker.WidthRequest = 20;

                periodPicker.Items.Add("Immedialtely");
                periodPicker.Items.Add("Daily");
                periodPicker.Items.Add("Weekly");
                periodPicker.Items.Add("Monthly");
                periodPicker.Items.Add("Schedule(Once)");
                
                dayPicker.Items.Add("Monday");
                dayPicker.Items.Add("Tuesday");
                dayPicker.Items.Add("Wednesday");
                dayPicker.Items.Add("Thursday");
                dayPicker.Items.Add("Friday");
                dayPicker.Items.Add("Saturday");
                dayPicker.Items.Add("Sunday");

                targetPicker.Items.Add("This device only");
                targetPicker.Items.Add("Whole Group");

                try
                {
                    x86URL.Unfocused += (object sender, FocusEventArgs e) =>
                    {
                        checkURLx86();
                    };
                }
                catch { }
                try
                {
                    x64URL.Unfocused += (object sender, FocusEventArgs e) =>
                    {
                        checkURLx64();
                    };
                }
                catch { }
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
        private void targetPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public async void checkURLx86()
        {
            string url32 = x86URL.Text;
            try
            {
                if (url32 != null && url32 != "" && url32 != " ")
                {
                    try
                    {
                        //check url
                        var client = new HttpClient();
                        client.BaseAddress = new Uri(App.api_url);
                        var values = new Dictionary<string, string>();

                        values.Add("url", url32);
                        var content = new FormUrlEncodedContent(values);
                        HttpResponseMessage response = await client.PostAsync("/itcrm/getURLInfo/", content);
                        var result = await response.Content.ReadAsStringAsync();
                        //  await DisplayAlert("result!", result, "ok");

                        checkURLResult chk_urlResult = JsonConvert.DeserializeObject<checkURLResult>(result);
                        if (chk_urlResult.result.status)
                        {
                            // await DisplayAlert("x86URL", chk_urlResult.result.target, "ok");
                            x86URL.Text = chk_urlResult.result.target;
                        }
                        else
                        {
                            //await DisplayAlert("x86URL", "Wrong url", "ok");
                            x86URL.TextColor = Color.Red;
                            x86URL.Focus();
                        }
                    }
                    catch (Exception ex)
                    {
                        x86URL.TextColor = Color.Red;
                        x86URL.Text = "Error on url check, URL is incorrect" + ex.Message;
                        //x86URL.Focus();
                        // await DisplayAlert("Error", "can't check url, due to: =>  " + ex.Message, "ok");
                    }
                }
                else
                {
                    x86URL.Text = "x64URL is Empty";
                    x86URL.TextColor = Color.Red;
                    // await DisplayAlert("warning!", "URL32  is empty", "ok");
                }
            }
            catch (Exception ex)
            {
                x86URL.TextColor = Color.Red;
                x86URL.Text = "Error on url check, " + ex.Message;
                //await DisplayAlert("Error!", "Error on start : " + ex.Message, "ok");
            }
        }
        public async void checkURLx64()
        {
            string url64 = x64URL.Text;
            try
            {
                if (url64 != null && url64 !="" && url64 != " ")
                {
                    try
                    {
                        //check url
                        var client = new HttpClient();
                        client.BaseAddress = new Uri(App.api_url);
                        var values = new Dictionary<string, string>();

                        values.Add("url", url64);
                        var content = new FormUrlEncodedContent(values);
                        HttpResponseMessage response = await client.PostAsync("/itcrm/getURLInfo/", content);
                        var result = await response.Content.ReadAsStringAsync();
                        //  await DisplayAlert("result!", result, "ok");

                        checkURLResult chk_urlResult = JsonConvert.DeserializeObject<checkURLResult>(result);
                        if (chk_urlResult.result.status)
                        {
                            //await DisplayAlert("x64URL", chk_urlResult.result.target, "ok");
                            x64URL.Text = chk_urlResult.result.target;
                        }
                        else
                        {
                            // await DisplayAlert("x64URL", "Wrong url", "ok");
                            x64URL.TextColor = Color.Red;
                            x64URL.Focus();
                        }
                    }
                    catch (Exception ex)
                    {
                        //  await DisplayAlert("Error", "can't check url, due to: =>  " + ex.Message, "ok");
                        x64URL.TextColor = Color.Red;
                        x64URL.Text = "Error on url check, URL is incorrect" + ex.Message;
                        //x64URL.Focus();
                    }
                }
                else
                {
                    // await DisplayAlert("warning!", "URL64  is empty", "ok");
                    x64URL.Text = "x64URL is Empty";
                    x64URL.TextColor = Color.Red;
                    //x64URL.Focus();
                }
            }
            catch (Exception ex)
            {
                // await DisplayAlert("Error!", "Error on start : " + ex.Message, "ok");
                x64URL.TextColor = Color.Red;
                x64URL.Text = "Error on url check, " + ex.Message;
                //x64URL.Focus();
            }
        }
        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new Warnings());
        }
        private void btnAddTask_Clicked(object sender, EventArgs e)
        {
            SubmitAddTask();
        }
        private async void SubmitAddTask()
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

                    var url32bit = x86URL.Text;
                    var url64bit = x64URL.Text;

                    var trgtIndex = targetPicker.SelectedIndex;
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

                        //values.Add("session_string", App.session_string);
                        values.Add("module", "device_tasks");
                        values.Add("id", "207");
                        values.Add("data", "{\"device_id\":\"" + device_id.ToString() + "\",\"iscompleted\":\"" + status.ToString() + "\",\"period\":\"" + period.ToString() + "\",\"date\":\"" + date.ToString() + "\",\"day\":\"" + day.ToString() + "\",\"time\":\"" + time.ToString() + "\",\"command\": \"1\",\"url\":\"" + url64bit.ToString() + "\",\"url0\":\"" + url32bit.ToString() + "\",\"delivery_method\":\"" + trgt.ToString() + "\"}");
                        //"[{\"tickets_type\": \"1\",\"topic_id\":\"" + selectedId.ToString() + "\",\"notify_user\": \"1\",\"source\": \"5\",\"full_name\":\"" + name + "\",\"email_address\":\"" + email + "\",\"device_id\":\"" + deviceID + "\",\"summary\":\"" + sumry + "\",\"detail\":\"" + detal + "\",\"status\": \"1\"}]"
                        var content = new FormUrlEncodedContent(values);
                        HttpResponseMessage response = await client.PostAsync("/itcrm/addRecord/", content);
                        var result = await response.Content.ReadAsStringAsync();
                        statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);
                        //await DisplayAlert("status!", chk_status.status.ToString(), "ok");
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
                            await DisplayAlert("Error!", "Task not added", "ok");
                        }
                    }
                    catch { }
                }
                catch (Exception e)
                {
                    await DisplayAlert("Error!", "Task could not be submitted due to some technicle issue, " + e.Message, "ok");
                }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }
    }
    public class ChkURL
    {
        public bool status { get; set; }
        public string target_url { get; set; }
    }

}