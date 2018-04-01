using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App14
{
    class location
    {
        public static double screenX = 0;
        public static double screenY = 0;
        public static double btnMenuLocationX = 0;
        public static double btnMenuLocationY = 0;
        public static double width = 0;
        public static double length = 0;
        public location()
        {
            width = Application.Current.MainPage.Width;
            length = Application.Current.MainPage.Height;

            screenX = Convert.ToDouble(width);
            screenY = Convert.ToDouble(length - 30);
            
            if (width > 300)
            {
                btnMenuLocationX = width - 60;
            }
            else
            {
                btnMenuLocationX = width - 30;
            }
            if (length > 500)
            {
                btnMenuLocationY = length - 130;
            }
            else
            {
                btnMenuLocationY = length - 65;
            }
        }

    }
}
