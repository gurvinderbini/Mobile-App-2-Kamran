using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebView : MyCustomContentPage
    {
        public string DEVICE_ID;
       
        public WebView(string device_id)
        {
            InitializeComponent();
            
            try
            {
                getWebRemote(device_id);
            }
            catch { }
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
                string remote_url =  "http://" + url_name + ".cloudschool.management/itcrm/admin/remoteZ/Ex?device=" + device_id;
                //DisplayAlert("remote_url", remote_url, "ok");
                if(!string.IsNullOrEmpty(remote_url))
                {
                    Device.OpenUri(new Uri(remote_url)); 
                }
               // webView.Source = remote_url;
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