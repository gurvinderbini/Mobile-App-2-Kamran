using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodayReminderList : ContentPage
    {
        location loc = new location();
        public static double btnLocationX;
        public static double btnLocationY;
        public static string date;
        public static List<string> list = new List<string>();
        public TodayReminderList(string getdate)
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutBounds(absMain, new Rectangle(0, 0, location.screenX, location.screenY - 25));

            /*btnLocationX = location.btnMenuLocationX;
            btnLocationY = location.btnMenuLocationY;
            AbsoluteLayout.SetLayoutBounds(absMain, new Rectangle(0, 0, location.screenX, location.screenY - 25));
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

            date = getdate;
            // on create schedule
            try
            {
                var tapGestureSschedule = new TapGestureRecognizer();
                tapGestureSschedule.Tapped += (s, e) =>
                {
                    try
                    {
                        App.NavigateMasterDetail(new addSchedule());
                    }
                    catch { }
                };
                lblSchedule.GestureRecognizers.Add(tapGestureSschedule);
            }
            catch { }
            // on more click
            /*try
            {
                var tapGestureMore = new TapGestureRecognizer();
                tapGestureMore.Tapped += (s, e) =>
                {
                    //DisplayAlert("more", "more", "more");
                    //CancelEventOnLabelClick();
                };
                lblMore.GestureRecognizers.Add(tapGestureMore);
            }
            catch { }*/
        }

        private async void lvEventsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new addSchedule() { BindingContext = e.SelectedItem as EventsList });
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //await DisplayAlert("date", date, "ok");
            //list = await App.Database.getAllEvents();
            lvEventsList.ItemsSource = await App.Database.getAllEvents();

            //var employeelist = App.Database.getAllEvents();
            //foreach (var emp in employeelist.ToString())
            //{
            //    DisplayAlert("ds", emp)
            //}
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new CalendarView());
            base.OnBackButtonPressed();
            return true;
        }
    }

    public class EventsList2
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string startDate { get; set; }
        public string strtDate { get; set; }
        public string strtMonth { get; set; }
        public string startTime { get; set; }
        public string endDate { get; set; }
        public string endTime { get; set; }
        public string overallTime { get; set; }
        public string email { get; set; }
        public string notificationTime { get; set; }
    }
}