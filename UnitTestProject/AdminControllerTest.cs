using Moq;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using Microsoft.Extensions.Logging;
using WebAppOppg2.Controllers;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace UnitTestProject
{
    public class AdminControllerTest
    {

        // Sett på Canvas-modul om enhetstesting.

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<ITicketRepository> mockRep = new Mock<ITicketRepository>();
        private readonly Mock<ILogger<AdminController>> mockLog = new Mock<ILogger<AdminController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task EditAdminOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(a => a.EditAdmin(It.IsAny<Admin>())).ReturnsAsync(true);
            var adminController = new AdminController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await adminController.EditAdmin(It.IsAny<Admin>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Admin endret - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditAdminError_LoginOK()
        {
            // Arrange
            mockRep.Setup(a => a.EditAdmin(It.IsAny<Admin>())).ReturnsAsync(false);
            var adminController = new AdminController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await adminController.EditAdmin(It.IsAny<Admin>()) as NotFoundObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Admin ikke endret, feil i DB - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditAdminModelError_LoginOK()
        {
            // Arrange
            mockRep.Setup(a => a.EditAdmin(It.IsAny<Admin>())).ReturnsAsync(true);
            var adminController = new AdminController(mockRep.Object, mockLog.Object);
            adminController.ModelState.AddModelError("Username", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await adminController.EditAdmin(It.IsAny<Admin>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Admin ikke endret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public async Task EditAdmin_LoginNone()
        {
            // Arrange
            mockRep.Setup(a => a.EditAdmin(It.IsAny<Admin>())).ReturnsAsync(true);
            var adminController = new AdminController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await adminController.EditAdmin(It.IsAny<Admin>()) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Admin ikke endret - status: ikke innlogget", result.Value);
        }

        [Fact]
        public async Task LoginOK()
        {
            // Arrange
            mockRep.Setup(u => u.LogIn(It.IsAny<Admin>())).ReturnsAsync(true);
            var adminController = new AdminController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await adminController.LogIn(It.IsAny<Admin>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.True((bool)result.Value);
        }

        [Fact]
        public async Task LoginError()
        {
            // Arrange
            mockRep.Setup(u => u.LogIn(It.IsAny<Admin>())).ReturnsAsync(false);
            var adminController = new AdminController(mockRep.Object, mockLog.Object);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await adminController.LogIn(It.IsAny<Admin>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.False((bool)result.Value);
        }

        [Fact]
        public async Task LoginModelError()
        {
            // Arrange
            mockRep.Setup(u => u.LogIn(It.IsAny<Admin>())).ReturnsAsync(true);
            var adminController = new AdminController(mockRep.Object, mockLog.Object);
            adminController.ModelState.AddModelError("Username", "Feil ved inputvalideringen på server");
            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await adminController.LogIn(It.IsAny<Admin>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        [Fact]
        public void LogOutOK()
        {
            // Arrange
            var adminController = new AdminController(mockRep.Object, mockLog.Object);
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            adminController.LogOut();

            // Assert
            Assert.Equal(_ikkeLoggetInn, mockSession[_loggetInn]);
        }
    }
}
