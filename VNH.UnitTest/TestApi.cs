using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
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
    public class TestApi
    {
        private readonly Mock<IUserStore<User>> mockUserStore = new Mock<IUserStore<User>>();
        private readonly Mock<IOptions<IdentityOptions>> mockIdentityOptions = new Mock<IOptions<IdentityOptions>>();
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<VietNamHistoryContext> _mockVietNamHistoryContext;

        public TestApi()
        {
            _mockUserManager = new Mock<UserManager<User>>(mockUserStore.Object, mockIdentityOptions.Object, null, null, null, null, null, null, null); 
            _mockSignInManager = new Mock<SignInManager<User>>(); 
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockVietNamHistoryContext = new Mock<VietNamHistoryContext>();
        }

        [Fact]
        public async Task Authencation_Success()
        {
            var userService = new UserService(
            context: _mockVietNamHistoryContext.Object,
            userManager: _mockUserManager.Object,
            signInManager: _mockSignInManager.Object,
            configuration: _mockConfiguration.Object,
            memoryCache: _mockMemoryCache.Object
        );

            var request = new LoginRequest("admin@gmail.com", "Aa@123");

            // Config 
            _mockUserManager.Setup(um => um.FindByNameAsync(request.Email))
                            .ReturnsAsync(new User { Email = request.Email, Status = false });
            _mockSignInManager.Setup(sm => sm.PasswordSignInAsync(It.IsAny<User>(), request.Password, true, true))
                             .ReturnsAsync(SignInResult.Success);
            _mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
                            .ReturnsAsync(new List<string> { "Role1", "Role2" });

            var result = await userService.Authenticate(request);

            Assert.NotNull(result);
            Assert.True(result.IsSuccessed);
        }
    }
}