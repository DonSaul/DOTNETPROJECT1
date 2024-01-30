using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Softserve.ProjectLab.ClientAPI.Models;
using Softserve.ProjectLab.ClientAPI.Services;
using System.Threading.Tasks;

namespace TestDemo2.TestClasses;
[TestClass]
public class WorkTypeTest
{
        [TestMethod]
         public async Task GetWorkTypesAsync_ShouldReturnWorkTypes()
        {
            // Arrange
            var apiConnectorMock = new Mock<IApiConnector>();
            apiConnectorMock.Setup(m => m.GetAsync<WorkType[]>(It.IsAny<string>()))
                .ReturnsAsync(new WorkType[]
                {
                    new WorkType { Id = 0, Name = "Repair" },
                    new WorkType { Id = 1, Name = "Install" },
                    new WorkType { Id = 2, Name = "Quote" },
                    new WorkType { Id = 3, Name = "Maintenance" }
                });
            //var workTypeService = new WorkTypeService(apiConnectorMock.Object);
            var workTypeServiceMock = new Mock<IWorkTypeService>();
            // Act
            var result = await workTypeServiceMock.Object.GetWorkTypesAsync();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual("Repair", result[0].Name);
            Assert.AreEqual("Install", result[1].Name);
            Assert.AreEqual("Quote", result[2].Name);
            Assert.AreEqual("Maintenance", result[3].Name);
         }
}
