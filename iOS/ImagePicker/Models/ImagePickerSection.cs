namespace ImagePicker.iOS.ImagePicker.Models
{
    using System;
    using System.Collections.Generic;
    using AssetsLibrary;

    public class ImagePickerSection
    {
        public DateTime Date;
        public List<AssetImage> Images;
        public bool Selected;

        public ImagePickerSection(DateTime dateTime)
        {
            Date = dateTime;
            Images = new List<AssetImage>();
        }
    }
}
