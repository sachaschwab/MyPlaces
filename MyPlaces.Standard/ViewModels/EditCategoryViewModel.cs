using System;
using System.Diagnostics;
using System.Windows.Input;
using MyPlaces.Standard.Data;
using Xamarin.Forms;

namespace MyPlaces.Standard.ViewModels
{
    public class EditCategoryViewModel : BaseViewModel
    {
        public EditCategoryViewModel()
        {
            MessagingCenter.Subscribe<object, Category>(this, "EDIT_CATEGORY", (sender, cat) => {
                Category = cat;
            });
        }


        private Category category;

        public Category Category
        {
            get => category;
            set {
                category = value;
                OnPropertyChanged(nameof(Category));
                Debug.WriteLine("inside setter of Category");
                if (category != null)
                {
                    Color currentColor = Color.FromHex(category.Color.Substring(1));
                    //color = Color.FromHex("0000FF");
                    Red = (int)(currentColor.R * 255);
                    Green = (int)(currentColor.G * 255);
                    Blue = (int)(currentColor.B * 255);
                    Color = color;
                }
                //else
                //{
                //    Color = Color.Gray;
                //    Red = Green = Blue = 0;
                //}
            }
        }

        private Color color;
        public Color Color
        {
            get => color;
            set {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        private int red;
        public int Red
        {
            get => red;
            set {
                red = value;
                OnPropertyChanged(nameof(Red));
                Color = Color.FromRgb(Red, Green, Blue);
            }
        }

        private int green;
        public int Green
        {
            get => green;
            set
            {
                green = value;
                OnPropertyChanged(nameof(Green));
                Color = Color.FromRgb(Red, Green, Blue);
            }
        }

        private int blue;
        public int Blue
        {
            get => blue;
            set
            {
                blue = value;
                OnPropertyChanged(nameof(Blue));
                Color = Color.FromRgb(Red, Green, Blue);
            }
        }

        private ICommand saveCommand;
        public ICommand SaveCommand {
        get
            {
                if (saveCommand == null)
                {
                    saveCommand = new Command(async () =>
                    {
                        if (Category != null)
                        {
                            Category.Color = $"#{(int)(color.R * 255):X2}{(int)(color.G * 255):X2}{(int)(Color.B * 255):X2}";
                            DataAccessLayer dal = new DataAccessLayer();
                            await dal.UpdateCategory(Category);
                        }

                    });
                }
                return saveCommand;
            }
        }
    }
}
