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
    public partial class EditTicket : ContentPage
    {
        public string status;
        public string ticket_id;
        location loc = new location();
        public static double btnLocationX;
        public static double btnLocationY;
        public List<string> list = new List<string>();
        ComClass comfun = new ComClass();

        public EditTicket(string tick_id)
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY - 25));
            /*
            btnLocationX = location.btnMenuLocationX;
            btnLocationY = location.btnMenuLocationY;
            //DisplayAlert("loc", "x: " + btnLocationX + " y: " + btnLocationY,"ok");
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY - 25));
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
            try
            {
                ticket_id = tick_id;
                GetTicketDetails(tick_id);
                try
                {
                    lblTicketStatus.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            statusGet();
                        })
                    });
                }
                catch { }
                
            }
            catch { }
            
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new TicketDetails(ticket_id));
            base.OnBackButtonPressed();
            return true;
        }

        private async void statusGet()
        {
            try
            {
                var action = await DisplayActionSheet("Ticket Status", "Ok", null, "Open", "Resolved", "Closed");
                lblTicketStatus.Text = action.ToString();
                if (action.ToString() == "Ok")
                {
                    if (action.ToString() == "Open")
                    {
                        status = "1";
                    }
                    else if (action.ToString() == "Resolved")
                    {
                        status = "2";
                    }
                    else
                    {
                        status = "3";
                    }
                }
            }
            catch { }
        }

        private async void GetTicketDetails(string tick_id)
        {
            if (comfun.isConnected())
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);
                    var values = new Dictionary<string, string>();
                    values.Add("operation", "query");
                    values.Add("session_string", App.session_string);
                    // values.Add("query", "select * from tickets where tickets_id = " + tick_id);
                    values.Add("query", "select * from tickets where id = " + tick_id);
                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/webservices/", content);
                    var result = await response.Content.ReadAsStringAsync();
                    //await DisplayAlert("Ticket Detail", result.ToString(), "ok");
                    TicketResult ticketDetail = JsonConvert.DeserializeObject<TicketResult>(result);
                    var detail = ticketDetail.result[0];

                    lblTicketStatus.Text = detail.tickets_status;
                    // DisplayAlert("status", status, "ok");
                    lblTicketNumber.Text = detail.tickets_id;
                    lblTicketSummary.Text = detail.tickets_summary;
                    lblTicketFullName.Text = detail.tickets_full_name;
                    lblTicketEmail.Text = detail.tickets_email_address;
                    lblTicketDevice.Text = detail.tickets_device_id;
                    lblTicketDetail.Text = detail.tickets_detail;
                    lblTicketSource.Text = detail.tickets_source;
                    //lblTicketTopic.Text = detail.tickets_topic;
                    lblTicketDueDate.Text = detail.tickets_due_date;
                    lblTicketCreated.Text = detail.tickets_created;
                    // device_id = detail.tickets_device_id_id;
                    // DisplayAlert("device id", device_id, "ok");
                    activityIndicator.IsRunning = false;
                }
                catch (Exception ex)
                { }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }


        private void PickerTicketStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                submitTicket();
            }
            catch { }
        }

        private async void submitTicket()
        {
            if (comfun.isConnected())
            {
                try
                {
                    string tick_id = lblTicketNumber.Text;
                    string name = lblTicketFullName.Text;
                    string email = lblTicketEmail.Text;
                    string deviceID = lblTicketDevice.Text;
                    string issue_summary = lblTicketSummary.Text;
                    string issue_detail = lblTicketDetail.Text;
                    // string topic = list[Pickertopic.SelectedIndex];
                    string source = lblTicketSource.Text;
                    string tick_status = lblTicketStatus.Text;
                    // string assign = lblAssignTo.Text;
                    //submit ticket format
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);
                    var values = new Dictionary<string, string>();
                    values.Add("id", ticket_id);
                    values.Add("module", "tickets");
                    values.Add("data", "{\"type\": \"1\",\"notify_user\": \"1\",\"source\": \"5\",\"full_name\":\"" + name + "\",\"email_address\":\"" + email + "\",\"device_id\":\"" + deviceID + "\",\"summary\":\"" + issue_summary + "\",\"detail\":\"" + issue_detail + "\",\"status\":\"" + tick_status.ToString() + "\"}");
                    var content = new FormUrlEncodedContent(values);
                    // await    DisplayAlert("info", "{\"type\": \"1\",\"topic\":\"" + topic.ToString() + "\",\"notify_user\": \"1\",\"source\": \"5\",\"full_name\":\"" + name + "\",\"email_address\":\"" + email + "\",\"device_id\":\"" + deviceID + "\",\"summary\":\"" + issue_summary + "\",\"detail\":\"" + issue_detail + "\",\"status\":\"" + tick_status.ToString() + "\"}", "ok");
                    HttpResponseMessage response = await client.PostAsync("/itcrm/updateRecord/", content);
                    var result = await response.Content.ReadAsStringAsync();
                    // await DisplayAlert("result!", result, "ok");
                    statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);
                    // await DisplayAlert("status!", chk_status.status.ToString(), "ok");
                    if (chk_status.status)
                    {
                        await DisplayAlert("Status", "Ticket has been Saved", "ok");
                        App.NavigateMasterDetail(new Tickets());
                    }
                    else
                    {
                        await DisplayAlert("Error!", "Ticket not Edited", "ok");
                    }
                }
                catch (Exception e)
                {
                    await DisplayAlert("Error!", "Ticket not Edited, Please try again later", "ok");
                }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }
            /*
                    private async void GetTicketTopics()
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
                            //await DisplayAlert("result!", result, "ok");
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

                    private void Pickertopic_SelectedIndexChanged(object sender, EventArgs e)
                    {
                        DisplayAlert("sd", "sdklInsert", "sskd");
                        lblTicketTopic.Text = Pickertopic.Items[Pickertopic.SelectedIndex];
                    }*/

        }
}