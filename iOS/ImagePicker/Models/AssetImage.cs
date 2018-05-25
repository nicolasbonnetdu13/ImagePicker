namespace ImagePicker.iOS.ImagePicker.Models
{
    using System;
    using AssetsLibrary;
    using global::ImagePicker.iOS.Helpers;
    using UIKit;

    public class AssetImage
    {
        public ALAsset Asset;
        public DateTime Date;
        public UIImage Thumbnail;

        public AssetImage(ALAsset asset)
        {
            Asset = asset;
            Date = asset.Date.ToDateTime().Date;

            if (asset.Thumbnail != null)
            {
                Thumbnail = new UIImage(asset.Thumbnail, 0.8f, UIImageOrientation.Up);
            }
            else
            {
                Console.WriteLine("Thumbnail was empty....");
                var rep = asset.DefaultRepresentation;
                if (rep != null)
                {
                    UIImageOrientation orientation = UIImageOrientation.Up;
                    var cgImage = rep.GetFullScreenImage();
                    Thumbnail = new UIImage(cgImage, 0.8f, orientation);
                }
            }
        }
    }
}
