namespace ImagePicker.iOS.Helpers
{
    using System;
    using CoreGraphics;
    using UIKit;

    public static class ImageHelper
    {
        public static UIImage ImageWithColor(UIColor color)
        {
            var rect = new CGRect(0, 0, 1, 1);
            UIGraphics.BeginImageContext(rect.Size);
            var context = UIGraphics.GetCurrentContext();
            context.SetFillColor(color.CGColor);
            context.FillRect(rect);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image;
        }
    }
}
