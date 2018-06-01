using System;
using UIKit;
using Foundation;

namespace ImagePicker.iOS.Helpers
{
    public static class CameraHelper
    {
        static UIImagePickerController Picker;
        static Action<NSDictionary> Callback;

        static void Init()
        {
            if (Picker != null)
                return;

            Picker = new UIImagePickerController();
            Picker.Delegate = new CameraDelegate();
        }

        class CameraDelegate : UIImagePickerControllerDelegate
        {
            public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
            {
                var callback = Callback;
                Callback = null;

                picker.DismissModalViewController(true);
                callback(info);
            }
        }

        public static void TakePicture(UIViewController parent, Action<NSDictionary> callback)
        {
            Init();
            try
            {
                Picker.SourceType = UIImagePickerControllerSourceType.Camera;
                Callback = callback;
                parent.PresentModalViewController(Picker, true);
            }
            catch (Exception)
            {
                //UIImagePickerControllerSourceType.Camera may not be available
            }
        }

        public static void SelectPicture(UIViewController parent, Action<NSDictionary> callback)
        {
            Init();
            Picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            Callback = callback;
            parent.PresentModalViewController(Picker, true);
        }
    }
}