using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App14
{
    class ButtonClickable
    {
        public Button btnHome;
        public Button btnDevice;
        public Button btnTickets;
        public Button btnWarning;
        public Button btnSignOut;
        public static double btnMenuLocationX = 0;
        public static double btnMenuLocationY = 0;
        public static double width = 0;
        public static double length = 0;

        public ButtonClickable()
        {
            width = Application.Current.MainPage.Width;
            length = Application.Current.MainPage.Height;

            if (width > 300)
            {
                btnMenuLocationX = width - 70;
                // btnMenuLocationX = 
            }
            else
            {
                btnMenuLocationX = width - 25;
            }
            if (length > 500)
            {
                btnMenuLocationY = length - 250;
            }
            else
            {
                btnMenuLocationY = length;
            }
        }


        public Button FixButtonHome()
        {
            btnHome = new Button
            {

                BorderRadius = 100,
                WidthRequest = 60,
                HeightRequest = 60,
                Text = "Home",
                BackgroundColor = Color.Red,
                //Image = "homeGray",
                Opacity = 0.5,
                // IsVisible = false,
            };
            btnHome.Clicked += BtnHome_Clicked;
            AbsoluteLayout.SetLayoutBounds(btnHome, new Rectangle(btnMenuLocationX - 15, btnMenuLocationY - 150, 50, 50));
            return btnHome;
        }
        public Button FixButtonDevice()
        {
            btnDevice = new Button
            {

                BorderRadius = 100,
                WidthRequest = 60,
                HeightRequest = 60,
                Text = "Device",
                //Image = "devices",
                BackgroundColor = Color.Red,
                Opacity = 0.5,
                ///  IsVisible = false,
            };
            btnDevice.Clicked += BtnDevice_Clicked;
            AbsoluteLayout.SetLayoutBounds(btnDevice, new Rectangle(btnMenuLocationX - 4, btnMenuLocationY - 80, 50, 50));
            return btnDevice;
        }
        public Button FixButtonTickets()
        {
            btnTickets = new Button
            {

                BorderRadius = 100,
                WidthRequest = 60,
                HeightRequest = 60,
                Text = "Tickets",
                BackgroundColor = Color.Red,
                //Image = "ticketsGray",
                Opacity = 0.5,
                //IsVisible = false,
            };
            btnTickets.Clicked += BtnTickets_Clicked;
            AbsoluteLayout.SetLayoutBounds(btnTickets, new Rectangle(btnMenuLocationX - 85, btnMenuLocationY - 280, 50, 50));
            return btnTickets;
        }
        public Button FixButtonWarning()
        {
            btnWarning = new Button
            {

                BorderRadius = 100,
                WidthRequest = 60,
                HeightRequest = 60,
                Text = "Warning",
                BackgroundColor = Color.Red,
                //Image = "warningGray",
                Opacity = 0.5,
                //  IsVisible = false,
            };
            btnWarning.Clicked += BtnWarning_Clicked;
            AbsoluteLayout.SetLayoutBounds(btnWarning, new Rectangle(-150, -340, 50, 50));

            return btnWarning;
        }
        public Button FixButtonSignOut()
        {
            btnSignOut = new Button
            {

                BorderRadius = 100,
                WidthRequest = 150,

                HeightRequest = 60,
                Text = "SignOut",
                BackgroundColor = Color.Red,
                //Image = "signoutGray",
                Opacity = 0.5,
                //  IsVisible = false,
            };
            btnSignOut.Clicked += BtnSignOut_Clicked;
            AbsoluteLayout.SetLayoutBounds(btnSignOut, new Rectangle(-50, -220, 50, 50));

            return btnSignOut;
        }

        public void addButtons()
        {
            try
            {
                // menuGrid.Children.Add(FixButtonHome());
                //menuGrid.Children.Add(FixButtonDevice());
                //menuGrid.Children.Add(FixButtonWarning());
                //menuGrid.Children.Add(FixButtonTickets());
                //menuGrid.Children.Add(FixButtonSignOut());

            }
            catch { }
        }

        private void BtnHome_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new Detail());
        }
        private void BtnDevice_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new Rooms());
        }
        private void BtnTickets_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new Tickets());
        }
        private void BtnWarning_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new Warnings());
        }
        private void BtnSignOut_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new SchoolLog());
        }
    }
}
