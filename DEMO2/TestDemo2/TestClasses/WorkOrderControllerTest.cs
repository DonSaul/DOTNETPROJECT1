using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Softserve.ProjectLab.ClientAPI.Controllers;
using Softserve.ProjectLab.ClientAPI.Models;
using Softserve.ProjectLab.ClientAPI.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TestDemo2.TestClasses
{
    [TestClass]
    public class WorkOrderControllerTest
    {
        [TestMethod]
        public async Task Get_ReturnsBadRequestResultOnError()
        {
            // Arrange
            var workOrderServiceMock = new Mock<IWorkOrderService>();
            workOrderServiceMock.Setup(x => x.GetWorkOrdersAsync()).ThrowsAsync(new Exception("Test error"));

            var controller = new WorkOrderController(workOrderServiceMock.Object);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ExportWorkOrderReportsToCsv_ReturnsFileResult()
        {
            // Arrange
            var workOrderServiceMock = new Mock<IWorkOrderService>();
            workOrderServiceMock.Setup(x => x.GetWorkOrderReports()).ReturnsAsync(new List<ReportData>());

            var controller = new WorkOrderController(workOrderServiceMock.Object);

            // Act
            var result = await controller.ExportWorkOrderReportsToCsv();

            // Assert
            Assert.IsInstanceOfType(result, typeof(FileResult));
        }

        [TestMethod]
        public async Task ExportWorkOrderReportsToCsv_ReturnsBadRequestResultOnError()
        {
            // Arrange
            var workOrderServiceMock = new Mock<IWorkOrderService>();
            workOrderServiceMock.Setup(x => x.GetWorkOrderReports()).ThrowsAsync(new Exception("Test error"));

            var controller = new WorkOrderController(workOrderServiceMock.Object);

            // Act
            var result = await controller.ExportWorkOrderReportsToCsv();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task ExportWorkOrderReportsToCsv_ReturnsCsvFile()
        {
            // Arrange
            var workOrderServiceMock = new Mock<IWorkOrderService>();
            var expectedReports = new List<ReportData>
            {
                new ReportData { /* Set properties for testing */ },
                new ReportData { /* Set properties for testing */ }
            };

            workOrderServiceMock.Setup(x => x.GetWorkOrderReports()).ReturnsAsync(expectedReports);

            var controller = new WorkOrderController(workOrderServiceMock.Object);

            // Act
            var result = await controller.ExportWorkOrderReportsToCsv();

            // Assert
            Assert.IsInstanceOfType(result, typeof(FileResult));

            var fileResult = (FileResult)result;
            Assert.AreEqual("text/csv", fileResult.ContentType);
            Assert.AreEqual("work_orders.csv", fileResult.FileDownloadName);
        }

        [TestMethod]
        public async Task Get_ReturnsNotFoundForNonexistentWorkOrder()
        {
            // Arrange
            var workOrderServiceMock = new Mock<IWorkOrderService>();
            workOrderServiceMock.Setup(x => x.GetWorkOrderAsync(It.IsAny<string>())).ReturnsAsync((WorkOrder)null);

            var controller = new WorkOrderController(workOrderServiceMock.Object);

            // Act
            var result = await controller.Get("NonexistentWorkOrder");

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Get_ReturnsOkForExistingWorkOrder()
        {
            // Arrange
            var workOrderServiceMock = new Mock<IWorkOrderService>();
            var expectedWorkOrder = new WorkOrder
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

            workOrderServiceMock.Setup(x => x.GetWorkOrderAsync(It.IsAny<string>())).ReturnsAsync(expectedWorkOrder);

            var controller = new WorkOrderController(workOrderServiceMock.Object);

            // Act
            var result = await controller.Get("ExistingWorkOrder");

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = (OkObjectResult)result;
            Assert.AreEqual(expectedWorkOrder, okResult.Value);
        }
    }
}
