using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using WebAppOppg2.RouteController;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UnitTestProject
{
    public class RouteControllerTest
    {
        // Sett på Canvas-modul om enhetstesting.

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<ITicketRepository> mockRep = new Mock<ITicketRepository>();
        private readonly Mock<ILogger<RouteController>> mockLog = new Mock<ILogger<RouteController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task SaveRouteOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(r => r.AddRoute(It.IsAny<Route>())).ReturnsAsync(true);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.SaveRoute(It.IsAny<Route>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Rute lagret - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SaveRouteError_LoginOK()
        {
            // Arrange
            mockRep.Setup(r => r.AddRoute(It.IsAny<Route>())).ReturnsAsync(false);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.SaveRoute(It.IsAny<Route>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Rute ikke lagret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SaveRouteModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(r => r.AddRoute(It.IsAny<Route>())).ReturnsAsync(true);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            routeController.ModelState.AddModelError("PortFrom", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.SaveRoute(It.IsAny<Route>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Rute ikke lagret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SaveRoute_LoginNone()
        {
            // Arrange
            mockRep.Setup(r => r.AddRoute(It.IsAny<Route>())).ReturnsAsync(true);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.SaveRoute(It.IsAny<Route>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Rute ikke lagret - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetOneOK_LoginOK()
        {
            // Assert
            var route1 = new Route
            {
                RouteID = 1,
                PortFrom = "Bergen",
                PortTo = "Hirtshals",
                RoutePrice = 100,
                Departure = "10:00"
            };

            mockRep.Setup(r => r.GetOneRoute(It.IsAny<int>())).ReturnsAsync(route1
                );
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.GetOneRoute(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<Route>(route1, (Route)result.Value);
        }

        [Fact]
        public async Task GetOneError_LoginOK()
        {
            // Assert
            mockRep.Setup(r => r.GetOneRoute(It.IsAny<int>())).ReturnsAsync(() => null);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.GetOneRoute(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Rute ikke hentet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task GetOne_LoginNone()
        {
            // Assert
            mockRep.Setup(r => r.GetOneRoute(It.IsAny<int>())).ReturnsAsync(() => null);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.GetOneRoute(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Rute ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetAllOK_LoginOK()
        {
            // Arrange
            var route1 = new Route
            {
                RouteID = 1,
                PortFrom = "Bergen",
                PortTo = "Hirtshals",
                RoutePrice = 100,
                Departure = "10:00"
            };
            var route2 = new Route
            {
                RouteID = 3,
                PortFrom = "Bergen",
                PortTo = "Kristiansand",
                RoutePrice = 149,
                Departure = "10:00"
            };
            var route3 = new Route
            {
                RouteID = 6,
                PortFrom = "Langesund",
                PortTo = "Stavanger",
                RoutePrice = 249,
                Departure = "18:00"
            };

            var orderList = new List<Route>();
            orderList.Add(route1);
            orderList.Add(route2);
            orderList.Add(route3);

            mockRep.Setup(r => r.GetAllRoutes()).ReturnsAsync(orderList);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.GetAllRoutes() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<List<Route>>((List<Route>)result.Value, orderList);
        }

        [Fact]
        public async Task GetAllError_LoginOK()
        {
            // Arrange
            var orderList = new List<Route>();
            mockRep.Setup(r => r.GetAllRoutes()).ReturnsAsync(() => null);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.GetAllRoutes() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAll_LoginNone()
        {
            // Arrange
            mockRep.Setup(r => r.GetAllRoutes()).ReturnsAsync(It.IsAny<List<Route>>());
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.GetAllRoutes() as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Ruter ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task DeleteRouteOK_LoginOK()
        {
            // Assert
            mockRep.Setup(r => r.DeleteRoute(It.IsAny<int>())).ReturnsAsync(true);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.DeleteRoute(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Rute slettet - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeleteRouteError_LoginOK()
        {
            // Assert
            mockRep.Setup(r => r.DeleteRoute(It.IsAny<int>())).ReturnsAsync(false);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.DeleteRoute(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Rute ikke slettet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeleteRoute_LoginNone()
        {
            // Assert
            mockRep.Setup(r => r.DeleteRoute(It.IsAny<int>())).ReturnsAsync(true);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.DeleteRoute(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Rute ikke slettet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task EditRouteOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(r => r.EditRoute(It.IsAny<Route>())).ReturnsAsync(true);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.EditRoute(It.IsAny<Route>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Rute endret - status: innlogget", result.Value);

        }

        [Fact]
        public async Task EditRouteError_LoginOK()
        {
            // Arrange
            mockRep.Setup(r => r.EditRoute(It.IsAny<Route>())).ReturnsAsync(false);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.EditRoute(It.IsAny<Route>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Rute ikke endret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditRouteModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(r => r.EditRoute(It.IsAny<Route>())).ReturnsAsync(true);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            routeController.ModelState.AddModelError("PortFrom", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.EditRoute(It.IsAny<Route>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Rute ikke endret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditRoute_LoginNone()
        {
            // Arrange
            mockRep.Setup(r => r.EditRoute(It.IsAny<Route>())).ReturnsAsync(true);
            var routeController = new RouteController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await routeController.EditRoute(It.IsAny<Route>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Rute ikke endret - status: ikke innlogget", result.Value);
        }
    }
}
