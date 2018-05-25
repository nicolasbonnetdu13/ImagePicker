using System;
using AssetsLibrary;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using ImagePicker.iOS.Helpers;
using UIKit;

namespace ImagePicker.iOS.ImagePicker.Cells
{
	public partial class SelectableImageCollectionViewCell : UICollectionViewCell
	{
		public static readonly string Identifier = "SelectableImageCollectionViewCellIdentifier";
		public static readonly string NibName = "SelectableImageCollectionViewCell";
		public const float SelectedConstraint = 12f;

		private ALAsset Asset;
		private bool IsSelected;

		private CAGradientLayer GradientLayer;

		protected SelectableImageCollectionViewCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void UpdateCell(ALAsset asset, bool isSelected)
		{
			MainImageView.Layer.MasksToBounds = true;
			Asset = asset;
			SetImageFromAsset(asset);
			UpdateSelectedState(isSelected, false);
			CreateGradientIfNeeded();
		}

		private void SetImageFromAsset(ALAsset asset)
		{
			var rep = asset.DefaultRepresentation;
			if (rep != null)
			{
				UIImageOrientation orientation = UIImageOrientation.Up;
				var cgImage = rep.GetFullScreenImage();
				MainImageView.Image = new UIImage(cgImage, 0.8f, orientation);
			}
		}

		public void UpdateSelectedState(bool isSelected, bool animated)
		{
			IsSelected = isSelected;

			SelectedIconImageView.Image = UIImage.FromBundle(IsSelected ? "circle_checked" : "circle_unchecked");
			SelectedIconImageView.TintColor = IsSelected ? LayoutHelper.PrimaryColor : LayoutHelper.UnselectedColor;

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
