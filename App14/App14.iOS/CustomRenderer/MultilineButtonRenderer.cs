using System;
using App14.CustomRenderer;
using App14.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Button), typeof(MultilineButtonRenderer))]
namespace App14.iOS
{
    public class MultilineButtonRenderer:ButtonRenderer
    {
        public MultilineButtonRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if(Control!=null)
                {
                Control.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
                }
                
        }
    }
}
