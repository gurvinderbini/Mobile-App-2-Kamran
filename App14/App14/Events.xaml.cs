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
    public partial class Events : ContentPage
    {
        public static List<string> list = new List<string>();
        private static readonly HttpClient client = new HttpClient();
        public static ObservableCollection<SetWarningsList> dt;
        location loc = new location();
        public static double btnLocationX;
        public static double btnLocationY;
        public Events()
        {
            InitializeComponent();
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));
            /*
            btnLocationX = location.btnMenuLocationX;
            btnLocationY = location.btnMenuLocationY;
            AbsoluteLayout.SetLayoutBounds(workingStack, new Rectangle(0, 0, location.screenX, location.screenY));
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
        }
        string title, sDate, sTime, eDate, eTime;

        private async void lvEventsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem != null)
            {
                await Navigation.PushAsync(new addSchedule() { BindingContext = e.SelectedItem as EventsList });
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            lvEventsList.ItemsSource = await App.Database.getAllEvents();
        }

        

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new addSchedule());
            base.OnBackButtonPressed();
            return true;
        }
    }
}