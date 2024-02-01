using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Softserve.ProjectLab.ClientAPI.Models;
using Softserve.ProjectLab.ClientAPI.Services;
using Softserve.ProjectLab.ClientAPI.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestDemo2.TestClasses;
[TestClass]
public class TechnicianControllerTest
{
    [TestMethod]
    public async Task GetAllTechnicians_ReturnsOkResult()
    {
        // Arrange
        var technicianServiceMock = new Mock<ITechnicianService>();
        var sampleTechnicians = new Technician[]
        {
            new Technician
            {
                TechnicianId = 32,
                Name = "Gabriel Zuñiga",
                Address = "Calle Blanco 9601, Santiago",
            },
            new Technician
            {
                TechnicianId = 27,
                Name = "Mauricio Montes",
                Address = "Diego Portales 19, Valparaiso",
            },
        };

        technicianServiceMock.Setup(service => service.GetTechniciansAsync())
            .ReturnsAsync(sampleTechnicians);

        var controller = new TechnicianController(technicianServiceMock.Object);

        // Act
        var result = await controller.Get();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        Assert.IsNotNull(((OkObjectResult)result).Value);
    }

    [TestMethod]
    public async Task GetTechnicianById_ReturnsOkResult()
    {
        // Arrange
        var technicianServiceMock = new Mock<ITechnicianService>();
        var sampleTechnician = new Technician
        {
            TechnicianId = 47,
            Name = "Pablo Ardiles",
            Address = "Calle Blanco 3803, Santiago",
        };
        technicianServiceMock.Setup(service => service.GetTechnicianAsync(sampleTechnician.TechnicianId))
            .ReturnsAsync(sampleTechnician);

        var controller = new TechnicianController(technicianServiceMock.Object);

        // Act
        var result = await controller.Get(sampleTechnician.TechnicianId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        Assert.IsNotNull(((OkObjectResult)result).Value);
    }

    [TestMethod]
    public async Task List_ReturnsViewResult()
    {
        // Arrange
        var technicianServiceMock = new Mock<ITechnicianService>();
        var sampleTechnicians = new Technician[]
        {
            new Technician
            {
                TechnicianId = 32,
                Name = "Gabriel Zuñiga",
                Address = "Calle Blanco 9601, Santiago",
            },
            new Technician
            {
                TechnicianId = 27,
                Name = "Mauricio Montes",
                Address = "Diego Portales 19, Valparaiso",
            },
        };
        technicianServiceMock.Setup(service => service.GetTechniciansAsync())
            .ReturnsAsync(sampleTechnicians);

        var controller = new TechnicianController(technicianServiceMock.Object);

        // Act
        var result = await controller.List();

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
    }

    [TestMethod]
    public async Task Details_ReturnsViewResult()
    {
        // Arrange
        var technicianServiceMock = new Mock<ITechnicianService>();
        var sampleTechnicians = new Technician[]
        {
            new Technician
            {
                TechnicianId = 32,
                Name = "Gabriel Zuñiga",
                Address = "Calle Blanco 9601, Santiago",
            },
            new Technician
            {
                TechnicianId = 27,
                Name = "Mauricio Montes",
                Address = "Diego Portales 19, Valparaiso",
            },
            new Technician
            {
                TechnicianId = 47,
                Name = "Pablo Ardiles",
                Address = "Calle Blanco 3803, Santiago",
            },
        };

        var workOrderDetails = new WorkOrderDetails
        {
            WorkOrderName = "WorkOrder1",
            Technician = "Pablo Ardiles",
            WorkType = "1",
            Status = "0",
            StartTime = DateTimeOffset.Now.AddDays(-1),
            EndTime = DateTimeOffset.Now
        };

        var technicianDetails = new TechnicianDetails
        {
            TechnicianId = 47,
            Technician = "Pablo Ardiles",
            Address = "Calle Blanco 3803, Santiago",
            WorkOrders = new WorkOrderDetails[] { workOrderDetails }
        };

        var technicianDetailsList = new List<TechnicianDetails> { technicianDetails };

        technicianServiceMock.Setup(service => service.GetTechnicianAsync(sampleTechnicians[2].TechnicianId))
            .ReturnsAsync(new Technician 
            {
                TechnicianId = 47,
                Name = "Pablo Ardiles",
                Address = "Calle Blanco 3803, Santiago",
            });

        technicianServiceMock.Setup(service => service.GetTechnicianByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(technicianDetailsList);

        var controller = new TechnicianController(technicianServiceMock.Object);

        // Act
        var result = await controller.Details(sampleTechnicians[2].TechnicianId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
    }

    [TestMethod]
    public async Task GetAllTechnicians_Exception_ReturnsBadRequest()
    {
        // Arrange
        var technicianServiceMock = new Mock<ITechnicianService>();
        technicianServiceMock.Setup(service => service.GetTechniciansAsync())
            .ThrowsAsync(new Exception("Test exception"));

        var controller = new TechnicianController(technicianServiceMock.Object);

        // Act
        var result = await controller.Get();

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        Assert.AreEqual("Test exception", ((BadRequestObjectResult)result).Value);
    }

    [TestMethod]
    public async Task GetTechnicianById_NotFound_ReturnsNotFound()
    {
        // Arrange
        var technicianServiceMock = new Mock<ITechnicianService>();
        technicianServiceMock.Setup(service => service.GetTechnicianAsync(It.IsAny<int>()))
            .ReturnsAsync((Technician)null);

        var controller = new TechnicianController(technicianServiceMock.Object);

        // Act
        var result = await controller.Get(274); // Provide a non-existing technician ID

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task GetTechnicianById_Exception_ReturnsBadRequest()
    {
        // Arrange
        var technicianServiceMock = new Mock<ITechnicianService>();
        technicianServiceMock.Setup(service => service.GetTechnicianAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Test exception"));

        var controller = new TechnicianController(technicianServiceMock.Object);

        // Act
        var result = await controller.Get(274); // Provide any technician ID

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        Assert.AreEqual("Test exception", ((BadRequestObjectResult)result).Value);
    }

    [TestMethod]
    public async Task GetTechnicianByName_NotFound_ReturnsNotFound()
    {
        // Arrange
        var technicianServiceMock = new Mock<ITechnicianService>();
        technicianServiceMock.Setup(service => service.GetTechnicianByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((List<TechnicianDetails>)null);

        var controller = new TechnicianController(technicianServiceMock.Object);

        // Act
        var result = await controller.Get("NonExistentTechnician"); // Provide a non-existing technician name

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task GetTechnicianByName_Exception_ReturnsBadRequest()
    {
        // Arrange
        var technicianServiceMock = new Mock<ITechnicianService>();
        technicianServiceMock.Setup(service => service.GetTechnicianByNameAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Test exception"));

        var controller = new TechnicianController(technicianServiceMock.Object);

        // Act
        var result = await controller.Get("AnyTechnician"); // Provide any technician name

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        Assert.AreEqual("Test exception", ((BadRequestObjectResult)result).Value);
    }
}