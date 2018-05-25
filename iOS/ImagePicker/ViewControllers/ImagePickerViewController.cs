namespace ImagePicker.iOS.ImagePicker.ViewControllers
{
    using System;
    using System.Collections.Generic;
    using CoreGraphics;
    using Foundation;
    using global::ImagePicker.iOS.ImagePicker.Cells;
    using UIKit;

    public partial class ImagePickerViewController : UICollectionViewController
    {
        public const float SelectableImageCollectionViewCellSpacing = 8f;
        public const float CollectionViewNumberOfColumn = 3;

        private UIBarButtonItem RightBarButtonItem;

        private List<bool> Images;

        public ImagePickerViewController(IntPtr intPtr) : base(intPtr)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var rand = new Random();
            Images = new List<bool>();

            for (int i = 0; i < 30; i++)
            {
                Images.Add(false);
            }

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Cancel",
                                                                   UIBarButtonItemStyle.Plain,
                                                                   (sender, e) => { DismissModalViewController(true); });

            RightBarButtonItem = new UIBarButtonItem("Next",
                                                     UIBarButtonItemStyle.Plain,
                                                     (sender, e) => { DismissModalViewController(true); });

            CollectionView.Delegate = this;
            CollectionView.RegisterNibForCell(UINib.FromName(SelectableImageCollectionViewCell.NibName, null), SelectableImageCollectionViewCell.Identifier);

            UpdateNavigationBar();
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            coordinator.AnimateAlongsideTransition((obj) => CollectionView.ReloadData(), (obj) => { });
        }

        #region CollectionView

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return Images.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            int index = (int)indexPath.Item;
            var cell = collectionView.DequeueReusableCell(SelectableImageCollectionViewCell.Identifier, indexPath) as SelectableImageCollectionViewCell;
            cell.UpdateCell(Images[index]);
            return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            int index = (int)indexPath.Item;
            Images[index] = !Images[index];

            var cell = collectionView.CellForItem(indexPath) as SelectableImageCollectionViewCell;
            cell.UpdateCell(Images[index]);
            UpdateNavigationBar();
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            nfloat ImageWidth = (collectionView.Frame.Width - SelectableImageCollectionViewCellSpacing * (CollectionViewNumberOfColumn - 1)) / CollectionViewNumberOfColumn;
            return new CGSize(ImageWidth, ImageWidth);
        }

        #endregion

        #region Private methods

        private void UpdateNavigationBar()
        {
            int numberSelectedImage = Images.FindAll((bool obj) => { return obj; }).Count;
            Title = numberSelectedImage > 0 ? $"{numberSelectedImage} selected pictures" : "Select pictures";
            NavigationItem.RightBarButtonItem = numberSelectedImage > 0 ? RightBarButtonItem : null;
        }

        #endregion
    }
}

