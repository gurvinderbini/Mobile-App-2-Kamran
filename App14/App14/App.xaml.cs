using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using SQLite;
using PCLStorage;
using App14.Models;
using App14.iOS.RightSideMenu;

//using Quobject.SocketIoClientDotNet.Client;

namespace App14
{
    public partial class App : Application
    {
        static SqlHelper database;        
        public static MasterDetailPage MasterDetail { get; set; }
        public static string session_string = "";
        public static string api_url = "";
        public static string url_username = "";
        public static string logged_user_name = "";
        public static string user_email = "";
        public static string user_id="";
        public static string school_name="";
        public static string tenant_id = "";
        public static int length;
        public static int width;

        public  async static void NavigateMasterDetail(Page page)
        {
            try
            {
                App.MasterDetail.IsPresented = false;
                await App.MasterDetail.Detail.Navigation.PushAsync(page);
                 
            }
            catch
            { }
        }

        public async void getSchoolName()
        {
            RegEntity userDetail = await App.Database.GetItemFirst();
            if (userDetail != null)
            {
                school_name = userDetail.school_name;
                
                api_url = userDetail.CompleteUrl;
            }
        }

        public App()
        {
            InitializeComponent();
           
            try
            {
                getSchoolName();
                
                if (school_name != "" && school_name != null)
                {
                    //MainPage = new NavigationPage(new Events());
                    MainPage = new NavigationPage(new DefaultORNewSchool());
                }
                else
                {
                    //MainPage = new NavigationPage(new Events());
                    MainPage = new NavigationPage(new SchoolLog());
                }                
            }
            catch { }
        }
        

        public static SqlHelper Database
        {
            get
            {
                if (database == null)
                {
                    database = new SqlHelper();
                }
                return database;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
