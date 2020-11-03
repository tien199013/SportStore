using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        void Cannot_checkout_empty_cart()
        {
            //Arrange 
            var mockRepo = new Mock<IOrderRepository>();
            var cart = new Cart();
            var order = new Order();

            var sut = new OrderController(mockRepo.Object, cart);
            
            //Act
            var result = sut.Checkout(order) as ViewResult;

            //Assert
            mockRepo.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        void Cannot_checkout_invalid_shipping_details()
        {
            //Arrange 
            var mockRepo = new Mock<IOrderRepository>();
            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            var order = new Order();

            var sut = new OrderController(mockRepo.Object, cart);
            sut.ModelState.AddModelError("error", "error");
            //Act
            var result = sut.Checkout(order) as ViewResult;

            //Assert
            mockRepo.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        void Can_checkout_and_submit_order()
        {
            //Arrange 
            var mockRepo = new Mock<IOrderRepository>();
            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            var order = new Order();

            var sut = new OrderController(mockRepo.Object, cart);

            //Act
            var result = sut.Checkout(order) as RedirectToPageResult;

            //Assert
            mockRepo.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("/Completed", result.PageName);
        }
    }
}
