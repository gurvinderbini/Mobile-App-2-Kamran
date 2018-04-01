using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App14.Models;

namespace App14
{
    class Common
    {
        public static string tenant_id="";

        public static string RootUserExists = "";

        public static bool NetworkStatus()
        {
            bool networkStatus = false;
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            if(networkConnection.IsConnected == true)
            {
                networkStatus = true;
            }

            return networkStatus;
        }

        public static string ProcessUrl(string Url)
        {
            Url = Url.Replace("http://", "");
            Url = Url.Replace("/", "");
            return Url;
        }

        public static async Task<bool> ValidateRootUser(string UserName)
        {
            bool valid = false;
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://cloudschool.management");
                var values = new Dictionary<string, string>();
                values.Add("module", "crud_users");
                values.Add("extra", "{\"group_id\":\"3\",\"user_name\":\"" + UserName.ToString() + "\"}");
                var content = new FormUrlEncodedContent(values);
                HttpResponseMessage response = await client.PostAsync("/itcrm/alreadyExistI/", content);
                var result = await response.Content.ReadAsStringAsync();
                //await DisplayAlert("res", result, "ok");
                PostLogin pstlgn = JsonConvert.DeserializeObject<PostLogin>(result);
                if (pstlgn.status)
                {
                    valid = true;
                    tenant_id = pstlgn.result[0];
                    getSchooolName(tenant_id);
                }
                else
                {
                    //  await DisplayAlert("Error!", "Task not added", "ok");
                }
            }
            catch
            { }
            return valid;
        }

        public static string ExtractRootUser(string Url)
        {
            string RootUser = "";
            String[] urlParts = Url.Split('.');
            RootUser = urlParts[0];
            return RootUser;
        }

        public static bool ValidateUrl(string Url)
        {
            bool UrlIsValid = false;

            Url = ProcessUrl(Url);

            String[] urlParts = Url.Split('.');

            string finalUrl = urlParts[1] + "." + urlParts[2];
            
            if(finalUrl == "cloudschool.management")
            {
                UrlIsValid = true;
            }

            return UrlIsValid;
        }
        
        public static async void getSchooolName(string tenant_id)
        {
            try
            {
                var client2 = new HttpClient();
                client2.BaseAddress = new Uri("http://cloudschool.management");
                var values2 = new Dictionary<string, string>();
                values2.Add("data", "{\"table\":\"company_information\",\"field\":\"company_name\"}");
                values2.Add("extra", "{\"tenant_id\":\"" + tenant_id.ToString() + "\"}");
                values2.Add("tenant_id", tenant_id.ToString());
                var content2 = new FormUrlEncodedContent(values2);
                HttpResponseMessage response2 = await client2.PostAsync("/itcrm/getElementVal/", content2);
                var result3 = await response2.Content.ReadAsStringAsync();
                validateResult chk_status = JsonConvert.DeserializeObject<validateResult>(result3);
                if (chk_status.status)
                {
                    try
                    {
                        string school_name = chk_status.result;
                        App.school_name = school_name;
                    }
                    catch { }
                }
                else
                {
                    // await DisplayAlert("Error!", "Task not added", "ok");
                }
            }
            catch { }
        }
    }

}
