using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App14
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();
                try
                {
                    this.Master = new Master();
                }
                catch (Exception w)
                {
                    DisplayAlert("Master", "Message : " + w.Message, "Ok");
                }
                
                try
                {
                    this.Detail = new NavigationPage(new Detail());
                }
                catch (Exception w)
                {
                    DisplayAlert("Detail", "Message : " + w.Message, "Ok");
                }
                try
                {
                    App.MasterDetail = this;
                }
                catch (Exception e)
                {
                    DisplayAlert("this", "Message : " + e.Message, "Ok");
                }
            }
            catch { }
        }
    }
}
