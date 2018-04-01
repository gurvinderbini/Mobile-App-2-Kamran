using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App14
{
    class ComClass
    {
        private bool _isRunning;
        public ComClass() { }
        public bool checkText(string text)
        {
            bool valid = false;
            try
            {
                if (text != "" && text != " ")
                {
                    valid = true;
                }
            }
            catch { }
            return valid;
        }
        public bool NumberText(string number)
        {
            try
            {
                Regex regex = new Regex(@"^([^0-9])$");
                Match match = regex.Match(number);
                if (match.Success)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool IsValidEmailId(string InputEmail)
        {
            try
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(InputEmail);
                if (match.Success)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool isConnected()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void timer(double time)
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                time += 1;
                TimeSpan t = TimeSpan.FromSeconds(time);
                //lblTimer.Text = String.Format("{0:D2}H : {1:D2}M : {2:D2}S", t.Hours, t.Minutes, t.Seconds);
                if (time == 10)
                {
                    return false;
                }

                return true;
            });
        }
    }
}
