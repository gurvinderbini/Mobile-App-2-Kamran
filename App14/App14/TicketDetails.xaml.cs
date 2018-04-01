using App14.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SlideOverKit;
using App14.iOS.RightSideMenu;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TicketDetails :MenuContainerPage 
    {
        public string device_id;
        public string tick_id;
        private string timeSheet_id;
        private string status_id;
        private string tick_sumary;
        private string email_tick;
        private string tick_name;

        location loc = new location();
        ComClass comfun = new ComClass();
        public static double btnLocationX;
        public static double btnLocationY;

        public static ObservableCollection<TicketDetailLv> dt;

        public TicketDetails(string ticket_id)
        {
            InitializeComponent();
            //DisplayAlert("index ticket id", ticket_id + " more context action", "OK");
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY - 25));
            this.SlideMenu = new RightSideMasterPage2();
            /*
             * 
            btnLocationX = location.btnMenuLocationX;
            btnLocationY = location.btnMenuLocationY;
            //DisplayAlert("loc", "x: " + btnLocationX + " y: " + btnLocationY,"ok");
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY -25));
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
                lvTickeDetail.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                    // don't do anything if we just de-selected the row
                    if (e.Item == null) return;
                    // do something with e.SelectedItem
                    ((ListView)sender).SelectedItem = null; // de-select the row
                };

                dt = new ObservableCollection<TicketDetailLv>();
                lvTickeDetail.ItemsSource = dt;


                tick_id = ticket_id;
                GetTicketDetails(ticket_id);
                getTimeSheetID(ticket_id);

            }
            catch { }
        }
        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new Tickets());
            base.OnBackButtonPressed();
            return true;
        }

        private void timer(double time)
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                time += 1;
                TimeSpan t = TimeSpan.FromSeconds(time);
                lblTimer.Text = String.Format("{0:D2}H : {1:D2}M : {2:D2}S", t.Hours, t.Minutes, t.Seconds);
                if (time == 0.10)
                {
                    DisplayAlert("Süre Doldu", "Geri sayım süresi bitti!", "Ok");
                    return false;
                }

                return true;
            });
        }
        private bool _isRunning;
        public void RunTimer(double time)
        {

            //  TimeSpan TimeElement = new TimeSpan();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                time += 1;
                TimeSpan t = TimeSpan.FromSeconds(time);
                lblTimer.Text = String.Format("{0:D2}H : {1:D2}M : {2:D2}S", t.Hours, t.Minutes, t.Seconds);
                return _isRunning;
            });
        }

        private async void getTimeSheetID(string ticket_id)
        {
            // getting timesheet id & timesheet status
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.api_url);
                var values = new Dictionary<string, string>();
                values.Add("session_string", App.session_string);
                values.Add("data", "{\"table\": \"timesheet\", \"fields\": \"id,status\"}");
                values.Add("extra", "{\"ticketid\":\"" + ticket_id.ToString() + "\"}");
                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage response = await client.PostAsync("/itcrm/getElements/", content);
                var result = await response.Content.ReadAsStringAsync();
                // await DisplayAlert("getting timesheet result", result.ToString(), "OK");

                TimeSheet timeSheet_status = JsonConvert.DeserializeObject<TimeSheet>(result);
                if (timeSheet_status.status)
                {
                    var detail = timeSheet_status.result[0];
                    timeSheet_id = detail.id; // time sheet id
                    status_id = detail.status; // time sheet status
                    ticketStatus(status_id);
                    getTimeSheetLogs();
                }
            }
            catch (Exception e)
            {
                //await DisplayAlert("Error!", e.Message, "OK");
            }
        }
        //Setting Time Sheet Status
        private void ticketStatus(string ticket_status)
        {
            if (ticket_status == "1")
            {
                btnstart.IsEnabled = true;
                btnstop.IsEnabled = false;
                btnpause.IsEnabled = false;
            }
            else if (ticket_status == "2")
            {
                btnstart.IsEnabled = false;
                btnpause.IsEnabled = true;
                btnstop.IsEnabled = true;
            }
            else if (ticket_status == "3")
            {
                btnpause.IsEnabled = false;
                btnstart.IsEnabled = true;
                btnstop.IsEnabled = true;
            }
            else
            {
                btnstart.IsEnabled = false;
                btnpause.IsEnabled = false;
                btnstop.IsEnabled = false;
            }
        }

        private async void getTimeSheetLogs()
        {
            try
            {
                // getting time of ticket
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.api_url);
                var values = new Dictionary<string, string>();
                values.Add("id", timeSheet_id);
                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage response = await client.PostAsync("/itcrm/getTimeSheetLogs/", content);
                var result = await response.Content.ReadAsStringAsync();
                //  await DisplayAlert("result", result.ToString(), "OK");

                TimeSheetLogs timeSheet_time = JsonConvert.DeserializeObject<TimeSheetLogs>(result);
                if (timeSheet_time.status)
                {
                    var detailTime = timeSheet_time.result;
                    lblTimer.Text = detailTime.ToString(); // writing timesheet time                         
                }
            }
            catch (Exception e)
            {

            }
        }
        private async void updateTimeSheet(string status)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.api_url);
                var values = new Dictionary<string, string>();
                values.Add("id", timeSheet_id.ToString());
                values.Add("data", "{\"status\":\"" + status + "\"}");
                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage response = await client.PostAsync("/itcrm/updateTimeSheet/", content);
                var result = await response.Content.ReadAsStringAsync();
                statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);
                if (chk_status.status)
                {
                    status_id = status;
                    ticketStatus(status_id);
                    getTimeSheetLogs();
                }
                else
                {
                    await DisplayAlert("Error!", "time not submitted, submit again", "ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error! ", ex.Message, "OK");
            }
        }
        private void btnstart_Clicked(object sender, EventArgs e)
        {
            updateTimeSheet("2");
        }
        private void btnpause_Clicked(object sender, EventArgs e)
        {
            updateTimeSheet("3");
        }
        private void btnstop_Clicked(object sender, EventArgs e)
        {
            updateTimeSheet("4");
        }
        private void btnRemote_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.NavigateMasterDetail(new WebViewPage(device_id));
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
            catch { }
        }
        private async void letCSfixIt(bool result)
        {
            try
            {
                if (result)
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);
                    var values = new Dictionary<string, string>();
                    values.Add("id", tick_id); //ticket id
                    values.Add("module", "tickets");
                    values.Add("data", "{\"lcsfi\": \"2\",\"assigned_to\":\"\"}");
                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/updateRecord/", content);
                    var res_result = await response.Content.ReadAsStringAsync();
                    statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(res_result);
                    //await DisplayAlert("status!", chk_status.status.ToString(), "ok");
                    if (chk_status.status)
                    {
                        await DisplayAlert("Status", "CloudSchool fix it has been submitted", "ok");
                    }
                    else
                    {
                        await DisplayAlert("Error!", "CloudSchool fix it not submitted", "ok");
                    }
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Error!", e.Message, "ok");
            }
        }

        private void tckt_edit_Clicked(object sender, EventArgs e)
        {
            Constants.tick_id = tick_id;
            Constants.tick_sumary = tick_sumary;
            Constants.tick_name = tick_name;
            Constants.email_tick = email_tick;
            this.ShowMenu();
            //App.NavigateMasterDetail(new EditTicket(tick_id));
        }
        private void tckt_tesponses_Clicked(object sender, EventArgs e)
        {
            
            this.ShowMenu();
           // App.NavigateMasterDetail(new Responses(tick_id, tick_sumary, tick_name, email_tick));
        }
        private void tckt_timeline_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new Timeline());
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
                    device_id = detail.tickets_device_id_id;

                    dt.Add(new TicketDetailLv()
                    {
                        title = "Name: ",
                        value = detail.tickets_full_name,
                    });
                    dt.Add(new TicketDetailLv()
                    {
                        title = "Email Address: ",
                        value = detail.tickets_email_address
                    });
                    dt.Add(new TicketDetailLv()
                    {
                        title = "Device Name: ",
                        value = detail.tickets_device_id
                    });
                    dt.Add(new TicketDetailLv()
                    {
                        title = "Summary: ",
                        value = detail.tickets_summary
                    });
                    dt.Add(new TicketDetailLv()
                    {
                        title = "Detail: ",
                        value = detail.tickets_detail
                    });
                    dt.Add(new TicketDetailLv()
                    {
                        title = "Source: ",
                        value = detail.tickets_source
                    });
                    dt.Add(new TicketDetailLv()
                    {
                        title = "Due Date: ",
                        value = detail.tickets_due_date
                    });
                    dt.Add(new TicketDetailLv()
                    {
                        title = "Created on: ",
                        value = detail.tickets_created
                    });
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

    }
    public class TicketDetailLv
    {
        public string title { get; set; }
        public string value { get; set; }
    }
    public class SetTicketListDetail
    {
        public string ticket_id { get; set; }
        public string tickets_device_id_id { get; set; }
        public string device_name { get; set; }
        public string ticket_type { get; set; }
        public string ticket_type_id { get; set; }
        public string ticket_fullname { get; set; }
        public string ticket_email { get; set; }
        public string ticket_topic { get; set; }
        public string ticket_status { get; set; }
        public string assigned_to { get; set; }
        public string summary { get; set; }
        public string detail { get; set; }
        public string source { get; set; }
        public string due_date { get; set; }
        public string ticket_created { get; set; }
    }
}