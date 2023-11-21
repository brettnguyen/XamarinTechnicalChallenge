using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTechnicalChallenge.ViewModels;

namespace XamarinTechnicalChallenge.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritePage : ContentPage
    {
       
        private readonly GalleryViewModel _galleryViewModel;
        public FavoritePage()
        {
            InitializeComponent();
            BindingContext = _galleryViewModel = new GalleryViewModel();
        }

        protected override void OnAppearing()
        {
            _galleryViewModel.OnFavoriteAppearing();
            _galleryViewModel.SelectedGalleryItem = null;
        }
    }
}