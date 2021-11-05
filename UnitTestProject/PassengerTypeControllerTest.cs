using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using WebAppOppg2.PassengerTypeController;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace UnitTestProject
{
    public class PassengerTypeControllerTest
    {
        // Sett på Canvas-modul om enhetstesting.

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<ITicketRepository> mockRep = new Mock<ITicketRepository>();
        private readonly Mock<ILogger<PassengerTypeController>> mockLog = new Mock<ILogger<PassengerTypeController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task SavePassengerTypeOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.AddPassengerType(It.IsAny<PassengerType>())).ReturnsAsync(true);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.SavePassenger(It.IsAny<PassengerType>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Passasjertype lagret - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SavePassengerTypeError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.AddPassengerType(It.IsAny<PassengerType>())).ReturnsAsync(false);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.SavePassenger(It.IsAny<PassengerType>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Passasjertype ikke lagret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SavePassengerTypeModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.AddPassengerType(It.IsAny<PassengerType>())).ReturnsAsync(true);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            passengerTypeController.ModelState.AddModelError("Firstname", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.SavePassenger(It.IsAny<PassengerType>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Passasjertype ikke lagret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SavePassengerType_LoginNone()
        {
            // Arrange
            mockRep.Setup(p => p.AddPassengerType(It.IsAny<PassengerType>())).ReturnsAsync(true);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.SavePassenger(It.IsAny<PassengerType>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Passasjertype ikke lagret - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetOneOK_LoginOK()
        {
            // Assert
            var passengerType1 = new PassengerType
            {
                PassengerTypeID = 3,
                PassengerTypeName = "voksen", // "voksen", PassengerTypeID = 3
                Discount = 0
            };

            mockRep.Setup(p => p.GetOnePassengerType(It.IsAny<int>())).ReturnsAsync(passengerType1);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.GetOnePassengerType(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<PassengerType>(passengerType1, (PassengerType)result.Value);
        }

        [Fact]
        public async Task GetOneError_LoginOK()
        {
            // Assert
            mockRep.Setup(p => p.GetOnePassengerType(It.IsAny<int>())).ReturnsAsync(() => null);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.GetOnePassengerType(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Passasjertype ikke hentet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task GetOne_LoginNone()
        {
            // Assert
            mockRep.Setup(p => p.GetOnePassengerType(It.IsAny<int>())).ReturnsAsync(() => null);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.GetOnePassengerType(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Passasjertype ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetAllOK_LoginOK()
        {
            // Arrange
            var passengerType1 = new PassengerType
            {
                PassengerTypeID = 3,
                PassengerTypeName = "voksen", // "voksen", PassengerTypeID = 3
                Discount = 0
            };
            var passengerType2 = new PassengerType
            {
                PassengerTypeID = 5,
                PassengerTypeName = "honnør", // "honnør", PassengerTypeID = 5
                Discount = 25
            };
            var passengerType3 = new PassengerType
            {
                PassengerTypeID = 2,
                PassengerTypeName = "barn", // "barn", PassengerTypeID = 2
                Discount = 50
            };

            var orderList = new List<PassengerType>();
            orderList.Add(passengerType1);
            orderList.Add(passengerType2);
            orderList.Add(passengerType3);

            mockRep.Setup(p => p.GetAllPassengerTypes()).ReturnsAsync(orderList);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.GetAllPassengerTypes() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<List<PassengerType>>((List<PassengerType>)result.Value, orderList);
        }

        [Fact]
        public async Task GetAllError_LoginOK()
        {
            // Arrange
            var orderList = new List<PassengerType>();
            mockRep.Setup(p => p.GetAllPassengerTypes()).ReturnsAsync(() => null);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.GetAllPassengerTypes() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAll_LoginNone()
        {
            // Arrange
            mockRep.Setup(p => p.GetAllPassengerTypes()).ReturnsAsync(It.IsAny<List<PassengerType>>());
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.GetAllPassengerTypes() as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Passasjertyper ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task DeletePassengerTypeOK_LoginOK()
        {
            // Assert
            mockRep.Setup(p => p.DeletePassengerType(It.IsAny<int>())).ReturnsAsync(true);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.DeletePassengerType(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Passasjertype slettet - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeletePassengerTypeError_LoginOK()
        {
            // Assert
            mockRep.Setup(p => p.DeletePassengerType(It.IsAny<int>())).ReturnsAsync(false);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.DeletePassengerType(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Passasjertype ikke slettet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeletePassengerType_LoginNone()
        {
            // Assert
            mockRep.Setup(p => p.DeletePassengerType(It.IsAny<int>())).ReturnsAsync(true);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.DeletePassengerType(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Passasjertype ikke slettet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task EditPassengerTypeOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.EditPassengerType(It.IsAny<PassengerType>())).ReturnsAsync(true);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.EditPassengerType(It.IsAny<PassengerType>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Passasjertype endret - status: innlogget", result.Value);

        }

        [Fact]
        public async Task EditPassengerTypeError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.EditPassengerType(It.IsAny<PassengerType>())).ReturnsAsync(false);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.EditPassengerType(It.IsAny<PassengerType>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Passasjertype ikke endret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditPassengerTypeModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.EditPassengerType(It.IsAny<PassengerType>())).ReturnsAsync(true);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            passengerTypeController.ModelState.AddModelError("PassengerTypeName", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.EditPassengerType(It.IsAny<PassengerType>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Passasjertype ikke endret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditPassengerType_LoginNone()
        {
            // Arrange
            mockRep.Setup(p => p.EditPassengerType(It.IsAny<PassengerType>())).ReturnsAsync(true);
            var passengerTypeController = new PassengerTypeController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerTypeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerTypeController.EditPassengerType(It.IsAny<PassengerType>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Passasjertype ikke endret - status: ikke innlogget", result.Value);
        }
    }
}
