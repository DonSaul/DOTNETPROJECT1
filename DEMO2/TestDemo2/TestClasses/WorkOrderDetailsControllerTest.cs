using Microsoft.AspNetCore.Http;
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
    public class WorkOrderDetailsControllerTest
    {
        private Mock<IWorkOrderDetailsService> _mockWorkOrderDetailsService;
        private WorkOrderDetailsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockWorkOrderDetailsService = new Mock<IWorkOrderDetailsService>();
            _controller = new WorkOrderDetailsController(_mockWorkOrderDetailsService.Object);
        }

        [TestMethod]
        public async Task Get_ValidParameters_ReturnsOk()
        {
            // Arrange
            var startTime = DateTimeOffset.UtcNow.AddDays(-1);
            var endTime = DateTimeOffset.UtcNow;
            var workType = "1";
            var status = "0";
            var workOrderDetailsList = new List<WorkOrderDetails>();

            _mockWorkOrderDetailsService.Setup(x => x.GetWorkOrderDetailsAsync(startTime, endTime, workType, status))
                .ReturnsAsync(workOrderDetailsList);

            // Act
            var result = await _controller.Get(startTime, endTime, workType, status);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

            _mockWorkOrderDetailsService.Verify(x => x.GetWorkOrderDetailsAsync(startTime, endTime, workType, status), Times.Once);
        }

        [TestMethod]
        public async Task Get_NoParameters_ReturnsOk()
        {
            // Arrange
            var workOrderDetailsList = new List<WorkOrderDetails>();

            _mockWorkOrderDetailsService.Setup(x => x.GetWorkOrderDetailsAsync())
                .ReturnsAsync(workOrderDetailsList);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

            _mockWorkOrderDetailsService.Verify(x => x.GetWorkOrderDetailsAsync(), Times.Once);
        }

        [TestMethod]
        public async Task List_ReturnsViewWithViewModel()
        {
            // Arrange
            var viewModel = new WorkOrderViewModel();
            _mockWorkOrderDetailsService.Setup(x => x.GetWorkOrderViewModelAsync())
                .ReturnsAsync(viewModel);

            // Act
            var result = await _controller.List();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.AreEqual(viewModel.WorkOrders, viewResult.Model);

            _mockWorkOrderDetailsService.Verify(x => x.GetWorkOrderViewModelAsync(), Times.Once);
        }
    }
}
