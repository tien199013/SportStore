using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace SportsStore.Tests
{
    using Components;
    using Models;

    public class NavigationMenuViewComponentTests
    {
        [Fact]
        void Can_select_categories()
        {
            //Arrange
            var mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products)
                .Returns(new[]
                {
                    new Product {ProductID = 1, Name = "p1", Category = "Cat1"},
                    new Product {ProductID = 2, Name = "p2", Category = "Cat2"},
                    new Product {ProductID = 3, Name = "p3", Category = "Cat1"},
                    new Product {ProductID = 4, Name = "p4", Category = "Cat2"},
                }.AsQueryable());

            var target = new NavigationMenuViewComponent(mockRepo.Object);

            //Act = get the set of categories
            var model = (target.Invoke() as ViewViewComponentResult).ViewData.Model;
            var results = model as IEnumerable<string>;

            //Assert
            Assert.True(new[] { "Cat1", "Cat2" }.SequenceEqual(results));
        }

        [Fact]
        void Indicates_selected_category()
        {
            //Arrange
            var selectedCategory = "Apples";

            var mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products)
                .Returns(new[]
                {
                    new Product {ProductID = 1, Name = "p1", Category = "Apples"},
                    new Product {ProductID = 2, Name = "p2", Category = "Oranges"},
                }.AsQueryable());

            var target = new NavigationMenuViewComponent(mockRepo.Object)
            {
                ViewComponentContext = new ViewComponentContext()
                {
                    ViewContext = new ViewContext
                    {
                        RouteData = new RouteData()
                    }
                }
            };
            target.RouteData.Values["category"] = selectedCategory;

            //Act = get the set of categories
            var result = (target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            //Assert
            Assert.Equal(selectedCategory, result);
        }
    }
}
