using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.Tests
{
    using System.Linq;
    using Models;
    using Xunit;

    public class CartTests
    {
        [Fact]
        void Can_add_new_lines()
        {
            //Arrange - create some tests products
            var product1 = new Product {ProductID = 1, Name = "P1"};
            var product2 = new Product {ProductID = 2, Name = "P2"};

            var sut = new Cart();


            //Act
            sut.AddItem(product1, 1);
            sut.AddItem(product2, 1);

            var result = sut.Lines.ToArray();

            //Assert
            Assert.Equal(2, result.Length);
            Assert.Equal(product1, result[0].Product);
            Assert.Equal(product2, result[1].Product);
        }

        [Fact]
        void Can_add_quantity_for_existing_lines()
        {
            //Arrange - create some tests products
            var product1 = new Product { ProductID = 1, Name = "P1" };
            var product2 = new Product { ProductID = 2, Name = "P2" };

            var sut = new Cart();


            //Act
            sut.AddItem(product1, 1);
            sut.AddItem(product2, 1);
            sut.AddItem(product1, 10);

            var result = sut.Lines.OrderBy(l => l.Product.ProductID).ToArray();

            //Assert
            Assert.Equal(2, result.Length);
            Assert.Equal(11, result[0].Quantity);
            Assert.Equal(1, result[1].Quantity);
        }

        [Fact]
        void Can_remove_line()
        {
            //Arrange - create some tests products
            var product1 = new Product { ProductID = 1, Name = "P1" };
            var product2 = new Product { ProductID = 2, Name = "P2" };
            var product3 = new Product { ProductID = 3, Name = "P3" };

            var sut = new Cart();

            sut.AddItem(product1, 1);
            sut.AddItem(product2, 3);
            sut.AddItem(product3, 5);
            sut.AddItem(product2, 1);

            //Act
            sut.RemoveLine(product2);

            //Assert
            Assert.Empty(sut.Lines.Where(l => l.Product == product2));
            Assert.Equal(2, sut.Lines.Count);
        }

        [Fact]
        void Calculate_cart_total()
        {
            //Arrange - create some tests products
            var product1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            var product2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            var sut = new Cart();

            sut.AddItem(product1, 1);
            sut.AddItem(product2, 1);
            sut.AddItem(product1, 3);

            //Act
            var total = sut.ComputeTotalValue();

            //Assert
            Assert.Equal(450M, total);
        }

        [Fact]
        void Can_clear_contents()
        {
            //Arrange - create some tests products
            var product1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            var product2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            var sut = new Cart();

            sut.AddItem(product1, 1);
            sut.AddItem(product2, 1);
            sut.AddItem(product1, 3);

            //Act
           sut.Clear();

            //Assert
            Assert.Empty(sut.Lines);
        }
    }
}
