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
    public partial class DetailPage : ContentPage
    {
        private readonly GalleryViewModel _galleryViewModel;
        public DetailPage(int selectedIndex, int holder)
        {
            InitializeComponent();
            BindingContext = _galleryViewModel = new GalleryViewModel();

            if (Device.RuntimePlatform == Device.iOS)
            {
                ListPosts.Position = selectedIndex;
            }


            Device.BeginInvokeOnMainThread(() =>
            {
                _galleryViewModel.ItemsPerPage = holder;
                _galleryViewModel.OnCarouselAppearing();
                if (Device.RuntimePlatform == Device.Android)
                {
                    ListPosts.Position = selectedIndex;
                }
            });
        }

        protected override void OnDisappearing()
        {
            base.OnAppearing();
            _galleryViewModel.OnDisappearing();

        }

    }
}