using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Expressions;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common;
using VNH.Application.Services.Catalog.Users;
using VNH.Domain;
using VNH.Infrastructure.Implement.Catalog.Users;
using VNH.Infrastructure.Presenters.Migrations;
using VNH.WebAPi.Controllers;
using Xunit.Sdk;

namespace VNH.UnitTest
{
    public class TestUserApi
    {
        private readonly Faker _faker;
        private readonly Mock<ILogger> _mockLogger;
        private readonly UserService _userService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<VietNamHistoryContext> _mockVietNamHistoryContext;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;

        public TestUserApi()
        {
            _faker = new Faker();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockVietNamHistoryContext = new Mock<VietNamHistoryContext>();
            _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _mockLogger = new Mock<ILogger>();
            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);

            _userService = new UserService(
                _mockVietNamHistoryContext.Object,
                _mockLogger.Object,
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockConfiguration.Object
            );
        }

        [Fact]
        public async Task AuthenticateValidReturnsSuccessResult()
        {
            // Arrange
            var loginRequest = new LoginRequest("admin@gmail.com", "Aa@123");
            var user = new User { Email = loginRequest.Email, Status = false };
            var roles = new[] { "Role1", "Role2" };
            _mockUserManager.Setup(manager => manager.FindByEmailAsync(loginRequest.Email))
            .ReturnsAsync(user);
            _mockSignInManager.Setup(signIn => signIn.PasswordSignInAsync(user, loginRequest.Password, true, true))
                .ReturnsAsync(SignInResult.Success);
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
            var email = _faker.Internet.Email();
            var password = _faker.Internet.Password();
            var registerRequest = new RegisterRequest(email, password, password);

            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
           .ReturnsAsync((string email) => null);

            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);


            var result = _userService.Register(registerRequest);

            Assert.NotNull(result);
            Assert.True(result.IsCompletedSuccessfully);
        }
    }
}