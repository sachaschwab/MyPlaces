using System;
namespace MyPlaces.Standard.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
        }

        public CategoryListViewModel CategoryListViewModel { get; set; } = new CategoryListViewModel();
    }
}
