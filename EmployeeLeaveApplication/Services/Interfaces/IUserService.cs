using EmployeeLeaveApplication.Models;

namespace EmployeeLeaveApplication.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ServiceResponse<User>> GetUser(int userId);
    }
}
