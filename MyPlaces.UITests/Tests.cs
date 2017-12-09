using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace MyPlaces.UITests
{
    [TestFixture(Platform.Android)]
    // [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AddCategory()
        {
            app.Tap("AddCategoryButton");
            app.ClearText(c => c.Marked("CategoryEntry"));
            app.EnterText(c=>c.Marked("CategoryEntry"), "Restaurants");
            app.Tap("SaveCategoryButton");
            app.Tap("Restaurants");
            Assert.AreEqual(1, app.Query("Restaurants").Count());
        }

        [Test]
        public void AddPlace()
        {
            // Add a category
            app.Tap("AddCategoryButton");
            app.ClearText(c => c.Marked("CategoryEntry"));
            app.EnterText(c => c.Marked("CategoryEntry"), "Aussicht");
            app.Tap("SaveCategoryButton");

            // add a place
            app.Tap("Places");
            app.Tap("AddPlaceButton");
            app.EnterText(c => c.Marked("TitleEntry"), "Schöne Aussicht");
            app.EnterText(c => c.Marked("DescriptionEntry"), "Hier muss ich unbedingt wieder mal hin");
            app.Tap("CategoryPicker");
            app.Tap("Aussicht");
            app.Tap("SaveButton");
            app.WaitForElement("OK");
            app.Tap("OK");

            // show it on the map
            app.Tap("NoResourceEntry-30");
            app.Screenshot("Map");
        }
    }
}
