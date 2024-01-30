using Microsoft.VisualStudio.TestTools.UnitTesting;
using Softserve.ProjectLab.ClientAPI.Models;
using Softserve.ProjectLab.ClientAPI.Services;
using Moq;
using System.Threading.Tasks;

namespace TestDemo2.TestClasses
{
    [TestClass]
    public class StatusServiceTest
    {
        [TestMethod]
        public async Task GetStatusesAsync_ShouldReturnStatuses()
        {
            // Arrange
            var apiConnectorMock = new Mock<IApiConnector>();
            apiConnectorMock.Setup(m => m.GetAsync<Status[]>(It.IsAny<string>()))
                .ReturnsAsync(new Status[]
                {
                    new Status { Id = 0, Name = "Cancelled" },
                    new Status { Id = 1, Name = "New" },
                    new Status { Id = 2, Name = "Scheduled" },
                    new Status { Id = 3, Name = "OnSite" },
                    new Status { Id = 4, Name = "Complete" }
                });

            var statusService = new StatusService(apiConnectorMock.Object);

            // Act
            var statusServiceMock = new Mock<IStatusService>();
            var result = await statusServiceMock.Object.GetStatusesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Length);
            Assert.AreEqual("Cancelled", result[0].Name);
            Assert.AreEqual("New", result[1].Name);
            Assert.AreEqual("Scheduled", result[2].Name);
            Assert.AreEqual("OnSite", result[3].Name);
            Assert.AreEqual("Complete", result[4].Name);
        }
    }
}
