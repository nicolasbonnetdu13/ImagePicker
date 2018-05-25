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
    [Register ("SelectableImageCollectionViewCell")]
    partial class SelectableImageCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView MainImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MainImageViewBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MainImageViewLeadingConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MainImageViewTopConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MainImageViewTrailingConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SelectedIconBackgroundView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView SelectedIconImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TopGradientView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MainImageView != null) {
                MainImageView.Dispose ();
                MainImageView = null;
            }

            if (MainImageViewBottomConstraint != null) {
                MainImageViewBottomConstraint.Dispose ();
                MainImageViewBottomConstraint = null;
            }

            if (MainImageViewLeadingConstraint != null) {
                MainImageViewLeadingConstraint.Dispose ();
                MainImageViewLeadingConstraint = null;
            }

            if (MainImageViewTopConstraint != null) {
                MainImageViewTopConstraint.Dispose ();
                MainImageViewTopConstraint = null;
            }

            if (MainImageViewTrailingConstraint != null) {
                MainImageViewTrailingConstraint.Dispose ();
                MainImageViewTrailingConstraint = null;
            }

            if (SelectedIconBackgroundView != null) {
                SelectedIconBackgroundView.Dispose ();
                SelectedIconBackgroundView = null;
            }

            if (SelectedIconImageView != null) {
                SelectedIconImageView.Dispose ();
                SelectedIconImageView = null;
            }

            if (TopGradientView != null) {
                TopGradientView.Dispose ();
                TopGradientView = null;
            }
        }
    }
}