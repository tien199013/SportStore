using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.Tests
{
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Models.ViewModels;
    using Moq;
    using Xunit;

    public class PageLinkTagHelperTests
    {
        [Fact]
        void Can_generate_page_links()
        {
            //Arrange
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");

            var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            mockUrlHelperFactory.Setup(m => m.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(mockUrlHelper.Object);

            var sut = new PageLinkTagHelper(mockUrlHelperFactory.Object)
            {
                PageModel = new PagingInfo()
                {
                    CurrentPage = 2,
                    TotalItems = 28,
                    ItemsPerPage = 10
                },
                PageAction = "Test"
            };

            var tagHelperContext = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(), "");

            var mockContent = new Mock<TagHelperContent>();
            var output = new TagHelperOutput("div",
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(mockContent.Object));

            //Act
            sut.Process(tagHelperContext, output);

            //Assert
            Assert.Equal(@"<a href=""Test/Page1"">1</a>"
                         + @"<a href=""Test/Page2"">2</a>"
                         + @"<a href=""Test/Page3"">3</a>", 
                output.Content.GetContent());
        }
    }
}
