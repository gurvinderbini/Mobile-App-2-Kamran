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
    public partial class Expenses : ContentPage
    {
        public Expenses()
        {
            InitializeComponent();
        }

        private void signOut_Clicked(object sender, EventArgs e)
        {
            App.NavigateMasterDetail(new DefaultORNewSchool());
        }
    }
}