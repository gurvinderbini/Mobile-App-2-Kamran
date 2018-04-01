using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using App14.Models;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TicketRoomSelect : ContentPage
    {
        public static List<string> iList = new List<string>();
        public ObservableCollection<SetRoomsList> dt;

        public TicketRoomSelect()
        {
            InitializeComponent();
            try
            {
                GetRooms();
            }
            catch { }
        }
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
                                await Navigation.PushAsync(new TicketDeviceSelect(did, dname));
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
            }
            catch { }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new AddNewTicket());
            base.OnBackButtonPressed();
            return true;
        }
    }
}