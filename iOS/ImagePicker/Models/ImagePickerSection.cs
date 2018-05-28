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
        public CellType Type;

        public ImagePickerSection()
        {
            Type = CellType.TakePhoto;
        }

        public ImagePickerSection(DateTime dateTime)
        {
            Type = CellType.SelectableImage;
            Date = dateTime;
            Images = new List<AssetImage>();
        }

        public enum CellType
        {
            SelectableImage,
            TakePhoto
        }
    }
}
