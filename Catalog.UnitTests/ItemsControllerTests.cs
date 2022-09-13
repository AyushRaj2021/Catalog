using System;
using Xunit;
using Moq;
using Catalog.Api.Repositories;
using Catalog.Api.Entities;
using Microsoft.Extensions.Logging;
using Catalog.Api.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.UnitTests
{
    public class ItemsControllerTests
    {
        [Fact]
        public async Task GetItemsAsync_WithUnexistingItem_ReturnsNotFound()
        {
            //Arrange
            var repositoryStub = new Mock<IItemsRepository>();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync((Item)null);
            //whenever controller invokes getasync method with any guid which mock is going to provide,it have to return null value.

            var loggerStub = new Mock<ILogger<ItemsController>>();

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
            
            //Act
            var result = await controller.GetItemAsync(Guid.NewGuid());
            //Assert
            Assert.IsType<NotFoundResult>(result.Result);

        }
    }
}
