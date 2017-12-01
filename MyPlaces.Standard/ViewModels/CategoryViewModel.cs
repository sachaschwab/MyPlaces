using System;
using System.Windows.Input;
using MyPlaces.Standard.Data;
using Xamarin.Forms;

namespace MyPlaces.Standard.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        public CategoryViewModel(Category category)
        {
            Category = category;
        }

        public Category Category { get; }

        private ICommand editCommand;
        public ICommand EditCommand {
            get {
                if (editCommand == null)
                {
                    editCommand = new Command(() => {
                        MessagingCenter.Send<object, Category>(this, "EDIT_CATEGORY", Category);
                    });
                }
                return editCommand;
            }
        } 
    }
}
