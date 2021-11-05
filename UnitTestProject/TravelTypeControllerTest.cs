using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using WebAppOppg2.TravelTypeController;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UnitTestProject
{
    public class TravelTypeControllerTest
    {
        // Sett på Canvas-modul om enhetstesting.

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<ITicketRepository> mockRep = new Mock<ITicketRepository>();
        private readonly Mock<ILogger<TravelTypeController>> mockLog = new Mock<ILogger<TravelTypeController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task SaveTravelTypeOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(t => t.AddTravelType(It.IsAny<TravelType>())).ReturnsAsync(true);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.SaveTravelType(It.IsAny<TravelType>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Reisetype lagret - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SaveTravelTypeError_LoginOK()
        {
            // Arrange
            mockRep.Setup(t => t.AddTravelType(It.IsAny<TravelType>())).ReturnsAsync(false);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.SaveTravelType(It.IsAny<TravelType>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Reisetype ikke lagret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SaveTravelTypeModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(t => t.AddTravelType(It.IsAny<TravelType>())).ReturnsAsync(true);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            travelTypeController.ModelState.AddModelError("TravelTypeName", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.SaveTravelType(It.IsAny<TravelType>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Reisetype ikke lagret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SaveTravelType_LoginNone()
        {
            // Arrange
            mockRep.Setup(t => t.AddTravelType(It.IsAny<TravelType>())).ReturnsAsync(true);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.SaveTravelType(It.IsAny<TravelType>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Reisetype ikke lagret - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetOneOK_LoginOK()
        {
            // Assert
            var travelType1 = new TravelType
            {
                TravelTypeID = 1,
                TravelTypeName = "Enveis"
            };

            mockRep.Setup(t => t.GetOneTravelType(It.IsAny<int>())).ReturnsAsync(travelType1);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.GetOneTravelType(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<TravelType>(travelType1, (TravelType)result.Value);
        }

        [Fact]
        public async Task GetOneError_LoginOK()
        {
            // Assert
            mockRep.Setup(t => t.GetOneTravelType(It.IsAny<int>())).ReturnsAsync(() => null);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.GetOneTravelType(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Reisetype ikke hentet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task GetOne_LoginNone()
        {
            // Assert
            mockRep.Setup(t => t.GetOneTravelType(It.IsAny<int>())).ReturnsAsync(() => null);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.GetOneTravelType(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Reisetype ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetAllOK_LoginOK()
        {
            // Arrange
            var travelType1 = new TravelType
            {
                TravelTypeID = 1,
                TravelTypeName = "Enveis"
            };
            var travelType2 = new TravelType
            {
                TravelTypeID = 2,
                TravelTypeName = "Tur/Retur"
            };
            var travelType3 = new TravelType
            {
                TravelTypeID = 3,
                TravelTypeName = "Cruise"
            };

            var orderList = new List<TravelType>();
            orderList.Add(travelType1);
            orderList.Add(travelType2);
            orderList.Add(travelType3);

            mockRep.Setup(t => t.GetAllTravelTypes()).ReturnsAsync(orderList);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.GetAllTravelTypes() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<List<TravelType>>((List<TravelType>)result.Value, orderList);
        }

        [Fact]
        public async Task GetAllError_LoginOK()
        {
            // Arrange
            var orderList = new List<TravelType>();
            mockRep.Setup(t => t.GetAllTravelTypes()).ReturnsAsync(() => null);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.GetAllTravelTypes() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAll_LoginNone()
        {
            // Arrange
            mockRep.Setup(t => t.GetAllTravelTypes()).ReturnsAsync(It.IsAny<List<TravelType>>());
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.GetAllTravelTypes() as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Reisetype ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task DeleteTravelTypeOK_LoginOK()
        {
            // Assert
            mockRep.Setup(t => t.DeleteTravelType(It.IsAny<int>())).ReturnsAsync(true);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.DeleteTravelType(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Reisetype slettet - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeleteTravelTypeError_LoginOK()
        {
            // Assert
            mockRep.Setup(t => t.DeleteTravelType(It.IsAny<int>())).ReturnsAsync(false);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.DeleteTravelType(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Reisetype ikke slettet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeleteTravelType_LoginNone()
        {
            // Assert
            mockRep.Setup(t => t.DeleteTravelType(It.IsAny<int>())).ReturnsAsync(true);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.DeleteTravelType(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Reisetype ikke slettet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task EditTravelTypeOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(t => t.EditTravelType(It.IsAny<TravelType>())).ReturnsAsync(true);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.EditTravelType(It.IsAny<TravelType>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Reisetype endret - status: innlogget", result.Value);

        }

        [Fact]
        public async Task EditTravelTypeError_LoginOK()
        {
            // Arrange
            mockRep.Setup(t => t.EditTravelType(It.IsAny<TravelType>())).ReturnsAsync(false);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.EditTravelType(It.IsAny<TravelType>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Reisetype ikke endret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditTravelTypeModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(t => t.EditTravelType(It.IsAny<TravelType>())).ReturnsAsync(true);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            travelTypeController.ModelState.AddModelError("TravelTypeName", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.EditTravelType(It.IsAny<TravelType>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Reisetype ikke endret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditTravelType_LoginNone()
        {
            // Arrange
            mockRep.Setup(t => t.EditTravelType(It.IsAny<TravelType>())).ReturnsAsync(true);
            var travelTypeController = new TravelTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            travelTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await travelTypeController.EditTravelType(It.IsAny<TravelType>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Reisetype ikke endret - status: ikke innlogget", result.Value);
        }
    }
}
