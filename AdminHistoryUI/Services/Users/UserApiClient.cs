using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Domain;
using VNH.Domain.Enums;

namespace AdminHistoryUI.Services.Users
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory,
                 IHttpContextAccessor httpContextAccessor,
                  IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // lấy được instance của HttpClient
            var client = _httpClientFactory.CreateClient();
            // thiết lập thuộc tính BaseAddress để tạo ra base address của Web API.
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            // triệu gọi action Post() của Web API.
            var response = await client.PostAsync("/Login", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(await response.Content.ReadAsStringAsync());
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<List<UserDetailDto>> getAllUser()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync("/User/GetAllUser");
            var body = await response.Content.ReadAsStringAsync();
            List<UserDetailDto> locations = JsonConvert.DeserializeObject<List<UserDetailDto>>(body);
            return locations;

        }


        public async Task<ApiResult<string>> RegisterUser(RegisterRequest registerRequest)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(registerRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/SignUp", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(result);
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id,UserDetailDto request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var requestContent = new MultipartFormDataContent();

           
            if (request.Fullname != null)
            {
                requestContent.Add(new StringContent(request.Fullname.ToString()), "FullName");
            }

            string DateOfBirth = request.DateOfBirth?.ToString("dd-MM-yyyy") ?? string.Empty;
            requestContent.Add(new StringContent(DateOfBirth), "DateOfBirth");

           /* requestContent.Add(new StringContent(((int)request.Gender).ToString()), "Gender");*/
           
            string Introduction = request.Introduction?.ToString() ?? string.Empty;
            requestContent.Add(new StringContent(Introduction), "Introduction");

            string PhoneNumber = request.PhoneNumber?.ToString() ?? string.Empty;
            requestContent.Add(new StringContent(PhoneNumber), "PhoneNumber");

            requestContent.Add(new StringContent(request.Email.ToString()), "Email");

         

            var response = await client.PutAsync($"/User/{id}", requestContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }


        public async Task<ApiResult<UserDetailDto>> GetById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync($"/User/{id}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<UserDetailDto>>(body);

            return JsonConvert.DeserializeObject<ApiErrorResult<UserDetailDto>>(body);
        }


        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.DeleteAsync($"/User/{id}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }




    }
}
