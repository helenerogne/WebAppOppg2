using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using Microsoft.Extensions.Logging;
using WebAppOppg2.Controllers;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using WebAppOppg2.OrderController;

namespace UnitTestProject
{
    public class AdminControllerTest
    {
        private const string loggedIn = "loggedIn";
        private const string notLoggedIn = "notLoggedIn";

        private readonly Mock<IAdminRepository> mockRep = new Mock<IAdminRepository>();
        private readonly Mock<ILogger<AdminController>> mockLog = new Mock<ILogger<AdminController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        // Litt usikker på denne rekken med enhetstester på Edit..
        // pga jeg er usikker på hva EditAdmin gjør.
        [Fact]
        public async Task EditAdminOK_LoginOK()
        {
            // Arrange
            mockRep.Setup(a => a.EditAdmin(It.IsAny<Admin>())).ReturnsAsync(true);
            var adminController = new AdminController(mockRep.Object, mockLog.Object);
            mockSession[loggedIn] = loggedIn;
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
            mockSession[loggedIn] = loggedIn;
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
            mockSession[loggedIn] = loggedIn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await adminController.EditAdmin(It.IsAny<Admin>()) as BadRequestObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Admin ikke endret, feil ved inputvalideringen på server - status: innlogget", result.Value);
        }

        // Forvirret av denne... Tror den må justeres
        // EditError_LoginNone
        [Fact]
        public async Task EditAdmin_LoginNone()
        {
            // Arrange
            mockRep.Setup(a => a.EditAdmin(It.IsAny<Admin>())).ReturnsAsync(true);
            var adminController = new AdminController(mockRep.Object, mockLog.Object);
            mockSession[loggedIn] = notLoggedIn;
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
            mockSession[loggedIn] = loggedIn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await adminController.LogIn(It.IsAny<Admin>()) as OkObjectResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.True((bool)result.Value);
        }

        // Feil passord eller brukernavn
        [Fact]
        public async Task LoginError()
        {
            // Arrange
            mockRep.Setup(u => u.LogIn(It.IsAny<Admin>())).ReturnsAsync(false);
            var adminController = new AdminController(mockRep.Object, mockLog.Object);
            mockSession[loggedIn] = notLoggedIn;
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
            mockSession[loggedIn] = loggedIn;
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
            mockSession[loggedIn] = notLoggedIn; // Tor hadde skrev _loggetInn som innhold her, men tror det var feil.
            adminController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            adminController.LogOut();

            // Assert
            Assert.Equal(notLoggedIn, mockSession[loggedIn]);
        }
    }
}
