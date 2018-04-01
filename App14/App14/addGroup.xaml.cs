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
    public partial class addGroup : ContentPage
    {
        location loc = new location();
        public static double btnLocationX;
        public static double btnLocationY;
        ComClass comfun = new ComClass();

        public addGroup()
        {
            try
            {
                InitializeComponent();
                AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));
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
                };*/

            }
            catch { }
        }

        private async Task btnAddGroup_Clicked(object sender, EventArgs e)
        {
            if (comfun.isConnected())
            {
                try
                {
                    string name;
                    name = lblgroupName.Text;
                    if (name != "" && name != " ")
                    {
                        try
                        {
                            var client = new HttpClient();
                            client.BaseAddress = new Uri(App.api_url);
                            var values = new Dictionary<string, string>();
                            values.Add("id", "178");
                            values.Add("module", "rooms");
                            values.Add("data", "{\"name\":\"" + name.ToString() + "\"}");
                            // values.Add("extra", );
                            //values.Add("extra", "{\"device_id\": \"" + device_id + "\"}");

                            //values.Add("operation", "query");
                            //values.Add("session_string", App.session_string);
                            //values.Add("query", "select * from groups");
                            var content = new FormUrlEncodedContent(values);
                            HttpResponseMessage response = await client.PostAsync("/itcrm/addRecord/", content);
                            var result = await response.Content.ReadAsStringAsync();
                            // await DisplayAlert("Rooms", result, "ok");
                            statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);
                            if (chk_status.status)
                            {
                                App.NavigateMasterDetail(new Rooms());
                            }
                            else
                            {
                                await DisplayAlert("Error!", "Room is not added, try later.", "ok");
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        await DisplayAlert("CloudSchool", "Group name can not be empty", "Ok");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("CloudSchool", "Error: " + ex.Message + ", Please try again", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }

        }
    }
}