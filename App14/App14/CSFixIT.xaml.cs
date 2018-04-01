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
    public partial class CSFixIT : ContentPage
    {
        public CSFixIT()
        {
            InitializeComponent();
            try
            {
                PickerSource.Items.Add("Web Application");
                PickerSource.Items.Add("Phone");
                PickerSource.Items.Add("Email");
                PickerSource.Items.Add("Desktop Application");
                PickerSource.Items.Add("Mobile Application");
                PickerSource.Items.Add("Other");


                PickerStatus.Items.Add("Open");
                PickerStatus.Items.Add("Resolved");
                PickerStatus.Items.Add("Closed");

                PickercsFixIT.Items.Add("Yes");
                PickercsFixIT.Items.Add("No");
            }
            catch { }
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {

        }

        private void btnCreate_Clicked(object sender, EventArgs e)
        {

        }

        private void PickercsFixIT_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PickerStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PickerSource_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void signOut_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new DefaultORNewSchool());
        }
    }
}