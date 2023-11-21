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
    public partial class Gallery : ContentPage
    {
        private readonly GalleryViewModel _galleryViewModel;
        public Gallery()
        {
            InitializeComponent();
            BindingContext = _galleryViewModel = new GalleryViewModel();
            Device.BeginInvokeOnMainThread(() =>
            {
                _galleryViewModel.OnAppearing();
            });
        }

        protected override void OnAppearing()
        {
            
            _galleryViewModel.SelectedGalleryItem = null;
        }


    }
}