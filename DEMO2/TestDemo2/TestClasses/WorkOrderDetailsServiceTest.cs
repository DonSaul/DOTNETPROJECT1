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

        [TestMethod]
        public async Task GetWorkOrderDetailsByNameAsync_ShouldReturnCorrectResult()
        {
            // Arrange
            var statusServiceMock = new Mock<IStatusService>();
            var technicianServiceMock = new Mock<ITechnicianService>();
            var workTypeServiceMock = new Mock<IWorkTypeService>();
            var workOrderServiceMock = new Mock<IWorkOrderService>();

            // Set up mock data for the test
            var workOrder = new WorkOrder
            {
                WorkOrderName = "WO-20240107-60156742",
                StatusId = 0,
                TechnicianId = 59,
                Duration = "00:00:00",
                CreatedDate = DateTimeOffset.Parse("2024-01-07T23:28:38+00:00"),
                StartTime = null,
                EndTime = null,
                WorkTypeId = 2
            };

            workOrderServiceMock.Setup(s => s.GetWorkOrderAsync(It.IsAny<string>())).ReturnsAsync(workOrder);

            var technicians = new Technician[]
            {
                new Technician
                {
                    TechnicianId = 15,
                    Name = "Mauricio Villanueva",
                    Address = "Av. Matucana 7414, Santiago",
                },
                new Technician
                {
                    TechnicianId = 61,
                    Name = "Diego Contreras",
                    Address = "Av. Matucana 6461, Santiago",
                },
            };

            technicianServiceMock.Setup(s => s.GetTechnicianAsync(It.IsAny<int>())).ReturnsAsync(technicians.FirstOrDefault());

            statusServiceMock.Setup(s => s.GetStatusesAsync()).ReturnsAsync(new Status[]
            {
                new Status { Id = 0, Name = "Cancelled" },
                new Status { Id = 1, Name = "New" },
                new Status { Id = 2, Name = "Scheduled" },
                new Status { Id = 3, Name = "OnSite" },
                new Status { Id = 4, Name = "Complete" }
            });

            workTypeServiceMock.Setup(s => s.GetWorkTypesAsync()).ReturnsAsync(new WorkType[]
            {
                new WorkType { Id = 0, Name = "Repair" },
                new WorkType { Id = 1, Name = "Install" },
                new WorkType { Id = 2, Name = "Quote" },
                new WorkType { Id = 3, Name = "Maintenance" }
            });

            // Set up the work order name for testing
            var workOrderName = "TestWorkOrder";

            var workOrderDetailsService = new WorkOrderDetailsService(
                statusServiceMock.Object,
                technicianServiceMock.Object,
                workTypeServiceMock.Object,
                workOrderServiceMock.Object
            );

            // Act
            var result = await workOrderDetailsService.GetWorkOrderDetailsByNameAsync(workOrderName);

            // Assert
            Assert.AreEqual("WO-20240107-60156742", result.WorkOrderName);
        }
    }
}
