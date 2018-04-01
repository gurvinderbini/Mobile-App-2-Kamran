using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace App14
{
    public partial class WebViewPage : MyCustomContentPage
    {
        public string DEVICE_ID;
        public WebViewPage(string device_id)
        {
            InitializeComponent();
            getWebRemote(device_id);
        }


        public async void getWebRemote(string device_id)
        {
            try
            {
                DEVICE_ID = device_id;

                string userURLName = "";
                RegEntity userDetail = await App.Database.GetItemFirst();
                if (userDetail != null)
                {
                    userURLName = userDetail.Url;
                }

                string url_name = userURLName;
                webView.HeightRequest = 1000;
                webView.WidthRequest = 1000;
                string remote_url = "http://" + url_name + ".cloudschool.management/itcrm/admin/remoteZ/Ex?device=" + device_id;

                webView.Source = remote_url;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "allowLandScapePortrait");
            await progress.ProgressTo(0.9, 900, Easing.SpringIn);
        }


        private void webView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            progress.IsVisible = true;
        }

        private void webView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            progress.IsVisible = false;
        }


    }
}
