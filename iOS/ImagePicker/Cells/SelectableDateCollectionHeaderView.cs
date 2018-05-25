namespace ImagePicker.iOS.ImagePicker.Cells
{
    using System;
    using Foundation;
    using global::ImagePicker.iOS.Helpers;
    using UIKit;

    public partial class SelectableDateCollectionHeaderView : UICollectionReusableView
    {
        public static readonly string Identifier = "SelectableDateCollectionHeaderViewIdentifier";
        public static readonly string NibName = "SelectableDateCollectionHeaderView";

        public DateTime DateTime;
        private bool IsSelected;

        protected SelectableDateCollectionHeaderView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
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

        public void UpdateHeader(DateTime dateTime, bool isSelected)
        {
            DateTime = dateTime;
            using (NSDateFormatter formatter = new NSDateFormatter())
            {
                formatter.DateStyle = NSDateFormatterStyle.Long;
                formatter.TimeStyle = NSDateFormatterStyle.None;
                DateLabel.Text = formatter.ToString(DateTime.ToNSDate());
            }
            UpdateSelectedState(isSelected, false);
        }

        public void UpdateSelectedState(bool isSelected, bool animated)
        {
            IsSelected = isSelected;
            SelectedIconImageView.Image = UIImage.FromBundle(IsSelected ? "circle_checked" : "circle_unchecked");
            SelectedIconImageView.TintColor = IsSelected ? LayoutHelper.PrimaryColor : new UIColor(0f, 0f, 0f, 0.3f);
        }
    }
}
