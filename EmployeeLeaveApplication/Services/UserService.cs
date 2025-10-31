using EmployeeLeaveApplication.Models;
using EmployeeLeaveApplication.Services.Interfaces;
using System.Text.Json;

namespace EmployeeLeaveApplication.Services
{
    public class UserService : IUserService
    {
        private static readonly HttpClient client = new HttpClient();
        public async Task<ServiceResponse<User>> GetUser(int userId)
        {
            var serviceResponse = new ServiceResponse<User>();
            string url = $"http://localhost:4000/users/{userId}";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var userData = JsonSerializer.Deserialize<User>(jsonString);
                if (userData is null)
                {
                    serviceResponse.Status = false;
                    serviceResponse.Message = "User data not found, its null.";
                    return serviceResponse;
                }
                serviceResponse.Status = true;
                serviceResponse.Data = userData;
            }
            else
            {
                serviceResponse.Status = false;
                serviceResponse.Message = "User not found.";
            }
            return serviceResponse;
        }
    }
}
