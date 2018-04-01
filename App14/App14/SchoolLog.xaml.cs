using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using App14.Models;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchoolLog : ContentPage
    {
        ComClass comfun = new ComClass();
        public static string url_name;
        public SchoolLog()
        {
            InitializeComponent();
            TxtUrl.Focus();
            lblURLSample.IsEnabled = false;
            lblErorTxtUrl.IsVisible = false;
            lblRemainingURL.IsEnabled = false;
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void btnContinue_Clicked(object sender, EventArgs e)
        {
            if (TxtUrl.Text != "")
            {
                try
                {
                    if (comfun.isConnected())
                    {
                        btnContinue.IsEnabled = false;
                        Continue();
                    }
                    else
                    {
                        btnContinue.IsEnabled = true;
                        await DisplayAlert("Internet Disabled", "You are not connected to internet", "ok");
                    }
                }
                catch {
                    btnContinue.IsEnabled = true;
                }
            }
            else
            {
                await DisplayAlert("Empty Username", "Username can't be empty", "Ok");
                TxtUrl.Focus();
                btnContinue.IsEnabled = true;
            }
        }

        private bool Validate(string url)
        {
            bool validation = true;
            if (Common.ValidateUrl(url) == false)
            {
                validation = false;
                lblErorTxtUrl.IsVisible = true;
                lblErorTxtUrl.Text = "Url does not exist on server";
                btnContinue.IsEnabled = true;
            }
            return validation;
        }

        private async void Continue()
        {
            try
            {
                string url = TxtUrl.Text;
                url = url + ".cloudschool.management";
                if (comfun.isConnected())
                {
                    if (Validate(url) == true)
                    {
                        SaveCred();
                        Post();
                    }
                    else
                    {
                        await DisplayAlert("Error!", "You are not connected to internet.", "Ok");
                        btnContinue.IsEnabled = true;
                    }
                }
                else
                {
                    await DisplayAlert("Error!", "You are not connected to internet.", "Ok");
                    btnContinue.IsEnabled = true;
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Error!", "Login Fail " + e.Message, "Ok");
                btnContinue.IsEnabled = true;
            }
            
        }

        public async void LoadCred()
        {
            RegEntity userDetail = await App.Database.GetItemFirst();
            if (userDetail != null)
            {
                TxtUrl.Text = userDetail.Url;
            }
        }

        public async void LoadCredURL()
        {
            try
            {
                RememberMeCredentials loginCheck = await App.Database.GetSaveRemeber();
                if (loginCheck != null)
                {
                    TxtUrl.Text = loginCheck.url_name;
                }
                else
                {
                }
            }
            catch { }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                RememberMeCredentials loginCheck = await App.Database.GetSaveRemeber();
                if (loginCheck != null)
                {
                    LoadCredURL();
                   /* if (loginCheck.sign_out == 1)
                    {
                        LoadCredURL();
                    }
                    else
                    {

                    }*/
                }
                else
                {

                }
            }
            catch { }
        }

        public async void SaveCred()
        {
            RegEntity userDetail = await App.Database.GetItemFirst();
            if (userDetail == null)
            {
                RegEntity OReg = new RegEntity();
                OReg.Url = TxtUrl.Text;
                OReg.CompleteUrl = "https://" +TxtUrl.Text + ".cloudschool.management";
                //await DisplayAlert("App.school_name", App.school_name, "ok");
                OReg.school_name = App.school_name;
                await App.Database.SaveItem(OReg);
            }

        }

        public async void Post()
        {            
            aiLogin.IsVisible = true;
            aiLogin.IsRunning = true;
            try
            {
                string url = Common.ProcessUrl(TxtUrl.Text + ".cloudschool.management");                
                string getRootUser = Common.ExtractRootUser(TxtUrl.Text);
                // await DisplayAlert("getRootUser", getRootUser,"ok");
                bool RootUserExists =  await Common.ValidateRootUser(getRootUser); //true; 
                                                                                   // await DisplayAlert("RootUserExists", RootUserExists.ToString(), "ok");
                if (RootUserExists == false)
                {
                    aiLogin.IsRunning = false;
                    aiLogin.IsVisible = false;
                    btnContinue.IsEnabled = true;
                    await DisplayAlert("Error!", "Invalid \"" + getRootUser + "\" in given url", "ok");
                    TxtUrl.Focus();
                }
                else
                {
                   // await DisplayAlert("school name", App.school_name, "ok");
                    SaveCred();                    
                    App.api_url = "http://" + url;
                    App.url_username = TxtUrl.Text;
                    aiLogin.IsVisible = false;
                    aiLogin.IsRunning = false;
                    btnContinue.IsEnabled = true;
                    url_name = TxtUrl.Text;
                    await Navigation.PushAsync(new LogIn(url_name));
                }

            }
            catch (Exception e)
            {
                aiLogin.IsRunning = false;
                aiLogin.IsVisible = false;
                await DisplayAlert("Error!", e.Message, "OK");
                TxtUrl.Focus();
            }
            
        }
    }
}