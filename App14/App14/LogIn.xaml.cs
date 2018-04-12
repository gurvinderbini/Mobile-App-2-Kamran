using System;
using System.Collections.Generic;
using System.Net.Http;
using App14.Helpers;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App14.Models;
using App14.iOS.RightSideMenu;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogIn : ContentPage
    {
        ///RememberMeCredentials remObj = new RememberMeCredentials();
        ComClass comfun = new ComClass();
        public static string URL_NAME;
        public static int Checked=1;
        public LogIn(string url_name)
        {
            InitializeComponent();
            URL_NAME = url_name;

            if(Device.RuntimePlatform==Device.Android)
            {
                imgCheckbox.IsVisible = false;
            }
            //DisplayAlert("URL_NAME", URL_NAME + " USERNAME duplicate: " + App.url_username + " " + remObj.user_name + " " + remObj.user_name, "ok");
            try
            {
                NavigationPage.SetHasBackButton(this, false);
            }
            catch { }
        }

        void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            Checked++;
            var imageSender = (Image)sender;
            //watch the monkey go from color to black&white!
            if ((Checked % 2) == 0)
            {
                imageSender.Source = "checked_checkbox1.png";
            }
            else
            {
                imageSender.Source = "Unchecked_checkbox.png";
            }

        }

        public async void LoadCred()
        {
            try
            {
               // RememberMeCredentials saveRemInfo2 = await App.Database.GetSaveRemeber();
                RememberMeCredentials remObj = new RememberMeCredentials();
                remObj = await App.Database.GetSaveRemeber();
                if (remObj != null)
                {
                    //await DisplayAlert("Detail", "record exist", "OK");
                    TxtUserName.Text = remObj.user_name;
                    TxtPassword.Text = remObj.password;
                }
                else
                {
                   // await DisplayAlert("Detail", "no record exist", "OK");
                }
                
            }
            catch(Exception e)
            {
                await DisplayAlert("Error", e.Message, "ok");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadCred();
        }
        

        private async void btnLoginLbl_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new RightSideDetailPage());
            if (comfun.isConnected())
            {
                btnLoginLbl.IsEnabled = false;
                Login();
            }
            else
            {
                await DisplayAlert("Internet Disabled", "You are not connected to internet", "ok");
                aiLogin.IsVisible = false;
                aiLogin.IsRunning = false;
                btnLoginLbl.IsEnabled = true;
            }
        }

        private void Login()
        {
            try
            {
                if (string.IsNullOrEmpty(TxtUserName.Text) && string.IsNullOrEmpty(TxtPassword.Text))
                {
                    btnLoginLbl.IsEnabled = true;
                    DisplayAlert("Empty Fields", "Email and Password are required", "Ok");
                    TxtUserName.Focus();
                }
                else if (string.IsNullOrEmpty(TxtUserName.Text))
                {
                    btnLoginLbl.IsEnabled = true;
                    DisplayAlert("Username", "Username is not given", "Ok");
                    TxtUserName.Focus();
                }
                else if (string.IsNullOrEmpty(TxtPassword.Text))
                {
                    btnLoginLbl.IsEnabled = true;
                    DisplayAlert("Password", "Password is not given", "Ok");
                    TxtPassword.Focus();
                }
                else
                {
                    try
                    {
                        string userName = TxtUserName.Text;
                        string password = TxtPassword.Text;
                        PostLogin();
                    }
                    catch (Exception e)
                    {
                        aiLogin.IsVisible = false;
                        aiLogin.IsRunning = false;
                        btnLoginLbl.IsEnabled = true;
                        DisplayAlert("Error!", "Login Fail " + e.Message, "Ok");
                    }
                }
            }
            catch
            {
                aiLogin.IsVisible = false;
                aiLogin.IsRunning = false;
                btnLoginLbl.IsEnabled = true;
            }
        }

        public async void PostLogin()
        {
            if (comfun.isConnected())
            {
                aiLogin.IsVisible = true;
                aiLogin.IsRunning = true;
                try
                {
                    string userName = TxtUserName.Text;
                    string password = TxtPassword.Text;
                    // await DisplayAlert("App.api_url", App.api_url + " " + userName + " " + password, "ok");
                    var client = new HttpClient();
                    client.BaseAddress = new Uri(App.api_url);
                    var values = new Dictionary<string, string>();
                    values.Add("operation", "loginI");
                    values.Add("url", URL_NAME);
                    values.Add("username", userName);
                    values.Add("password", password);
                    var content = new FormUrlEncodedContent(values);
                    HttpResponseMessage response = await client.PostAsync("/itcrm/webservices/", content);
                    var result = await response.Content.ReadAsStringAsync();
                    //await DisplayAlert("result!", result, "OK");
                    LoginData lgn = JsonConvert.DeserializeObject<LoginData>(result);
                    LoginDataResult result2 = new LoginDataResult();
                    result2 = lgn.result[0];
                    if (lgn.status == true)
                    {
                        var session = result2.session_string;
                        App.session_string = Convert.ToString(session);
                        var email = result2.user_email;
                        App.user_email = Convert.ToString(email);
                        App.logged_user_name = Convert.ToString(result2.user_full_name);
                        App.user_id = Convert.ToString(result2.user_id);
                        App.user_tenant_id = Convert.ToString(result2.user_tenant_id);

                        PushDeviceToken();

                        //save in database
                        if (rememberMe.Checked == true)
                        {
                            try
                            {
                                RememberMeCredentials saveRemInfo = new RememberMeCredentials();
                                saveRemInfo.url_name = URL_NAME;
                                saveRemInfo.user_name = TxtUserName.Text;
                                saveRemInfo.password = TxtPassword.Text;
                                var rec = await App.Database.SaveRememberMeInfo(saveRemInfo);
                                //await DisplayAlert("rec", "record " + rec.ToString() + " Inserted.", "ok");
                            }
                            catch { }
                        }///////////////////////////////////////////////////////////////////////////
                        else
                        {
                            try
                            {
                                RememberMeCredentials a = new RememberMeCredentials();
                                //a.ID = 1;
                                var rec = await App.Database.DeleteSaveUser(a);
                                // await DisplayAlert("rec", rec.ToString() + " deleted.", "ok");
                            }
                            catch (Exception e)
                            {
                                // await DisplayAlert("error", "during " + e.Message, "ok");
                            }
                        }

                        // App.tenant_id = Convert.ToString(result2.te)
                        // await DisplayAlert("app save !", "id : " + App.user_id + " UN : " + App.logged_user_name + " App.user_email : " + App.user_email + " App.session_string " + App.session_string, "OK");
                        aiLogin.IsVisible = false;
                        aiLogin.IsRunning = false;
                        btnLoginLbl.IsEnabled = true;
                        await Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                        aiLogin.IsVisible = false;
                        aiLogin.IsRunning = false;
                        btnLoginLbl.IsEnabled = true;
                        await DisplayAlert("Error", result2.message + "...", "ok");
                    }
                }
                catch (Exception e)
                {
                    btnLoginLbl.IsEnabled = true;
                    aiLogin.IsVisible = false;
                    aiLogin.IsRunning = false;
                    await DisplayAlert("Error!", "Due to => " + e.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }

        public async void PushDeviceToken()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(App.api_url);
            var values = new Dictionary<string, string>();
            values.Add("operation", "insert_device_token");
            values.Add("user_id", App.user_id);
            values.Add("parent_id", App.tenant_id);
            values.Add("device_token", Settings.DeviceToken);
            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync("/itcrm/ws/webservices", content);
            var result = await response.Content.ReadAsStringAsync();
        }


    }

}