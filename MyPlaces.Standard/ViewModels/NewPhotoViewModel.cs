using System;
using System.Collections.Generic;
using MyPlaces.Standard.Data;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using System.IO;

namespace MyPlaces.Standard.ViewModels
{
    public class NewPhotoViewModel: BaseViewModel
    {
        private DataAccessLayer _accessLayer = new DataAccessLayer();
        //private bool _isEditable = true;

        private List<Category> _categories;
        private Category _selectedCategory;
        private string _imagePath;
        private DateTime _imageDateTime;
        private string _title;
        private string _comment;
        private string newImagePath;
        private int? placeId;


        public NewPhotoViewModel()
        {
            LoadData().ContinueWith(t => {
                if (t.IsFaulted)
                    throw t.Exception;
            });

            MessagingCenter.Subscribe<object>(this, MessageNames.CATEGORY_EDITED, sender => {
                LoadData().ContinueWith(t => {
                    if (t.IsFaulted)
                        throw t.Exception;
                });
            });
        }

        public async Task SetPlace(int placeId)
        {
            Place place = await _accessLayer.GetPhotoById(placeId);
            ImagePath = !string.IsNullOrEmpty(place.Path) ? Path.Combine(App.PhotoUtility.PhotoBasePath, place.Path) : null;
            Title = place.Title;
            Comment = place.Description;
            SelectedCategory = Categories.FirstOrDefault(c => c.CategoryId == place.CategoryId);
            this.placeId = place.PlaceId;
        }


        public List<Category> Categories {
            get { return _categories; }
            set {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public string ImagePath {
            get { 
                return _imagePath; 
            }
            set {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        public DateTime ImageDateTime
        {
            get { return _imageDateTime; }
            set
            {
                _imageDateTime = value;
                OnPropertyChanged(nameof(ImageDateTime));
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
                SetIsSaveable();
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
                SetIsSaveable();
            }
        }

        //public bool IsEditable => true;
        //{
        //    get { return _isEditable; }
        //    set {
        //        _isEditable = value;
        //        OnPropertyChanged(nameof(IsEditable));
        //    }
        //}

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                SetIsSaveable();
            }
        }

        private bool isSaveable = false;
        public bool IsSaveable
        {
            get => isSaveable;
            set {
                isSaveable = value;
                OnPropertyChanged(nameof(IsSaveable));
            }
        }

        private void SetIsSaveable()
        {
            IsSaveable = !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Comment) && SelectedCategory != null;
        }

        public async Task PrepareForNewPlace()
        {
            await LoadData();
            ImagePath = null;
            Title = "";
            Comment = "";
            // IsEditable = true;
        }

        private async Task LoadData()
        {
            Categories = await _accessLayer.GetAllCategories();
            _selectedCategory = Categories.FirstOrDefault();
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get {
                if (_saveCommand == null)
                {
                    _saveCommand = new Command(() => {
                        SavePlace();
                    });
                }
                return _saveCommand;
            }
        }

        private ICommand _takeAPictureCommand;
        public ICommand TakeAPictureCommand
        {
            get {
                if (_takeAPictureCommand == null)
                {
                    _takeAPictureCommand = new Command(() =>
                    {
                        TakeAPictureAsync();
                    });
                }
                return _takeAPictureCommand;
            }
        }




        private async void TakeAPictureAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                //await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Name = "myPlace_" + DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString() + ".jpg",
                CustomPhotoSize = 50, 
            });

            if (file == null) return;

            ImagePath = file.Path;

            if (Device.RuntimePlatform == Device.Android)
            {
                newImagePath = System.IO.Path.Combine(App.PhotoUtility.PhotoBasePath, System.IO.Path.GetFileName(ImagePath));
                System.IO.File.Copy(ImagePath, newImagePath);
                System.IO.File.Delete(ImagePath);
                ImagePath = newImagePath;
            }
        }


        private async void SavePlace()
        {
            Place newPlace = new Place();
            newPlace.PlaceId = placeId;
            newPlace.Path = Path.GetFileName(ImagePath);
            newPlace.Title = Title;
            newPlace.Description = Comment;
            newPlace.Date = DateTime.Now;
            newPlace.CategoryId = SelectedCategory.CategoryId;

            IPermission permissions = ((App)App.Current).Permissions;

            bool saveLocation = permissions.HasLocationPermission;
            if (!saveLocation)
            {
                saveLocation = await permissions.RequestLocationPermission();
            }
            if (saveLocation)
            {
                var position = await GetLocationAsync();
                if (position != null)
                {
                    newPlace.Latitude = position.Latitude;
                    newPlace.Longitude = position.Longitude;
                }
            }

            if (!string.IsNullOrEmpty(ImagePath))
                App.PhotoUtility.GenerateThumbnail(_imagePath, Device.RuntimePlatform == Device.Android ? 120 : 50);

            await _accessLayer.SavePlace(newPlace);

            await App.Current.MainPage.DisplayAlert("SAVED", "This place was saved", "OK");
        }

        protected async Task<Position> GetLocationAsync()
        {
            CrossGeolocator.Current.DesiredAccuracy = 100;

            return await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10));

        }

        private ICommand showOnMapCommand;
        public ICommand ShowOnMapCommand
        {
            get {
                if (showOnMapCommand == null)
                {
                    showOnMapCommand = new Command(() => {
                        ((App)App.Current).SelectedPlaceId = this.placeId;
                        MainPage mainPage = (MainPage)App.Current.MainPage;
                        mainPage.CurrentPage = mainPage.Children.OfType<MapPage>().First();
                    });
                }
                return showOnMapCommand;
            }
        }
    }
}
