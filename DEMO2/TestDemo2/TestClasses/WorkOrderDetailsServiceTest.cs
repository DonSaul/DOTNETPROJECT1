using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Softserve.ProjectLab.ClientAPI.Models;
using Softserve.ProjectLab.ClientAPI.Services;

namespace TestDemo2.TestClasses
{
    [TestClass]
    public class WorkOrderDetailsServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task GetWorkOrderDetailsByTechnicianAsync_ShouldThrowNotImplementedException()
        {
            // Arrange
            var statusServiceMock = new Mock<IStatusService>();
            var technicianServiceMock = new Mock<ITechnicianService>();
            var workTypeServiceMock = new Mock<IWorkTypeService>();
            var workOrderServiceMock = new Mock<IWorkOrderService>();

            var workOrderDetailsService = new WorkOrderDetailsService(
                statusServiceMock.Object,
                technicianServiceMock.Object,
                workTypeServiceMock.Object,
                workOrderServiceMock.Object
            );

            // Act & Assert
            await workOrderDetailsService.GetWorkOrderDetailsByTechnicianAsync("TestTechnician");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetWorkOrderDetailsAsync_Exception()
        {
            // Arrange
            var statusServiceMock = new Mock<IStatusService>();
            var technicianServiceMock = new Mock<ITechnicianService>();
            var workTypeServiceMock = new Mock<IWorkTypeService>();
            var workOrderServiceMock = new Mock<IWorkOrderService>();

            // Set up mocks to throw exceptions
            statusServiceMock.Setup(s => s.GetStatusesAsync()).Throws(new Exception("Simulated exception"));

            var workOrderDetailsService = new WorkOrderDetailsService(
                statusServiceMock.Object,
                technicianServiceMock.Object,
                workTypeServiceMock.Object,
                workOrderServiceMock.Object
            );

            // Act && Assert
            var result = await workOrderDetailsService.GetWorkOrderDetailsAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetWorkOrderDetailsAsync_InvalidTimeRange()
        {
            // Arrange
            var statusServiceMock = new Mock<IStatusService>();
            var technicianServiceMock = new Mock<ITechnicianService>();
            var workTypeServiceMock = new Mock<IWorkTypeService>();
            var workOrderServiceMock = new Mock<IWorkOrderService>();

            var workOrderDetailsService = new WorkOrderDetailsService(
                statusServiceMock.Object,
                technicianServiceMock.Object,
                workTypeServiceMock.Object,
                workOrderServiceMock.Object
            );

            // Act && Assert
            var result = await workOrderDetailsService.GetWorkOrderDetailsAsync(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(-1), "all", "all");
        }

        [TestMethod]
        public async Task GetWorkOrderDetailsAsync_NullTasks()
        {
            // Arrange
            var statusServiceMock = new Mock<IStatusService>();
            var technicianServiceMock = new Mock<ITechnicianService>();
            var workTypeServiceMock = new Mock<IWorkTypeService>();
            var workOrderServiceMock = new Mock<IWorkOrderService>();

            var workOrderDetailsService = new WorkOrderDetailsService(
                statusServiceMock.Object,
                technicianServiceMock.Object,
                workTypeServiceMock.Object,
                workOrderServiceMock.Object
            );

            // Act
            var result = await workOrderDetailsService.GetWorkOrderDetailsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count); // Expect an empty list
        }

        [TestMethod]
        public async Task GetWorkOrderDetailsAsync_NoWorkOrders()
        {
            // Arrange
            var statusServiceMock = new Mock<IStatusService>();
            var technicianServiceMock = new Mock<ITechnicianService>();
            var workTypeServiceMock = new Mock<IWorkTypeService>();
            var workOrderServiceMock = new Mock<IWorkOrderService>();

            var workOrderDetailsService = new WorkOrderDetailsService(
                statusServiceMock.Object,
                technicianServiceMock.Object,
                workTypeServiceMock.Object,
                workOrderServiceMock.Object
            );

            // Act
            var result = await workOrderDetailsService.GetWorkOrderDetailsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count); // Expect an empty list
        }
    }
}
