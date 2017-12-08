using System;
namespace MyPlaces.Standard.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public CategoryListViewModel CategoryListViewModel { get; set; } = new CategoryListViewModel();
        public PlacesViewModel PlacesViewModel { get; set; } = new PlacesViewModel();
        public NewPhotoViewModel NewPhotoViewModel { get; set; } = new NewPhotoViewModel();
    }
}
