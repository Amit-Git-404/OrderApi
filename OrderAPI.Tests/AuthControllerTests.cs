using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OrderApi.Controllers;
using OrderApi.Model;
using System;

namespace OrderApi.Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private AuthController _authController;
        private Mock<ILogger<AuthController>> _loggerMock;
        private Mock<IConfiguration> _configurationMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<AuthController>>();
            _configurationMock = new Mock<IConfiguration>();
            _authController = new AuthController(_loggerMock.Object, _configurationMock.Object);
        }

        [Test]
        public void Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var user = new LoginModel { Username = "amit", Password = "Pass@777" };

            // Act
            var result = _authController.Login(user) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.GetType().GetProperty("Token"), Is.Not.Null);
        }

        [Test]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var user = new LoginModel { Username = "invalidUser", Password = "invalidPassword" };

            // Act
            var result = _authController.Login(user) as UnauthorizedResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(401));
        }

        [Test]
        public void Login_NullUser_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = _authController.Login(null) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void Login_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            _configurationMock.Setup(c => c.GetSection("JwtSettings")).Throws(new Exception("Simulated Exception"));

            // Act
            var result = _authController.Login(new LoginModel { Username = "amit", Password = "Pass@777" }) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(500));
            Assert.That(result.Value.GetType().GetProperty("Message").GetValue(result.Value), Is.EqualTo("Internal Server Error"));
        }
    }
}
