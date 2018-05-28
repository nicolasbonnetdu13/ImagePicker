using System;
using AssetsLibrary;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using ImagePicker.iOS.Helpers;
using ImagePicker.iOS.ImagePicker.Models;
using UIKit;

namespace ImagePicker.iOS.ImagePicker.Cells
{
    public partial class SelectableImageCollectionViewCell : UICollectionViewCell
    {
        public static readonly string Identifier = "SelectableImageCollectionViewCellIdentifier";
        public static readonly string NibName = "SelectableImageCollectionViewCell";
        public const float SelectedConstraint = 12f;

        public AssetImage Image;
        private bool IsSelected;

        private CAGradientLayer GradientLayer;

        protected SelectableImageCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    GradientLayer?.Dispose();
                    GradientLayer = null;

                    ReleaseDesignerOutlets();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while disposing TabBarController {ex}");
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public void UpdateCell(AssetImage image, bool isSelected)
        {
            MainImageView.Layer.MasksToBounds = true;
            Image = image;
            UIImageOrientation orientation = UIImageOrientation.Up;
            MainImageView.Image = new UIImage(Image.Thumbnail, 0.8f, orientation);

            SelectedIconBackgroundView.Layer.CornerRadius = SelectedIconBackgroundView.Frame.Width / 2;

            UpdateSelectedState(isSelected, false);
            CreateGradientIfNeeded();
        }

        public void UpdateSelectedState(bool isSelected, bool animated)
        {
            IsSelected = isSelected;

            SelectedIconImageView.Image = IsSelected ? UIImage.FromBundle("circle_checked") : UIImage.FromBundle("circle_unchecked");
            SelectedIconImageView.TintColor = IsSelected ? LayoutHelper.PrimaryColor : new UIColor(1f, 1f, 1f, 0.8f);
            SelectedIconBackgroundView.Hidden = !isSelected;

            MainImageViewTopConstraint.Constant = IsSelected ? SelectedConstraint : 0f;
            MainImageViewTrailingConstraint.Constant = IsSelected ? SelectedConstraint : 0f;
            MainImageViewBottomConstraint.Constant = IsSelected ? SelectedConstraint : 0f;
            MainImageViewLeadingConstraint.Constant = IsSelected ? SelectedConstraint : 0f;
            Action animation = () =>
            {
                LayoutIfNeeded();
                if (GradientLayer != null)
                {
                    GradientLayer.Frame = TopGradientView.Bounds;
                }
            };
            if (animated)
            {
                UIView.Animate(0.2f, animation);
            }
            else
            {
                animation();
            }
        }

        public void CreateGradientIfNeeded()
        {
            if (TopGradientView.Layer.Sublayers == null || TopGradientView.Layer.Sublayers?.Length == 0)
            {
                GradientLayer = new CAGradientLayer();
                GradientLayer.Frame = TopGradientView.Bounds;
                GradientLayer.NeedsDisplayOnBoundsChange = true;
                GradientLayer.MasksToBounds = true;
                GradientLayer.Colors = new CGColor[] { UIColor.FromWhiteAlpha(0f, 0.1f).CGColor, UIColor.Clear.CGColor };
                TopGradientView.Layer.AddSublayer(GradientLayer);
            }
            else
            {
                GradientLayer.Frame = TopGradientView.Bounds;
            }
        }
    }
}
