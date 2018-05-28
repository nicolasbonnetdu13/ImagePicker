using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace ImagePicker.iOS.ImagePicker.Cells
{
    public partial class TakePhotoCollectionViewCell : UICollectionViewCell
    {
        public static readonly string Identifier = "TakePhotoCollectionViewCellIdentifier";
        public static readonly string NibName = "TakePhotoCollectionViewCell";
        static float CornerRadius = 8f;

        static TakePhotoCollectionViewCell()
        {
        }

        protected TakePhotoCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void UpdateCell()
        {
            BackgroundView.Layer.CornerRadius = CornerRadius;
            BackgroundView.Layer.ShadowRadius = 2;
            BackgroundView.Layer.ShadowColor = UIColor.DarkGray.CGColor;
            BackgroundView.Layer.ShadowOpacity = 0.5f;
            BackgroundView.Layer.ShadowOffset = new CGSize(0, 1);
        }
    }
}
