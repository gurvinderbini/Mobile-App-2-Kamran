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
    public partial class selectGroup : ContentPage
    {
        public static List<string> iList = new List<string>();
        public ObservableCollection<SetRoomsList> dt;
        public string DEVICE_ID;
        public selectGroup(string dev_id)
        {
            InitializeComponent();
            try
            {
                // DisplayAlert("CloudSchool", "Select Group", "Ok");
                DEVICE_ID = dev_id;
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
                        //   await DisplayAlert("CloudSchool", "Error: " + e.Message, "ok");
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
                        //  await DisplayAlert("CloudSchool", "Error: " + e.Message, "ok");
                    }
                }
            }
            catch { }
        }
        private async void lvRoomsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var index = (lvRoomsList.ItemsSource as ObservableCollection<SetRoomsList>).IndexOf(e.SelectedItem as SetRoomsList);
                var a = new SetRoomsList();
                ObservableCollection<SetRoomsList> dt = new ObservableCollection<SetRoomsList>();
                string dep_id = iList[index];
                //await DisplayAlert("selected group id :", dep_id, "ok");
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);
                    var values = new Dictionary<string, string>();
                    values.Add("id", DEVICE_ID);
                    values.Add("module", "devices");
                    values.Add("data", "{\"department\": \"" + dep_id + "\"}");
                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/updateRecordI/", content);
                    var result = await response.Content.ReadAsStringAsync();
                    // await DisplayAlert("Rooms", result, "ok");
                    statusCheck chk_status = JsonConvert.DeserializeObject<statusCheck>(result);
                    if (chk_status.status)
                    {
                        await DisplayAlert("CloudSchool", "Device Group changed", "Ok");
                        App.NavigateMasterDetail(new Rooms());

                    }
                    else
                    {
                        await DisplayAlert("CloudSchool", "Error on changing Device Group!", "Ok");
                        App.NavigateMasterDetail(new Rooms());
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("CloudSchool", "Error: " + ex.Message, "ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("CloudSchool", "Error: " + ex.Message, "ok");
            }
        }
    }
}