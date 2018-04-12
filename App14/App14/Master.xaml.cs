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
    public partial class Master : ContentPage
    {
        public Master()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception w)
            {
                DisplayAlert("Error ! ", "Message : " + w.Message, "OK");
            }
           
            // Dashboard
            try
            {
                btnDashboard.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        App.NavigateMasterDetail(new Detail());
                    })
                });
            }
            catch (Exception e)
            {
                DisplayAlert("Dashboard", "Message : " + e.Message, "OK");
            }

            // Tickets
            try
            {
                btnTickets.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        App.NavigateMasterDetail(new Tickets());
                    })
                });
            }
            catch (Exception e)
            {
                DisplayAlert("Tickets", "Message : " + e.Message, "OK");
            }

            // Groups
            try
            {
                btnRooms.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        App.NavigateMasterDetail(new Rooms());
                    })
                });
            }
            catch (Exception e)
            {
                DisplayAlert("Rooms", "Message : " + e.Message, "OK");
            }

            // Warnings
            try
            {
                btnWarnings.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        App.NavigateMasterDetail(new Warnings());
                    })
                });
            }
            catch (Exception e)
            {
                DisplayAlert("Warnings", "Message : " + e.Message, "OK");
            }

            // Scheule
            try
            {
                btnSchedule.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        App.NavigateMasterDetail(new CalendarView());
                    })
                });
            }
            catch (Exception e)
            {
                DisplayAlert("Calender", "Message : " + e.Message, "OK");
            }

            // Help
            try
            {
                btnHelp.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        Device.OpenUri(new Uri("http://cloudschool.management/itcrm/helpPortal"));
                    })
                });
            }
            catch (Exception e)
            {
                DisplayAlert("Rooms", "Message : " + e.Message, "OK");
            }

            // Signout
            try
            {
                btnSignout.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        App.NavigateMasterDetail(new DefaultORNewSchool());
                    })
                });
            }
            catch (Exception e)
            {
                DisplayAlert("Sign Out", "Message : " + e.Message, "OK");
            }
            // Notifications
            try
            {
                btnNotification.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        App.NavigateMasterDetail(new NotificationsPage());
                    })
                });
            }
            catch (Exception e)
            {
                //DisplayAlert("Sign Out", "Message : " + e.Message, "OK");
            }

        }
    }
}