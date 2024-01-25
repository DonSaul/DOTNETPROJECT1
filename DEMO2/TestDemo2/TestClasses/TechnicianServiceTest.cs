using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Softserve.ProjectLab.ClientAPI.Models;
using Softserve.ProjectLab.ClientAPI.Services;
using System.Threading.Tasks;

namespace TestDemo2.TestClasses;
[TestClass]
public class TechnicianServiceTest
{
    [TestMethod]
    public async Task GetTechniciansAsync_ShouldReturnTechnicians()
    {
        // Arrange
        var apiConnectorMock = new Mock<IApiConnector>();

        var sampleTechnicians = new Technician[]
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

        apiConnectorMock.Setup(m => m.GetAsync<Technician[]>(It.IsAny<string>()))
                        .ReturnsAsync(sampleTechnicians);
        var serviceProviderMock = new Mock<IServiceProvider>();

        var technicianService = new TechnicianService(serviceProviderMock.Object, apiConnectorMock.Object);

        // Act
        var resultTechnicians = await technicianService.GetTechniciansAsync();

        // Assert
        Assert.IsNotNull(resultTechnicians);
    }

    [TestMethod]
    public async Task GetTechnicianAsync_ShouldReturnTechnicianById()
    {
        // Arrange
        var apiConnectorMock = new Mock<IApiConnector>();

        var sampleTechnician = new Technician
        {
            TechnicianId = 47,
            Name = "Pablo Ardiles",
            Address = "Calle Blanco 3803, Santiago",
        };

        apiConnectorMock.Setup(m => m.GetAsync<Technician>(It.IsAny<string>()))
                        .ReturnsAsync(sampleTechnician);
        var serviceProviderMock = new Mock<IServiceProvider>();

        var technicianService = new TechnicianService(serviceProviderMock.Object, apiConnectorMock.Object);

        // Act
        var resultTechnician = await technicianService.GetTechnicianAsync(47);
        var resultTechnicianNonExistent = await technicianService.GetTechnicianAsync(274);

        // Assert
        Assert.IsNotNull(resultTechnician);
        Assert.AreEqual("Pablo Ardiles", resultTechnician.Name);
        Assert.IsNotNull(resultTechnicianNonExistent);
        Assert.Fail("Error: the return type is " + resultTechnicianNonExistent.GetType());
    }

    [TestMethod]
    public async Task GetTechnicianByNameAsync_ShouldReturnTechniciansByName()
    {
        // Arrange
        var apiConnectorMock = new Mock<IApiConnector>();

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

        apiConnectorMock.Setup(m => m.GetAsync<Technician[]>(It.IsAny<string>()))
                        .ReturnsAsync(sampleTechnicians);
        var serviceProviderMock = new Mock<IServiceProvider>();
        var workOrderServiceMock = new Mock<IWorkOrderService>();
        var statusServiceMock = new Mock<IStatusService>();
        var workTypeServiceMock = new Mock<IWorkTypeService>();

        serviceProviderMock.Setup(sp => sp.GetService(typeof(IWorkOrderService))).Returns(workOrderServiceMock.Object);
        serviceProviderMock.Setup(sp => sp.GetService(typeof(IStatusService))).Returns(statusServiceMock.Object);
        serviceProviderMock.Setup(sp => sp.GetService(typeof(IWorkTypeService))).Returns(workTypeServiceMock.Object);

        var technicianService = new TechnicianService(serviceProviderMock.Object, apiConnectorMock.Object);

        // Act
        var resultTechnicians = await technicianService.GetTechnicianByNameAsync("Mauricio Montes");
        var resultTechnicians2 = await technicianService.GetTechnicianByNameAsync("Gabriel Zuñiga");
        var resultTechniciansNonExistent = await technicianService.GetTechnicianByNameAsync("Señor Polainas");
        var resultTechniciansNoInputText = await technicianService.GetTechnicianByNameAsync("");

        // Assert
        Assert.IsNotNull(resultTechnicians);
        Assert.IsNotNull(resultTechnicians2);
        Assert.IsNotNull(resultTechniciansNonExistent);
        Assert.AreEqual(0, resultTechniciansNonExistent.Count);
        Assert.IsNotNull(resultTechniciansNoInputText);
        Assert.AreEqual(0, resultTechniciansNoInputText.Count);
    }
}