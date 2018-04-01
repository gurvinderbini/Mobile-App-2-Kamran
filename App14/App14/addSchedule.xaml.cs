using App14.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SlideOverKit;
using App14.iOS.RightSideMenu;

namespace App14
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class addSchedule : MenuContainerPage
    {
        location loc = new location();
        public static double btnLocationX;
        public static double btnLocationY;

        public addSchedule()
        {
            try
            {
                InitializeComponent();
                AbsoluteLayout.SetLayoutBounds(absMain, new Rectangle(0, 0, location.screenX, location.screenY - 25));
                DateTime now = DateTime.Now.ToLocalTime();
                lblDatePicked.Text = getSpecificDateFormat(now);
                lblEndTimePicked.Text = getSpecificTimeFormat2(addHourInCurrentTime(now, 1));

                lblSDatePicked.Text = getSpecificDateFormat(now);
                string time = getSpecificTimeFormat2(addHourInCurrentTime(now, 0));
                lblStartTimePicked.Text = time;
                this.SlideMenu = new RightSideMasterPage5();

                /*btnLocationX = location.btnMenuLocationX;
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
                */
                DateTime date2 = DateTime.Now;
                string month = date2.ToString("MM");
                string Date = date2.ToString("dd");
                string year = date2.ToString("yyyy");
                string hour = date2.ToString("hh");
                string min = date2.ToString("mm");

                //DisplayAlert("detail", "Y: " + year + " M: " + month + " D: " + Date + " H: " + hour + " M: " + min, "ok");
                //string time2 = DateTime.Now.TimeOfDay.ToString();
                //DisplayAlert("time2", time2, "ok");
                //startTimepick.Time = DateTime.Now.TimeOfDay;

                reminderPicker.Items.Add("10 mins before, Notification");
                reminderPicker.Items.Add("20 mins before, Notification");
                reminderPicker.Items.Add("30 mins before, Notification");
                reminderPicker.SelectedIndex = 2;

                foreach (string email in DependencyService.Get<CalendarConnect>().CalendarList())
                {
                    emailPicker.Items.Add(email);
                }

                // on save click
                try
                {
                    var tapGestureSave = new TapGestureRecognizer();
                    tapGestureSave.Tapped += (s, e) =>
                    {
                        //DisplayAlert("save", ".", ".");
                        SaveEventOnLabelClick();
                    };
                    lblSave.GestureRecognizers.Add(tapGestureSave);
                }
                catch { }
                // on cancel click
                try
                {
                    var tapGestureCancel = new TapGestureRecognizer();
                    tapGestureCancel.Tapped += (s, e) =>
                    {
                        //DisplayAlert("cancel", ".", ".");
                        CancelEventOnLabelClick();
                    };
                    lblCancel.GestureRecognizers.Add(tapGestureCancel);
                }
                catch { }

            }
            catch { }
        }

        public DateTime addHourInCurrentTime(DateTime date, int hour)
        {
            date = date.AddHours(hour);
            return date;
        }

        public string getSpecificDateFormat(DateTime date)
        {
            string dt = "";
            try
            {
                dt = date.ToString("ddd, dd MMM yyyy");
            }
            catch { }
            return dt;
        }

        public string getSpecificTimeFormat(TimeSpan time)
        {
            string dt = "";
            try
            {
                dt = time.ToString("hh:mm:ss");
            }
            catch { }
            return dt;
        }

        public string getSpecificTimeFormat2(DateTime time)
        {
            string dt = "";
            try
            {
                dt = time.ToString("hh:mm:ss");
            }
            catch { }
            return dt;
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new CalendarView());
            base.OnBackButtonPressed();
            return true;
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
                else if (month == 02)
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
                else if (month == 10)
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

        public string[] ChangeDateFormat(DateTime date)
        {
            string dt = date.ToString();
            string[] dtArr = dt.Split(' ');
            return dtArr;

        }

        public string ChangeTimeFormat(DateTime time)
        {
            string dt = time.ToString();
            string[] dtArr = dt.Split(' ');
            return dtArr[0];
        }

        private async void SaveEventOnLabelClick()
        {
            try
            {
                string title = reminderTitle.Text;
                if (!string.IsNullOrWhiteSpace(title))
                {
                    DateTime startDate = startDatepick.Date;
                    DateTime endDate = Datepick.Date;

                    string SDATE = startDatepick.Date.ToString("dd-MM-yyyy");
                    string EDATE = Datepick.Date.ToString("dd-MM-yyyy");
                    string sTime = startTimepick.Time.ToString();
                    string eTime = EndTimepick.Time.ToString();

                    //await DisplayAlert("DATE", "SDATE: " + SDATE + " EDATE: " + EDATE, "ok");                    
                    //await DisplayAlert("TIME", "sTime: " + sTime + " eTime: " + eTime, "ok");
                    
                    string shour = startTimepick.Time.ToString("hh");
                    string smin = startTimepick.Time.ToString("mm");

                    string ehour = EndTimepick.Time.ToString("hh");
                    string emin = EndTimepick.Time.ToString("mm");
                                        
                    string smonth = startDate.ToString("MM");
                    smonth = monthConvert(smonth);
                    string sDate = startDate.ToString("dd");
                    string syear = startDate.ToString("yyyy");
                    string emonth = endDate.ToString("MM");
                    string eDate = endDate.ToString("dd");
                    string eyear = endDate.ToString("yyyy");

                    string overallTime = sTime + " - " + eTime;
                  //  await DisplayAlert(title, "sTime: " + sTime + " smin: " + smin + " syear: " + syear + " smonth: " + smonth + " sDate: " + sDate + " eTime: " + eTime + " emin: " + emin + " eyear: " + eyear + " emonth: " + emonth + " eDate: " + eDate, "OK");

                    string respone = DependencyService.Get<CalendarConnect>().AddEvent(title, startDate, shour, smin, endDate, ehour, emin);

                    SaveEvents(title, SDATE, sDate, smonth, sTime, EDATE, eTime, overallTime);
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
                else {
                    await DisplayAlert("Title", "Title can not be Empty", "OK");
                    reminderTitle.Focus();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("error in Create Event", "Error: " + ex.Message, "ok");
            }
        }

        public async void SaveEvents(string remTitle, string sDatepick, string sDate, string smonth, string sTimepick, string eDatepick, string eTimepick, string overallTime)
        {
            try
            {
                EventsList OReg = new EventsList();
                OReg.title = remTitle;
                OReg.startDate = sDatepick;
                OReg.strtDate = sDate;
                OReg.strtMonth = smonth;
                OReg.startTime = sTimepick;
                OReg.endDate = eDatepick;
                OReg.endTime = eTimepick;
                OReg.overallTime = overallTime;
                int result = await App.Database.SaveEvents(OReg);
                //await DisplayAlert("res", result.ToString(), "ok");               
            }
            catch (Exception e)
            {
                await DisplayAlert("error", e.Message, "ok");
            }

        }

        private void CancelEventOnLabelClick()
        {
            try
            {
                App.NavigateMasterDetail(new CalendarView());
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
            this.ShowMenu();
            //try
            //{
            //    await Navigation.PushAsync(new Events());
            //}
            //catch { }
        }

        private void Datepick_DateSelected(object sender, DateChangedEventArgs e)
        {

            lblDatePicked.Text = getSpecificDateFormat(Datepick.Date);
        }

        private void startDatepick_DateSelected(object sender, DateChangedEventArgs e)
        {
            lblSDatePicked.Text = getSpecificDateFormat(startDatepick.Date);
        }
        
        private void startTimepick_PropertyChanged_1(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                 lblStartTimePicked.Text = startTimepick.Time.ToString();
            }
        }

        private void EndTimepick_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                lblEndTimePicked.Text = EndTimepick.Time.ToString();
            }
        }
    }
}