using EmployeeLeaveApplication.Models;
using Microsoft.Exchange.WebServices.Data;
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

        public async Task<User?> GetUserAsync(int id, bool fetchRoles)
        {
            try
            {
                HttpResponseMessage response;
                if (fetchRoles)
                    response = await _httpClient.GetAsync($"{BaseUrl}/users/{id}?_expand=role");
                else
                    response = await _httpClient.GetAsync($"{BaseUrl}/users/{id}");
                
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
            var user = await GetUserAsync(leave.UserId, false);
            if (leave.LeaveStatusId == 2)
            {
                if (user == null)
                {
                    return "User not found.";
                }
                if (user.BalanceLeaves <= 0)
                {
                    return "You have exhausted your available leaves.";
                }
            }

            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/leaves/{leave.Id}", leave);

            if (response.IsSuccessStatusCode)
            {
                if (leave.LeaveStatusId == 2 && user != null)
                {
                    user.BalanceLeaves = user.BalanceLeaves - 1;                    
                    _ = UpdateUserAsync(user);
                }
                return "Leave request updated successfully.";
            }
            else
            {
                return "Failed to update leave request.";
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/users/{user.Id}", user);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
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

        public async Task<List<Leave>> GetLeaveRequestOfUserAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/leaves");
                if (response.IsSuccessStatusCode)
                {
                    var leaves = await response.Content.ReadFromJsonAsync<List<Leave>>();
                    var userLeaves = leaves?.Where(l => l.UserId == id).ToList() ?? new List<Leave>();
                    return userLeaves;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return [];
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error fetching user data", ex);
            }
            return [];
        }

        public async Task<List<LeaveStatus>> GetLeaveTypessAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<LeaveStatus>>($"{BaseUrl}/leaveStatus");
            return response?.Where(x=>x.Id != 1).ToList() ?? [];
        }
    }
}
