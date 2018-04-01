using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SlideOverKit;
using App14.iOS.RightSideMenu;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tickets : MenuContainerPage
    {
        location loc = new location();
        ComClass comfun = new ComClass();
        public static double btnLocationX;
        public static double btnLocationY;
        public static string[] colors = new string[] { "#0378B2", "#9D151C", "#31AD31", "#0174CA" };
        private static int count = 0;
        public static List<string> list = new List<string>();
        public string device_id;
        public Tickets()
        {
            InitializeComponent();
            count = 0;
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));
            this.SlideMenu = new RightSideMasterPage1();
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
               GetTickets();
            }
            catch { }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new Detail());
            base.OnBackButtonPressed();
            return true;
        }

        private async void GetTickets()
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
                    values.Add("query", "select * from tickets");
                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/webservices/", content);
                    var result = await response.Content.ReadAsStringAsync();
                    //await DisplayAlert("result!", result.ToString(), "ok");

                    TicketResult tickets_list = JsonConvert.DeserializeObject<TicketResult>(result);
                    ObservableCollection<SetTicketList> dt = new ObservableCollection<SetTicketList>();
                    //lvTicketList.ItemsSource = dt;
                    var lst = tickets_list.result;
                    try
                    {
                        for (int i = 0; i < lst.Length; i++)
                        {
                            try
                            {
                                //    await DisplayAlert("Error!", lst[i].tickets_id, "ok");
                                list.Add(lst[i].tickets_id);
                            }
                            catch (Exception e)
                            {
                                //   await DisplayAlert("Error!", e.Message, "ok");
                            }
                        }
                    }
                    catch { }
                    try
                    {
                        for (int i = 0; i <= lst.Length; i++)
                        {
                            try
                            {
                                dt.Add(new SetTicketList()
                                {

                                    ticket_id = "#" + lst[i].tickets_device_id,
                                    color = colors[count],
                                    summary = lst[i].tickets_summary,
                                    device_name = lst[i].tickets_device_id,
                                    detail = Tickets.TruncateAtWord(lst[i].tickets_detail, 3),
                                    tickets_created = lst[i].tickets_created,
                                    icon = frstWord(lst[i].tickets_detail) //"https://cloudschool.management/itcrm/media/images/tickets.png" 
                                });
                            }

                            catch (Exception e)
                            {
                                //await DisplayAlert("Error!", e.Message, "ok");
                            }
                            if (count > 3)
                            { count = 0; }
                            count++;
                        }
                        aiDevices.IsRunning = false;
                        lvTicketList.ItemsSource = dt;
                    }
                    catch { }
                    aiDevices.IsRunning = false;
                }
                catch { }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }

        public string frstWord(string val)
        {
            char[] a = val.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            string data = a[0].ToString();
            return data;
        }
        private void lvTicketList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lvTicketList.SelectedItem = Color.Transparent;   
            try
            {
                var index = (lvTicketList.ItemsSource as ObservableCollection<SetTicketList>).IndexOf(e.SelectedItem as SetTicketList);
                  //     DisplayAlert("index ticket id", index.ToString()  , "OK");
                var a = new SetTicketList();
                ObservableCollection<SetTicketList> dt = new ObservableCollection<SetTicketList>();
                // DisplayAlert("index ticket id", list[index].ToString() + " more context action", "OK");
                App.NavigateMasterDetail(new TicketDetails(list[index]));
            }
            catch { }
        }

        private void newTicket_Clicked(object sender, EventArgs e)
        {
            this.ShowMenu();
            //App.NavigateMasterDetail(new AddNewTicket());
        }

        private void Details_Clicked(object sender, EventArgs e)
        {

        }

        private void Response_Clicked(object sender, EventArgs e)
        {

        }

        private void InternalNotes_Clicked(object sender, EventArgs e)
        {

        }

        private void Timelines_Clicked(object sender, EventArgs e)
        {

        }

        private void Expenses_Clicked(object sender, EventArgs e)
        {

        }

        private void TimeSheet_Clicked(object sender, EventArgs e)
        {

        }

        public static string TruncateAtWord(string input, int length)
        {
            if (input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);
            return string.Format("{0}...", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }

    }
    public class SetTicketList
    {
        public string ticket_id { get; set; }
        public string assigned_to { get; set; }
        public string device_name { get; set; }
        public string summary { get; set; }
        public string detail { get; set; }
        public string icon { get; set; }
        public string color { get; set; }
        public string tickets_created { get; set; }
    }

}