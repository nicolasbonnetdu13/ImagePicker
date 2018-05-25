// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ImagePicker.iOS.ImagePicker.ViewControllers
{
    [Register ("ImagePickerViewController")]
    partial class ImagePickerViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView CollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ToasterLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ToasterView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CollectionView != null) {
                CollectionView.Dispose ();
                CollectionView = null;
            }

            if (ToasterLabel != null) {
                ToasterLabel.Dispose ();
                ToasterLabel = null;
            }

            if (ToasterView != null) {
                ToasterView.Dispose ();
                ToasterView = null;
            }
        }
    }
}