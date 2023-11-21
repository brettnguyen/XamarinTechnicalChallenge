using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace XamarinTechnicalChallenge.Models
{
    public class GalleryModel : INotifyPropertyChanged
    {
        private string _galleryImage;
        public string GalleryImage
        {
            get { return _galleryImage; }
            set
            {
                if (_galleryImage != value)
                {
                    _galleryImage = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool favorite;
        public bool Favorite
        {
            get { return favorite; }
            set
            {
                favorite = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
