using System;
using System.Collections.Generic;
using System.Linq;
using App14.Helpers;
using App14.Models;
using Foundation;
using Plugin.FirebasePushNotification;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace App14.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override  bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

            FirebasePushNotificationManager.Initialize(options, true);
            CrossFirebasePushNotification.Current.RegisterForPushNotifications();

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
             //   UserDialogs.Instance.AlertAsync(p.Token);

                UIPasteboard clipboard = UIPasteboard.General;
                clipboard.String = p.Token;
                Settings.DeviceToken = p.Token;
                //var stack = new StackLayout();
                //var btn = new Button() {Text = "Click"};
                //stack.Children.Add(new Entry() { Text = p.Token });
                //stack.Children.Add(btn);
                //var testpage = new TestPage(){Content = stack};
                //App.Current.MainPage = testpage;
                //btn.Clicked += (ss, e) => { App.Current.MainPage = new Login(); };


            };
            //      Push message received event usage sample:

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {


                System.Diagnostics.Debug.WriteLine("Received");
                Dictionary<string, string> dic = p.Data as Dictionary<string, string>;

                NotificationBO notificationBo = new NotificationBO();



                foreach (var item in p.Data)
                {
                    if (item.Key.Contains("title"))
                    {
                        notificationBo.Title = Convert.ToString(item.Value);
                    }
                    if (item.Key.Contains("message"))
                    {
                        notificationBo.Message = Convert.ToString(item.Value);
                    }
                    if (item.Key.Contains("screen"))
                    {
                        notificationBo.Screen = Convert.ToString(item.Value);
                    }
                    if (item.Key.Contains("body"))
                    {
                        notificationBo.Body = Convert.ToString(item.Value);
                    }
                    if (item.Key == "sound")
                    {
                        notificationBo.Sound = Convert.ToString(item.Value);
                    }
                    if (item.Key.Contains("content_available"))
                    {
                        notificationBo.ContentAvailable = Convert.ToString(item.Value);
                    }
                }

                App.Database.InsertNotification(notificationBo);


                ProcessNotification(dic, false);
            };
            // Push message opened event usage sample:


            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {


                App.Current.MainPage = new MainPage()
                {
                    Detail = new NavigationPage(new NotificationsPage())
                };

                //System.Diagnostics.Debug.WriteLine("Opened");
                //foreach (var data in p.Data)
                //{
                //    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                //}

                //if (!string.IsNullOrEmpty(p.Identifier))
                //{
                //    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                //}

            };

            LoadApplication(new App());


            SlideOverKit.iOS.SlideOverKit.Init();
            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);

        }
        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
           FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);



        }



        public override void OnActivated(UIApplication uiApplication)
        {
            FirebasePushNotificationManager.Connect();

        }
        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
            FirebasePushNotificationManager.Disconnect();
        }

        void ProcessNotification(Dictionary<string, string> options, bool fromFinishedLaunching)
        {
            string aps = String.Empty;
            // Check to see if the dictionary has the aps key.  This is the notification payload you would have sent

            string alert = string.Empty;
            if (null != options)
            {
                var keys = options.Keys;
                foreach (var val in options)
                {
                    if (val.Key.Contains("message"))
                    {
                        alert = Convert.ToString(val.Value);
                    }
                }
                //Get the aps dictionary
                //  var aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                //Extract the alert text
                // NOTE: If you're using the simple alert by just specifying
                // "  aps:{alert:"alert msg here"}  ", this will work fine.
                // But if you're using a complex alert with Localization keys, etc.,
                // your "alert" object from the aps dictionary will be another NSDictionary.
                // Basically the JSON gets dumped right into a NSDictionary,
                // so keep that in mind.


                //  if (aps.ContainsKey(new NSString("alert")))
                //    alert = (aps[new NSString("alert")] as NSString).ToString();

                //If this came from the ReceivedRemoteNotification while the app was running,
                // we of course need to manually process things like the sound, badge, and alert.
                if (!fromFinishedLaunching)
                {
                    //Manually show an alert
                    if (!string.IsNullOrEmpty(alert))
                    {
                        UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);

                        avAlert.Show();
                    }
                }
            }
        }
        //public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        //{

        //    var mainPage = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Last();
        //    if (mainPage is MasterDetailPage)
        //    {
        //        if (((MasterDetailPage)mainPage).Detail.Navigation.NavigationStack.Last() is WebViewPage)
        //        {
        //            return UIInterfaceOrientationMask.LandscapeRight;
        //        }
        //    }
        //    return UIInterfaceOrientationMask.Portrait;
        //}
    
    }
}
