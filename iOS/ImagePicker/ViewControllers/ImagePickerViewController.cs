﻿namespace ImagePicker.iOS.ImagePicker.ViewControllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AssetsLibrary;
    using CoreGraphics;
    using Foundation;
    using global::ImagePicker.iOS.Helpers;
    using global::ImagePicker.iOS.ImagePicker.Cells;
    using global::ImagePicker.iOS.ImagePicker.Models;
    using UIKit;

    public partial class ImagePickerViewController : UIViewController, IUICollectionViewDataSource, IUICollectionViewDelegate, IUIScrollViewDelegate, IUICollectionViewSource
    {
        public const float SelectableImageCollectionViewCellSpacing = 10f;
        public const float MaximumImageWidth = 80f;
        static NSString headerId = new NSString("Header");

        private UIBarButtonItem RightBarButtonItem;
        private List<UITapGestureRecognizer> HeaderTapGestureRecognizers;

        private List<AssetImage> Images;
        private List<ALAsset> SelectedImages;
        private List<ImagePickerSection> Sections;

        public ImagePickerViewController(IntPtr intPtr) : base(intPtr)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Images = new List<AssetImage>();
            SelectedImages = new List<ALAsset>();
            Sections = new List<ImagePickerSection> { new ImagePickerSection() };
            HeaderTapGestureRecognizers = new List<UITapGestureRecognizer>();

            GetAllAssets();

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Cancel",
                                                                   UIBarButtonItemStyle.Plain,
                                                                   (sender, e) =>
                                                                   {
                                                                       DismissModalViewController(true);
                                                                   });

            RightBarButtonItem = new UIBarButtonItem("Next",
                                                     UIBarButtonItemStyle.Plain,
                                                     (sender, e) =>
                                                     {
                                                         DismissModalViewController(true);
                                                     });

            CollectionView.Delegate = this;
            CollectionView.DataSource = this;
            CollectionView.RegisterNibForCell(UINib.FromName(SelectableImageCollectionViewCell.NibName, null), SelectableImageCollectionViewCell.Identifier);
            CollectionView.RegisterNibForCell(UINib.FromName(TakePhotoCollectionViewCell.NibName, null), TakePhotoCollectionViewCell.Identifier);
            CollectionView.RegisterNibForSupplementaryView(UINib.FromName(SelectableDateCollectionHeaderView.NibName, null), UICollectionElementKindSection.Header, SelectableDateCollectionHeaderView.Identifier);
            CollectionView.RegisterClassForSupplementaryView(typeof(UICollectionReusableView), UICollectionElementKindSection.Header, headerId);

            //Remove the extra Header height from Take Photo cell
            CollectionView.ContentInset = new UIEdgeInsets(-50, 0, 0, 0);
            CollectionView.SetCollectionViewLayout(CreateCollectionViewLayout(), false);

            UpdateNavigationBar();
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            coordinator.AnimateAlongsideTransition((obj) =>
            {
                CollectionView?.SetCollectionViewLayout(CreateCollectionViewLayout(), true);
                CollectionView?.ReloadData();
            },
                                                   (obj) => { });
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    RightBarButtonItem?.Dispose();
                    RightBarButtonItem = null;

                    foreach (var gesture in HeaderTapGestureRecognizers)
                    {
                        gesture?.Dispose();
                    }
                    HeaderTapGestureRecognizers.Clear();

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

        #region CollectionView

        [Export("numberOfSectionsInCollectionView:")]
        public nint NumberOfSections(UICollectionView collectionView)
        {
            return Sections.Count;
        }

        [Export("collectionView:numberOfItemsInSection:")]
        public nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            var imagePickerSection = Sections[(int)section];
            switch (imagePickerSection.Type)
            {
                case ImagePickerSection.CellType.SelectableImage:
                    return imagePickerSection.Images.Count;
                case ImagePickerSection.CellType.TakePhoto:
                    return 1;
            }
            return 0;
        }

        [Export("collectionView:viewForSupplementaryElementOfKind:atIndexPath:")]
        public UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            var section = Sections[indexPath.Section];
            switch (section.Type)
            {
                case ImagePickerSection.CellType.SelectableImage:
                    {
                        using (var headerKind = new NSString("UICollectionElementKindSectionHeader"))
                        {
                            if (elementKind.Equals(headerKind) && section.Type == ImagePickerSection.CellType.SelectableImage)
                            {
                                var headerView = collectionView.DequeueReusableSupplementaryView(elementKind, SelectableDateCollectionHeaderView.Identifier, indexPath) as SelectableDateCollectionHeaderView;
                                headerView.UpdateHeader(section.Date, section.Selected);
                                headerView.AddGestureRecognizer(CreateHeaderTapGesture());
                                return headerView;
                            }
                        }
                        break;
                    }
                case ImagePickerSection.CellType.TakePhoto:
                    return collectionView.DequeueReusableSupplementaryView(elementKind, headerId, indexPath);
            }
            return null;
        }

        [Export("collectionView:cellForItemAtIndexPath:")]
        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var section = Sections[indexPath.Section];
            switch (section.Type)
            {
                case ImagePickerSection.CellType.SelectableImage:
                    {
                        var image = section.Images[(int)(indexPath.Item)];

                        var cell = collectionView.DequeueReusableCell(SelectableImageCollectionViewCell.Identifier, indexPath) as SelectableImageCollectionViewCell;
                        cell.UpdateCell(image, SelectedImages.Contains(image.Asset));
                        return cell;
                    }
                case ImagePickerSection.CellType.TakePhoto:
                    {
                        var cell = collectionView.DequeueReusableCell(TakePhotoCollectionViewCell.Identifier, indexPath) as TakePhotoCollectionViewCell;
                        cell.UpdateCell();
                        return cell;
                    }
            }
            return null;
        }

        [Export("collectionView:shouldSelectItemAtIndexPath:")]
        public bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var section = Sections[indexPath.Section];

            switch (section.Type)
            {
                case ImagePickerSection.CellType.SelectableImage:
                    {
                        var image = Sections[indexPath.Section].Images[(int)(indexPath.Item)];
                        bool isImageSelected = SelectedImages.Contains(image.Asset);
                        if (SelectedImages.Count < 10 || isImageSelected)
                        {
                            return true;
                        }
                        else
                        {
                            SetToasterText("You can select a maximum of 10 images");
                            return false;
                        }
                    }
                case ImagePickerSection.CellType.TakePhoto:
                    return true;
            }
            return false;
        }

        [Export("collectionView:didSelectItemAtIndexPath:")]
        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var section = Sections[(int)indexPath.Section];
            switch (section.Type)
            {
                case ImagePickerSection.CellType.SelectableImage:
                    {
                        var image = Sections[indexPath.Section].Images[(int)(indexPath.Item)];
                        bool isImageSelected = SelectedImages.Contains(image.Asset);
                        if (isImageSelected)
                        {
                            UnselectImage(image.Asset, indexPath.Section);
                        }
                        else
                        {
                            SelectImageIfPossible(image.Asset, indexPath.Section);
                        }

                        var cell = collectionView.CellForItem(indexPath) as SelectableImageCollectionViewCell;
                        cell.UpdateSelectedState(!isImageSelected, true);
                        break;
                    }
                case ImagePickerSection.CellType.TakePhoto:
                    {
                        var controller = new UIViewController();
                        controller.View.BackgroundColor = UIColor.White;
                        var navigationController = new UINavigationController(controller);
                        controller.NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Cancel",
                                                                                          UIBarButtonItemStyle.Plain,
                                                                                          (sender, e) =>
                                                                                          {
                                                                                              controller.DismissModalViewController(true);
                                                                                          });
                        PresentModalViewController(navigationController, true);
                        break;
                    }
            }
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var section = Sections[(int)indexPath.Section];
            switch (section.Type)
            {
                case ImagePickerSection.CellType.SelectableImage:
                    {
                        var numberOfColumn = (int)(CollectionView.Frame.Width / MaximumImageWidth);
                        nfloat ImageWidth = (collectionView.Frame.Width - SelectableImageCollectionViewCellSpacing * (numberOfColumn - 1)) / numberOfColumn;
                        return new CGSize(ImageWidth, ImageWidth);
                    }
                case ImagePickerSection.CellType.TakePhoto:
                    return new CGSize(collectionView.Frame.Width, 100);
            }
            return new CGSize();
        }

        #endregion

        #region Private methods

        private UICollectionViewFlowLayout CreateCollectionViewLayout()
        {
            var layout = new UICollectionViewFlowLayout();
            layout.HeaderReferenceSize = new CGSize(CollectionView.Frame.Width, 50);
            CollectionView.SetCollectionViewLayout(layout, false);
            return layout;
        }

        private void UpdateNavigationBar()
        {
            int numberSelectedImage = SelectedImages.Count;
            Title = numberSelectedImage > 0 ? $"{numberSelectedImage} selected pictures" : "Select pictures";
            NavigationItem.RightBarButtonItem = numberSelectedImage > 0 ? RightBarButtonItem : null;
        }

        private void ReloadData()
        {
            Sections.Clear();
            Sections.Add(new ImagePickerSection());
            if (Images.Count == 0)
            {
                return;
            }
            Images.Sort((x, y) => { return y.Date.CompareTo(x.Date); });

            var lastDate = Images[0].Date;
            int currentSection = 1;
            Sections.Add(new ImagePickerSection(lastDate));

            foreach (var image in Images)
            {
                if (!image.Date.Equals(lastDate))
                {
                    lastDate = image.Date;
                    currentSection++;
                    Sections.Add(new ImagePickerSection(lastDate));
                }
                Sections[currentSection].Images.Add(image);
            }
            CollectionView.ReloadData();
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
                                                Images.Add(new AssetImage(asset));
                                            }
                                        });
                                        ReloadData();
                                    },
                                    (NSError obj) =>
                                    {

                                    });
        }

        private UITapGestureRecognizer CreateHeaderTapGesture()
        {
            var headerTapGestureRecognizer = new UITapGestureRecognizer();
            headerTapGestureRecognizer.AddTarget(HandleHeaderTapGesture);
            HeaderTapGestureRecognizers.Add(headerTapGestureRecognizer);
            return headerTapGestureRecognizer;
        }

        private void HandleHeaderTapGesture(NSObject obj)
        {
            if (obj is UITapGestureRecognizer gesture)
            {
                if (gesture.View is SelectableDateCollectionHeaderView header)
                {
                    var section = GetSectionFromDateTime(header.DateTime);
                    int sectionIndex = Sections.IndexOf(section);
                    bool isSectionSelected = section.Selected;
                    for (int i = 0; i < section.Images.Count; ++i)
                    {
                        if (CollectionView.CellForItem(NSIndexPath.FromItemSection(i, sectionIndex)) is SelectableImageCollectionViewCell cell)
                        {
                            if (isSectionSelected)
                            {
                                UnselectImage(cell.Image.Asset, sectionIndex);
                                cell.UpdateSelectedState(false, true);
                                continue;
                            }
                            if (!SelectedImages.Contains(cell.Image.Asset))
                            {
                                if (SelectImageIfPossible(cell.Image.Asset, sectionIndex))
                                {
                                    cell.UpdateSelectedState(true, true);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UnselectImage(ALAsset asset, int sectionIndex)
        {
            SelectedImages.Remove(asset);
            UpdateNavigationBar();

            if (!Sections[sectionIndex].Selected)
            {
                return;
            }
            Sections[sectionIndex].Selected = false;
            using (var headerKind = new NSString("UICollectionElementKindSectionHeader"))
            {
                if (CollectionView.GetSupplementaryView(headerKind, NSIndexPath.FromItemSection(0, sectionIndex)) is SelectableDateCollectionHeaderView header)
                {
                    header.UpdateSelectedState(false, true);
                }
            }
        }

        private bool SelectImageIfPossible(ALAsset asset, int sectionIndex)
        {
            if (SelectedImages.Count == 10)
            {
                SetToasterText("You can select a maximum of 10 images");
                return false;
            }
            SelectedImages.Add(asset);
            UpdateNavigationBar();

            bool allSectionImageSelected = true;
            foreach (var image in Sections[sectionIndex].Images)
            {
                if (!SelectedImages.Contains(image.Asset))
                {
                    allSectionImageSelected = false;
                    break;
                }
            }

            if (allSectionImageSelected)
            {
                Sections[sectionIndex].Selected = true;
                using (var headerKind = new NSString("UICollectionElementKindSectionHeader"))
                {
                    if (CollectionView.GetSupplementaryView(headerKind, NSIndexPath.FromItemSection(0, sectionIndex)) is SelectableDateCollectionHeaderView header)
                    {
                        header.UpdateSelectedState(true, true);
                    }
                }
            }

            return true;
        }

        private ImagePickerSection GetSectionFromDateTime(DateTime date)
        {
            foreach (var section in Sections)
            {
                if (section.Date.Equals(date))
                {
                    return section;
                }
            }

            return null;
        }

        private void SetToasterText(string text)
        {
            ToasterLabel.Text = text;
            ToasterView.LayoutIfNeeded();
            ToasterView.Layer.CornerRadius = ToasterView.Frame.Height / 2;
            UIView.Animate(0.3f, () =>
            {
                ToasterView.Hidden = false;
            });
            Task.Run(async () =>
            {
                await Task.Delay(3000);
                if (ToasterView != null)
                {
                    InvokeOnMainThread(() =>
                    {
                        UIView.Animate(0.3f, () =>
                        {
                            ToasterView.Alpha = 0f;
                        }, () =>
                        {
                            ToasterView.Alpha = 1f;
                            ToasterView.Hidden = true;
                        });
                    });
                }
            });
        }

        #endregion
    }
}

