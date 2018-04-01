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
using static App14.TicketCreate;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewTicket : ContentPage
    {
        public List<string> list = new List<string>();
        ComClass comfun = new ComClass();
        location loc = new location();
        public static double btnLocationX;
        public static double btnLocationY;
        public static string DEVICE_ID = "";

        public AddNewTicket()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));
            //DisplayAlert("DEVICE_ID", "DEVICE_ID : " + DEVICE_ID + "YES", "OK");
            if (DEVICE_ID == "")
            {
                userID.Text = "";
            }
            else
            {
                userID.Text = DEVICE_ID;
            }
            try
            {
                //lblDevice.GestureRecognizers.Add(new TapGestureRecognizer
                //{
                //    Command = new Command(async () => {
                //        await Navigation.PushAsync(new TicketRoomSelect());
                //    })
                //});
            }
            catch (Exception e)
            {
                DisplayAlert("CloudShool", "Another Account Message : " + e.Message, "OK");
            }
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
            try
            {
                lblDevice.Items.Add("Device");
                lblDevice.Items.Add("Other");
            }
            catch { }
            try
            {
                GetTicketTopics();
            }
            catch
            { }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new Tickets());
            base.OnBackButtonPressed();
            return true;
        }

        private async void GetTicketTopics()
        {
            if (comfun.isConnected())
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);
                    var values = new Dictionary<string, string>();
                    values.Add("session_string", App.session_string);
                    values.Add("data", "{\"table\":\"ticket_topics\",\"fields\":\"id,topic_title\"}");
                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/getElements/", content);
                    var result = await response.Content.ReadAsStringAsync();
                    //  await DisplayAlert("result!", result, "ok");
                    TicketTopic topic_list = JsonConvert.DeserializeObject<TicketTopic>(result);
                    ObservableCollection<SetTicketTopic> dt = new ObservableCollection<SetTicketTopic>();
                    var listview = new ListView();
                    listview.ItemsSource = dt;
                    var lst = topic_list.result;
                    for (int i = 0; i < lst.Length; i++)
                    {
                        try
                        {
                            Pickertopic.Items.Add(lst[i].topic_title);
                            // await DisplayAlert("done!", lst[i].id, "ok");
                            list.Add(lst[i].id);
                        }
                        catch (Exception e)
                        {
                            await DisplayAlert("Error in list!", e.Message, "ok");
                        }
                    }
                }
                catch (Exception e)
                {
                    await DisplayAlert("Error ! ", e.Message, "Ok");
                }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.NavigateMasterDetail(new Tickets());
            }
            catch { }
        }
        private void btnCreate_Clicked(object sender, EventArgs e)
        {
            try
            {
                submitTicket();
            }
            catch { }
        }
        private void Pickertopic_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private async void submitTicket()
        {
            try
            {
                string name = Name.Text;
                string email = emailID.Text;
                // id
                string deviceID = userID.Text;
                string issue_summary = summary.Text;
                string issue_detail = detail.Text;
                string topic = list[Pickertopic.SelectedIndex];
                if (comfun.checkText(name) && comfun.checkText(issue_summary) && comfun.checkText(issue_detail) && comfun.checkText(topic.ToString()) && comfun.checkText(deviceID) && comfun.checkText(email))
                {
                    if (comfun.IsValidEmailId(email)) //  && comfun.NumberText(deviceID)
                    {
                        submit(topic, name, email, deviceID, issue_summary, issue_detail);
                    }
                    else
                    {
                        /*if (!comfun.NumberText(deviceID))
                        {
                            await DisplayAlert("CloudSchool", "Invalid Device ID!!!", "Ok");
                            userID.Focus();
                        }
                        else
                        {*/
                            await DisplayAlert("CloudSchool", "Invalid Email!!!", "Ok");
                            emailID.Focus();
                        //}
                    }
                }
                else
                {
                    await DisplayAlert("CloudSchool", "All fields are necessary, Please complete Credentials!!!", "Ok");
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("CloudSchool", "All fields are necessary, Please complete Credentials!!!", "Ok");
                // await DisplayAlert("Error!", "Ticket not submitted, Please try again later", "ok");
            }
        }

        private async void submit(string topic, string name, string email, string deviceID, string issue_summary, string issue_detail)
        {
            if (comfun.isConnected())
            {
                //submit ticket format
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);
                    var values = new Dictionary<string, string>();
                    values.Add("session_string", App.session_string);
                    values.Add("module", "tickets");
                    values.Add("id", "169");
                    values.Add("data", "{\"type\": \"1\",\"topic\":\"" + topic.ToString() + "\",\"notify_user\": \"1\",\"source\": \"5\",\"full_name\":\"" + name + "\",\"email_address\":\"" + email + "\",\"device_id\":\"" + deviceID.ToString() + "\",\"summary\":\"" + issue_summary.ToString() + "\",\"detail\":\"" + issue_detail.ToString() + "\",\"status\": \"1\"}");

                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/addRecord/", content);
                    var result = await response.Content.ReadAsStringAsync();
                    // await DisplayAlert("result!", result, "ok");
                    statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);
                    //  await DisplayAlert("status!", chk_status.status.ToString(), "ok");
                    if (chk_status.status)
                    {
                        await DisplayAlert("Status", "Ticket has been submitted", "ok");
                        DEVICE_ID = "";
                        App.NavigateMasterDetail(new Tickets());
                    }
                    else
                    {
                        await DisplayAlert("Error!", "Ticket not submitted", "ok");
                    }
                }
                catch (Exception e)
                {
                    await DisplayAlert("CLoudSchool", "Ticket not submitted, Please try again Error : " + e.Message, "Ok");
                }
            }
            else
            {
                await DisplayAlert("Connection", "Ticket could not be submit, Please check Internet Connection...", "Ok");
            }
        }

        private async void lblDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedValue = lblDevice.Items[lblDevice.SelectedIndex];
            if (selectedValue.ToString() == "Device")
            {
                await Navigation.PushAsync(new TicketRoomSelect());
            }
            else
            {
                userID.Text = selectedValue.ToString();
            }
        }
    }
}