using System;

using Foundation;
using UIKit;

namespace ImagePicker.iOS.ImagePicker.Cells
{
    public partial class TakePhotoCollectionViewCell : UICollectionViewCell
    {
        public static readonly string Identifier = "TakePhotoCollectionViewCellIdentifier";
        public static readonly string NibName = "TakePhotoCollectionViewCell";

        static TakePhotoCollectionViewCell()
        {
        }

        protected TakePhotoCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
