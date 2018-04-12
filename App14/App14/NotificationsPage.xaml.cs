using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App14.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsPage : ContentPage
    {
        public NotificationsPage()
        {
            InitializeComponent();

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var list = await App.Database.GetAllNotfications();

            NotificationsListview.ItemsSource = list ?? new List<NotificationBO>();
        }

        private void NotificationsListview_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            if (e.SelectedItem is NotificationBO selectedItem && selectedItem.Screen == "tickets")
            {
                Navigation.PushAsync(new Tickets());
            }
            else
            {
                Navigation.PushAsync(new Warnings());
            }
        }

    }
}