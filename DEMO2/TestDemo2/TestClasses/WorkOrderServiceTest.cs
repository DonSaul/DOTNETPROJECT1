using Microsoft.VisualStudio.TestTools.UnitTesting;
using Softserve.ProjectLab.ClientAPI.Services;
using Softserve.ProjectLab.ClientAPI.Models;
using Moq; //instalado el nuget

namespace TestDemo2.TestClasses;
[TestClass]
public class WorkOrderServiceTest
{
    [TestMethod]
    public async Task GetWorkOrdersAsync_ShouldReturnWorkOrders()
    {
        // Arrange
        var apiConnectorMock = new Mock<IApiConnector>();

        apiConnectorMock.Setup(m => m.GetAsync<WorkOrder[]>(It.IsAny<string>()))
            .ReturnsAsync(new WorkOrder[] {
                new WorkOrder
                {
                    WorkOrderName = "WO-20240107-60156742",
                    StatusId = 0,
                    TechnicianId = 59,
                    Duration = "00:00:00",
                    CreatedDate = DateTimeOffset.Parse("2024-01-07T23:28:38+00:00"),
                    StartTime = null,
                    EndTime = null,
                    WorkTypeId = 2
                }, 
            });

        var statusServiceMock = new Mock<IStatusService>();
        var technicianServiceMock = new Mock<ITechnicianService>();
        var workTypeServiceMock = new Mock<IWorkTypeService>();

        var workOrderService = new WorkOrderService(
            apiConnectorMock.Object,
            null,
            statusServiceMock.Object,
            technicianServiceMock.Object,
            workTypeServiceMock.Object
        );

        // Act
        var result = await workOrderService.GetWorkOrdersAsync();

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetWorkOrderAsync_ShouldReturnWorkOrdersByWorkOrderName()
    {
        // Arrange
        var apiConnectorMock = new Mock<IApiConnector>();

        var sampleWorkOrders = new WorkOrder[]
        {
            new WorkOrder
            {
                WorkOrderName = "WO-20240108-11574467",
                StatusId = 0,
                TechnicianId = 12,
                Duration = "00:00:00",
                CreatedDate = DateTimeOffset.Parse("2024-01-08T11:21:38+00:00"),
                StartTime = null,
                EndTime = null,
                WorkTypeId = 1
            },
            new WorkOrder
            {
                WorkOrderName = "WO-20240113-05772746",
                StatusId = 2,
                TechnicianId = 47,
                Duration = "02:54:00",
                CreatedDate = DateTimeOffset.Parse("2024-01-13T02:48:38+00:00"),
                StartTime = DateTimeOffset.Parse("2024-01-26T16:47:00+00:00"),
                EndTime = DateTimeOffset.Parse("2024-01-26T19:41:00+00:00"),
                WorkTypeId = 3
            },
        };

        apiConnectorMock.Setup(m => m.GetAsync<WorkOrder>(It.IsAny<string>()))
            .ReturnsAsync((string endpoint) =>
            {
                string workOrderName = endpoint.Split('/').Last();
                return sampleWorkOrders.FirstOrDefault(w => w.WorkOrderName == workOrderName);
            });

        var statusServiceMock = new Mock<IStatusService>();
        var technicianServiceMock = new Mock<ITechnicianService>();
        var workTypeServiceMock = new Mock<IWorkTypeService>();

        var workOrderService = new WorkOrderService(
            apiConnectorMock.Object,
            null,
            statusServiceMock.Object,
            technicianServiceMock.Object,
            workTypeServiceMock.Object
        );

        // Act
        var resultWorkOrder1 = await workOrderService.GetWorkOrderAsync("WO-20240108-11574467");
        var resultWorkOrder2 = await workOrderService.GetWorkOrderAsync("WO-20240113-05772746");
        var resultNonExistingWorkOrder = await workOrderService.GetWorkOrderAsync("WO123");
        var resultEmptyString = await workOrderService.GetWorkOrderAsync("");

        // Assert
        Assert.IsNotNull(resultWorkOrder1);
        Assert.IsNotNull(resultWorkOrder2);
        Assert.IsNull(resultNonExistingWorkOrder);
        Assert.IsNull(resultEmptyString);
    }
}