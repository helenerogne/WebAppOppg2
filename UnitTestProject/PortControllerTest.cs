using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using WebAppOppg2.PortController;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UnitTestProject
{
    public class PortControllerTest
    {
        // Sett på Canvas-modul om enhetstesting.

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<ITicketRepository> mockRep = new Mock<ITicketRepository>();
        private readonly Mock<ILogger<PortController>> mockLog = new Mock<ILogger<PortController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task SavePortOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.AddPort(It.IsAny<Port>())).ReturnsAsync(true);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.SavePort(It.IsAny<Port>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Havn lagret - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SavePortError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.AddPort(It.IsAny<Port>())).ReturnsAsync(false);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.SavePort(It.IsAny<Port>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Havn ikke lagret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SavePortModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.AddPort(It.IsAny<Port>())).ReturnsAsync(true);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            portController.ModelState.AddModelError("Portname", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.SavePort(It.IsAny<Port>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Havn ikke lagret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SavePort_LoginNone()
        {
            // Arrange
            mockRep.Setup(p => p.AddPort(It.IsAny<Port>())).ReturnsAsync(true);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.SavePort(It.IsAny<Port>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Havn ikke lagret - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetOneOK_LoginOK()
        {
            // Assert
            var port1 = new Port
            {
                PortID = 1,
                PortName = "Bergen" // "Bergen", PortID = 1
            };

            mockRep.Setup(p => p.GetOnePort(It.IsAny<int>())).ReturnsAsync(port1);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.GetOnePort(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<Port>(port1, (Port)result.Value);
        }

        [Fact]
        public async Task GetOneError_LoginOK()
        {
            // Assert
            mockRep.Setup(p => p.GetOnePort(It.IsAny<int>())).ReturnsAsync(() => null);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.GetOnePort(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Havn ikke hentet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task GetOne_LoginNone()
        {
            // Assert
            mockRep.Setup(p => p.GetOnePort(It.IsAny<int>())).ReturnsAsync(() => null);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.GetOnePort(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Havn ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetAllOK_LoginOK()
        {
            // Arrange
            var port1 = new Port
            {
                PortID = 1,
                PortName = "Bergen" // "Bergen", PortID = 1
            };
            var port2 = new Port
            {
                PortID = 3,
                PortName = "Langesund" // "Langesund", PortID = 3
            };
            var port3 = new Port
            {
                PortID = 5,
                PortName = "Stavanger" // "Stavanger", PortID = 5
            };

            var orderList = new List<Port>();
            orderList.Add(port1);
            orderList.Add(port2);
            orderList.Add(port3);

            mockRep.Setup(p => p.GetAllPorts()).ReturnsAsync(orderList);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.GetAllPorts() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<List<Port>>((List<Port>)result.Value, orderList);
        }

        [Fact]
        public async Task GetAllError_LoginOK()
        {
            // Arrange
            var orderList = new List<Port>();
            mockRep.Setup(p => p.GetAllPorts()).ReturnsAsync(() => null);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.GetAllPorts() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAll_LoginNone()
        {
            // Arrange
            mockRep.Setup(p => p.GetAllPorts()).ReturnsAsync(It.IsAny<List<Port>>());
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.GetAllPorts() as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Havner ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task DeletePortOK_LoginOK()
        {
            // Assert
            mockRep.Setup(p => p.DeletePort(It.IsAny<int>())).ReturnsAsync(true);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.DeletePort(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Havn slettet - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeletePortError_LoginOK()
        {
            // Assert
            mockRep.Setup(p => p.DeletePort(It.IsAny<int>())).ReturnsAsync(false);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.DeletePort(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Havn ikke slettet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeletePort_LoginNone()
        {
            // Assert
            mockRep.Setup(p => p.DeletePort(It.IsAny<int>())).ReturnsAsync(true);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.DeletePort(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Havn ikke slettet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task EditPortOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.EditPort(It.IsAny<Port>())).ReturnsAsync(true);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.EditPort(It.IsAny<Port>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Havn endret - status: innlogget", result.Value);

        }

        [Fact]
        public async Task EditPortError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.EditPort(It.IsAny<Port>())).ReturnsAsync(false);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.EditPort(It.IsAny<Port>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Havn ikke endret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditPortModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.EditPort(It.IsAny<Port>())).ReturnsAsync(true);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            portController.ModelState.AddModelError("PortName", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.EditPort(It.IsAny<Port>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Havn ikke endret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditPort_LoginNone()
        {
            // Arrange
            mockRep.Setup(p => p.EditPort(It.IsAny<Port>())).ReturnsAsync(true);
            var portController = new PortController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await portController.EditPort(It.IsAny<Port>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Havn ikke endret - status: ikke innlogget", result.Value);
        }
    }
}