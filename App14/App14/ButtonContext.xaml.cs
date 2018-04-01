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
    public partial class ButtonContext : ContentView
    {
      //  location loc = new location();
        public ButtonContext()
        {
            InitializeComponent();
            btnDevice.IsVisible = true;
            btnHome.IsVisible = true;
            btnSignOut.IsVisible = true;
            btnTickets.IsVisible = true;
            btnWarning.IsVisible = true;
            btnHelp.IsVisible = true;
            AbsoluteLayout.SetLayoutBounds(btnHome, new Rectangle(location.btnMenuLocationX - 400, location.btnMenuLocationY - 480, 240, 60));
            AbsoluteLayout.SetLayoutBounds(btnWarning, new Rectangle(location.btnMenuLocationX - 370, location.btnMenuLocationY - 425, 240, 60));
            AbsoluteLayout.SetLayoutBounds(btnDevice, new Rectangle(location.btnMenuLocationX - 320, location.btnMenuLocationY - 370, 240, 60));
            AbsoluteLayout.SetLayoutBounds(btnTickets, new Rectangle(location.btnMenuLocationX - 270, location.btnMenuLocationY - 315, 240, 60));
            AbsoluteLayout.SetLayoutBounds(btnCalendar, new Rectangle(location.btnMenuLocationX - 250, location.btnMenuLocationY - 250, 240, 60));
            AbsoluteLayout.SetLayoutBounds(btnHelp, new Rectangle(location.btnMenuLocationX - 230, location.btnMenuLocationY - 185, 240, 60));
            AbsoluteLayout.SetLayoutBounds(btnSignOut, new Rectangle(location.btnMenuLocationX - 220, location.btnMenuLocationY - 120, 240, 60));
                      
        }

        private void btnDevice_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new Rooms());
        }

        private void btnHome_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new Detail());
        }

        private void btnSignOut_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new DefaultORNewSchool());
        }

        private void btnTickets_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new Tickets());
        }

        private void btnWarning_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new Warnings());
        }

        private void btnHelp_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("http://cloudschool.management/itcrm/helpPortal"));
        }
        
        private void btnCalendar_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new CalendarView());
        }
    }
}