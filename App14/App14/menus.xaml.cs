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
    public partial class menus : ContentView
    {
        location loc = new location();
        public event EventHandler ItemTapped;
        private bool _isAnimating = false;
        private uint _animationDelay = 100;

        public menus()
        {
            InitializeComponent();

            devices.IsVisible = false;
            homeGray.IsVisible = false;
            signoutGray.IsVisible = false;
            help.IsVisible = false;
            schedule.IsVisible = false;
            ticketsGray.IsVisible = false;
            warningGray.IsVisible = false;
            HandleOptionsClicked2();
            HandleMenuCenterClicked();
            HandleCloseClicked();
        }

        private void HandleOptionsClicked2()
        {
            try
            {
                HandleOptionClicked(devices, "devices");
                HandleOptionClicked(homeGray, "homeGray");
                HandleOptionClicked(signoutGray, "signoutGray");
                HandleOptionClicked(schedule, "schedule");
                HandleOptionClicked(ticketsGray, "ticketsGray");
                HandleOptionClicked(warningGray, "warningGray");
                HandleOptionClicked(closecircle, "closecircle");
                HandleOptionClicked(menucircle, "menucircle");
            }
            catch { }
        }
        private void HandleOptionClicked(Image image, string value)
        {
            try
            {
                image.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        ItemTapped?.Invoke(this, new SelectedItemChangedEventArgs(value));
                       // await CloseMenu();
                    }),
                    NumberOfTapsRequired = 1
                });
            }
            catch { }
        }

        
        private void HandleCloseClicked()
        {
            try
            {
                closecircle.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        await CloseMenu();
                    }),
                    NumberOfTapsRequired = 1
                });
            }
            catch { }
        }
        private async Task CloseMenu()
        {
            try
            {
                if (!_isAnimating)
                {

                    _isAnimating = true;

                    closecircle.IsVisible = true;
                    await HideButtons();

                    await closecircle.RotateTo(0, _animationDelay);
                    closecircle.IsVisible = false;
                    menucircle.IsVisible = true;
                    //await closecircle.FadeTo(0, _animationDelay);
                    //await menucircle.RotateTo(0, _animationDelay);
                    //await menucircle.FadeTo(1, _animationDelay);
                    //await OuterCircle.ScaleTo(1, 1000, Easing.BounceOut);
                    _isAnimating = false;
                }
            }
            catch { }
        }

        private void HandleMenuCenterClicked()
        {
            try
            {
                menucircle.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        if (!_isAnimating)
                        {
                            _isAnimating = true;

                            closecircle.IsVisible = true;
                            await menucircle.RotateTo(360, _animationDelay);
                            menucircle.IsVisible = false;
                            await closecircle.RotateTo(360, _animationDelay);
                            await ShowButtons();
                            _isAnimating = false;
                        }
                    }),
                    NumberOfTapsRequired = 1
                });
            }
            catch { }
        }

        private async Task HideButtons()
        {
            try
            {
                var speed = 5U;                

                await homeGray.TranslateTo(0, 0, speed);  //await homeGray.FadeTo(1, speed);
                homeGray.IsVisible = false;
                await warningGray.TranslateTo(0, 0, speed);  //await warningGray.FadeTo(1, speed);
                warningGray.IsVisible = false;
                await devices.TranslateTo(0, 0, speed); //await devices.FadeTo(1, speed);
                devices.IsVisible = false;
                await ticketsGray.TranslateTo(0, 0, speed);  // await ticketsGray.FadeTo(1, speed);
                ticketsGray.IsVisible = false;
                await schedule.TranslateTo(0, 0, speed);  //await signoutGray.FadeTo(1, speed);
                schedule.IsVisible = false;
                await help.TranslateTo(0, 0, speed);  //await signoutGray.FadeTo(1, speed);
                help.IsVisible = false;
                await signoutGray.TranslateTo(0, 0, speed);  //await signoutGray.FadeTo(1, speed);
                signoutGray.IsVisible = false;
            }
            catch { }
        }

        private async Task ShowButtons()
        {
            try
            {
                var speed = 10U;

                //signoutGray.IsVisible = true;
                //await signoutGray.TranslateTo(-3, -40, speed);  //await signoutGray.FadeTo(1, speed);
                //ticketsGray.IsVisible = true;
                //await ticketsGray.TranslateTo(-9, -80, speed);  // await ticketsGray.FadeTo(1, speed);
                //devices.IsVisible = true;
                //await devices.TranslateTo(-18, -120, speed); //await devices.FadeTo(1, speed);
                //warningGray.IsVisible = true;
                //await warningGray.TranslateTo(-31, -160, speed);  //await warningGray.FadeTo(1, speed);
                //homeGray.IsVisible = true;
                //await homeGray.TranslateTo(-50, -200, speed);  //await homeGray.FadeTo(1, speed);



                signoutGray.IsVisible = true;
                await signoutGray.TranslateTo(-6, -65, speed);  //await signoutGray.FadeTo(1, speed);
                help.IsVisible = true;
                await help.TranslateTo(-12, -130, speed);  //await signoutGray.FadeTo(1, speed);
                ticketsGray.IsVisible = true;
                await schedule.TranslateTo(-24, -195, speed);  //await signoutGray.FadeTo(1, speed);
                schedule.IsVisible = true;
                await ticketsGray.TranslateTo(-48, -260, speed);  // await ticketsGray.FadeTo(1, speed);
                devices.IsVisible = true;
                await devices.TranslateTo(-90, -320, speed); //await devices.FadeTo(1, speed);
                warningGray.IsVisible = true;
                await warningGray.TranslateTo(-135, -375, speed);  //await warningGray.FadeTo(1, speed);
                homeGray.IsVisible = true;
                await homeGray.TranslateTo(-185, -430, speed);  //await homeGray.FadeTo(1, speed);

            }
            catch { }
        }


    }
}