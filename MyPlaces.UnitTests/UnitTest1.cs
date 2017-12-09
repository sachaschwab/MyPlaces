using System;
using Xunit;

namespace MyPlaces.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void CheckTitle()
        {
            var newPhotoVM = new MyPlaces.Standard.ViewModels.NewPhotoViewModel();

            newPhotoVM.Title = "TestTitle";
            Assert.Equal(newPhotoVM.Title,"TestTitle");
        }

        [Fact]
        public void CheckComment()
        {
            var newPhotoVM = new MyPlaces.Standard.ViewModels.NewPhotoViewModel();

            newPhotoVM.Comment = "TestComment";
            Assert.Equal(newPhotoVM.Comment, "TestComment");
        }
    }
}
