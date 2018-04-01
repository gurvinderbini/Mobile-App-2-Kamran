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
    public partial class addSchedule : ContentPage
    {
        location loc = new location();
        public static double btnLocationX;
        public static double btnLocationY;

        public addSchedule()
        {
           
            try
            {
                InitializeComponent();
                btnLocationX = location.btnMenuLocationX;
                btnLocationY = location.btnMenuLocationY;
                AbsoluteLayout.SetLayoutBounds(absMain, new Rectangle(0, 0, location.screenX, location.screenY - 25));
                AbsoluteLayout.SetLayoutBounds(backlayout, new Rectangle(0, 0, location.screenX, location.screenY));
                AbsoluteLayout.SetLayoutBounds(btnlayout, new Rectangle(45, 45, location.screenX, location.screenY));
                AbsoluteLayout.SetLayoutBounds(mainStack, new Rectangle(btnLocationX, btnLocationY, 45, 45));

                Menu.ItemTapped += (sender, e) =>
                {
                    var evnt = (SelectedItemChangedEventArgs)e;
                    string text = (string)evnt.SelectedItem;
                    if (text == "menucircle")
                    {
                        backlayout.IsVisible = true;
                        btnlayout.IsVisible = true;
                    }
                    else if (text == "closecircle")
                    {
                        btnlayout.IsVisible = false;
                        backlayout.IsVisible = false;
                    }
                };

                DateTime date2 = DateTime.Now;
                string month = date2.ToString("MM");
                string Date = date2.ToString("dd");
                string year = date2.ToString("yyyy");
                string hour = date2.ToString("hh");
                string min = date2.ToString("mm");
                
                //DisplayAlert("detail", "Y: " + year + " M: " + month + " D: " + Date + " H: " + hour + " M: " + min, "ok");
                
                startTimepick.Time = DateTime.Now.TimeOfDay;
                
                reminderPicker.Items.Add("10 mins before, Notification");
                reminderPicker.Items.Add("20 mins before, Notification");
                reminderPicker.Items.Add("30 mins before, Notification");

                reminderPicker.Title = "10 mins before, Notification";
                emailPicker.Title = "mail@example.com";
                foreach (string email in DependencyService.Get<CalendarConnect>().CalendarList())
                {
                    emailPicker.Items.Add(email);
                }
               
            }
            catch { }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new Detail());
            base.OnBackButtonPressed();
            return true;
        }

        private async void btnCreate_Clicked(object sender, EventArgs e)
        {
            try
            {
                string title = reminderTitle.Text;
                DateTime startDate = startDatepick.Date;
                string SDATE = startDate.ToString();
                SDATE = SDATE.Replace(" 12:00:00 AM", "");

                DateTime endDate = Datepick.Date;
                string EDATE = endDate.ToString();
                EDATE = EDATE.Replace(" 12:00:00 AM", "");

                //await DisplayAlert("DATE", "startDate: " + startDate.ToString("dd-MM-yyyy") + " endDate: " + endDate.ToString("dd-MM-yyyy"), "ok");

                string sTime = startTimepick.Time.ToString();
                string[] split = sTime.Split('.');
                sTime = split[0];

                string eTime = Timepick.Time.ToString();
                string[] split2 = eTime.Split('.');
                eTime = split2[0];
               // await DisplayAlert("Time", "shour: " + sTime.ToString() + " ehour: " + eTime.ToString(), "ok");

                string shour = startTimepick.Time.ToString("hh");
                string smin = startTimepick.Time.ToString("mm");

                string ehour = Timepick.Time.ToString("hh");
                string emin = Timepick.Time.ToString("mm");

                //await DisplayAlert("hour", "shour: " + shour.ToString() + " ehour: " + ehour.ToString(), "ok");
                //await DisplayAlert("MIN", "smin: " + smin.ToString() + " emin: " + emin.ToString(), "ok");

                //string sTime = shour;
                //string eTime = ehour;

                string smonth = startDate.ToString("MM");
                string sDate = startDate.ToString("dd");
                string syear = startDate.ToString("yyyy");
                string emonth = endDate.ToString("MM");
                string eDate = endDate.ToString("dd");
                string eyear = endDate.ToString("yyyy");
                
                smonth = monthConvert(smonth);

                //string totalTime = startDate.ToString("dd-MM-yyyy") + "(" + sTime + ") - " + endDate.ToString("dd-MM-yyyy") + "(" + eTime + ")";
                string totalTime = sTime + " - " + eTime;
               // await DisplayAlert("totalTime", totalTime, "ok");
                //await DisplayAlert(title, "sTime: " + sTime + " smin: " + smin + " syear: " + syear + " smonth: " + smonth + " sDate: " + sDate + " eTime: " + eTime + " emin: " + emin + " eyear: " + eyear + " emonth: " + emonth + " eDate: " + eDate, "OK");

                string respone = DependencyService.Get<CalendarConnect>().AddEvent(title, startDate, shour, smin, endDate, ehour, emin);

                SaveEvents(title, startDate.ToString("dd-MM-yyyy"), sDate, smonth, sTime, endDate.ToString("dd-MM-yyyy"), eTime, totalTime);
                await DisplayAlert("CloudSchool", "Event successfully created", "ok");

                string EmailStatus = ""; string status = "";
                string EmailIndex = "";
                // email
                try
                {
                    EmailIndex = emailPicker.Items[emailPicker.SelectedIndex]; // emailPicker.SelectedIndex;
                    /*
                    if (EmailIndex.ToString() == "0")
                    {
                        EmailStatus = "1";
                    }
                    else if (EmailIndex.ToString() == "1")
                    {
                        EmailStatus = "2";
                    }
                    else if (EmailIndex.ToString() == "2")
                    {
                        EmailStatus = "3";
                    }
                    else
                    {
                        EmailStatus = "1";
                    }
                    */
                }
                catch { }
                // reminder
                try
                {
                    var reminderIndex = reminderPicker.SelectedIndex;
                    if (reminderIndex.ToString() == "0")
                    {
                        status = "1";
                    }
                    else if (reminderIndex.ToString() == "1")
                    {
                        status = "2";
                    }
                    else if (reminderIndex.ToString() == "2")
                    {
                        status = "3";
                    }
                    else
                    {
                        status = "1";
                    }
                }
                catch { }

                App.NavigateMasterDetail(new addSchedule());
                // DisplayAlert("Status", "EmailPick: " + EmailIndex + " ReminderStatus: " + status + " Date: " + date.ToString() + " time: " + time.ToString(),  "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("error in Create Event", "Error: " + ex.Message, "ok");
            }
        }

        private string monthConvert(string id)
        {
            string monthInEng = "";
            try
            {
                int month = Convert.ToInt16(id);
                if (month == 01)
                {
                    monthInEng = "JAN";
                }
                else if(month == 02)
                {
                    monthInEng = "FEB";
                }
                else if (month == 03)
                {
                    monthInEng = "MAR";
                }
                else if (month == 04)
                {
                    monthInEng = "APR";
                }
                else if (month == 05)
                {
                    monthInEng = "MAY";
                }
                else if (month == 06)
                {
                    monthInEng = "JUN";
                }
                else if (month == 07)
                {
                    monthInEng = "JULY";
                }
                else if (month == 08)
                {
                    monthInEng = "AUG";
                }
                else if (month == 09)
                {
                    monthInEng = "SEP";
                }
                else if (month ==10)
                {
                    monthInEng = "OCT";
                }
                else if (month == 11)
                {
                    monthInEng = "NOV";
                }
                else
                {
                    monthInEng = "DEC";
                }
            }
            catch { }
            return monthInEng;
        }

        public async void SaveEvents(string remTitle, string sDatepick, string sDate, string sMonth, string sTimepick, string eDatepick, string eTimepick, string totalTime)
        {
            try
            {
                EventsList OReg = new EventsList();
                OReg.title = remTitle;
                OReg.startDate = sDatepick;
                OReg.strtDate = sDate;
                OReg.strtMonth = sMonth;
                OReg.startTime = sTimepick;
                OReg.endDate = eDatepick;
                OReg.endTime = eTimepick;
                OReg.overallTime = totalTime;
                int result = await App.Database.SaveEvents(OReg);
                await DisplayAlert("res", result.ToString(), "ok");               
            }
            catch(Exception e)
            {
               await DisplayAlert("error", e.Message, "ok");
            }

        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.NavigateMasterDetail(new Detail());
            }
            catch { }
        }

        private void reminderPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void emailPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void event_lists_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Events());
            }
            catch { }
        }
    }
}