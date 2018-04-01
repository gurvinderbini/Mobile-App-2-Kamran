using App14.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Responses : ContentPage
    {
        ComClass comfun = new ComClass();
        public static List<string> iList = new List<string>();
        private string TICKETID;
        private string TICKET_SUMMARY;
        private string TICKET_NAME;
        private string TICKET_EMAIL;

        public Responses(string ticket_id, string tick_summary, string name, string email)
        {
            InitializeComponent();
            try
            {
                TICKETID = ticket_id;
                TICKET_SUMMARY = tick_summary;
                TICKET_NAME = name;
                TICKET_EMAIL = email;
                getResponses(ticket_id);
            }
            catch { }
        }

        private async void getResponses(string ticket_id)
        {
            bool val = true;
            if (comfun.isConnected())
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);

                    var values = new Dictionary<string, string>();
                    values.Add("session_string", App.session_string);
                    values.Add("data", "{\"table\":\"ticket_responses\"}");
                    values.Add("extra", "{\"ticket_id\": \"" + ticket_id + "\"}");
                    values.Add("global", val.ToString());

                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/getElements/", content);
                    var result = await response.Content.ReadAsStringAsync();
                    statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);
                    if (chk_status.status)
                    {
                        responses response_list = JsonConvert.DeserializeObject<responses>(result);
                        ObservableCollection<SetResponseList> dt = new ObservableCollection<SetResponseList>();

                        var lst = response_list.result;
                        for (int i = 0; i < lst.Length; i++)
                        {
                            // get all response in string array
                            try
                            {
                                iList.Add(lst[i].response_content);
                            }
                            catch (Exception e)
                            {
                                //   await DisplayAlert("Error!", e.Message, "ok");
                            }
                        }
                        for (int i = 0; i <= lst.Length; i++)
                        {
                            try
                            {
                                dt.Add(new SetResponseList()
                                {
                                    full_name = lst[i].full_name,
                                    created = lst[i].created,
                                    response_content = StripHTML(lst[i].response_content),
                                    icon = "https://cloudschool.management/itcrm/media/images/person-flat.png",
                                });
                            }
                            catch (Exception e)
                            {
                                //   await DisplayAlert("Error!", e.Message, "ok");
                            }
                        }
                        lvResponseList.ItemsSource = dt;
                    }
                    else
                    {
                        // await DisplayAlert("status!", "false", "ok");
                    }
                }
                catch (Exception e)
                {
                    //await DisplayAlert("Response Error!", "", "ok");
                }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }
        public string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }        

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new TicketDetails(TICKETID));
            base.OnBackButtonPressed();
            return true;
        }
        
        private async void btnSend_Clicked(object sender, EventArgs e)
        {
            if (comfun.isConnected())
            {
                btnSend.IsEnabled = false;
                string res_text = entryResp.Text;
                string data = "\"<div class=\"row\"\">\"<\"div class=\"col - sm - 12\"\">" + res_text + "\"</div\">\"<\"div class=\"col - sm - 12\"\">\"<\"ul class=\"list - group\"\">\"</ul\">\"</div\">\"</div\">";
                //await DisplayAlert("Data", data, "ok");
                string res_tex_html = res_text;//"<div class=\"row\"><div class=\"col - sm - 12\">" + res_text + "</div><div class=\"col - sm - 12\"><ul class=\"list - group\"></ul></div></div>";
                string user_name = App.logged_user_name;
                string user_id = App.user_id;
                // await DisplayAlert("Data", res_tex_html + " user name " + user_name + " user id " + user_id + " tick id " + TICKETID, "ok");
                if (res_text != "" && res_text != " ")
                {
                    try
                    {
                        var client = new HttpClient();
                        client.BaseAddress = new Uri(App.api_url);
                        var values = new Dictionary<string, string>();
                        values.Add("user_id", App.user_id);
                        values.Add("id", "175");
                        values.Add("module", "ticket_responses");
                        values.Add("data", "{\"user_id\": \"" + user_id + "\",\"ticket_id\": \"" + TICKETID + "\",\"full_name\": \"" + user_name + "\",\"response_content\": \"" + res_tex_html + "\"}");

                        var content = new FormUrlEncodedContent(values);
                        HttpResponseMessage response = await client.PostAsync("/itcrm/addRecord/", content);
                        var result = await response.Content.ReadAsStringAsync();
                        //  await DisplayAlert("result!", result, "ok");
                        statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);

                        if (chk_status.status)
                        {
                            //App.NavigateMasterDetail(new Responses(TICKETID, TICKET_SUMMARY, TICKET_NAME, TICKET_NAME));
                            string description = "A response has been added by " + user_name;
                            var client2 = new HttpClient();
                            client2.BaseAddress = new Uri(App.api_url);
                            var values2 = new Dictionary<string, string>();
                            values2.Add("user_id", App.user_id);
                            values2.Add("id", "208");
                            values2.Add("module", "timeline");
                            values2.Add("data", "{\"record_id\": \"" + TICKETID + "\",\"description\": \"" + description + "\"}");
                            var content2 = new FormUrlEncodedContent(values2);
                            HttpResponseMessage response2 = await client2.PostAsync("/itcrm/addRecord/", content2);
                            var result2 = await response2.Content.ReadAsStringAsync();
                            ///  await DisplayAlert("result!", result2, "ok");
                            statusCheck chk_status2 = JsonConvert.DeserializeObject<statusCheck>(result2);

                            if (chk_status2.status)
                            {
                                // App.NavigateMasterDetail(new Responses(TICKETID, TICKET_SUMMARY, TICKET_NAME, TICKET_NAME));
                                // send email
                                string subject = "Reply: Ticket # " + TICKETID;
                                string body = "From: " + user_name + " Ticket Summary: " + TICKET_SUMMARY + " Reply: " + res_tex_html; //@" < h4><b>From: </b>" + user_name + "</h4><p><b>Ticket Summary:</b>" + TICKET_SUMMARY + "</p><p><b>Reply:</b>" + res_tex_html + "</p>";
                                var client3 = new HttpClient();
                                client3.BaseAddress = new Uri(App.api_url);
                                var values3 = new Dictionary<string, string>();
                                values3.Add("subject", subject);
                                values3.Add("recipients", "{\"" + TICKET_EMAIL + "\":\"" + TICKET_NAME + "\"}");
                                values3.Add("body", body);
                                var content3 = new FormUrlEncodedContent(values3);
                                HttpResponseMessage response3 = await client3.PostAsync("/itcrm/addRecord/", content3);
                                var result3 = await response3.Content.ReadAsStringAsync();
                                //   await DisplayAlert("result!", result3, "ok");
                                App.NavigateMasterDetail(new Responses(TICKETID, TICKET_SUMMARY, TICKET_NAME, TICKET_NAME));

                            }
                            else { }
                        }
                        else
                        {
                            //await DisplayAlert("Error!", "Task not Added", "ok");
                        }
                    }
                    catch { }
                }
                else
                {
                }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }
    }
    public class SetResponseList
    {
        public string id { get; set; }
        public string ticket_id { get; set; }
        public string user_id { get; set; }
        public string full_name { get; set; }
        public string response_content { get; set; }
        public string response_files { get; set; }
        public string created_by { get; set; }
        public string created { get; set; }
        public string tenant_id { get; set; }
        public string icon { get; set; }
    }
}