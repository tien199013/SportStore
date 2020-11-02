using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace SportsStore.Tests
{
    using System;
    using Controllers;
    using Models;
    using Models.ViewModels;

    public class ProductControllerTests
    {
        [Fact]
        void Can_use_repository()
        {
            //Arrange
            var mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns(
                new[]
                {
                    new Product {ProductID = 1, Name = "p1"},
                    new Product {ProductID = 2, Name = "p2"},
                }.AsQueryable());

            var sut = new HomeController(mockRepo.Object);

            //Act
            var result = ((sut.Index(null) as ViewResult).ViewData.Model as ProductsListViewModel).Products.ToArray();

            //Assert
            Assert.True(result.Length == 2);
            Assert.Equal("p1", result[0].Name);
            Assert.Equal("p2", result[1].Name);
        }

        [Fact]
        void Can_Paginate()
        {
            //Arrange
            var mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns(
                new[]
                {
                    new Product {ProductID = 1, Name = "p1"},
                    new Product {ProductID = 2, Name = "p2"},
                    new Product {ProductID = 3, Name = "p3"},
                    new Product {ProductID = 4, Name = "p4"},
                    new Product {ProductID = 5, Name = "p5"},
                    new Product {ProductID = 6, Name = "p6"},
                }.AsQueryable());

            var sut = new HomeController(mockRepo.Object);

            //Act
            var result = ((sut.Index(null, 2) as ViewResult).ViewData.Model as ProductsListViewModel).Products.ToArray();

            //Assert
            Assert.True(result.Length == 2);
            Assert.Equal("p5", result[0].Name);
            Assert.Equal("p6", result[1].Name);
        }

        [Fact]
        void Can_send_pagination_view_model()
        {
            //Arrange
            var mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns(
                new[]
                {
                    new Product {ProductID = 1, Name = "p1"},
                    new Product {ProductID = 2, Name = "p2"},
                    new Product {ProductID = 3, Name = "p3"},
                    new Product {ProductID = 4, Name = "p4"},
                    new Product {ProductID = 5, Name = "p5"},
                    new Product {ProductID = 6, Name = "p6"},
                }.AsQueryable());

            var sut = new HomeController(mockRepo.Object)
            {
                PageSize = 3
            };

            //Act
            var result = ((sut.Index(null, 2) as ViewResult).ViewData.Model as ProductsListViewModel).PagingInfo;

            //Assert
            Assert.Equal(2, result.CurrentPage);
            Assert.Equal(3, result.ItemsPerPage);
            Assert.Equal(6, result.TotalItems);
            Assert.Equal(2, result.TotalPages);
        }

        [Fact]
        void Can_filter_products()
        {
            //Arrange
            var mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns(
                new[]
                {
                    new Product {ProductID = 1, Name = "p1", Category = "Cat1"},
                    new Product {ProductID = 2, Name = "p2", Category = "Cat2"},
                    new Product {ProductID = 3, Name = "p3", Category = "Cat1"},
                    new Product {ProductID = 4, Name = "p4", Category = "Cat2"},
                    new Product {ProductID = 5, Name = "p5", Category = "Cat3"},
                    new Product {ProductID = 6, Name = "p6", Category = "Cat1"},
                }.AsQueryable());

            var sut = new HomeController(mockRepo.Object)
            {
                PageSize = 3
            };

            //Act
            var result = ((sut.Index("Cat2", 1) as ViewResult).ViewData.Model as ProductsListViewModel).Products.ToArray();


            //Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "p2" && result[0].Category == "Cat2"); 
            Assert.True(result[1].Name == "p4" && result[1].Category == "Cat2");
        }

        [Fact]
        void Generate_specific_product_count()
        {
            //Arrange
            var mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns(
                new[]
                {
                    new Product {ProductID = 1, Name = "p1", Category = "Cat1"},
                    new Product {ProductID = 2, Name = "p2", Category = "Cat2"},
                    new Product {ProductID = 3, Name = "p3", Category = "Cat1"},
                    new Product {ProductID = 4, Name = "p4", Category = "Cat2"},
                    new Product {ProductID = 5, Name = "p5", Category = "Cat3"},
                }.AsQueryable());

            var sut = new HomeController(mockRepo.Object)
            {
                PageSize = 3
            };

            Func<ViewResult, ProductsListViewModel> getModel = result => result?.ViewData?.Model as ProductsListViewModel;

            //Action
            var res1 = getModel(sut.Index("Cat1") as ViewResult)?.PagingInfo.TotalItems;
            var res2 = getModel(sut.Index("Cat2") as ViewResult)?.PagingInfo.TotalItems;
            var res3 = getModel(sut.Index("Cat3") as ViewResult)?.PagingInfo.TotalItems;
            var resAll = getModel(sut.Index(null) as ViewResult)?.PagingInfo.TotalItems;

            //Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
