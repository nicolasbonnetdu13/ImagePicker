// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace ImagePicker.iOS.ImagePicker.Cells
{
    [Register ("SelectableDateCollectionHeaderView")]
    partial class SelectableDateCollectionHeaderView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView SelectedIconImageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DateLabel != null) {
                DateLabel.Dispose ();
                DateLabel = null;
            }

            if (SelectedIconImageView != null) {
                SelectedIconImageView.Dispose ();
                SelectedIconImageView = null;
            }
        }
    }
}