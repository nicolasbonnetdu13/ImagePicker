namespace ImagePicker.iOS.ImagePicker.ViewControllers
{
    using System;
    using System.Collections.Generic;
    using AssetsLibrary;
    using CoreGraphics;
    using Foundation;
    using global::ImagePicker.iOS.ImagePicker.Cells;
    using UIKit;

    public partial class ImagePickerViewController : UICollectionViewController
    {
        public const float SelectableImageCollectionViewCellSpacing = 8f;
        public const float CollectionViewNumberOfColumn = 3;

        private UIBarButtonItem RightBarButtonItem;

        private List<ALAsset> Images;
        private List<ALAsset> SelectedImages;

        public ImagePickerViewController(IntPtr intPtr) : base(intPtr)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Images = new List<ALAsset>();
            SelectedImages = new List<ALAsset>();

            GetAllAssets();

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
            ALAsset asset = Images[(int)indexPath.Item];

            var cell = collectionView.DequeueReusableCell(SelectableImageCollectionViewCell.Identifier, indexPath) as SelectableImageCollectionViewCell;
            cell.UpdateCell(asset, SelectedImages.Contains(asset));
            return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            ALAsset asset = Images[(int)indexPath.Item];
            bool isImageSelected = SelectedImages.Contains(asset);
            if (isImageSelected)
            {
                SelectedImages.Remove(asset);
            }
            else
            {
                SelectedImages.Add(asset);
            }

            var cell = collectionView.CellForItem(indexPath) as SelectableImageCollectionViewCell;
            cell.UpdateSelectedState(!isImageSelected, true);
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
            int numberSelectedImage = SelectedImages.Count;
            Title = numberSelectedImage > 0 ? $"{numberSelectedImage} selected pictures" : "Select pictures";
            NavigationItem.RightBarButtonItem = numberSelectedImage > 0 ? RightBarButtonItem : null;
        }

        private void GetAllAssets()
        {
            var assetsLibrary = new ALAssetsLibrary();

            assetsLibrary.Enumerate(ALAssetsGroupType.All,
                                    (ALAssetsGroup group, ref bool stop) =>
                                    {
                                        group?.SetAssetsFilter(ALAssetsFilter.AllPhotos);
                                        group?.Enumerate((ALAsset asset, nint index, ref bool st) =>
                                        {
                                            int notfound = Int32.MaxValue;
                                            if (asset != null && index != notfound)
                                            {
                                                Images.Add(asset);
                                            }
                                        });
                                        CollectionView.ReloadData();
                                    },
                                    (NSError obj) =>
                                    {

                                    });
        }

        #endregion
    }
}

