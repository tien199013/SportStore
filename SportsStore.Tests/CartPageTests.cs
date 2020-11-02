using System.Linq;
using Moq;

namespace SportsStore.Tests
{
    using Models;
    using Xunit;
    using Pages;

    public class CartPageTests
    {
        [Fact]
        void Can_load_cart()
        {
            //Arrange
            var product1 = new Product { ProductID = 1, Name = "P1" };
            var product2 = new Product { ProductID = 2, Name = "P2" };

            var mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products)
                .Returns(new[]
                {
                    product1, product2
                }.AsQueryable);

            var testCart = new Cart();
            testCart.AddItem(product1, 2);
            testCart.AddItem(product2, 1);
            
            //Action
            var sut = new CartModel(mockRepo.Object, testCart);

            sut.OnGet("myUrl");

            //Assert
            Assert.Equal(2, sut.Cart.Lines.Count);
            Assert.Equal("myUrl", sut.ReturnUrl);
        }

        [Fact]
        void Can_update_cart()
        {
            //Arrange
            var product1 = new Product { ProductID = 1, Name = "P1" };
            var product2 = new Product { ProductID = 2, Name = "P2" };

            var mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products)
                .Returns(new[]
                {
                    product1, product2
                }.AsQueryable);

            var testCart = new Cart();

            //Action
            var sut = new CartModel(mockRepo.Object, testCart);

            sut.OnPost(1, "myUrl");

            //Assert
            Assert.Single(testCart.Lines);
            Assert.Equal("P1", testCart.Lines.First().Product.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);
        }
    }
}
