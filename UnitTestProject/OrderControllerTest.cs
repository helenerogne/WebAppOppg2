using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using WebAppOppg2.OrderController;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UnitTestProject
{
    public class OrderControllerTest
    {
        // Sett på Canvas-modul om enhetstesting.

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<ITicketRepository> mockRep = new Mock<ITicketRepository>();
        private readonly Mock<ILogger<OrderController>> mockLog = new Mock<ILogger<OrderController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task SaveTicketOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(o => o.SaveTicket(It.IsAny<Ticket>())).ReturnsAsync(true);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.SaveTicket(It.IsAny<Ticket>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Billett lagret - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SaveTicketError_LoginOK()
        {
            // Arrange
            mockRep.Setup(o => o.SaveTicket(It.IsAny<Ticket>())).ReturnsAsync(false);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.SaveTicket(It.IsAny<Ticket>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Billett ikke lagret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SaveTicketModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(o => o.SaveTicket(It.IsAny<Ticket>())).ReturnsAsync(true);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            orderController.ModelState.AddModelError("Firstname", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.SaveTicket(It.IsAny<Ticket>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Billett ikke lagret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task SaveTicket_LoginNone()
        {
            // Arrange
            mockRep.Setup(o => o.SaveTicket(It.IsAny<Ticket>())).ReturnsAsync(true);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.SaveTicket(It.IsAny<Ticket>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Billett ikke lagret - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetOneOK_LoginOK()
        {
            // Assert
            var ticket1 = new Ticket
            {
                TicketID = 1,
                PassengerID = 1,
                Firstname = "Jane",
                Lastname = "Doe",
                Email = "janedoe@gmail.com",
                PassengerType = "voksen", // "voksen", PassengerTypeID = 3
                RouteID = 1,
                TravelType = "Tur/Retur", // "Tur/Retur", TravelTypeID = 2
                RouteFrom = "Bergen", // Bergen, PortID = 1
                RouteTo = "Hirtshals", // Hirtshals, PortID = 2
                Departure = "11:00",
                TicketDate = "11-11-21",
                Price = 150
            };

            mockRep.Setup(o => o.GetOne(It.IsAny<int>())).ReturnsAsync(ticket1);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.GetOne(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<Ticket>(ticket1, (Ticket)result.Value);
        }

        [Fact]
        public async Task GetOneError_LoginOK()
        {
            // Assert
            mockRep.Setup(o => o.GetOne(It.IsAny<int>())).ReturnsAsync(() => null);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.GetOne(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Billett ikke hentet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task GetOne_LoginNone()
        {
            // Assert
            mockRep.Setup(o => o.GetOne(It.IsAny<int>())).ReturnsAsync(() => null);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.GetOne(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Billett ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task GetAllOK_LoginOK()
        {
            // Arrange
            var ticket1 = new Ticket
            {
                TicketID = 1,
                PassengerID = 1,
                Firstname = "Jane",
                Lastname = "Doe",
                Email = "janedoe@gmail.com",
                PassengerType = "voksen", // "voksen", PassengerTypeID = 3
                RouteID = 1,
                TravelType = "Tur/Retur", // "Tur/Retur", TravelTypeID = 2
                RouteFrom = "Bergen", // Bergen, PortID = 1
                RouteTo = "Hirtshals", // Hirtshals, PortID = 2
                Departure = "11:00",
                TicketDate = "11-11-21",
                Price = 150
            };
            var ticket2 = new Ticket
            {
                TicketID = 2,
                PassengerID = 2,
                Firstname = "John",
                Lastname = "Doe",
                Email = "j.doe@hotmail.com",
                PassengerType = "honnør", // "honnør", PassengerTypeID = 5
                RouteID = 1,
                TravelType = "Envei", // "Envei", TravelTypeID = 1
                RouteFrom = "Langesund", // Langesund, PortID = 3
                RouteTo = "Stavanger", // Stavanger, PortID = 5
                Departure = "18:30",
                TicketDate = "28-01-22",
                Price = 150
            };
            var ticket3 = new Ticket
            {
                TicketID = 3,
                PassengerID = 3,
                Firstname = "Dwight",
                Lastname = "Schrute",
                Email = "dwights@live.com",
                PassengerType = "barn", // "barn", PassengerTypeID = 2
                RouteID = 1,
                TravelType = "Cruise", // "Cruise", TravelTypeID = 3
                RouteFrom = "Hirtshals", // Hirtshals, PortID = 2
                RouteTo = "Hirtshals", // Hirtshals, PortID = 2
                Departure = "09:00",
                TicketDate = "03-12-21",
                Price = 150
            };

            var orderList = new List<Ticket>();
            orderList.Add(ticket1);
            orderList.Add(ticket2);
            orderList.Add(ticket3);

            mockRep.Setup(o => o.GetAll()).ReturnsAsync(orderList);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.GetAll() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal<List<Ticket>>((List<Ticket>)result.Value, orderList);
        }

        [Fact]
        public async Task GetAllError_LoginOK()
        {
            // Arrange
            var orderList = new List<Ticket>();
            mockRep.Setup(o => o.GetAll()).ReturnsAsync(() => null);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.GetAll() as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAll_LoginNone()
        {
            // Arrange
            mockRep.Setup(o => o.GetAll()).ReturnsAsync(It.IsAny<List<Ticket>>());
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.GetAll() as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Billetter ikke hentet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task DeleteTicketOK_LoginOK()
        {
            // Assert
            mockRep.Setup(o => o.DeleteTicket(It.IsAny<int>())).ReturnsAsync(true);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.DeleteTicket(It.IsAny<int>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Billett slettet - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeleteTicketError_LoginOK()
        {
            // Assert
            mockRep.Setup(o => o.DeleteTicket(It.IsAny<int>())).ReturnsAsync(false);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.DeleteTicket(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Billett ikke slettet, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task DeleteTicket_LoginNone()
        {
            // Assert
            mockRep.Setup(o => o.DeleteTicket(It.IsAny<int>())).ReturnsAsync(true);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.DeleteTicket(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Billett ikke slettet, feil i DB - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task EditTicketOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(o => o.EditTicket(It.IsAny<Ticket>())).ReturnsAsync(true);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.EditTicket(It.IsAny<Ticket>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Billett endret - status: innlogget", result.Value);

        }

        [Fact]
        public async Task EditTicketError_LoginOK()
        {
            // Arrange
            mockRep.Setup(o => o.EditTicket(It.IsAny<Ticket>())).ReturnsAsync(false);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.EditTicket(It.IsAny<Ticket>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Billett ikke endret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditTicketModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(o => o.EditTicket(It.IsAny<Ticket>())).ReturnsAsync(true);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            orderController.ModelState.AddModelError("Firstname", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.EditTicket(It.IsAny<Ticket>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Billett ikke endret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditTicket_LoginNone()
        {
            // Arrange
            mockRep.Setup(o => o.EditTicket(It.IsAny<Ticket>())).ReturnsAsync(true);
            var orderController = new OrderController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await orderController.EditTicket(It.IsAny<Ticket>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Billett ikke endret - status: ikke innlogget", result.Value);
        }
    }
}