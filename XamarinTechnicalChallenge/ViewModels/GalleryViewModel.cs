using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XamarinTechnicalChallenge.Models;
using XamarinTechnicalChallenge.Views;

namespace XamarinTechnicalChallenge.ViewModels
{
    public class GalleryViewModel : BaseViewModel
    {
        private ObservableCollection<GalleryModel> _pictures;
        public ObservableCollection<GalleryModel> Pictures
        {
            get { return _pictures; }
            set { SetProperty(ref _pictures, value); }
        }

        private int _itemsPerPage;
        public int ItemsPerPage
        {
            get { return _itemsPerPage; }
            set { SetProperty(ref _itemsPerPage, value); }
        }

        private GalleryModel _selectedGalleryItem;
        public GalleryModel SelectedGalleryItem
        {
            get => _selectedGalleryItem;
            set
            {
                SetProperty(ref _selectedGalleryItem, value);
                if (value != null)
                {
                    int selectedIndex = Pictures.IndexOf(value);
                    SelectedIndexCommand?.Execute(selectedIndex);
                }
            }
        }

        public ICommand SelectedIndexCommand { get; }

        public ICommand RemoveStatusCommand { get; set; }
        public ICommand RemainingItemsThresholdReachedCommand { get; }

        public ICommand SetStatusCommand { get; set; }

        private int _currentPage = 1;
  
        private bool _hasMoreItems = true;

        public GalleryViewModel()
        {
            SelectedIndexCommand = new Command<int>(async (index) =>
            {
                _itemsPerPage = Pictures.Count;
                await Shell.Current.Navigation.PushAsync(new DetailPage(index, _itemsPerPage));
            });

            RemainingItemsThresholdReachedCommand = new Command(OnCollectionViewRemainingItemsThresholdReached);

            SetStatusCommand = new Command(SetStatus);

            RemoveStatusCommand = new Command(RemoveStatus);

            Pictures = new ObservableCollection<GalleryModel>();

        }

        public void OnAppearing()
        {
            IsBusy = true;
            _itemsPerPage = 10;
            LoadImages();
            IsBusy = false;
        }


        public void OnCarouselAppearing()
        {
            IsBusy = true;
            LoadImages();
            IsBusy = false;
        }

        public void OnDisappearing()
        {
            IsBusy = true;
            IsBusy = false;
        }

        public void LoadImages()
        {
            var allImagePaths = LoadImagePathsFromSharedFolder();
            var paginatedPaths = allImagePaths.Skip((_currentPage - 1) * _itemsPerPage).Take(_itemsPerPage);

            foreach (var (path, index) in paginatedPaths.Select((path, index) => (path, index)))
            {
                bool isFavorite = Preferences.Get($"Favorite_{index}", false);
                Pictures.Add(new GalleryModel { GalleryImage = path, Favorite = isFavorite });
            }

            _hasMoreItems = allImagePaths.Count > _currentPage * _itemsPerPage;
        }


        public List<string> LoadImagePathsFromSharedFolder()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(GalleryViewModel)).Assembly;
            var imagePaths = new List<string>();

            string imagesNamespace = "XamarinTechnicalChallenge.GalleryTestPhotos";
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.StartsWith(imagesNamespace) && (res.EndsWith(".jpg") || res.EndsWith(".png")))
                {
                    imagePaths.Add(res);
                }
            }

            return imagePaths;
        }

        public void OnCollectionViewRemainingItemsThresholdReached()
        {

            if (_hasMoreItems)
            {
                Console.Write(_itemsPerPage);
                _currentPage++;
                LoadImages();

            }
        }

        public void SetStatus(Object obj)
        {
            int index = Pictures.IndexOf(obj);
            Pictures[index].Favorite = true;
            SaveFavoriteStatus(index, true);
        }

        public void RemoveStatus(Object obj)
        {
            int index = Pictures.IndexOf(obj);
            Pictures[index].Favorite = false;
            SaveFavoriteStatus(index, false);
        }

        public void SaveFavoriteStatus(int index, bool isFavorite)
        {
            Preferences.Set($"Favorite_{index}", isFavorite);
        }

        public void LoadFavoriteImages()
        {
            var allImagePaths = LoadImagePathsFromSharedFolder();

            Pictures.Clear();

            foreach (var (path, index) in allImagePaths.Select((path, index) => (path, index)))
            {
                bool isFavorite = Preferences.Get($"Favorite_{index}", false);

                if (isFavorite)
                {
                    Pictures.Add(new GalleryModel { GalleryImage = path, Favorite = isFavorite });
                }
            }
            OnPropertyChanged(nameof(Pictures));
        }

        public void OnFavoriteAppearing()
        {
            IsBusy = true;
            LoadFavoriteImages();
            IsBusy = false;
        }
    }
}

