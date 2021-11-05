using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using WebAppOppg2.PassengerController;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UnitTestProject
{
    public class PassengerControllerTest
    {
        // Sett på Canvas-modul om enhetstesting.

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<ITicketRepository> mockRep = new Mock<ITicketRepository>();
        private readonly Mock<ILogger<PassengerController>> mockLog = new Mock<ILogger<PassengerController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task SavePassengerOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.AddPassenger(It.IsAny<Passenger>())).ReturnsAsync(true);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.SavePassenger(It.IsAny<Passenger>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Passasjer lagret - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SavePassengerError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.AddPassenger(It.IsAny<Passenger>())).ReturnsAsync(false);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.SavePassenger(It.IsAny<Passenger>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Passasjer ikke lagret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SavePassengerModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.AddPassenger(It.IsAny<Passenger>())).ReturnsAsync(true);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            passengerController.ModelState.AddModelError("Firstname", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.SavePassenger(It.IsAny<Passenger>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Passasjer ikke lagret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SavePassenger_LoginNone()
        {
            // Arrange
            mockRep.Setup(p => p.AddPassenger(It.IsAny<Passenger>())).ReturnsAsync(true);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.SavePassenger(It.IsAny<Passenger>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Passasjer ikke lagret - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetOneOK_LoginOK()
        {
            // Assert
            var passenger1 = new Passenger
            {
                PassengerID = 1,
                Firstname = "Jane",
                Lastname = "Doe",
                Email = "janedoe@gmail.com",
                PassengerTypeID = 3,
                PassengerType = "voksen" // "voksen", PassengerTypeID = 3
            };

            mockRep.Setup(p => p.GetOnePassenger(It.IsAny<int>())).ReturnsAsync(passenger1);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.GetOnePassenger(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<Passenger>(passenger1, (Passenger)result.Value);
        }

        [Fact]
        public async Task GetOneError_LoginOK()
        {
            // Assert
            mockRep.Setup(p => p.GetOnePassenger(It.IsAny<int>())).ReturnsAsync(() => null);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.GetOnePassenger(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Passasjer ikke hentet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task GetOne_LoginNone()
        {
            // Assert
            mockRep.Setup(p => p.GetOnePassenger(It.IsAny<int>())).ReturnsAsync(() => null);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.GetOnePassenger(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Passasjer ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetAllOK_LoginOK()
        {
            // Arrange
            var passenger1 = new Passenger
            {
                PassengerID = 1,
                Firstname = "Jane",
                Lastname = "Doe",
                Email = "janedoe@gmail.com",
                PassengerTypeID = 3,
                PassengerType = "voksen" // "voksen", PassengerTypeID = 3
            };
            var passenger2 = new Passenger
            {
                PassengerID = 2,
                Firstname = "John",
                Lastname = "Doe",
                Email = "j.doe@hotmail.com",
                PassengerTypeID = 5,
                PassengerType = "honnør", // "honnør", PassengerTypeID = 5
            };
            var passenger3 = new Passenger
            {
                PassengerID = 3,
                Firstname = "Dwight",
                Lastname = "Schrute",
                Email = "dwights@live.com",
                PassengerTypeID = 2,
                PassengerType = "barn", // "barn", PassengerTypeID = 2
            };

            var orderList = new List<Passenger>();
            orderList.Add(passenger1);
            orderList.Add(passenger2);
            orderList.Add(passenger3);

            mockRep.Setup(p => p.GetAllPassengers()).ReturnsAsync(orderList);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.GetAllPassengers() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<List<Passenger>>((List<Passenger>)result.Value, orderList);
        }

        [Fact]
        public async Task GetAllError_LoginOK()
        {
            // Arrange
            var orderList = new List<PassengerType>();
            mockRep.Setup(p => p.GetAllPassengers()).ReturnsAsync(() => null);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.GetAllPassengers() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAll_LoginNone()
        {
            // Arrange
            mockRep.Setup(p => p.GetAllPassengers()).ReturnsAsync(It.IsAny<List<Passenger>>());
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.GetAllPassengers() as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Passasjerer ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task DeletePassengerOK_LoginOK()
        {
            // Assert
            mockRep.Setup(p => p.DeletePassenger(It.IsAny<int>())).ReturnsAsync(true);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.DeletePassenger(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Passasjer slettet - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeletePassengerError_LoginOK()
        {
            // Assert
            mockRep.Setup(p => p.DeletePassenger(It.IsAny<int>())).ReturnsAsync(false);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.DeletePassenger(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Passasjer ikke slettet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeletePassenger_LoginNone()
        {
            // Assert
            mockRep.Setup(p => p.DeletePassenger(It.IsAny<int>())).ReturnsAsync(true);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.DeletePassenger(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Passasjer ikke slettet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task EditPassengerOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.EditPassenger(It.IsAny<Passenger>())).ReturnsAsync(true);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.EditPassenger(It.IsAny<Passenger>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Passasjer endret - status: innlogget", result.Value);

        }

        [Fact]
        public async Task EditPassengerError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.EditPassenger(It.IsAny<Passenger>())).ReturnsAsync(false);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.EditPassenger(It.IsAny<Passenger>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Passasjer ikke endret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditPassengerModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(p => p.EditPassenger(It.IsAny<Passenger>())).ReturnsAsync(true);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            passengerController.ModelState.AddModelError("Firstname", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.EditPassenger(It.IsAny<Passenger>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Passasjer ikke endret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditPassenger_LoginNone()
        {
            // Arrange
            mockRep.Setup(p => p.EditPassenger(It.IsAny<Passenger>())).ReturnsAsync(true);
            var passengerController = new PassengerController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            passengerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await passengerController.EditPassenger(It.IsAny<Passenger>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Passasjer ikke endret - status: ikke innlogget", result.Value);
        }
    }
}
