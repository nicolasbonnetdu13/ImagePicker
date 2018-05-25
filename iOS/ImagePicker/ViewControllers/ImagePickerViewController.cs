using System;

using UIKit;

namespace ImagePicker.iOS.ImagePicker.ViewControllers
{
	public partial class ImagePickerViewController : UICollectionViewController
	{
		public ImagePickerViewController(IntPtr intPtr) : base(intPtr)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

