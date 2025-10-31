using EmployeeLeaveApplication.Models;
using System.Net;

namespace EmployeeLeaveApplication.Services
{
    public class JsonServerService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:3000";

        public JsonServerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var users = await _httpClient.GetFromJsonAsync<List<User>>($"{BaseUrl}/users");
            return users ?? [];           
        }

        public async Task<User?> GetUserAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/users/{id}?_expand=role");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<User>();
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error fetching user data", ex);
            }
            return null;
        }

        public async Task<string> AddLeaveAsync(Leave leave)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/leaves", leave);
            if (response.IsSuccessStatusCode)
            {
                return "Leave request submitted successfully.";
            }
            else
            {
                return "Failed to submit leave request.";
            }
        }

        public async Task<string> UpdateLeaveAsync(Leave leave)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/leaves/{leave.Id}", leave);
            if (response.IsSuccessStatusCode)
            {
                return "Leave request updated successfully.";
            }
            else
            {
                return "Failed to update leave request.";
            }
        }

        public async Task<Leave?> GetLeaveRequestAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/leaves/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Leave>();                    
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error fetching user data", ex);
            }
            return null;
        }
    }
}
