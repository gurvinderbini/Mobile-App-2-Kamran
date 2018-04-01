using App14.Models;
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
    public partial class DefaultORNewSchool : ContentPage
    {
        RememberMeCredentials remObj = new RememberMeCredentials();
        ComClass comfun = new ComClass();
        public DefaultORNewSchool()
        {
            InitializeComponent();
            btnContinue.IsEnabled = true;
            if (App.school_name == "")
            {
                Common.getSchooolName(Common.tenant_id);
            }
            else
            {
                lblSchoolName.Text = App.school_name;
            }
            try
            {
                lblOtherAccount.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async() => {
                        try
                        {
                            RememberMeCredentials a = new RememberMeCredentials();
                            a.ID = 1;
                            var rec = await App.Database.DeleteSaveUser(a);
                            //await DisplayAlert("rec", rec.ToString() + " deleted.", "ok");
                        }
                        catch (Exception e)
                        {
                           // await DisplayAlert("error", e.Message, "ok");
                        }
                        await Navigation.PushAsync(new SchoolLog());
                    })
                });
            }
            catch (Exception e)
            {
                DisplayAlert("CloudShool", "Another Account Message : " + e.Message, "OK");
            }
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

        private async void btnContinue_Clicked(object sender, EventArgs e)
        {
            btnContinue.IsEnabled = false;
            if (comfun.isConnected())
            {
                btnContinue.IsEnabled = true;
                await Navigation.PushAsync(new LogIn(App.url_username));
            }
            else
            {
                btnContinue.IsEnabled = true;
                await DisplayAlert("Connection", "Internet Connection Disabled", "Ok");
            }
        }
    }
}