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
    public partial class Rooms : MenuContainerPage
    {
        ComClass comfun = new ComClass();
        public static List<string> iList = new List<string>();
        public ObservableCollection<SetRoomsList> dt;
        location loc = new location();
        public static double btnLocationX;
        public static double btnLocationY;
        public Rooms()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));

            this.SlideMenu = new RightSideMasterPage();
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
                GetRooms();
            }
            catch { }
        }
        private async void GetRooms()
        {
            if (comfun.isConnected())
            {
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);
                    var values = new Dictionary<string, string>();
                    values.Add("session_string", App.session_string);
                    values.Add("data", "{\"table\": \"rooms\"}");
                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/getElements/", content);
                    var result = await response.Content.ReadAsStringAsync();

                    rooms rooms_list = JsonConvert.DeserializeObject<rooms>(result);

                    var lst = rooms_list.result;

                    int totalRecords = lst.Length;
                    int remainder = totalRecords % 3;
                    int remainingThreeBased = totalRecords - remainder;
                    int totalRows = (remainingThreeBased / 3) + 1;

                    var grid = new Grid();
                    for (int i = 0; i < totalRows; i++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    }
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    int counter = 0;
                    for (int i = 0; i < totalRows; i++)
                    {
                        try
                        {
                            // iList.Add(lst[i].id);
                            for (int a = 0; a <= 2; a++)
                            {
                                var button = new Button();
                                string did = lst[counter].id;
                                string dname = lst[counter].name;
                                button.Text = dname;
                                button.BackgroundColor = Color.FromHex("#34CBFE");
                                button.TextColor = Color.White;
                                button.Margin = 2;
                                button.WidthRequest = 50;
                                button.HeightRequest = 75;
                                button.Clicked += async delegate {
                                    await Navigation.PushAsync(new Devices(did, dname));
                                };
                                grid.Children.Add(button, a, i);
                                counter++;
                            }
                        }
                        catch (Exception e)
                        {
                            //   await DisplayAlert("Error!", e.Message, "ok");
                        }
                    }
                    workingStack.Children.Add(grid);
                    aiDevices.IsRunning = false;
                }
                catch { }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new Detail());
            base.OnBackButtonPressed();
            return true;
        }

        /*
        private async void GetRooms()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(App.api_url);
                var values = new Dictionary<string, string>();
                values.Add("session_string", App.session_string);
                values.Add("data", "{\"table\": \"rooms\"}");
                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage response = await client.PostAsync("/itcrm/getElements/", content);
                var result = await response.Content.ReadAsStringAsync();
                //await DisplayAlert("Rooms", result, "ok");
                rooms rooms_list = JsonConvert.DeserializeObject<rooms>(result);
                dt = new ObservableCollection<SetRoomsList>();
                lvRoomsList.ItemsSource = dt;
                var lst = rooms_list.result;
                for (int i = 0; i < lst.Length; i++)
                {
                    try
                    {
                        iList.Add(lst[i].id);
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
                        dt.Add(new SetRoomsList()
                        {
                            rooms_name = lst[i].name,
                            id = lst[i].id,
                            icon = "https://cloudschool.management/itcrm/media/images/blackrooms.png",
                        });
                    }
                    catch (Exception e)
                    {
                        //   await DisplayAlert("Error!", e.Message, "ok");
                    }
                }
            }
            catch { }
        }
        */
        //public void OnMore(object sender, EventArgs e)
        //{
        //    var mi = ((MenuItem)sender);
        //    DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        //}
        //public void OnDelete(object sender, EventArgs e)
        //{
        //    var mi = ((MenuItem)sender);
        //    DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        //}
        /*
        private void lvRoomsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var index = (lvRoomsList.ItemsSource as ObservableCollection<SetRoomsList>).IndexOf(e.SelectedItem as SetRoomsList);
                var a = new SetRoomsList();
                ObservableCollection<SetRoomsList> dt = new ObservableCollection<SetRoomsList>();
                App.NavigateMasterDetail(new Devices(iList[index]));
            }
            catch { }
        }
        */

        private void btnAddGroup_Clicked(object sender, EventArgs e)
        {
            this.ShowMenu();
           // App.NavigateMasterDetail(new addGroup());
        }

        private async void delete_Clicked(object sender, EventArgs e)
        {
            bool resp = await DisplayAlert("Confirm", "Are you sure to delete this Group", "Yes", "No");
            if (resp)
            {
                try
                {
                    var mi = ((MenuItem)sender);
                    string dep_id = mi.CommandParameter.ToString();
                    try
                    {
                        var client = new HttpClient();
                        client.BaseAddress = new Uri(App.api_url);
                        var values = new Dictionary<string, string>();
                        values.Add("module", "devices");
                        values.Add("extra", "{\"department\": \"" + dep_id.ToString() + "\"}");
                        var content = new FormUrlEncodedContent(values);
                        HttpResponseMessage response = await client.PostAsync("/itcrm/alreadyExistI/", content);
                        var result = await response.Content.ReadAsStringAsync();
                        statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);
                        if (chk_status.status)
                        {
                            await DisplayAlert("CloudSchool", "Group cannot be deleted since it has devices in it", "Ok");
                        }
                        else
                        {
                            try
                            {
                                var client2 = new HttpClient();
                                client2.BaseAddress = new Uri(App.api_url);
                                var values2 = new Dictionary<string, string>();
                                values2.Add("id", dep_id);
                                values2.Add("module", "rooms");
                                var content2 = new FormUrlEncodedContent(values2);
                                HttpResponseMessage response2 = await client2.PostAsync("/itcrm/delRecord/", content2);
                                var result2 = await response2.Content.ReadAsStringAsync();
                                statusCheck chk_status2 = JsonConvert.DeserializeObject<statusCheck>(result2);
                                if (chk_status2.status)
                                {
                                    await DisplayAlert("CloudSchool", "Group deleted", "Ok");
                                    App.NavigateMasterDetail(new Rooms());
                                }
                                else
                                {
                                    await DisplayAlert("CloudSchool", "Group not deleted, try again", "Ok");
                                }
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("CloudSchool", "Error on Group deletion " + ex.Message, "ok");
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        await DisplayAlert("Error on room deletion", er.Message, "ok");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error on getting room id !", ex.Message, "Ok");
                }
            }
            else
            {
            }
        }
    }
    public class SetRoomsList
    {
        public string rooms_name { get; set; }
        public string id { get; set; }
        public string icon { get; set; }
        //public string rooms_person { get; set; }
        //public string rooms_person_id { get; set; }
        //public string rooms_location { get; set; }
        //public string rooms_created { get; set; }
    }
}