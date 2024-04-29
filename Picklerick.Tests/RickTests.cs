using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Picklerick.Controllers;
using Picklerick.Data;
using Picklerick.Models;
using Xunit;

namespace Picklerick.Tests
{
    public class RickControllerTests
    {
        private readonly Mock<DataContextEF> _mockDataContext;
        private readonly RicksController _controller;

        public RickControllerTests()
        {
            // Mocking Ricks data
            var mockRicks = new List<Rick>
            {
                new Rick { Id = 1, Universe = "Dimension C-137", IsMortyAlive = true },
                new Rick { Id = 2, Universe = "Dimension D-348", IsMortyAlive = false }
            };

            // Mock DataContextEF and DbSet<Rick>
            _mockDataContext = new Mock<DataContextEF>();
            var mockDbSet = new Mock<DbSet<Rick>>();
            mockDbSet.As<IQueryable<Rick>>().Setup(m => m.Provider).Returns(mockRicks.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Rick>>().Setup(m => m.Expression).Returns(mockRicks.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Rick>>().Setup(m => m.ElementType).Returns(mockRicks.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Rick>>().Setup(m => m.GetEnumerator()).Returns(mockRicks.GetEnumerator());

            _mockDataContext.Setup(m => m.Ricks).Returns(mockDbSet.Object);

            // Create RicksController instance with mocked DataContextEF
            _controller = new RicksController(null, _mockDataContext.Object);
        }

        [Fact]
        public void GetRicks_ReturnsListOfRicks()
        {
            // Act
            var result = _controller.GetRicks() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

            var model = result.Value as IEnumerable<Rick>;
            Assert.NotNull(model);

            var expectedRicks = _mockDataContext.Object.Ricks.ToList();
            Assert.Equal(expectedRicks.Count(), model.Count());
        }
    }
}