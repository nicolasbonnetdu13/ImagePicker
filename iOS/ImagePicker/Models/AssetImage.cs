namespace ImagePicker.iOS.ImagePicker.Models
{
    using System;
    using System.Threading.Tasks;
    using AssetsLibrary;
    using CoreGraphics;
    using global::ImagePicker.iOS.Helpers;
    using UIKit;

    public class AssetImage
    {
        public ALAsset Asset;
        public DateTime Date;
        public CGImage Thumbnail;

        public AssetImage(ALAsset asset)
        {
            Asset = asset;
            Date = asset.Date.ToDateTime().Date;

            if (Asset.Thumbnail != null)
            {
                Thumbnail = Asset.Thumbnail;
            }
            else
            {
                var rep = Asset.DefaultRepresentation;
                if (rep != null)
                {
                    Thumbnail = rep.GetFullScreenImage();
                }
            }
        }
    }
}
