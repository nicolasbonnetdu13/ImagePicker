using System;

using Foundation;
using UIKit;

namespace ImagePicker.iOS.ImagePicker.Cells
{
	public partial class SelectableImageCollectionViewCell : UICollectionViewCell
	{
		public static readonly string Key = "SelectableImageCollectionViewCellIdentifier";
		public static readonly string NibName = "SelectableImageCollectionViewCell";

		protected SelectableImageCollectionViewCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
	}
}
