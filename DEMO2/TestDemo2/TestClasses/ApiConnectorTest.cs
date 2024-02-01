using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Softserve.ProjectLab.ClientAPI.Services;
using Softserve.ProjectLab.ClientAPI.Models;

namespace TestDemo2.TestClasses
{
    [TestClass]
    public class ApiConnectorTest
    {
        private ApiConnector _apiConnector;
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _apiConnector = new ApiConnector(_httpClientFactoryMock.Object);
        }

        [TestMethod]
        public async Task GetAsync_SuccessfulResponse_ReturnsDeserializedObject()
        {
            // Arrange
            var expectedResponse = new Technician {
                TechnicianId = 47,
                Name = "Pablo Ardiles",
                Address = "Calle Blanco 3803, Santiago",
            };
            var serializedResponse = Newtonsoft.Json.JsonConvert.SerializeObject(expectedResponse);
            var endpoint = "api/example";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(serializedResponse)
                });

            // Act
            var result = await _apiConnector.GetAsync<Technician>(endpoint);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.Name, result.Name);
            Assert.AreEqual(expectedResponse.TechnicianId, result.TechnicianId);
            Assert.AreEqual(expectedResponse.Address, result.Address);
        }

        [TestMethod]
        public async Task GetAsync_HttpIOException_ReturnsDefault()
        {
            // Arrange
            var endpoint = "api/example";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpIOException(HttpRequestError.ResponseEnded));

            // Act
            var result = await _apiConnector.GetAsync<Technician>(endpoint);

            // Assert
            Assert.AreEqual(default(Technician), result);
        }

        [TestMethod]
        public async Task HandleResponseAsync_NotFound_ThrowsHttpIOException()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.NotFound
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<HttpIOException>(() => _apiConnector.HandleResponseAsync(response));
        }

        [TestMethod]
        public async Task HandleResponseAsync_NonSuccessStatusCode_ThrowsException()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                ReasonPhrase = "Internal Server Error"
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _apiConnector.HandleResponseAsync(response));
        }
    }
}