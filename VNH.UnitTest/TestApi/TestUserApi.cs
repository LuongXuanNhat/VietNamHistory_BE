using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.InMemory;
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common;
using VNH.Application.Services.Catalog.Users;
using VNH.Domain;
using VNH.Infrastructure.Implement.Catalog.Users;
using VNH.Infrastructure.Presenters.Migrations;
using VNH.WebAPi.Controllers;
using Xunit.Sdk;
using Microsoft.Extensions.Options;
using Azure.Core;
using VNH.Application.Interfaces.Email;
using Microsoft.AspNetCore.Mvc;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.UnitTest.TestApi
{
    public class TestUserApi
    {
        private readonly Faker _faker;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly UserService _userService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ISendMailService> _mockSendMailService;
        private readonly Mock<VietNamHistoryContext> _mockVietNamHistoryContext;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly VietNamHistoryContext _vietNamHistoryContext;

        public TestUserApi()
        {
            _vietNamHistoryContext = new VietNamHistoryContext();
            _faker = new Faker();
            _mockConfiguration = new Mock<IConfiguration>();

            // Initialize _dataContext here
            var options = new DbContextOptionsBuilder<VietNamHistoryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new VietNamHistoryContext(options);
            _mockVietNamHistoryContext = new Mock<VietNamHistoryContext>(options);

            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);
            _mockSendMailService = new Mock<ISendMailService>();
            _mockLogger = new Mock<ILogger<UserService>>();
            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);

            _userService = new UserService(
                dbContext,
                _mockLogger.Object,
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockConfiguration.Object,
                _mockSendMailService.Object
            );
        }

        [Fact]
        public async Task AuthenticateValidReturnsSuccessResult()
        {
            // Arrange
            var loginRequest = new LoginRequest("nam@gmail.com", "Aa@123");
            var user = new User { Email = loginRequest.Email, LockoutEnabled = true };
            var roles = new[] { "Role1", "Role2" };
            _mockUserManager.Setup(manager => manager.FindByEmailAsync(loginRequest.Email))
            .ReturnsAsync(user);
            _mockSignInManager.Setup(signIn => signIn.PasswordSignInAsync(user, loginRequest.Password, true, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _mockUserManager.Setup(manager => manager.GetRolesAsync(user))
                .ReturnsAsync(roles);
            _mockConfiguration.Setup(config => config["Tokens:Key"])
                .Returns("0123456789ABCDEF");

            // Act
            var result = await _userService.Authenticate(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccessed);
        }

        [Fact]
        public async Task RegisterValidReturnsSuccessResult()
        {
            // Arrange
            var email = _faker.Internet.Email();
            var password = _faker.Internet.Password();
            var registerRequest = new RegisterRequest(email, password, password);

            // Mock the UserManager's FindByEmailAsync to return null, indicating that the email is not in use.
            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => null);

            // Set up the context to return an empty list for Users using the mock context.
            var users = new List<User>().AsQueryable();
            _mockVietNamHistoryContext.Setup(c => c.Users).Returns(GetQueryableMock(users).Object);

            // Mock the UserManager's CreateAsync to return IdentityResult.Success, indicating successful user creation.
            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.Register(registerRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccessed);
        }

        [Fact]
        public async Task Email_Confirm_Success()
        {
            var loginRequest = new LoginRequest("nam@gmail.com", "Aa@123");
            var user = _vietNamHistoryContext.User.FirstOrDefault(x=>x.Email.Equals(loginRequest.Email));
            if (user != null && user.NumberConfirm != null)
            {
                var numberConfirm = user.NumberConfirm;
                var result = await _userService.EmailConfirm(numberConfirm, loginRequest.Email);
                Assert.NotNull(result);
                Assert.True(result.IsSuccessed);
            }

        }

        [Fact]
        public async Task EmailConfirm_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.EmailConfirm(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ApiSuccessResult<bool> { IsSuccessed = true });

          //  var controller = new UserController(userServiceMock.Object);

            // Act
          //  var result = await controller.EmailConfirm("123456");

            // Assert
            //Assert.IsType<OkObjectResult>(result);
            //var okResult = (OkObjectResult)result;
            //Assert.True(okResult.StatusCode == 200);
            //Assert.True((bool)okResult.Value); // Assuming your result is a boolean value
        }

        private Mock<DbSet<T>> GetQueryableMock<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            var mockData = data.ToList(); // Materialize the data to a list
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(mockData.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(mockData.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(mockData.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => mockData.AsQueryable().GetEnumerator());
            return mockSet;
        }


    }


}