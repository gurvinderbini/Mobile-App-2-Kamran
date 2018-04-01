using System;
using System.Collections.Generic;
using SlideOverKit;
using Xamarin.Forms;

namespace App14.iOS.RightSideMenu
{
    public partial class RightSideMasterPage4 : SlideMenuView
    {
        public RightSideMasterPage4()
        {
            InitializeComponent();
            // You must set IsFullScreen in this case, 
            // otherwise you need to set HeightRequest, 
            // just like the QuickInnerMenu sample
            this.IsFullScreen = true;
            // You must set WidthRequest in this case
            this.WidthRequest = 150;
            this.MenuOrientations = MenuOrientation.RightToLeft;
            // You must set BackgroundColor, 
            // and you cannot put another layout with background color cover the whole View
            // otherwise, it cannot be dragged on Android
            this.BackgroundColor = Color.White;

            // This is shadow view color, you can set a transparent color
            this.BackgroundViewColor = Color.Transparent;
        }

        public void Handle_Clicked(object sender, System.EventArgs e)
        {
            this.HideWithoutAnimations();
            Navigation.PushAsync(new addSchedule());
        }
    }
}
