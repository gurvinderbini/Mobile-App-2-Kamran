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
    public partial class TicketCreate : ContentPage
    {
        public string deviceID;
        ComClass comfun = new ComClass();
        public List<string> list = new List<string>();
        public Array helpTopicsIds;
        location loc = new location();
        public static double btnLocationX;
        public static double btnLocationY;

        public TicketCreate()
        {
            InitializeComponent();
        }
        public TicketCreate(string device_id)
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));

            /*btnLocationX = location.btnMenuLocationX;
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
                deviceID = device_id;
                userID.Text = device_id;
                userID.IsEnabled = false;
                GetTicketTopics();
            }
            catch { }
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
            App.NavigateMasterDetail(new Warnings());
        }
        private void btnCreate_Clicked(object sender, EventArgs e)
        {
            try
            {
                submitTicket();
            }
            catch
            { }
        }
        private void Pickertopic_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void submitTicket()
        {
            try
            {
                string dev_id = userID.Text;
                string name = Name.Text;
                string email = emailID.Text;
                string issue_summary = summary.Text;
                string issue_detail = detail.Text;
                string topic = list[Pickertopic.SelectedIndex];
                if (comfun.checkText(name) && comfun.checkText(issue_summary) && comfun.checkText(issue_detail) && comfun.checkText(topic.ToString()) && comfun.checkText(deviceID) && comfun.checkText(email))
                {
                    if (comfun.IsValidEmailId(email))
                    {
                        submit(topic, name, email, deviceID, issue_summary, issue_detail);
                    }
                    else
                    {
                        await DisplayAlert("CloudSchool", "Invalid Email!!!", "Ok");
                        emailID.Focus();
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
            //submit ticket format
            if (comfun.isConnected())
            {
                try
                {
                    //submit ticket format
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);
                    var values = new Dictionary<string, string>();

                    values.Add("session_string", App.session_string);
                    values.Add("module", "tickets");
                    values.Add("id", "169");
                    values.Add("data", "{\"type\": \"1\",\"topic\":\"" + topic.ToString() + "\",\"notify_user\": \"1\",\"source\": \"5\",\"full_name\":\"" + name + "\",\"email_address\":\"" + email + "\",\"device_id\":\"" + deviceID + "\",\"summary\":\"" + issue_summary + "\",\"detail\":\"" + issue_detail + "\",\"status\": \"1\"}");
                    //"[{\"tickets_type\": \"1\",\"topic_id\":\"" + selectedId.ToString() + "\",\"notify_user\": \"1\",\"source\": \"5\",\"full_name\":\"" + name + "\",\"email_address\":\"" + email + "\",\"device_id\":\"" + deviceID + "\",\"summary\":\"" + sumry + "\",\"detail\":\"" + detal + "\",\"status\": \"1\"}]"
                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/addRecord/", content);

                    var result = await response.Content.ReadAsStringAsync();
                    statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);
                    if (chk_status.status)
                    {
                        await DisplayAlert("Status", "Ticket has been submitted", "ok");
                        App.NavigateMasterDetail(new Warnings());
                    }
                    else
                    {
                        await DisplayAlert("Error!", "Ticket not submitted, Please try again", "ok");
                    }
                }
                catch (Exception e)
                {
                    await DisplayAlert("CLoudSchool", "Ticket not submitted, Please try again Error : " + e.Message, "Ok");
                }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }
    }

    public class SetTicketTopic
    {
        public string topic_id;
        public string ticket_topic;
    }
}